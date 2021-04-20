using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Test.Base
{
	public class ContextsTest
	{
		[Test]
		public void UnfinishedContextDisposingFails()
		{
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			var question = new TestQuestionCreateNestedContext(false);
			Assert.DoesNotThrow(() =>
			{
				question.Ask(knowledgeBase.Context);
			});

			question = new TestQuestionCreateNestedContext(true);
			Assert.Throws<InvalidOperationException>(() =>
			{
				question.Ask(knowledgeBase.Context);
			});
		}

		[Test]
		public void ContextDisposingRemovesRelatedKnowledge()
		{
			var language = Language.Default;
			var knowledgeBase = new KnowledgeBase(language);

			new TestQuestionCreateContextKnowledge().Ask(knowledgeBase.Context);

			Assert.IsFalse(knowledgeBase.Statements.Enumerate<TestStatement>().Any());
		}

		private class TestQuestionCreateNestedContext : Question<TestQuestionCreateNestedContext>
		{
			private readonly bool _createNestedContext;

			public TestQuestionCreateNestedContext(bool createNestedContext)
			{
				_createNestedContext = createNestedContext;
			}

			protected override IAnswer Process(IQuestionProcessingContext<TestQuestionCreateNestedContext> context)
			{
				if (_createNestedContext)
				{
					new QuestionProcessingContext<TestQuestionCreateNestedContext>(context, new TestQuestionCreateNestedContext(false));
				}

				return null;
			}
		}

		private class TestQuestionCreateContextKnowledge : Question<TestQuestionCreateContextKnowledge>
		{
			protected override IAnswer Process(IQuestionProcessingContext<TestQuestionCreateContextKnowledge> context)
			{
				IStatement testStatement;
				context.KnowledgeBase.Statements.Add(testStatement = new TestStatement());
				testStatement.Context = context;
				context.Scope.Add(testStatement);

				Assert.IsTrue(context.KnowledgeBase.Statements.Enumerate<TestStatement>(context).Any());

				return null;
			}
		}

		private class TestStatement : IStatement
		{
			public ILocalizedString Name
			{ get; } = null;

			public IContext Context
			{ get; set; }

			public ILocalizedString Hint
			{ get; } = null;

			public IEnumerable<IConcept> GetChildConcepts()
			{
				return new IConcept[0];
			}

			public FormattedLine DescribeTrue(ILanguage language)
			{
				throw new NotSupportedException();
			}

			public FormattedLine DescribeFalse(ILanguage language)
			{
				throw new NotSupportedException();
			}

			public FormattedLine DescribeQuestion(ILanguage language)
			{
				throw new NotSupportedException();
			}

			public Boolean CheckUnique(IEnumerable<IStatement> statements)
			{
				throw new NotSupportedException();
			}
		}
	}
}
