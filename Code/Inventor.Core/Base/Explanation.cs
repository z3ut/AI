using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Base
{
	public class Explanation : IExplanation
	{
		#region Properties

		public ICollection<IStatement> Statements
		{ get; private set; }

		#endregion

		public Explanation(IEnumerable<IStatement> statements)
		{
			Statements = statements.ToArray();
		}

		public Explanation(IStatement statement)
			: this(new[] { statement })
		{ }

		public void Expand(IEnumerable<IStatement> statements)
		{
			var list = new List<IStatement>(Statements);
			list.AddRange(statements);
			Statements = list;
		}
	}
}
