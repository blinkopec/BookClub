using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
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

namespace BookClub
{
    /// <summary>
    /// Логика взаимодействия для AuthorizedWindow.xaml
    /// </summary>
    public partial class AuthorizedWindow : Window
    {
        public AuthorizedWindow()
        {
            InitializeComponent();
            itemsControl.ItemsSource = GenerateProductList();

            var orders = BookClubEntities.GetContext().Order
                .Where(d=>d.idUser == UserInfo.idUser)
                .ToList();

            foreach (var order in orders)
            {
                if (order.idStatusOrder == 3)
                {
                    TrashButton.IsEnabled = true;
                }
            }
        }

        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            List<Order> orders = BookClubEntities.GetContext().Order.Where(b => b.idUser == UserInfo.idUser).ToList();
            
            Order trashOrder = new Order();
            bool trueOrder = false;
            foreach (Order order in orders)
            {
                if (order.idStatusOrder == 3)
                {
                    trueOrder = true;
                    trashOrder = order;
                }
            }
            if (trueOrder)
            {
                TrashWindow tw = new TrashWindow(trashOrder.id);
                tw.Show();
                this.Close();
            }
            else
            {
                Order order = new Order() { idUser = UserInfo.idUser, idStatusOrder = 3 };

                BookClubEntities.GetContext().Order.Add(order);
                BookClubEntities.GetContext().SaveChanges();

                TrashWindow tw = new TrashWindow(order.id);
                tw.Show();
                this.Close();
            }



            
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as MenuItem;
            if (button == null)
                return;
            var product = button.DataContext as Product;

            if (product == null) 
                return;


            List<Order> orders = BookClubEntities.GetContext().Order.Where(b => b.idUser == UserInfo.idUser).ToList();

            Order trashOrder = new Order();
            bool trueOrder = false;
            foreach (Order order in orders)
            {
                if (order.idStatusOrder == 3)
                {
                    trueOrder = true;
                    trashOrder = order;
                }
            }
            if (trueOrder)
            {
                List<ContentOrder> contents = BookClubEntities.GetContext().ContentOrder
                    .Where(b=>b.idOrder == trashOrder.id)
                    .ToList();

                if (contents.Count > 0)
                {
                    using (var context = new BookClubEntities())
                    {
                        foreach (var content in context.ContentOrder)
                        {
                            if (content.idProduct == product.id && content.idOrder == trashOrder.id)
                            {
                                content.amount += 1;
                            }
                        }
                        context.SaveChanges();
                       
                    }
                }
                else 
                {
                    ContentOrder contentOrder = new ContentOrder() { idOrder = trashOrder.id, idProduct = product.id, amount = 1 };

                    BookClubEntities.GetContext().ContentOrder.Add(contentOrder);
                    BookClubEntities.GetContext().ContentOrder.Add(contentOrder);
                    BookClubEntities.GetContext().SaveChanges();
                }
            }
            else
            {
                Order order = new Order() { idUser = UserInfo.idUser, idStatusOrder = 3 };

                BookClubEntities.GetContext().Order.Add(order);
                BookClubEntities.GetContext().SaveChanges();

                ContentOrder contentOrder = new ContentOrder() { idOrder = order.id, idProduct = product.id, amount = 1 };

                BookClubEntities.GetContext().ContentOrder.Add(contentOrder);
                BookClubEntities.GetContext().SaveChanges();
            }

            using (var context = new BookClubEntities())
            {
                foreach (var prd in context.Product)
                {
                    if (prd.id == product.id)
                    {
                        prd.amount -= 1;
                    }
                }
                context.SaveChanges();
            }
            TrashButton.IsEnabled = true;
            itemsControl.ItemsSource = null;
            itemsControl.ItemsSource = GenerateProductList();
        }
    
        private List<Product> GenerateProductList()
        {
            List<Product> result= new List<Product>();

            var products = BookClubEntities.GetContext().Product.ToList();

            foreach (var product in products)
            {
                if (product.amount > 0)
                {
                    if (product.discount <= 0)
                    {
                        product.discount = 0;
                        result.Add(product);
                    }
                    else
                        result.Add(product);
                }
            }
            return result;
        }
    }
}
