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

namespace BookClub
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

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectProductsButton_Click(object sender, RoutedEventArgs e)
        {
            dg.ItemsSource = BookClubEntities.GetContext().Product.ToList();
            dg.Columns[8].MaxWidth = 0;
            dg.Columns[9].MaxWidth = 0;
        }

        private void SelectOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            EditWindow ew = new EditWindow(1);
            ew.Show();
            this.Close();
        }

        private Product RemoveSpaces(Product product)
        {
            if (product != null)
            {
                product.description.Replace(" ", "");
                product.name.Replace(" ", "");
            }

            return product;
        }
    }
}
