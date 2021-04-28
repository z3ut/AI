namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class HasSignsQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.HasSignStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "QuestionNames.ParamRecursive")]
		public bool Recursive
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.HasSignStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.HasSignStatement>(
				//Concept, Recursive);
				doesStatementMatch: (context, statement) =>
				{
					1
				},
				createAnswer: (context, statements) =>
				{
					2
				},
				areEnoughToAnswer: (context, statements) =>
				{
					3
				},
				getNestedQuestions: context =>
				{
					4
				});
		}
	}
}
