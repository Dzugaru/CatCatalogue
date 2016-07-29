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
        ObservableCollection<object> testList;

        public string TestText
        {
            get { return testText; }
            set { SetProperty(ref testText, value); }
        }

        public ICollectionView TestList { get; private set; }

        public DelegateCommand TestCommand { get; private set; }

        public ViewModel()
        {
            var vendor1 = new Vendor("Тимотеи");
            vendor1.products.Add(new Product() { Name = "Шампунь", Rating = 10 });

            testList = new ObservableCollection<object>(new List<object>()
            {
                "A",
                "Б",
                vendor1
            });    
            TestList = new ListCollectionView(testList);

            TestCommand = new DelegateCommand(() => TestText = "Bla");
        }
    }

    class Vendor : BindableBase
    {
        string name;

        public ObservableCollection<Product> products;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public ICollectionView Products { get; private set; }
        
        public Vendor() {}

        public Vendor(string name)
        {
            this.name = name;
            this.products = new ObservableCollection<Product>();
            Products = new ListCollectionView(this.products); //TODO: what if after deserialize?
        }        
    }

    class Product : BindableBase
    {
        string name;
        int rating;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public int Rating
        {
            get { return rating; }
            set { SetProperty(ref rating, value); }
        }

        public Product()
        {

        }
    }

    class TestTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            string resourceName;
            if (item is Vendor)
                resourceName = "VendorInfo";
            else if (item is string)
                resourceName = "CapitalLetter";
            else
                throw new NotImplementedException();

            return (DataTemplate)((FrameworkElement)container).FindResource(resourceName);
        }
    }
}
