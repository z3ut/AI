using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class IsSignQuestion : StatementQuestion<IsSignQuestion, HasSignStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public IsSignQuestion(IConcept concept)
			: base(DoesStatementMatch, CreateAnswer)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<IsSignQuestion> context, ICollection<HasSignStatement> statements)
		{
			return CreateCommonBooleanAnswer(
				context,
				statements,
				context.Question.Concept.HasAttribute<IsSignAttribute>(),
				a => a.SignTrue,
				a => a.SignFalse,
				q => new Dictionary<String, INamed>
				{
					{ Strings.ParamConcept, context.Question.Concept },
				});
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<IsSignQuestion> context, HasSignStatement statement)
		{
			return statement.Sign == context.Question.Concept;
		}
	}
}
