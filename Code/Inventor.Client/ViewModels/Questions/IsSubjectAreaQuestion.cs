using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsSubjectAreaQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.GroupStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamArea")]
		public Core.IConcept Area
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.GroupStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.GroupStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Area == Area && statement.Concept == Concept;
				},
				createAnswer: (context, statements) =>
				{
					return Core.Questions.StatementQuestion<Core.Statements.GroupStatement>.CreateCommonBooleanAnswer(
						context,
						statements,
						statements.Any(),
						a => a.IsSubjectAreaTrue,
						a => a.IsSubjectAreaFalse,
						q => new Dictionary<String, Core.INamed>
						{
							{ Core.Localization.Strings.ParamArea, Area },
							{ Core.Localization.Strings.ParamConcept, Concept },
						});
				});
		}
	}
}
