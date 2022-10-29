using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mhub_5._0
{
    /// <summary>
    /// Логика взаимодействия для EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {
        public static Product product;
        public List<TypeProduct> ProductTypes = DataBase.DatebaseConection.TypeProduct.ToList();
        public EditProduct()
        {
            InitializeComponent();

            #region Filter component fill
            var found = ProductTypes.Find(pt => product.TypeProduct == pt);
            ProductTypes.Remove(found);
            ProductTypes.Insert(0, found);

            FilterComponent.SelectedItem = ProductTypes.First();
            FilterComponent.ItemsSource = ProductTypes;
            FilterComponent.DisplayMemberPath = "Name";
            #endregion


            EditDrid.DataContext = product;

            //foreach (var item in MainWindow.DefaultProdutcts)
            //{
            //    if (item != product)
            //        continue;

            //    var quary = from material in MainWindow.DefaultMaterial
            //            from productMaterial in MainWindow.DefaultProdutctsMaterial
            //            where productMaterial.idProduct == product.id 
            //            && material.id == productMaterial.idMaterial
            //            select material;

            //    foreach (var materialText in quary)
            //        MaterialBox.Text += materialText.Name;

            //    break;
            //}
        }
            
    }
} 

