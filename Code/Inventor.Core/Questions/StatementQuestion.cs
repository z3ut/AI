using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core.Questions
{
	public delegate Boolean DoesStatementMatchDelegate<in QuestionT, in StatementT>(IQuestionProcessingContext<QuestionT> context, StatementT statement)
		where QuestionT : StatementQuestion<StatementT>
		where StatementT : IStatement;

	public delegate Boolean AreEnoughToAnswerDelegate<in QuestionT, StatementT>(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements)
		where QuestionT : StatementQuestion<StatementT>
		where StatementT : IStatement;

	public delegate IAnswer CreateAnswerDelegate<in QuestionT, StatementT>(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements)
		where QuestionT : StatementQuestion<StatementT>
		where StatementT : IStatement;

	public delegate IEnumerable<NestedQuestion> GetNestedQuestionsDelegate<in QuestionT>(IQuestionProcessingContext<QuestionT> context)
		where QuestionT : Question<QuestionT>;

	public delegate IAnswer ProcessChildAnswersDelegate<in QuestionT, StatementT>(
		IQuestionProcessingContext<QuestionT> context,
		ICollection<StatementT> statements,
		IDictionary<IAnswer, ICollection<IStatement>> childAnswers)
		where QuestionT : StatementQuestion<StatementT>
		where StatementT : IStatement;

	public class StatementQuestion<StatementT> : Question<StatementQuestion<StatementT>>
		where StatementT : IStatement
	{
		private readonly DoesStatementMatchDelegate<StatementQuestion<StatementT>, StatementT> _doesStatementMatch;
		private readonly AreEnoughToAnswerDelegate<StatementQuestion<StatementT>, StatementT> _areEnoughToAnswer;
		private readonly CreateAnswerDelegate<StatementQuestion<StatementT>, StatementT> _createAnswer;
		private readonly GetNestedQuestionsDelegate<StatementQuestion<StatementT>> _getNestedQuestions;
		private readonly ProcessChildAnswersDelegate<StatementQuestion<StatementT>, StatementT> _processChildAnswers;

		public StatementQuestion(
			DoesStatementMatchDelegate<StatementQuestion<StatementT>, StatementT> doesStatementMatch,
			CreateAnswerDelegate<StatementQuestion<StatementT>, StatementT> createAnswer,
			AreEnoughToAnswerDelegate<StatementQuestion<StatementT>, StatementT> areEnoughToAnswer = null,
			GetNestedQuestionsDelegate<StatementQuestion<StatementT>> getNestedQuestions = null,
			ProcessChildAnswersDelegate<StatementQuestion<StatementT>, StatementT> processChildAnswers = null)
		{
			if (doesStatementMatch == null) throw new ArgumentNullException(nameof(doesStatementMatch));
			if (createAnswer == null) throw new ArgumentNullException(nameof(createAnswer));

			_doesStatementMatch = doesStatementMatch;
			_areEnoughToAnswer = areEnoughToAnswer ?? AreEnoughToAnswer;
			_createAnswer = createAnswer;
			_getNestedQuestions = getNestedQuestions ?? GetNestedQuestions;
			_processChildAnswers = processChildAnswers ?? ProcessChildAnswers;
		}

		protected override IAnswer Process(IQuestionProcessingContext<StatementQuestion<StatementT>> context)
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

		private Boolean AreEnoughToAnswer(IQuestionProcessingContext<StatementQuestion<StatementT>> context, ICollection<StatementT> statements)
		{
			return statements.Count > 0;
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<StatementQuestion<StatementT>> context)
		{
			yield break;
		}

		private IAnswer ProcessChildAnswers(IQuestionProcessingContext<StatementQuestion<StatementT>> context, ICollection<StatementT> statements, IDictionary<IAnswer, ICollection<IStatement>> childAnswers)
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

		public static IAnswer CreateCommonConceptsAnswer(
			IQuestionProcessingContext<StatementQuestion<StatementT>> context,
			ICollection<StatementT> statements,
			Func<StatementT, IConcept> statementConceptSelector,
			Func<StatementQuestion<StatementT>, IConcept> questionConceptSelector,
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

		public static IAnswer CreateCommonBooleanAnswer(
			IQuestionProcessingContext<StatementQuestion<StatementT>> context,
			ICollection<StatementT> statements,
			Boolean success,
			Func<ILanguageAnswers, String> trueAnswerFormatSelector,
			Func<ILanguageAnswers, String> falseAnswerFormatSelector,
			Func<StatementQuestion<StatementT>, Dictionary<String, INamed>> questionConceptsSelector)
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
