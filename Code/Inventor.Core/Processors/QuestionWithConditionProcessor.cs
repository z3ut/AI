using System;
using System.Collections.Generic;

using Inventor.Core.Base;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class QuestionWithConditionProcessor<StatementT> : QuestionProcessor<QuestionWithCondition, StatementT>
		where StatementT : IStatement
	{
		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<QuestionWithCondition> context, StatementT statement)
		{
			
		}

		protected override Boolean AreEnoughToAnswer(IQuestionProcessingContext<QuestionWithCondition> context, ICollection<StatementT> statements)
		{
			
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<QuestionWithCondition> context, ICollection<StatementT> statements)
		{
			
		}

		protected override IEnumerable<Tuple<IQuestion, ICollection<IStatement>>> GetNestedQuestions(IQuestionProcessingContext<QuestionWithCondition> context)
		{
			foreach (var  in context.KnowledgeBase.)
			{
				yield return ;
			}
		}

		public override IAnswer Process(IQuestionProcessingContext<QuestionWithCondition> context)
		{
			var question = context.Question;
			return question.Question.Ask(context, question.Conditions);
		}
	}
}
