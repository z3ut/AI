namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class EnumeratePartsQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.HasPartStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>(Concept);
		}
	}
}
