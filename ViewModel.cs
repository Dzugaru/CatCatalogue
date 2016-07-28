using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;

namespace CatCatalogue
{    
    class ViewModel : BindableBase
    {
        string testText = "Alb";
        ObservableCollection<int> testList;

        public string TestText
        {
            get { return testText; }
            set { SetProperty(ref testText, value); }
        }

        public ICollectionView TestList { get; private set; }

        public DelegateCommand TestCommand { get; private set; }

        public ViewModel()
        {
            testList = new ObservableCollection<int>(new List<int>() { 1, 2, 3, 4, 5 });
            TestList = new ListCollectionView(testList);

            TestCommand = new DelegateCommand(() => TestText = "Bla");
        }
    }

    class TestTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            int x = (int)item;           
            return (DataTemplate)((FrameworkElement)container).FindResource(x >= 3 ? "CompanyInfo" : "CapitalLetter");
        }
    }
}
