﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class DescribeSubjectAreaQuestion : StatementQuestion<DescribeSubjectAreaQuestion, GroupStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public DescribeSubjectAreaQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<DescribeSubjectAreaQuestion> context, ICollection<GroupStatement> statements)
		{
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Concept).ToList().Enumerate(out format);
				parameters.Add(Strings.ParamAnswer, Concept);
				return new ConceptsAnswer(
					statements.Select(s => s.Concept).ToList(),
					new FormattedText(() => context.Language.Answers.SubjectAreaConcepts + format + ".", parameters),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<DescribeSubjectAreaQuestion> context, GroupStatement statement)
		{
			return statement.Area == Concept;
		}
	}
}
