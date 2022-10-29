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
        public Product ProductEditing;
        public List<TypeProduct> ProductTypes = MainWindow.DefaultTypeProduct.ToList();
        public List<Material> ProductMaterials = MainWindow.DefaultMaterial.ToList();

        private readonly Material _defaultValue = new Material() { Name = "Материалы" };

        public IEnumerable<Material> Materials
        {
            get { return (IEnumerable<Material>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Materials", typeof(IEnumerable<Material>), typeof(EditProduct));



        public EditProduct(Product productEditing)
        {
            InitializeComponent();

            ProductEditing = productEditing;

            #region Combobox ProductType 
            TypeProduct.SelectedItem = ProductEditing.TypeProduct;
            TypeProduct.ItemsSource = ProductTypes;
            TypeProduct.DisplayMemberPath = "Name";
            #endregion

            #region ComboBox Material
            ProductMaterials.Insert(0, _defaultValue);

            MaterialComboBox.SelectedItem = _defaultValue;
            MaterialComboBox.ItemsSource = ProductMaterials;
            MaterialComboBox.DisplayMemberPath = "Name";

            EditDrid.DataContext = ProductEditing;
            #endregion
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Заполнение листа материалами продукта
            Materials = from product_material in MainWindow.DefaultProductsMaterial.ToArray()
                        where product_material.idProduct == ProductEditing.id
                        select product_material.Material;

            ListMaterials.ItemsSource = Materials;
            #endregion
        }

        #region
        private void TypeProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductEditing.TypeProduct = TypeProduct.SelectedItem as TypeProduct;
        }
        #endregion

        private void MaterialComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialComboBox.SelectedItem == null || MaterialComboBox.SelectedItem == _defaultValue)
                return;

            Materials = Materials.Append(MaterialComboBox.SelectedItem as Material);
        }

        #region сохранение изменений
        private void SaveEditProdutc_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.DefaultProdutcts.Any(x =>x.Min == Convert.ToInt32(MinEdit) || x.TypeProduct.Name == TypeProduct.SelectedItem as string || x.Name == NameEdit.Text))

            {
                var result = MessageBox.Show("Ничего не изменилось\nВыйти?", "Окно уведомлений", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ProductEditing.Name = NameEdit.Text;
                        ProductEditing.TypeProduct = TypeProduct.SelectedItem as TypeProduct;
                        ProductEditing.Min = Convert.ToInt32(MinEdit);
                        DataBase.DatebaseConection.SaveChanges();

                        MessageBox.Show("Измениния сохранены");

                        MainWindow.RefreshProducts();
                        Close();
                        break;
                    case MessageBoxResult.No:
                        Close();
                        break;
                }
                return;
            }
        }
        #endregion

    }
} 

