using System;
using System.Collections.Generic;
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
using BookClub.Logic;

namespace BookClub.Customer
{
    /// <summary>
    /// Логика взаимодействия для TempTrashWindow.xaml
    /// </summary>
    public partial class TempTrashWindow : Window
    {
        List<TrashProduct> newprd;
        public TempTrashWindow()
        {
            InitializeComponent();
            newprd = new List<TrashProduct>();
            GetProducts();
        }

        /// <summary>
        /// Обработчик событий, открывает окно гостя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        /// <summary>
        /// Обработчик событий, открывает окно авторизации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow aw = new AuthorizationWindow();
            aw.Show();
            this.Close();
        }

        /// <summary>
        /// Обработчик событий, удаляет товар из корзины
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var trashProduct = button.DataContext as TrashProduct;

            if (trashProduct == null)
                return;

            try
            {
                foreach (var i in TempTrash.Products)
                {
                    if (i.Product.id == trashProduct.id)
                    {
                        TempTrash.Products.Remove(i);
                    }
                }
            }
            catch { }
            GetProducts();
        }

        /// <summary>
        /// Метод, выводит продукты из корзины
        /// </summary>
        private void GetProducts()
        {
            if (TempTrash.Products.Count != 0)
            {
                try
                {
                    foreach (var prod in TempTrash.Products)
                    {
                        var prd = prod.Product;
                        newprd.Add(new TrashProduct(prd.id, 0, prd.name, prd.image,
                                        prd.description, prd.idManufacturer, prd.price, prd.discount, prod.amount));
                    }
                    itemsControl.ItemsSource = newprd;
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Корзина пуста");
                itemsControl.ItemsSource = null;
            }
            GeneratePrice();
        }

        /// <summary>
        /// Обработчик событий, добавляет количество товара в корзину
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AmountBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
                return;
            var trashProduct = comboBox.DataContext as TrashProduct;

            if (trashProduct == null)
                return;
            int value = (int)comboBox.SelectedValue;
            if (value == 0)
            {
                try
                {
                    foreach (var prd in TempTrash.Products)
                    {
                        if (prd.Product.id == trashProduct.id)
                        {
                            TempTrash.Products.Remove(prd);
                            GetProducts();
                        }
                    }
                }
                catch { }
            }
            else
            {
                foreach (var prd in TempTrash.Products)
                {
                    if (prd.Product.id == trashProduct.id)
                    {
                        prd.amount = value;
                        GeneratePrice();
                    }
                    
                }
            }

        }

        /// <summary>
        /// Метод, генерирует цену и скидку
        /// </summary>
        private void GeneratePrice()
        {
            // КОЛИЧЕСТВО
            int price = 0;
            float discount = 0;

            foreach (var prd in newprd)
            {
                for (int i = 0; i < prd.amount; i++)
                {
                    price += prd.price;
                    if (prd.discount != null)
                        discount += (float)(prd.price * prd.discount) / 100;
                }
            }

            PriceLabel.Content = price + "₽";
            DiscountLabel.Content = discount + "₽";
        }
    }
}
