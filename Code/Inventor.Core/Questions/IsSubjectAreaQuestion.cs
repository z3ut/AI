﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public sealed class IsSubjectAreaQuestion : StatementQuestion<IsSubjectAreaQuestion, GroupStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public IConcept Area
		{ get; }

		#endregion

		public IsSubjectAreaQuestion(IConcept concept, IConcept area)
			: base(DoesStatementMatch, CreateAnswer)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (area == null) throw new ArgumentNullException(nameof(area));

			Concept = concept;
			Area = area;
		}

		private static IAnswer CreateAnswer(IQuestionProcessingContext<IsSubjectAreaQuestion> context, ICollection<GroupStatement> statements)
		{
			return CreateCommonBooleanAnswer(
				context,
				statements,
				statements.Any(),
				a => a.IsSubjectAreaTrue,
				a => a.IsSubjectAreaFalse,
				q => new Dictionary<String, INamed>
				{
					{ Strings.ParamArea, context.Question.Area },
					{ Strings.ParamConcept, context.Question.Concept },
				});
		}

		private static Boolean DoesStatementMatch(IQuestionProcessingContext<IsSubjectAreaQuestion> context, GroupStatement statement)
		{
			return statement.Area == context.Question.Area && statement.Concept == context.Question.Concept;
		}
	}
}
