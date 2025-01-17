﻿using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;

namespace Inventor.Core.Base
{
	public abstract class Statement : IStatement
	{
		#region Properties

		public ILocalizedString Name
		{ get; }

		public IContext Context
		{ get; set; }

		public ILocalizedString Hint
		{ get; }

		#endregion

		public abstract IEnumerable<IConcept> GetChildConcepts();

		public override sealed String ToString()
		{
			return string.Format("{0} \"{1}\"", Strings.TostringStatement, Name);
		}

		protected Statement(ILocalizedString name, ILocalizedString hint = null)
		{
			if (name != null)
			{
				Name = name;
			}
			else
			{
				throw new ArgumentNullException(nameof(name));
			}

			Hint = hint ?? LocalizedString.Empty;
		}

		#region Description

		public FormattedLine DescribeTrue(ILanguage language)
		{
			return new FormattedLine(GetDescriptionText(language.TrueStatementFormatStrings), GetDescriptionParameters());
		}

		public FormattedLine DescribeFalse(ILanguage language)
		{
			return new FormattedLine(GetDescriptionText(language.FalseStatementFormatStrings), GetDescriptionParameters());
		}

		public FormattedLine DescribeQuestion(ILanguage language)
		{
			return new FormattedLine(GetDescriptionText(language.QuestionStatementFormatStrings), GetDescriptionParameters());
		}

		protected abstract Func<String> GetDescriptionText(ILanguageStatements language);

		protected abstract IDictionary<String, INamed> GetDescriptionParameters();

		#endregion

		public abstract Boolean CheckUnique(IEnumerable<IStatement> statements);

#pragma warning disable 659
		public abstract override Boolean Equals(Object obj);

		public override Int32 GetHashCode()
		{
// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
		}
#pragma warning restore 659
	}

	public abstract class Statement<StatementT> : Statement, IEquatable<StatementT>
		where StatementT : Statement<StatementT>
	{
		protected Statement(LocalizedString name, LocalizedString hint = null) : base(name, hint)
		{ }

		public override sealed Boolean CheckUnique(IEnumerable<IStatement> statements)
		{
			return statements.OfType<StatementT>().Count(Equals) == 1;
		}

		public abstract Boolean Equals(StatementT other);

#pragma warning disable 659
		public override sealed Boolean Equals(Object obj)
		{
			return Equals(obj as StatementT);
		}

		public override Int32 GetHashCode()
		{
			// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
		}
#pragma warning restore 659
	}
}
