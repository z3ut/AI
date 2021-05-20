using System.Collections.Generic;
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
				return createAnswer(statements.First(), context.Language);
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

		private static StatementAnswer createAnswer(ComparisonStatement statement, ILanguage language, ICollection<IStatement> transitiveStatements = null)
		{
			var text = new FormattedText();
			text.Add(statement.DescribeTrue(language));

			var explanation = new Explanation(statement);
			if (transitiveStatements != null)
			{
				explanation.Expand(transitiveStatements);
			}

			return new StatementAnswer(statement, text, explanation);
		}

#warning Проверить на непротеворечивость систем сравнения значений и последовательности процессов.

		protected override IAnswer ProcessChildAnswers(IQuestionProcessingContext<ComparisonQuestion> context, ICollection<ComparisonStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			foreach (var answer in childAnswers)
			{
				var childStatement = (answer.Answer as StatementAnswer)?.Result as ComparisonStatement;
				if (childStatement != null)
				{
					var transitiveStatement = (ComparisonStatement) answer.TransitiveStatements.Single();
					var childStatementValues = new HashSet<IConcept> { childStatement.LeftValue, childStatement.RightValue };
					var transitiveStatementValues = new HashSet<IConcept> { transitiveStatement.LeftValue, transitiveStatement.RightValue };
					var transitiveValue = childStatementValues.Intersect(transitiveStatementValues).Single();
					ComparisonStatement resultStatement = null;

					/*if (childStatement.ComparisonSign == SystemConcepts.IsEqualTo && transitiveStatement.ComparisonSign == SystemConcepts.IsEqualTo)
					{
						resultStatement = new ComparisonStatement(
							context.Question.LeftValue,
							context.Question.RightValue,
							SystemConcepts.IsEqualTo);
					}*/
					if (childStatement.ComparisonSign == SystemConcepts.IsEqualTo)
					{
						resultStatement = new ComparisonStatement(
							transitiveStatement.LeftValue != transitiveValue ? transitiveStatement.LeftValue : ,
							transitiveStatement.RightValue != transitiveValue ? transitiveStatement.RightValue : ,
							transitiveStatement.ComparisonSign);
					}
					else if (transitiveStatement.ComparisonSign == SystemConcepts.IsEqualTo)
					{
						resultStatement = new ComparisonStatement(
							childStatement.LeftValue != transitiveValue ? childStatement.LeftValue : ,
							childStatement.RightValue != transitiveValue ? childStatement.RightValue : ,
							childStatement.ComparisonSign);
					}
					else if ()
					{
						1
					}
					else if ()
					{
						1
					}
					else if ()
					{
						1
					}

					if (resultStatement != null)
					{
						
						return createAnswer(resultStatement, context.Language, answer.TransitiveStatements);
					}
				}
			}

			return Answer.CreateUnknown(context.Language);
		}
	}
}
