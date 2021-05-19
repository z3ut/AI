﻿using System;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateChildrenQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public EnumerateChildrenQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
