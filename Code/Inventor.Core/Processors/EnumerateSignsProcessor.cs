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
	public sealed class EnumerateSignsProcessor : QuestionProcessor<EnumerateSignsQuestion, HasSignStatement>
	{
		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<EnumerateSignsQuestion> context, HasSignStatement statement)
		{
			
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateSignsQuestion> context, ICollection<HasSignStatement> statements)
		{
			
		}

		protected override IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<EnumerateSignsQuestion> context)
		{
			foreach (var  in context.KnowledgeBase.)
			{
				yield return ;
			}
		}

		public override IAnswer Process(IQuestionProcessingContext<EnumerateSignsQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();
			var allStatements = context.KnowledgeBase.Statements.Enumerate(activeContexts);

			var statements = HasSignStatement.GetSigns(allStatements, question.Concept, question.Recursive);
			if (statements.Any())
			{
				var signs = statements.Select(hs => hs.Sign).ToList();
				String format;
				var parameters = signs.Enumerate(out format);
				parameters[Strings.ParamConcept] = question.Concept;
				return new ConceptsAnswer(
					signs,
					new FormattedText(
						() => string.Format(context.Language.Answers.ConceptSigns, question.Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse, format),
						parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}
	}
}
