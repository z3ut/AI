﻿namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class DescribeSubjectAreaQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Concept Concept
		{ get; set; }
	}
}
