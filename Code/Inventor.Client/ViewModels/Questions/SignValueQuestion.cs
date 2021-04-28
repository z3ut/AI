using System.Collections.Generic;
using System.Linq;

namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class SignValueQuestion : QuestionViewModel<Core.Questions.StatementQuestion<Core.Statements.SignValueStatement>>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamSign")]
		public Core.IConcept Sign
		{ get; set; }

		public override Core.Questions.StatementQuestion<Core.Statements.SignValueStatement> BuildQuestion()
		{
			return new Core.Questions.StatementQuestion<Core.Statements.SignValueStatement>(
				doesStatementMatch: (context, statement) =>
				{
					return statement.Concept == Concept && statement.Sign == Sign;
				},
				createAnswer: (context, statements) =>
				{
					if (statements.Any())
					{
						var statement = statements.First();
						return new Core.Answers.ConceptAnswer(
							statement.Value,
							formatSignValue(statement, Concept, context.Language),
							new Core.Base.Explanation(statements));
					}
					else
					{
						return Core.Answers.Answer.CreateUnknown(context.Language);
					}
				},
				getNestedQuestions: context =>
				{
					var alreadyViewedConcepts = new HashSet<Core.IConcept>(context.ActiveContexts.OfType<Core.IQuestionProcessingContext<Core.Questions.SignValueQuestion>>().Select(questionContext => Concept));

					var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<Core.Statements.IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == Concept);

					foreach (var transitiveStatement in transitiveStatements)
					{
						var parent = transitiveStatement.Parent;
						if (!alreadyViewedConcepts.Contains(parent))
						{
							yield return new Core.Questions.NestedQuestion(new SignValueQuestion(parent, Sign), new Core.IStatement[] { transitiveStatement });
						}
					}
				});
		}

		private static Core.FormattedText formatSignValue(Core.Statements.SignValueStatement value, Core.IConcept original, Core.ILanguage language)
		{
			return value != null
				? new Core.FormattedText(
					() => language.Answers.SignValue,
					new Dictionary<System.String, Core.INamed>
					{
						{ Core.Localization.Strings.ParamConcept, original },
						{ Core.Localization.Strings.ParamSign, value.Sign },
						{ Core.Localization.Strings.ParamValue, value.Value },
						{ Core.Localization.Strings.ParamDefined, value.Concept },
					})
				: null;
		}
	}
}
