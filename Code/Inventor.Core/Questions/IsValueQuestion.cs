using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class IsValueQuestion : StatementQuestion<IsValueQuestion, SignValueStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public IsValueQuestion(IConcept concept)
			: base(DoesStatementMatch, CreateAnswer)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<IsValueQuestion> context, ICollection<SignValueStatement> statements)
		{
			return CreateCommonBooleanAnswer(
				context,
				statements,
				context.Question.Concept.HasAttribute<IsValueAttribute>(),
				a => a.ValueTrue,
				a => a.ValueFalse,
				q => new Dictionary<String, INamed>
				{
					{ Strings.ParamConcept, context.Question.Concept },
				});
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<IsValueQuestion> context, SignValueStatement statement)
		{
			return statement.Value == context.Question.Concept;
		}
	}
}
