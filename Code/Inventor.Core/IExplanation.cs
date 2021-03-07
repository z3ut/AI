﻿using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IExplanation
	{
		ICollection<IStatement> Statements
		{ get; }

		void Expand(IEnumerable<IStatement> statements);
	}
}
