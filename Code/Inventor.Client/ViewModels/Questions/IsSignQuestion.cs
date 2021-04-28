using System;
using System.Collections.Generic;

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
			return new Core.Questions.StatementQuestion<Core.Statements.HasSignStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Sign == Concept;
				},
				createAnswer: (context, statements) =>
				{
					return Core.Questions.StatementQuestion<Core.Statements.HasSignStatement>.CreateCommonBooleanAnswer(
						context,
						statements,
						Concept.HasAttribute<Core.Attributes.IsSignAttribute>(),
						a => a.SignTrue,
						a => a.SignFalse,
						q => new Dictionary<String, Core.INamed>
						{
							{ Core.Localization.Strings.ParamConcept, Concept },
						});
				});
		}
	}
}
