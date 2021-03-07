﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class EnumeratePartsProcessor : QuestionProcessor<EnumeratePartsQuestion, HasPartStatement>
	{
		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<EnumeratePartsQuestion> context, HasPartStatement statement)
		{
			return statement.Whole == context.Question.Concept;
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<EnumeratePartsQuestion> context, ICollection<HasPartStatement> statements)
		{
			String format;
			var parameters = statements.Select(r => r.Part).ToList().Enumerate(out format);
			parameters.Add(Strings.ParamParent, context.Question.Concept);
			return new ConceptsAnswer(
				statements.Select(s => s.Part).ToList(),
				new FormattedText(() => context.Language.Answers.EnumerateParts + format + ".", parameters),
				new Explanation(statements));
		}
	}
}
