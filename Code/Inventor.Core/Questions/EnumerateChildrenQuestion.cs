using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
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
			if (statements.Any())
			{
				var concepts = statements.Select(r => r.Descendant).ToList();

				String format;
				var parameters = concepts.Enumerate(out format);
				parameters.Add(Strings.ParamParent, context.Question.Concept);
				return new ConceptsAnswer(
					concepts,
					new FormattedText(() => context.Language.Answers.Enumerate + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<EnumerateChildrenQuestion> context, IsStatement statement)
		{
			return statement.Ancestor == context.Question.Concept;
		}
	}
}
