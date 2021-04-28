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
			return new Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Whole == Concept;
				},
				createAnswer: (context, statements) =>
				{
					return Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>.CreateCommonConceptsAnswer(
						context,
						statements,
						r => r.Part,
						q => Concept,
						a => a.EnumerateParts);
				});
		}
	}
}
