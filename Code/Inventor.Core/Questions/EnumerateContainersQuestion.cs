using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateContainersQuestion : StatementQuestion<EnumerateContainersQuestion, HasPartStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateContainersQuestion(IConcept concept)
			: base(DoesStatementMatch, CreateAnswer)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateContainersQuestion> context, ICollection<HasPartStatement> statements)
		{
			return CreateCommonConceptsAnswer(
				context,
				statements,
				r => r.Whole,
				q => q.Concept,
				a => a.EnumerateContainers);
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<EnumerateContainersQuestion> context, HasPartStatement statement)
		{
			return statement.Part == context.Question.Concept;
		}
	}
}
