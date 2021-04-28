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
			return new Core.Questions.StatementQuestion<Core.Statements.IsStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Ancestor == Concept;
				},
				createAnswer: (context, statements) =>
				{
					return Core.Questions.StatementQuestion<Core.Statements.IsStatement>.CreateCommonConceptsAnswer(
						context,
						statements,
						r => r.Descendant,
						q => Concept,
						a => a.Enumerate);
				});
		}
	}
}
