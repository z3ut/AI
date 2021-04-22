namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsSignQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.HasSignStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.HasSignStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.HasSignStatement>(Concept);
		}
	}
}
