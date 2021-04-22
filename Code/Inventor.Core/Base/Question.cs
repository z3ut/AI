using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public abstract class Question : IQuestion
	{
		public ICollection<IStatement> Conditions
		{ get; } = new List<IStatement>();

		public abstract IAnswer Ask(IKnowledgeBaseContext knowledgeBaseContext);
	}

	public abstract class Question<QuestionT> : Question
		where QuestionT : Question<QuestionT>
	{
		public override IAnswer Ask(IKnowledgeBaseContext knowledgeBaseContext)
		{
			using (var questionContext = (IQuestionProcessingContext<QuestionT>) knowledgeBaseContext.CreateQuestionContext(this))
			{
				foreach (var statement in Conditions)
				{
					statement.Context = questionContext;
					questionContext.KnowledgeBase.Statements.Add(statement);
				}

				return Process(questionContext);
			}
		}

		protected abstract IAnswer Process(IQuestionProcessingContext<QuestionT> context);
	}
}
