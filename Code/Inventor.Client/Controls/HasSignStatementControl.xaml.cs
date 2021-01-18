﻿using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class HasSignStatementControl
	{
		public HasSignStatementControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var wrappedConcepts = knowledgeBase.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxConcept.ItemsSource = wrappedConcepts;
			_comboBoxSign.ItemsSource = wrappedConcepts;

			_groupConcept.Header = language.Ui.Editing.PropertyConcept;
			_groupSign.Header = language.Ui.Editing.PropertySign;
		}

		public ViewModels.HasSignStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.HasSignStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
