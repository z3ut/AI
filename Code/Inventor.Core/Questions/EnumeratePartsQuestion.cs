using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
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
			if (statements.Any())
			{
				var concepts = statements.Select(r => r.Part).ToList();

				String format;
				var parameters = concepts.Enumerate(out format);
				parameters.Add(Strings.ParamParent, context.Question.Concept);
				return new ConceptsAnswer(
					concepts,
					new FormattedText(() => context.Language.Answers.EnumerateParts + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<EnumeratePartsQuestion> context, HasPartStatement statement)
		{
			return statement.Whole == context.Question.Concept;
		}
	}
}
