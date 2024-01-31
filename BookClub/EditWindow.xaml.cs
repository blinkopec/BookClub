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
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private int i;
        public EditWindow(int i)
        {
            InitializeComponent();
            this.i = i;
            GetOrders();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (i == 0)
            {
                ManagerWindow mw = new ManagerWindow();
                mw.Show();
                this.Close();
            }

            if (i == 1)
            {
                AdminWindow aw = new AdminWindow();
                aw.Show();
                this.Close();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var item = button.DataContext as TrashProduct;

            if (item == null)
                return;

            var product = BookClubEntities.GetContext().ContentOrder
                .Where(b => b.idProduct == item.id && b.idOrder == item.idOrder)
                .Single();

            BookClubEntities.GetContext().ContentOrder.Remove(product);
            BookClubEntities.GetContext().SaveChanges();

            GetOrders();
        }

        private List<TrashProduct> GetProducts(int idOrder)
        {
            List<TrashProduct> result = new List<TrashProduct>();
            var productsInTrash = BookClubEntities.GetContext().ContentOrder
                .Where(b => b.idOrder == idOrder)
                .ToList();

            if (productsInTrash.Count != 0)
            {
                foreach (var prod in BookClubEntities.GetContext().ContentOrder
                    .Where(b => b.idOrder == idOrder).ToList())
                {
                    var prd = prod.Product;
                    result.Add(new TrashProduct(prd.id, idOrder, prd.name, prd.image,
                                    prd.description, prd.idManufacturer, prd.price, prd.discount, prod.amount));
                }
            }

            return result;
        }

        private void GetOrders()
        {
            List<EditableOrder> result = new List<EditableOrder>();
            List<Order> orders = BookClubEntities.GetContext().Order
                .ToList();

            foreach (Order order in orders)
            {
                result.Add(new EditableOrder(order.id, order.idUser, order.idStatusOrder, GetProducts(order.id)));
            }

            itemsControl.ItemsSource = result;
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
                foreach (var contentOrder in BookClubEntities.GetContext().ContentOrder.Where(b => b.idOrder == trashProduct.idOrder
                        && b.idProduct == trashProduct.id).ToList())
                {
                    BookClubEntities.GetContext().ContentOrder.Remove(contentOrder);
                    BookClubEntities.GetContext().SaveChanges();
                    GetOrders();
                }
            }
            else
            {
                foreach (var contentOrder in BookClubEntities.GetContext().ContentOrder.Where(b => b.idOrder == trashProduct.idOrder
                        && b.idProduct == trashProduct.id).ToList())
                {
                    contentOrder.amount = value;
                }
                BookClubEntities.GetContext().SaveChanges();
            }

        }
    }
}
