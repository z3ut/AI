using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core.Questions
{
	public sealed class CheckStatementQuestion : Question<CheckStatementQuestion>
	{
		#region Properties

		public IStatement Statement
		{ get; }

		#endregion

		public CheckStatementQuestion(IStatement statement)
		{
			if (statement == null) throw new ArgumentNullException(nameof(statement));

			Statement = statement;
		}

		protected override IAnswer Process(IQuestionProcessingContext<CheckStatementQuestion> context)
		{
			var allStatements = context.KnowledgeBase.Statements.Enumerate(context.ActiveContexts);

			IEnumerable<IStatement> statements;
			var parentChild = Statement as IParentChild<IConcept>;
			if (parentChild != null)
			{
				statements = allStatements.FindPath(Statement.GetType(), parentChild.Parent, parentChild.Child);
			}
			else
			{
				var statement = allStatements.FirstOrDefault(p => p.Equals(Statement));
				statements = statement != null ? new[] { statement } : new IStatement[0];
			}

			var result = new FormattedText(
				() => Strings.ParamAnswer,
				new Dictionary<String, INamed> { { Strings.ParamAnswer, statements.Any() ? context.KnowledgeBase.True : context.KnowledgeBase.False } });
			result.Add(statements.Any() ? Statement.DescribeTrue(context.Language) : Statement.DescribeFalse(context.Language));
			return new BooleanAnswer(
				statements.Any(),
				result,
				new Explanation(statements));
		}
	}
}
