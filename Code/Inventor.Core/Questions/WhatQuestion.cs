﻿using System;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class WhatQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public WhatQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
