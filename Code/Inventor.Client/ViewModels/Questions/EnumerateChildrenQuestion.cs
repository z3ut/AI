namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateChildrenQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.IsStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.IsStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.IsStatement>(Concept);
		}
	}
}
