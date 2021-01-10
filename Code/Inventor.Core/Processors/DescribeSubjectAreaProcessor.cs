﻿using System;
using System.Linq;

using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class DescribeSubjectAreaProcessor : QuestionProcessor<DescribeSubjectAreaQuestion>
	{
		public override IAnswer Process(IProcessingContext<DescribeSubjectAreaQuestion> context)
		{
			var question = context.QuestionX;
			var statements = context.KnowledgeBase.Statements.OfType<GroupStatement>().Where(c => c.Area == question.Concept).ToList();
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Concept).ToList().Enumerate(out format);
				parameters.Add("#AREA#", question.Concept);
				return new Answer(
					statements.Select(s => s.Concept),
					new FormattedText(() => context.Language.Answers.SubjectAreaConcepts + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}
	}
}