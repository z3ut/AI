using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inventor.Core.Base
{
	public class QuestionRepository : IQuestionRepository
	{
		public IDictionary<Type, QuestionDefinition> QuestionDefinitions
		{ get; }

		public QuestionRepository()
		{
			QuestionDefinitions = new Dictionary<Type, QuestionDefinition>();

			foreach (var questionType in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IQuestion).IsAssignableFrom(t) && !t.IsAbstract))
			{
				DefineQuestion(new QuestionDefinition(questionType));
			}
		}

		public void DefineQuestion(QuestionDefinition questionDefinition)
		{
			QuestionDefinitions[questionDefinition.QuestionType] = questionDefinition;
		}
	}
}
