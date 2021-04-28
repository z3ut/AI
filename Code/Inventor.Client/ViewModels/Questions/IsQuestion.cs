using System;
using System.Collections.Generic;
using System.Linq;
//using Inventor.Core;
//using Inventor.Core.Localization;
//using Inventor.Core.Questions;
//using Inventor.Core.Statements;

namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.IsStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public Core.IConcept Child
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public Core.IConcept Parent
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.IsStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.IsStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Parent == Parent && statement.Child == Child;
				},
				createAnswer: (context, statements) =>
				{
					return Core.Questions.StatementQuestion<Core.Statements.IsStatement>.CreateCommonBooleanAnswer(
						context,
						statements,
						statements.Any(),
						a => a.IsTrue,
						a => a.IsFalse,
						q => new Dictionary<String, Core.INamed>
						{
							{ Core.Localization.Strings.ParamParent, Child },
							{ Core.Localization.Strings.ParamChild, Parent },
						});
				},
				getNestedQuestions: context =>
				{
					var alreadyViewedConcepts = new HashSet<Core.IConcept>(context.ActiveContexts.OfType<Core.IQuestionProcessingContext<Core.Questions.IsQuestion>>().Select(questionContext => questionContext.Question.Child));

					var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<Core.Statements.IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == Child);

					foreach (var transitiveStatement in transitiveStatements)
					{
						var parent = transitiveStatement.Parent;
						if (!alreadyViewedConcepts.Contains(parent))
						{
							yield return new Core.Questions.NestedQuestion(new Core.Questions.IsQuestion(parent, Parent), new Core.IStatement[] { transitiveStatement });
						}
					}
				});
		}
	}
}
