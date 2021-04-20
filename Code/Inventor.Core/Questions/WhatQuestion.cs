using System;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class WhatQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public WhatQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
