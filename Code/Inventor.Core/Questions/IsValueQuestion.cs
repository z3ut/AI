﻿using System;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class IsValueQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public IsValueQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
