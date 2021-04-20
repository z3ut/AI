namespace Inventor.Core
{
	public interface IQuestion
	{
		IAnswer Ask(IKnowledgeBaseContext knowledgeBaseContext);
	}
}
