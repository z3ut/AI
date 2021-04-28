namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateContainersQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.HasPartStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Part == Concept;
				},
				createAnswer: (context, statements) =>
				{
					return Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>.CreateCommonConceptsAnswer(
						context,
						statements,
						r => r.Whole,
						q => Concept,
						a => a.EnumerateContainers);
				});
		}
	}
}
