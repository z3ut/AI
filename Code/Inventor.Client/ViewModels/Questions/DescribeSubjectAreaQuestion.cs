namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class DescribeSubjectAreaQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.GroupStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.GroupStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.GroupStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Area == Concept;
				},
				createAnswer: (context, statements) =>
				{
					return Core.Questions.StatementQuestion<Core.Statements.GroupStatement>.CreateCommonConceptsAnswer(
						context,
						statements,
						r => r.Concept,
						q => Concept,
						a => a.SubjectAreaConcepts);
				});
		}
	}
}
