using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class DescribeSubjectAreaQuestion : StatementQuestion<DescribeSubjectAreaQuestion, GroupStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public DescribeSubjectAreaQuestion(IConcept concept)
			: base(DoesStatementMatch, CreateAnswer)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<DescribeSubjectAreaQuestion> context, ICollection<GroupStatement> statements)
		{
			return CreateCommonConceptsAnswer(
				context,
				statements,
				r => r.Concept,
				q => q.Concept,
				a => a.SubjectAreaConcepts);
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<DescribeSubjectAreaQuestion> context, GroupStatement statement)
		{
			return statement.Area == context.Question.Concept;
		}
	}
}
