using MVVMDemoDB.Command;
using MVVMDemoDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVMDemoDB.ViewModel
{
    public class ProductViewModel:INotifyPropertyChanged
    {

        public void OnPropertyChanged(string propertyName) 
        {
            if (PropertyChanged != null) 
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));  
            }
        }


        //1     add all Product class required properties
        //ProductId
        //ProductName
        //UnitPrice

        private Product domObject = new Product();

        public string UI_ProductId
        {
            get { return domObject.ProductID.ToString(); }
            set
            {
                if (value != string.Empty)
                {
                    domObject.ProductID = Convert.ToInt32(value);
                    OnPropertyChanged("UI_ProductId");
                }
            }
        }

        public string UI_ProductName
        {
            get { return domObject.ProductName; }
            set
            {
                if (value != string.Empty)
                {
                    domObject.ProductName = value;
                    OnPropertyChanged("UI_ProductName");
                }
            }
        }
        public string UI_UnitPrice 
        {
            get { return domObject.UnitPrice.ToString(); }
            set
            {
                if (value != string.Empty)
                {
                    domObject.UnitPrice = Convert.ToDecimal(value);
                    OnPropertyChanged("UI_UnitPrice");
                }
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;


        //2     Implement INotifyPropertyChanged interface



        //3     Add Observalble Collection of Product


        ObservableCollection<Product> productList = null;
        public ObservableCollection<Product> ProductList 
        {
            get { return productList; }
            set
            {
                productList = value;
                OnPropertyChanged("ProductList");
            }
        }

         //4     decalre NorthwindEntities class reference (db)
               NorthwindEntities db = null;

        //5     in constructor load Product table data into observable collection
        public ProductViewModel() 
        {
            db=new NorthwindEntities();
            //var data = db.Products.ToList();
            ProductList = new ObservableCollection<Product>();

            loadCommand = new RelayCommand(LoadProduct);
            clearCommand = new RelayCommand(ClearMethod);
            searchCommand = new RelayCommand(SearchM);
            addProductCommand = new RelayCommand(AddProductM);
            updateCommand = new RelayCommand(UpdateM);
        }

        //----------------------------- Command-----------------------------------

        private ICommand loadCommand;
        public ICommand LoadCommand
        {
            get { return loadCommand; }
        }

        private ICommand clearCommand;
        public ICommand ClearCommand
        {
            get { return clearCommand; }
        }


        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get { return searchCommand; }
        }

        private ICommand addProductCommand;
       public ICommand AddProductCommand 
        {
            get { return addProductCommand; }
        }

        private ICommand updateCommand;
        public ICommand UpdateCommand
        {
            get { return updateCommand; }
        }

        //-----------------------Manipulation Methods--------------------------
        public void LoadProduct (object obj)
        {
            var data=db.Products.ToList();
            ProductList = new ObservableCollection<Product>(data);
            if (ProductList.Count > 0)
            {
                this.UI_ProductId = ProductList[0].ProductID.ToString();
                this.UI_ProductName = ProductList[0].ProductName.ToString();
                this.UI_UnitPrice = ProductList[0].UnitPrice.ToString();
            }
            else 
            {
                MessageBox.Show("Record not found");
            }
        }

        public void ClearMethod(Object obj) 
        {
            UI_ProductId = null;
            UI_ProductName = null;
            UI_UnitPrice=null;
            ProductList=null;
                
        }


        public void SearchM(Object obj)
        {
            int productid = Convert.ToInt32(this.UI_ProductId);
            

              //var isFound = ProductList.Where(x => x.ProductID == productid).FirstOrDefault();
            //var isFound=ProductList.FirstOrDefault(x => x.ProductID == productid );

            var isFound = ProductList.Where(x => x.ProductID == productid || x.ProductName==this.UI_ProductName).FirstOrDefault();


            if (isFound != null)
            {

                this.UI_ProductId = isFound.ProductID.ToString();
                this.UI_ProductName = isFound.ProductName.ToString();
                this.UI_UnitPrice = isFound.UnitPrice.ToString();
                MessageBox.Show("record  Found");
            }
            else
            {
                MessageBox.Show("record not Found");
            }
        }


        public void AddProductM(Object obj)
        {
            //domObject.ProductID = Convert.ToInt32(this.UI_ProductId);
            domObject.ProductName = this.UI_ProductName;
            if (this.UI_UnitPrice.ToString() !=string.Empty)
            {
                domObject.UnitPrice = Convert.ToDecimal(this.UI_UnitPrice);
            }

            if (domObject != null)
            {
                ProductList.Add(domObject);
                db.Products.Add(domObject);
                db.SaveChanges();
                MessageBox.Show("Record inserted");
            }
            else
            {
                MessageBox.Show("Please insert record");
            }

        }

        public void UpdateM(Object obj) 
        {
           int temp = Convert.ToInt32(this.UI_ProductId);




            //for (int i=0;i<ProductList.Count;i++)
            //{
            //    if (temp == ProductList[i].ProductID) 
            //    {
            //        domObject.ProductID = Convert.ToInt32(this.UI_ProductId);
            //        domObject.ProductName = this.UI_ProductName;
            //        domObject.UnitPrice = Convert.ToDecimal(this.UI_UnitPrice);

            //        db.Products.Add(domObject);
            //        db.SaveChanges();

            //        MessageBox.Show("Record Updated");
            //        break;
            //    }

            //}


            //Product objCourse = O.COURSEs.Single(course => course.course_name == "B.Tech");
            Product objProduct = db.Products.SingleOrDefault(id => id.ProductID == temp);

            //Save the changes back to the database.  
            objProduct.ProductName=this.UI_ProductName;
            objProduct.UnitPrice = Convert.ToDecimal(this.UI_UnitPrice);
            
            db.SaveChanges();
            MessageBox.Show("Record Updated");



        }

        






    }
}
