using System;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateChildrenQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateChildrenQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
