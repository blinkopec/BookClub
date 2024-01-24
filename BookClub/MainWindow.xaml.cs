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
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow aw = new AuthorizationWindow();
            aw.Show();
            this.Close();
        }

        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
