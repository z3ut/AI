using System;

using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public sealed class CheckStatementQuestion : Question
	{
		#region Properties

		public IStatement Statement
		{ get; }

		#endregion

		public CheckStatementQuestion(IStatement statement)
		{
			if (statement == null) throw new ArgumentNullException(nameof(statement));

			Statement = statement;
		}
	}
}
