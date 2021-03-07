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
			
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<EnumeratePartsQuestion> context, ICollection<HasPartStatement> statements)
		{
			
		}

		protected override IEnumerable<Tuple<IQuestion, ICollection<IStatement>>> GetNestedQuestions(IQuestionProcessingContext<EnumeratePartsQuestion> context)
		{
			foreach (var  in context.KnowledgeBase.)
			{
				yield return ;
			}
		}

		public override IAnswer Process(IQuestionProcessingContext<EnumeratePartsQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var statements = context.KnowledgeBase.Statements.Enumerate<HasPartStatement>(activeContexts).Where(c => c.Whole == question.Concept).ToList();
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Part).ToList().Enumerate(out format);
				parameters.Add(Strings.ParamParent, question.Concept);
				return new ConceptsAnswer(
					statements.Select(s => s.Part).ToList(),
					new FormattedText(() => context.Language.Answers.EnumerateParts + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}
	}
}
