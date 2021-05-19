﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateSignsQuestion : StatementQuestion<EnumerateSignsQuestion, HasSignStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		public EnumerateSignsQuestion(IConcept concept, Boolean recursive)
			: base(DoesStatementMatch, CreateAnswer, AreEnoughToAnswer, GetNestedQuestions, ProcessChildAnswers)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
			Recursive = recursive;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateSignsQuestion> context, ICollection<HasSignStatement> statements)
		{
			if (statements.Any())
			{
				return formatAnswer(context, statements.Select(hs => hs.Sign).ToList(), statements.OfType<IStatement>().ToList());
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<EnumerateSignsQuestion> context, HasSignStatement statement)
		{
			return statement.Concept == context.Question.Concept;
		}

		private static Boolean AreEnoughToAnswer(IQuestionProcessingContext<EnumerateSignsQuestion> context, ICollection<HasSignStatement> statements)
		{
			return !context.Question.Recursive;
		}

		private static IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<EnumerateSignsQuestion> context)
		{
			if (!context.Question.Recursive) yield break;

			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<EnumerateSignsQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == context.Question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new EnumerateSignsQuestion(parent, true), new IStatement[] { transitiveStatement });
				}
			}
		}

		private static IAnswer ProcessChildAnswers(IQuestionProcessingContext<EnumerateSignsQuestion> context, ICollection<HasSignStatement> statements, IDictionary<IAnswer, ICollection<IStatement>> answers)
		{
			var allSigns = new HashSet<IConcept>();
			var allStatements = new HashSet<IStatement>();

			foreach (var statement in statements)
			{
				allSigns.Add(statement.Sign);
				allStatements.Add(statement);
			}

			foreach (var answer in answers)
			{
				var conceptsAnswer = answer.Key as ConceptsAnswer;
				if (conceptsAnswer != null)
				{
					foreach (var sign in conceptsAnswer.Result)
					{
						allSigns.Add(sign);
					}
					foreach (var statement in conceptsAnswer.Explanation.Statements)
					{
						allStatements.Add(statement);
					}
					foreach (var statement in answer.Value)
					{
						allStatements.Add(statement);
					}
				}
			}

			return allSigns.Count > 0
				? formatAnswer(context, allSigns, allStatements)
				: Answer.CreateUnknown(context.Language);
		}

		private static IAnswer formatAnswer(IQuestionProcessingContext<EnumerateSignsQuestion> context, ICollection<IConcept> signs, ICollection<IStatement> statements)
		{
			String format;
			var parameters = signs.Enumerate(out format);
			parameters[Strings.ParamConcept] = context.Question.Concept;
			return new ConceptsAnswer(
				signs,
				new FormattedText(
					() => string.Format(context.Language.Answers.ConceptSigns, context.Question.Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse, format),
					parameters),
				new Explanation(statements));
		}
	}
}
