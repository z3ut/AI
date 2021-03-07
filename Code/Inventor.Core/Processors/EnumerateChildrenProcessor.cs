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
	public sealed class EnumerateChildrenProcessor : QuestionProcessor<EnumerateChildrenQuestion, HasPartStatement>
	{
		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<EnumerateChildrenQuestion> context, HasPartStatement statement)
		{
			
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateChildrenQuestion> context, ICollection<HasPartStatement> statements)
		{
			
		}

		protected override IEnumerable<Tuple<IQuestion, ICollection<IStatement>>> GetNestedQuestions(IQuestionProcessingContext<EnumerateChildrenQuestion> context)
		{
			foreach (var  in context.KnowledgeBase.)
			{
				yield return ;
			}
		}

		public override IAnswer Process(IQuestionProcessingContext<EnumerateChildrenQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(activeContexts).Where(c => c.Ancestor == question.Concept).ToList();
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Descendant).ToList().Enumerate(out format);
				parameters.Add(Strings.ParamParent, question.Concept);
				return new ConceptsAnswer(
					statements.Select(s => s.Descendant).ToList(),
					new FormattedText(() => context.Language.Answers.Enumerate + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}
	}
}
