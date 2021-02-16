using System;
using System.Collections.Generic;

using Inventor.Core.Base;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class QuestionWithConditionProcessor<StatementT> : QuestionProcessor<QuestionWithCondition, StatementT>
		where StatementT : IStatement
	{
		protected override Boolean DoesStatementMatch(StatementT statement)
		{
			
		}

		protected override Boolean AreEnoughToAnswer(IEnumerable<StatementT> statements)
		{
			
		}

		protected override IAnswer CreateAnswer(ICollection<StatementT> statements)
		{
			
		}

		public override IAnswer Process(IQuestionProcessingContext<QuestionWithCondition> context)
		{
			var question = context.Question;
			return question.Question.Ask(context, question.Conditions);
		}
	}
}
