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
using Prism.Interactivity.InteractionRequest;

namespace CatCatalogue
{    
    class ViewModel : BindableBase
    {  
        ObservableCollection<object> productList;        

        public ICollectionView ProductList { get; private set; }

        public DelegateCommand AddProductCmd { get; private set; }

        public InteractionRequest<INotification> EditProductRequest { get; private set; }

        public ViewModel()
        {
            var vendor1 = new Vendor("Тимотеи");
            vendor1.products.Add(new Product() { Name = "Шампунь", Rating = 10 });

            productList = new ObservableCollection<object>(new List<object>()
            {
                "A",
                "Б",
                vendor1
            });    
            ProductList = new ListCollectionView(productList);

            AddProductCmd = new DelegateCommand(AddProduct);
            EditProductRequest = new InteractionRequest<INotification>();
        }

        private void AddProduct()
        {
            EditProductRequest.Raise(new EditProductNotification("3137"));
        }
    }

    class EditProductNotification : Confirmation
    {
        public string Text;

        public EditProductNotification(string text)
        {
            this.Text = text;
            this.Title = "Редактировать";
        }
    }

    class EditProductViewModel : BindableBase, IInteractionRequestAware
    {
        INotification notification;

        public Action FinishInteraction { get; set; }
        public INotification Notification
        {
            get { return notification; }
            set { notification = value; OnNotificationSet(); }
        }

        string testText;
        public string TestText
        {
            get { return testText; }
            set { SetProperty(ref testText, value); }
        }

        public EditProductViewModel()
        {
            TestText = "BlablablaBla";
        }

        void OnNotificationSet()
        {
            EditProductNotification tNotif = (EditProductNotification)notification;
            TestText = tNotif.Text;
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
