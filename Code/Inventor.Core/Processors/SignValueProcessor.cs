﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class SignValueProcessor : QuestionProcessor<SignValueQuestion, SignValueStatement>
	{
		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<SignValueQuestion> context, SignValueStatement statement)
		{
			return statement.Concept == context.Question.Concept && statement.Sign == context.Question.Sign;
		}

		protected override Boolean AreEnoughToAnswer(IQuestionProcessingContext<SignValueQuestion> context, ICollection<SignValueStatement> statements)
		{
			return statements.Count > 0;
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<SignValueQuestion> context, ICollection<SignValueStatement> statements)
		{
			var statement = statements.Single();
			return new ConceptAnswer(statement.Value, description, new Explanation(statement));
		}

		protected override IEnumerable<Tuple<IQuestion, ICollection<IStatement>>> GetNestedQuestions(IQuestionProcessingContext<SignValueQuestion> context)
		{
			foreach (var  in context.KnowledgeBase.)
			{
				yield return ;
			}
		}

		public override IAnswer Process(IQuestionProcessingContext<SignValueQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();
			var allStatements = context.KnowledgeBase.Statements.Enumerate(activeContexts);

			var signValues = allStatements.Enumerate<SignValueStatement>(activeContexts).ToList();
			var statement = getSignValue(signValues, question.Concept, question.Sign);
			FormattedText description = null;
			if (statement != null)
			{
				description = formatSignValue(statement, question.Concept, context.Language);
			}
			else
			{
				var parents = allStatements.GetParentsAllLevels<IConcept, IsStatement>(question.Concept);
				foreach (var parent in parents)
				{
					statement = getSignValue(signValues, parent, question.Sign);
					if (statement != null)
					{
						description = formatSignValue(statement, question.Concept, context.Language);
						break;
					}
				}
			}
			return description != null
				? new ConceptAnswer(statement.Value, description, new Explanation(statement))
				: Answer.CreateUnknown(context.Language);
		}

		private static SignValueStatement getSignValue(IEnumerable<SignValueStatement> statements, IConcept concept, IConcept sign)
		{
			return statements.FirstOrDefault(v => v.Concept == concept && v.Sign == sign);
		}

		private static FormattedText formatSignValue(SignValueStatement value, IConcept original, ILanguage language)
		{
			return value != null
				? new FormattedText(
					() => language.Answers.SignValue,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, original },
						{ Strings.ParamSign, value.Sign },
						{ Strings.ParamValue, value.Value },
						{ Strings.ParamDefined, value.Concept },
					})
				: null;
		}
	}
}
