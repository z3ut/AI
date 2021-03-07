﻿using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public class ProcessesStatement : Statement<ProcessesStatement>
	{
		#region Properties

		public IConcept ProcessA
		{ get; private set; }

		public IConcept ProcessB
		{ get; private set; }
		public IConcept SequenceSign
		{ get; private set; }

		#endregion

		public ProcessesStatement(IConcept processA, IConcept processB, IConcept sequenceSign)
			: base(new Func<ILanguage, String>(language => language.StatementNames.Processes), new Func<ILanguage, String>(language => language.StatementHints.Processes))
		{
			Update(processA, processB, sequenceSign);
		}

		public void Update(IConcept processA, IConcept processB, IConcept sequenceSign)
		{
			if (processA == null) throw new ArgumentNullException(nameof(processA));
			if (processB == null) throw new ArgumentNullException(nameof(processB));
			if (sequenceSign == null) throw new ArgumentNullException(nameof(sequenceSign));
			if (!processA.HasAttribute<IsProcessAttribute>()) throw new ArgumentException("Process A concept has to be marked as IsProcess Attribute.", nameof(processA));
			if (!processB.HasAttribute<IsProcessAttribute>()) throw new ArgumentException("Process B concept has to be marked as IsProcess Attribute.", nameof(processB));
			if (!sequenceSign.HasAttribute<IsSequenceSignAttribute>()) throw new ArgumentException("Sequence Sign concept has to be marked as IsSequenceSign Attribute.", nameof(sequenceSign));

			ProcessA = processA;
			ProcessB = processB;
			SequenceSign = sequenceSign;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return ProcessA;
			yield return ProcessB;
			yield return SequenceSign;
		}

		#region Description

		protected override Func<String> GetDescriptionText(ILanguageStatements language)
		{
			return () => language.Processes;
		}

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ Strings.ParamProcessA, ProcessA },
				{ Strings.ParamProcessB, ProcessB },
				{ Strings.ParamSequenceSign, SequenceSign },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(ProcessesStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.ProcessA == ProcessA &&
						other.ProcessB == ProcessB &&
						other.SequenceSign == SequenceSign;
			}
			else return false;
		}

		#endregion
	}
}
