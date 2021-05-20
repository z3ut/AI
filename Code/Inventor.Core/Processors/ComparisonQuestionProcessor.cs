﻿using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class ComparisonQuestionProcessor : QuestionProcessor<ComparisonQuestion, ComparisonStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ComparisonStatement> statements)
		{
			if (statements.Any())
			{
				var statement = statements.First();
				var text = new FormattedText();
				text.Add(statement.DescribeTrue(context.Language));

				return new StatementAnswer(
					statement,
					text,
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		protected override bool DoesStatementMatch(IQuestionProcessingContext<ComparisonQuestion> context, ComparisonStatement statement)
		{
			return	(statement.LeftValue == context.Question.LeftValue && statement.RightValue == context.Question.RightValue) ||
					(statement.RightValue == context.Question.LeftValue && statement.LeftValue == context.Question.RightValue);
		}

		protected override IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<ComparisonQuestion> context)
		{
			foreach (var statement in context.KnowledgeBase.Statements.Enumerate<ComparisonStatement>(context.ActiveContexts))
			{
				if (statement.LeftValue == context.Question.LeftValue)
				{
					yield return new NestedQuestion(new ComparisonQuestion(statement.RightValue, context.Question.RightValue), new IStatement[] { statement });
				}
				else if (statement.LeftValue == context.Question.RightValue)
				{
					yield return new NestedQuestion(new ComparisonQuestion(statement.RightValue, context.Question.LeftValue), new IStatement[] { statement });
				}
				else if (statement.RightValue == context.Question.LeftValue)
				{
					yield return new NestedQuestion(new ComparisonQuestion(statement.LeftValue, context.Question.RightValue), new IStatement[] { statement });
				}
				else if (statement.RightValue == context.Question.RightValue)
				{
					yield return new NestedQuestion(new ComparisonQuestion(statement.LeftValue, context.Question.LeftValue), new IStatement[] { statement });
				}
			}
		}

		protected override IAnswer ProcessChildAnswers(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ComparisonStatement> statements, IDictionary<IAnswer, ICollection<IStatement>> childAnswers)
		{
			foreach (var answer in childAnswers)
			{
				if ()
				{
					answer.Key.Explanation.Expand(answer.Value);
					return answer.Key;
				}
			}

			return Answer.CreateUnknown(context.Language);
		}
	}
}