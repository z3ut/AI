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
	public sealed class IsPartOfQuestion : StatementQuestion<IsPartOfQuestion, HasPartStatement>
	{
		#region Properties

		public IConcept Parent
		{ get; }

		public IConcept Child
		{ get; }

		#endregion

		public IsPartOfQuestion(IConcept child, IConcept parent)
			: base(DoesStatementMatch, CreateAnswer)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<IsPartOfQuestion> context, ICollection<HasPartStatement> statements)
		{
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(statements.Any() ? new Func<String>(() => context.Language.Answers.IsPartOfTrue) : () => context.Language.Answers.IsPartOfFalse, new Dictionary<String, INamed>
				{
					{ Strings.ParamParent, context.Question.Parent },
					{ Strings.ParamChild, context.Question.Child },
				}),
				new Explanation(statements));
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<IsPartOfQuestion> context, HasPartStatement statement)
		{
			return statement.Whole == context.Question.Parent && statement.Part == context.Question.Child;
		}
	}
}
