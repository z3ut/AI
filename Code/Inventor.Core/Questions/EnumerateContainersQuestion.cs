using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
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
			if (statements.Any())
			{
				var concepts = statements.Select(r => r.Whole).ToList();

				String format;
				var parameters = concepts.Enumerate(out format);
				parameters.Add(Strings.ParamChild, context.Question.Concept);
				return new ConceptsAnswer(
					concepts,
					new FormattedText(() => context.Language.Answers.EnumerateContainers + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<EnumerateContainersQuestion> context, HasPartStatement statement)
		{
			return statement.Part == context.Question.Concept;
		}
	}
}
