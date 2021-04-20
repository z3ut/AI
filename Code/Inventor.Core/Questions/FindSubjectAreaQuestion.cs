using System;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class FindSubjectAreaQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public FindSubjectAreaQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
