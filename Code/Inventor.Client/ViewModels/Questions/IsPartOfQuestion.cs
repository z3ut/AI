using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsPartOfQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public Core.IConcept Parent
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public Core.IConcept Child
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.HasPartStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Whole == Parent && statement.Part == Child;
				},
				createAnswer: (context, statements) =>
				{
					return Core.Questions.StatementQuestion<Core.Statements.HasPartStatement>.CreateCommonBooleanAnswer(
						context,
						statements,
						statements.Any(),
						a => a.IsPartOfTrue,
						a => a.IsPartOfFalse,
						q => new Dictionary<String, Core.INamed>
						{
							{ Core.Localization.Strings.ParamParent, Parent },
							{ Core.Localization.Strings.ParamChild, Child },
						});
				});
		}
	}
}
