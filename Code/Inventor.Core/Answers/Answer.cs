﻿using System;
using System.Collections.Generic;

using Inventor.Core.Base;

namespace Inventor.Core.Answers
{
	public class Answer : IAnswer
	{
		#region Properties

		public FormattedText Description
		{ get; }

		public IExplanation Explanation
		{ get; }

		public Boolean IsEmpty
		{ get; }

		#endregion

		public Answer(FormattedText description, IExplanation explanation, Boolean isEmpty = false)
		{
			Description = description;
			Explanation = explanation;
			IsEmpty = isEmpty;
		}

		public static IAnswer CreateUnknown(ILanguage language)
		{
			return new Answer(
				new FormattedText(() => language.Answers.Unknown, new Dictionary<String, INamed>()),
				new Explanation(new IStatement[0]),
				true);
		}
	}
}
