using System;
using System.Collections.Generic;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class QuestionWithCondition : Question<QuestionWithCondition>
	{
		#region Properties

		public ICollection<IStatement> Conditions
		{ get; }

		public IQuestion Question
		{ get; }

		#endregion

		public QuestionWithCondition(IEnumerable<IStatement> conditions, IQuestion question)
		{
			if (conditions == null) throw new ArgumentNullException(nameof(conditions));
			if (question == null) throw new ArgumentNullException(nameof(question));

			Conditions = new List<IStatement>(conditions);
			Question = question;
		}

		protected override IAnswer Process(IQuestionProcessingContext<QuestionWithCondition> context)
		{
			return Question.Ask(context);
		}

		protected override IEnumerable<IStatement> GetPreconditions(IQuestionProcessingContext<QuestionWithCondition> context)
		{
			foreach (var precondition in Conditions)
			{
				yield return precondition;
			}
		}
	}
}
