using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public sealed class IsQuestion : StatementQuestion<IsQuestion, IsStatement>
	{
		#region Properties

		public IConcept Child
		{ get; }

		public IConcept Parent
		{ get; }

		#endregion

		public IsQuestion(IConcept child, IConcept parent)
			: base(DoesStatementMatch, CreateAnswer, getNestedQuestions: GetNestedQuestions)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<IsQuestion> context, ICollection<IsStatement> statements)
		{
			Boolean yes = statements.Any();
			return new BooleanAnswer(
				yes,
				new FormattedText(
					yes ? new Func<String>(() => context.Language.Answers.IsTrue) : () => context.Language.Answers.IsFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamParent, context.Question.Child },
						{ Strings.ParamChild, context.Question.Parent },
					}),
				new Explanation(statements));
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<IsQuestion> context, IsStatement statement)
		{
			return statement.Parent == context.Question.Parent && statement.Child == context.Question.Child;
		}

		private static IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<IsQuestion> context)
		{
			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<IsQuestion>>().Select(questionContext => questionContext.Question.Child));

			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == context.Question.Child);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new IsQuestion(parent, context.Question.Parent), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
