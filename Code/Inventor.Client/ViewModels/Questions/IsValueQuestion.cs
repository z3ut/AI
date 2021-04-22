namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsValueQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.SignValueStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.SignValueStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.SignValueStatement>(Concept);
		}
	}
}
