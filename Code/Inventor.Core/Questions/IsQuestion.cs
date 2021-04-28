using System;
using System.Collections.Generic;
using System.Linq;

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
			
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<IsQuestion> context, IsStatement statement)
		{
			
		}

		private static IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<IsQuestion> context)
		{
			
		}
	}
}
