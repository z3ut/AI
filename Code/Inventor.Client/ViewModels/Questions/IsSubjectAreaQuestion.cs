namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsSubjectAreaQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.GroupStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamArea")]
		public Core.IConcept Area
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.GroupStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.GroupStatement>(Concept, Area);
		}
	}
}
