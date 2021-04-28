using System.Collections.Generic;
using System.Linq;

//using Inventor.Core;

namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateSignsQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.HasSignStatement>>
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
				doesStatementMatch: (context, statement) =>
				{
					return statement.Concept == Concept;
				},
				createAnswer: (context, statements) =>
				{
					if (statements.Any())
					{
						return formatAnswer(context, statements.Select(hs => hs.Sign).ToList(), statements.OfType<Core.IStatement>().ToList());
					}
					else
					{
						return Core.Answers.Answer.CreateUnknown(context.Language);
					}
				},
				areEnoughToAnswer: (context, statements) =>
				{
					return !Recursive;
				},
				getNestedQuestions: context =>
				{
					if (!Recursive) yield break;

					var alreadyViewedConcepts = new HashSet<Core.IConcept>(context.ActiveContexts.OfType<Core.IQuestionProcessingContext<EnumerateSignsQuestion>>().Select(questionContext => questionContext.Question.Concept));

					var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<Core.Statements.IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == Concept);

					foreach (var transitiveStatement in transitiveStatements)
					{
						var parent = transitiveStatement.Parent;
						if (!alreadyViewedConcepts.Contains(parent))
						{
							yield return new Core.Questions.NestedQuestion(new EnumerateSignsQuestion(parent, true), new Core.IStatement[] { transitiveStatement });
						}
					}
				});
		}
	}
}
