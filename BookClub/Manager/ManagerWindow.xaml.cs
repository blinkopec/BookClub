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
using BookClub.Customer;

namespace BookClub.Manager
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
            itemsControl.ItemsSource = GenerateProductList();

            var orders = BookClubEntities.GetContext().Order
                .Where(d => d.idUser == UserInfo.idUser)
                .ToList();

            foreach (var order in orders)
            {
                if (order.idStatusOrder == 3)
                {
                    TrashButton.IsEnabled = true;
                }
            }

            if (TempTrash.Products.Count != null)
                TrashButton.IsEnabled = true;
        }

        /// <summary>
        /// Обработчий событий, переход в корзину
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            TrashWindow tw = new TrashWindow(GetEmptyOrder().id);
            tw.Show();
            this.Close();
        }

        /// <summary>
        /// Обработчик событий, добавляет товар в корзину
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as MenuItem;
            if (button == null)
                return;
            var product = button.DataContext as Product;

            if (product == null)
                return;


            List<Order> orders = BookClubEntities.GetContext().Order.Where(b => b.idUser == UserInfo.idUser).ToList();

            Order trashOrder = GetEmptyOrder();
            bool emptyContent = true;
            foreach (var content in trashOrder.ContentOrder)
            {
                if (content.idProduct == product.id && content.idOrder == trashOrder.id)
                {
                    emptyContent = false;
                    content.amount += 1;

                    BookClubEntities.GetContext().SaveChanges();
                }
            }
            if (emptyContent)
            {
                ContentOrder contentOrder = new ContentOrder() { idOrder = trashOrder.id, idProduct = product.id, amount = 1 };

                BookClubEntities.GetContext().ContentOrder.Add(contentOrder);
                BookClubEntities.GetContext().SaveChanges();
            }
            TrashButton.IsEnabled = true;
            itemsControl.ItemsSource = GenerateProductList();
        }

        /// <summary>
        /// Метод, выводит товары
        /// </summary>
        /// <returns></returns>
        private List<Product> GenerateProductList()
        {
            List<Product> result = new List<Product>();

            var products = BookClubEntities.GetContext().Product.ToList();

            foreach (var product in products)
            {
                if (product.amount > 0)
                {
                    if (product.discount <= 0)
                    {
                        Product product1 = new Product()
                        {
                            amount = product.amount,
                            id = product.id,
                            name = product.name,
                            image = product.image,
                            description = product.description,
                            price = product.price,
                            idManufacturer = product.idManufacturer,
                            discount = 0
                        };
                        result.Add(product1);
                    }
                    else
                        result.Add(product);
                }
            }
            return result;
        }

        /// <summary>
        /// Метод, создает пустой заказа
        /// </summary>
        /// <returns>пустой заказ</returns>
        private Order GetEmptyOrder()
        {
            foreach (var i in BookClubEntities.GetContext().Order
                .Where(b => b.idUser == UserInfo.idUser)
                .ToList())
            {
                if (i.idStatusOrder == 3)
                {
                    return i;
                }
            }

            Order ordr = new Order() { idUser = UserInfo.idUser, idStatusOrder = 3 };

            BookClubEntities.GetContext().Order.Add(ordr);
            BookClubEntities.GetContext().SaveChanges();
            return ordr;


        }


        /// <summary>
        /// Обработик событий, переход на окно редактирования заказов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditWindow ew = new EditWindow(0);
            ew.Show();
            this.Close();
        }
    }
}
