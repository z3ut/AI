using System;
using System.Collections.Generic;

using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
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
			bool isSign = context.Question.Concept.HasAttribute<IsSignAttribute>();
			return new BooleanAnswer(
				isSign,
				new FormattedText(
					isSign ? new Func<String>(() => context.Language.Answers.SignTrue) : () => context.Language.Answers.SignFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, context.Question.Concept },
					}),
				new Explanation(statements));
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<IsSignQuestion> context, HasSignStatement statement)
		{
			return statement.Sign == context.Question.Concept;
		}
	}
}
