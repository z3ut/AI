using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core.Questions
{
	public delegate Boolean DoesStatementMatchDelegate<in QuestionT, in StatementT>(IQuestionProcessingContext<QuestionT> context, StatementT statement)
		where QuestionT : StatementQuestion<QuestionT, StatementT>
		where StatementT : IStatement;

	public delegate Boolean AreEnoughToAnswerDelegate<in QuestionT, StatementT>(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements)
		where QuestionT : StatementQuestion<QuestionT, StatementT>
		where StatementT : IStatement;

	public delegate IAnswer CreateAnswerDelegate<in QuestionT, StatementT>(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements)
		where QuestionT : StatementQuestion<QuestionT, StatementT>
		where StatementT : IStatement;

	public delegate IEnumerable<NestedQuestion> GetNestedQuestionsDelegate<in QuestionT>(IQuestionProcessingContext<QuestionT> context)
		where QuestionT : Question<QuestionT>;

	public delegate IAnswer ProcessChildAnswersDelegate<in QuestionT, StatementT>(
		IQuestionProcessingContext<QuestionT> context,
		ICollection<StatementT> statements,
		IDictionary<IAnswer, ICollection<IStatement>> childAnswers)
		where QuestionT : StatementQuestion<QuestionT, StatementT>
		where StatementT : IStatement;

	public class StatementQuestion<QuestionT, StatementT> : Question<QuestionT>
		where QuestionT : StatementQuestion<QuestionT, StatementT>
		where StatementT : IStatement
	{
		private readonly DoesStatementMatchDelegate<QuestionT, StatementT> _doesStatementMatch;
		private readonly AreEnoughToAnswerDelegate<QuestionT, StatementT> _areEnoughToAnswer;
		private readonly CreateAnswerDelegate<QuestionT, StatementT> _createAnswer;
		private readonly GetNestedQuestionsDelegate<QuestionT> _getNestedQuestions;
		private readonly ProcessChildAnswersDelegate<QuestionT, StatementT> _processChildAnswers;

		public StatementQuestion(
			DoesStatementMatchDelegate<QuestionT, StatementT> doesStatementMatch,
			CreateAnswerDelegate<QuestionT, StatementT> createAnswer,
			AreEnoughToAnswerDelegate<QuestionT, StatementT> areEnoughToAnswer = null,
			GetNestedQuestionsDelegate<QuestionT> getNestedQuestions = null,
			ProcessChildAnswersDelegate<QuestionT, StatementT> processChildAnswers = null)
		{
			if (doesStatementMatch == null) throw new ArgumentNullException(nameof(doesStatementMatch));
			if (createAnswer == null) throw new ArgumentNullException(nameof(createAnswer));

			_doesStatementMatch = doesStatementMatch;
			_areEnoughToAnswer = areEnoughToAnswer ?? AreEnoughToAnswer;
			_createAnswer = createAnswer;
			_getNestedQuestions = getNestedQuestions ?? GetNestedQuestions;
			_processChildAnswers = processChildAnswers ?? ProcessChildAnswers;
		}

		protected override IAnswer Process(IQuestionProcessingContext<QuestionT> context)
		{
			var statements = context.KnowledgeBase.Statements.Enumerate<StatementT>(context.ActiveContexts).Where(statement => _doesStatementMatch(context, statement)).ToList();

			if (_areEnoughToAnswer(context, statements))
			{
				return _createAnswer(context, statements);
			}

			IDictionary<IAnswer, ICollection<IStatement>> valuableAnswers = new Dictionary<IAnswer, ICollection<IStatement>>();
			foreach (var nested in _getNestedQuestions(context))
			{
				var answer = nested.Question.Ask(context);
				if (!answer.IsEmpty)
				{
					valuableAnswers.Add(answer, nested.TransitiveStatements);
				}
			}

			return _processChildAnswers(context, statements, valuableAnswers);
		}

		private Boolean AreEnoughToAnswer(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements)
		{
			return statements.Count > 0;
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<QuestionT> context)
		{
			yield break;
		}

		private IAnswer ProcessChildAnswers(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements, IDictionary<IAnswer, ICollection<IStatement>> childAnswers)
		{
			if (childAnswers.Count > 0)
			{
				var answer = childAnswers.First();
				answer.Key.Explanation.Expand(answer.Value);
				return answer.Key;
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		protected static IAnswer CreateCommonConceptsAnswer(
			IQuestionProcessingContext<QuestionT> context,
			ICollection<StatementT> statements,
			Func<StatementT, IConcept> statementConceptSelector,
			Func<QuestionT, IConcept> questionConceptSelector,
			Func<ILanguageAnswers, String> answerFormatSelector)
		{
			if (statements.Any())
			{
				var concepts = statements.Select(statementConceptSelector).ToList();

				String format;
				var parameters = concepts.Enumerate(out format);
				parameters.Add(Strings.ParamAnswer, questionConceptSelector(context.Question));
				return new ConceptsAnswer(
					concepts,
					new FormattedText(() => answerFormatSelector(context.Language.Answers) + format + ".", parameters),
					new Explanation(statements.OfType<IStatement>()));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		protected static IAnswer CreateCommonBooleanAnswer(
			IQuestionProcessingContext<QuestionT> context,
			ICollection<StatementT> statements,
			Boolean success,
			Func<ILanguageAnswers, String> trueAnswerFormatSelector,
			Func<ILanguageAnswers, String> falseAnswerFormatSelector,
			Func<QuestionT, Dictionary<String, INamed>> questionConceptsSelector)
		{
			return new BooleanAnswer(
				success,
				new FormattedText(
					success ? new Func<String>(() => trueAnswerFormatSelector(context.Language.Answers)) : () => falseAnswerFormatSelector(context.Language.Answers),
					questionConceptsSelector(context.Question)),
				new Explanation(statements.OfType<IStatement>()));
		}
	}

	public class NestedQuestion
	{
		public IQuestion Question
		{ get; }

		public ICollection<IStatement> TransitiveStatements
		{ get; }

		public NestedQuestion(IQuestion question, ICollection<IStatement> transitiveStatements)
		{
			Question = question;
			TransitiveStatements = transitiveStatements;
		}
	}
}
