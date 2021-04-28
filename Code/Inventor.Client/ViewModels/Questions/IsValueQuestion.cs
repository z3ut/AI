using System;
using System.Collections.Generic;

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
			return new Core.Questions.StatementQuestion<Core.Statements.SignValueStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Value == Concept;
				},
				createAnswer: (context, statements) =>
				{
					return Core.Questions.StatementQuestion<Core.Statements.SignValueStatement>.CreateCommonBooleanAnswer(
						context,
						statements,
						Concept.HasAttribute<Core.Attributes.IsValueAttribute>(),
						a => a.ValueTrue,
						a => a.ValueFalse,
						q => new Dictionary<String, Core.INamed>
						{
							{ Core.Localization.Strings.ParamConcept, Concept },
						});
				});
		}
	}
}
