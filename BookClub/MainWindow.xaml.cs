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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookClub
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserInfo.authationType = AuthorizationType.unauthorized;
            itemsControl.ItemsSource = BookClubEntities.GetContext().Product.ToList();

            if (TempTrash.Products.Count == 0)
                TrashButton.IsEnabled = false;
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as MenuItem;
            if (button == null)
                return;
            var product = button.DataContext as Product;

            if (product == null)
                return;

            TrashButton.IsEnabled = true;
            if (TempTrash.Products.Where(b=>b.Product.id == product.id).Count() > 0)
            {
                foreach (var tw in TempTrash.Products)
                {
                    if (tw.Product.id == product.id)
                    {
                        tw.amount += 1;
                    }
                }
            }
            else
                TempTrash.Products.Add(new TempProduct(product, 1));
        }

        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow aw = new AuthorizationWindow();
            aw.Show();
            this.Close();
        }

        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            TempTrashWindow ttw = new TempTrashWindow();
            ttw.Show();
            this.Close();
        }
    }
}
