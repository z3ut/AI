﻿namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class FindSubjectAreaQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Concept Concept
		{ get; set; }
	}
}
