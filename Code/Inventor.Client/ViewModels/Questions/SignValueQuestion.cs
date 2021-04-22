namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class SignValueQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.SignValueStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamSign")]
		public Core.IConcept Sign
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.SignValueStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.SignValueStatement>(Concept, Sign);
		}
	}
}
