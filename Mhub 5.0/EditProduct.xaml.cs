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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Mhub_5._0
{
    /// <summary>
    /// Логика взаимодействия для EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {
        public List<TypeProduct> ProductTypes = MainWindow.DefaultTypeProduct.ToList();
        public List<Material> ProductMaterials = MainWindow.DefaultMaterial.ToList();

        private readonly Material _defaultValue = new Material() { Name = "Материалы" };
        
        public Product ProductEditing
        {
            get { return (Product)GetValue(ProductEditingProperty); }
            set { SetValue(ProductEditingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for My.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductEditingProperty =
            DependencyProperty.Register("ProductEditing", typeof(Product), typeof(EditProduct), new PropertyMetadata(null));

        public IEnumerable<(Material material, int count)> Materials
        {
            get { return (IEnumerable<(Material, int)>)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Materials", typeof(IEnumerable<(Material, int)>), typeof(EditProduct));



        public EditProduct(Product productEditing)
        {
            ProductEditing = productEditing; // присваиваем выбранный продукт к переменной

            Materials = from product_material in MainWindow.DefaultProductsMaterial.ToArray()
                        where product_material.idProduct == ProductEditing.id
                        select
                        (
                            product_material.Material,
                            (int)product_material.Count
                        );

            InitializeComponent();

            ListMaterials.ItemsSource = Materials;

            ListMaterials.DataContext = Materials;

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
            #endregion

            MainEditProduct.DataContext = ProductEditing;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Заполнение листа материалами продукта
            #endregion
        }

        #region обновляем тип продукта
        private void TypeProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductEditing.TypeProduct = TypeProduct.SelectedItem as TypeProduct;
        }
        #endregion

        #region добавляем новые мятериалы в продукт
        private void MaterialComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialComboBox.SelectedItem == null || MaterialComboBox.SelectedItem == _defaultValue)
                return;

            Materials =Materials.Append((MaterialComboBox.SelectedItem as Material, 1));
        }
        #endregion

        #region сохранение изменений в бд
        private void SaveEditProdutc_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.DefaultProdutcts.Any(x =>x.Min == Convert.ToInt32(MinEdit) || x.TypeProduct.Name == TypeProduct.SelectedItem as string || x.Name == NameEdit.Text))

            {
                var result = MessageBox.Show("Ничего не изменилось\nВыйти?", "Окно уведомлений", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ProductEditing.Name = NameEdit.Text; // сохранение нового названия
                        ProductEditing.TypeProduct = TypeProduct.SelectedItem as TypeProduct; // сохранение нового типа
                        ProductEditing.Min = Convert.ToInt32(MinEdit); // сохранение новой минимальной цены

                        foreach (var (material, count) in Materials)
                        {
                            ProductMaterial productMaterial = new ProductMaterial
                            {
                                idMaterial = material.id,
                                idProduct = ProductEditing.id,
                                Count = count
                            };

                            if (DataBase.DatebaseConection.ProductMaterial.Contains(productMaterial))
                                continue;
                            
                            DataBase.DatebaseConection.ProductMaterial.Add(productMaterial); // сохранение новых материалов
                        }

                        DataBase.DatebaseConection.SaveChanges(); // обновление бд

                        MessageBox.Show("Измениния сохранены");

                        MainWindow.RefreshProducts(); // обновление таблицы
                        Close();
                        break;

                    case MessageBoxResult.No:
                        MainWindow.RefreshProducts(); // обновление таблицы
                        Close();
                        break;
                }
                return;
            }
        }
        #endregion

    }
}

