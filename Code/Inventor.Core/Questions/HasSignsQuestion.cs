using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class HasSignsQuestion : StatementQuestion<HasSignsQuestion, HasSignStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		public HasSignsQuestion(IConcept concept, Boolean recursive)
			: base(DoesStatementMatch, CreateAnswer, AreEnoughToAnswer, GetNestedQuestions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
			Recursive = recursive;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<HasSignsQuestion> context, ICollection<HasSignStatement> statements)
		{
			return CreateCommonBooleanAnswer(
				context,
				statements,
				statements.Any(),
				a => String.Format(a.HasSignsTrue, context.Question.Recursive ? a.RecursiveTrue : a.RecursiveFalse),
				a => String.Format(a.HasSignsFalse, context.Question.Recursive ? a.RecursiveTrue : a.RecursiveFalse),
				q => new Dictionary<String, INamed>
				{
					{ Strings.ParamConcept, context.Question.Concept },
				});
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<HasSignsQuestion> context, HasSignStatement statement)
		{
			return statement.Concept == context.Question.Concept;
		}

		private static Boolean AreEnoughToAnswer(IQuestionProcessingContext<HasSignsQuestion> context, ICollection<HasSignStatement> statements)
		{
			return !context.Question.Recursive;
		}

		private static IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<HasSignsQuestion> context)
		{
			if (!context.Question.Recursive) yield break;

			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<HasSignsQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == context.Question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new HasSignsQuestion(parent, true), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
