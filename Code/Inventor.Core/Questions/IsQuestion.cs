﻿using System;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class IsQuestion : Question
	{
		public IConcept Child
		{ get; }

		public IConcept Parent
		{ get; }

		public IsQuestion(IConcept child, IConcept parent)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
		}
	}
}
