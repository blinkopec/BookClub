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

namespace BookClub
{
    /// <summary>
    /// Логика взаимодействия для TrashWindow.xaml
    /// </summary>
    public partial class TrashWindow : Window
    {
        private int idOrder { get; set; }
        public TrashWindow(int idOrder)
        {
            InitializeComponent();
            this.idOrder = idOrder;
            GetProducts();


        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizedWindow aw = new AuthorizedWindow();
            aw.Show();
            this.Close();
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            Order ord = BookClubEntities.GetContext().Order
                .Where(b => b.id == this.idOrder)
                .Single();
            BuyWindow bw = new BuyWindow(ord);
            bw.Show();
            this.Close();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var item = button.DataContext as Product;

            if (item == null)
                return;

            List<ContentOrder> contents = BookClubEntities.GetContext().ContentOrder.ToList();
            foreach (var contentOrder in contents)
            {
                if (contentOrder.idOrder == this.idOrder)
                {
                    if (contentOrder.idProduct == item.id)
                    {
                        BookClubEntities.GetContext().ContentOrder.Remove(contentOrder);
                        BookClubEntities.GetContext().SaveChanges();
                    }
                }
            }
            GetProducts();
        }

        private void GetProducts()
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
                List<TrashProduct> newprd = new List<TrashProduct>();
                this.idOrder = trashOrder.id;
                var productsInTrash = BookClubEntities.GetContext().ContentOrder
                .Where(b => b.idOrder == trashOrder.id)
                .ToList();
                foreach (var prod in productsInTrash)
                {
                    using (var context = new BookClubEntities())
                    {
                        foreach (var prd in context.Product)
                        {
                            if (prd.id == prod.idProduct)
                            {
                                newprd.Add(new TrashProduct(prd.id, prd.name, prd.image, prd.description, prd.idManufacturer, prd.price, prd.discount, prod.amount));
                            }
                        }

                    }
                }
                itemsControl.ItemsSource = newprd;
            }
            else
            {
                MessageBox.Show("Корзина пуста");
            }
        }

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
                using (var context = new BookClubEntities())
                {
                    foreach (var contentOrder in context.ContentOrder)
                    {
                        if (contentOrder.idOrder == this.idOrder)
                        {
                            if (contentOrder.idProduct == trashProduct.id)
                            {
                                BookClubEntities.GetContext().ContentOrder.Remove(contentOrder);
                                BookClubEntities.GetContext().SaveChanges();
                            }
                        }
                    }
                }
            }
            else
            {
                using (var context = new BookClubEntities())
                {
                    foreach (var contentOrder in context.ContentOrder)
                    {
                        if (contentOrder.idOrder == this.idOrder)
                        {
                            if (contentOrder.idProduct == trashProduct.id)
                            {
                                contentOrder.amount = value;
                            }
                        }
                    }
                    context.SaveChanges();
                }
            }
            
        }
    }
}
