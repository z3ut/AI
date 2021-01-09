﻿using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processing
{
	public sealed class EnumerateChildrenProcessor : QuestionProcessor<EnumerateChildrenQuestion>
	{
		protected override Answer ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, EnumerateChildrenQuestion question, ILanguage language)
		{
			var statements = knowledgeBase.Statements.OfType<IsStatement>().Where(c => c.Parent == question.Concept).ToList();
			if (statements.Any())
			{
				string format;
				var parameters = statements.Select(r => r.Child).ToList().Enumerate(out format);
				parameters.Add("#PARENT#", question.Concept);
				return new Answer(
					statements.Select(s => s.Child),
					new FormattedText(() => language.Answers.Enumerate + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(language);
			}
		}
	}
}
