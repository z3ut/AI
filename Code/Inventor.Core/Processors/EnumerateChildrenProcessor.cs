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
	public sealed class EnumerateChildrenProcessor : QuestionProcessor<EnumerateChildrenQuestion, IsStatement>
	{
		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<EnumerateChildrenQuestion> context, IsStatement statement)
		{
			return statement.Ancestor == context.Question.Concept;
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateChildrenQuestion> context, ICollection<IsStatement> statements)
		{
			String format;
			var parameters = statements.Select(r => r.Descendant).ToList().Enumerate(out format);
			parameters.Add(Strings.ParamParent, context.Question.Concept);
			return new ConceptsAnswer(
				statements.Select(s => s.Descendant).ToList(),
				new FormattedText(() => context.Language.Answers.Enumerate + format + ".", parameters),
				new Explanation(statements));
		}
	}
}
