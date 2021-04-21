using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IQuestion
	{
		ICollection<IStatement> Conditions
		{ get; }

		IAnswer Ask(IKnowledgeBaseContext knowledgeBaseContext);
	}
}
