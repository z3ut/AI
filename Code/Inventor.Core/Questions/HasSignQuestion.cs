using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class HasSignQuestion : StatementQuestion<HasSignQuestion, HasSignStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public IConcept Sign
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		public HasSignQuestion(IConcept concept, IConcept sign, Boolean recursive)
			: base(DoesStatementMatch, CreateAnswer, AreEnoughToAnswer, GetNestedQuestions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (sign == null) throw new ArgumentNullException(nameof(sign));

			Concept = concept;
			Sign = sign;
			Recursive = recursive;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<HasSignQuestion> context, ICollection<HasSignStatement> statements)
		{
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(
					() => String.Format(statements.Any() ? context.Language.Answers.HasSignTrue : context.Language.Answers.HasSignFalse, context.Question.Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse),
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, context.Question.Concept },
						{ Strings.ParamSign, context.Question.Sign },
					}),
				new Explanation(statements));
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<HasSignQuestion> context, HasSignStatement statement)
		{
			return statement.Sign == context.Question.Sign;
		}

		private static Boolean AreEnoughToAnswer(IQuestionProcessingContext<HasSignQuestion> context, ICollection<HasSignStatement> statements)
		{
			return !context.Question.Recursive;
		}

		private static IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<HasSignQuestion> context)
		{
			if (!context.Question.Recursive) yield break;

			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<HasSignQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == context.Question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new HasSignQuestion(parent, context.Question.Sign, true), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
