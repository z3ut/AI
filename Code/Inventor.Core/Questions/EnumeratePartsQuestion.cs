using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class EnumeratePartsQuestion : StatementQuestion<EnumeratePartsQuestion, HasPartStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumeratePartsQuestion(IConcept concept)
			: base(DoesStatementMatch, CreateAnswer)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<EnumeratePartsQuestion> context, ICollection<HasPartStatement> statements)
		{
			return CreateCommonConceptsAnswer(
				context,
				statements,
				r => r.Part,
				q => q.Concept,
				a => a.EnumerateParts);
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<EnumeratePartsQuestion> context, HasPartStatement statement)
		{
			return statement.Whole == context.Question.Concept;
		}
	}
}
