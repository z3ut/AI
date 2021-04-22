namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsPartOfQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public Core.IConcept Parent
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public Core.IConcept Child
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.HasPartStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>(Child, Parent);
		}
	}
}
