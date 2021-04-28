using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class FindSubjectAreaQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.GroupStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.GroupStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.GroupStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Concept == Concept;
				},
				createAnswer: (context, statements) =>
				{
					if (statements.Any())
					{
						var result = new Core.FormattedText();
						foreach (var statement in statements)
						{
							result.Add(() => context.Language.Answers.SubjectArea, new Dictionary<String, Core.INamed>
							{
								{ Core.Localization.Strings.ParamConcept, Concept },
								{ Core.Localization.Strings.ParamArea, statement.Area },
							});
						}
						return new Core.Answers.ConceptsAnswer(
							statements.Select(s => s.Area).ToList(),
							result,
							new Core.Base.Explanation(statements));
					}
					else
					{
						return Core.Answers.Answer.CreateUnknown(context.Language);
					}
				});
		}
	}
}
