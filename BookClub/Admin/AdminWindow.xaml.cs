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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using BookClub.Manager;
using Microsoft.Win32;

namespace BookClub.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private bool isOrders;
        private bool isProducts;

        public AdminWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик событий, сохраняет товар или изменения товара в базу данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Product tmp = dg.SelectedItem as Product;
            if (tmp != null)
            {
                tmp = RemoveSpaces(tmp);
                if (tmp.id == 0)
                    BookClubEntities.GetContext().Product.Add(tmp);
                BookClubEntities.GetContext().SaveChanges();
                dg.ItemsSource = BookClubEntities.GetContext().Product.ToList();
                dg.Columns[8].MaxWidth=0;
                dg.Columns[9].MaxWidth = 0;
            }
        }

        /// <summary>
        /// Обработчик событий, удаляет товар из базы данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            Product tmp = dg.SelectedItem as Product;
            if (tmp != null)
            {
                BookClubEntities.GetContext().Product.Remove(tmp);
                BookClubEntities.GetContext().SaveChanges();
                dg.ItemsSource = BookClubEntities.GetContext().Product.ToList();
                dg.Columns[8].MaxWidth = 0;
                dg.Columns[9].MaxWidth = 0;

            }
        }

       /// <summary>
       /// Обработчик событий, выводит в dataGrid продукты
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void SelectProductsButton_Click(object sender, RoutedEventArgs e)
        {
            dg.ItemsSource = BookClubEntities.GetContext().Product.ToList();
            dg.Columns[8].MaxWidth = 0;
            dg.Columns[9].MaxWidth = 0;
        }

        /// <summary>
        /// Обработичк событий, переводит на окно редактирования заказов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            EditWindow ew = new EditWindow(1);
            ew.Show();
            this.Close();
        }

        /// <summary>
        /// Метод удаляет пробелы у продуктов
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private Product RemoveSpaces(Product product)
        {
            if (product != null)
            {
                product.description.Replace(" ", "");
                product.name.Replace(" ", "");
            }

            return product;
        }

        /// <summary>
        /// Обработчик событий, добавляет фото к товару
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
           // open file dialog и загрузка фотки в бд и комментарии к коду   
            Product tmp = dg.SelectedItem as Product;
            if (tmp != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                  //  tmp.image   

                }
                
                BookClubEntities.GetContext().SaveChanges();
                dg.ItemsSource = BookClubEntities.GetContext().Product.ToList();
                dg.Columns[8].MaxWidth = 0;
                dg.Columns[9].MaxWidth = 0;

            }
        }
    }
}
