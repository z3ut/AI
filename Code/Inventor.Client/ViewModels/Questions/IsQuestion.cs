namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.IsStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public Core.IConcept Child
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public Core.IConcept Parent
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.IsStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.IsStatement>(Child, Parent);
		}
	}
}
