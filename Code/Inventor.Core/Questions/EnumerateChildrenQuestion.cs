using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateChildrenQuestion : StatementQuestion<EnumerateChildrenQuestion, IsStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateChildrenQuestion(IConcept concept)
			: base(DoesStatementMatch, CreateAnswer)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateChildrenQuestion> context, ICollection<IsStatement> statements)
		{
			return CreateCommonConceptsAnswer(
				context,
				statements,
				r => r.Descendant,
				q => q.Concept,
				a => a.Enumerate);
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<EnumerateChildrenQuestion> context, IsStatement statement)
		{
			return statement.Ancestor == context.Question.Concept;
		}
	}
}
