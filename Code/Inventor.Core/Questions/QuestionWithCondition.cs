﻿using System;
using System.Collections.Generic;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class QuestionWithCondition : Question
	{
		public ICollection<IStatement> Conditions
		{ get; }

		public IQuestion Question
		{ get; }

		public QuestionWithCondition(IEnumerable<IStatement> conditions, IQuestion question)
		{
			if (conditions == null) throw new ArgumentNullException(nameof(conditions));
			if (question == null) throw new ArgumentNullException(nameof(question));

			Conditions = new List<IStatement>(conditions);
			Question = question;
		}
	}
}
