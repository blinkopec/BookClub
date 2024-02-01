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
using BookClub.Manager;
using BookClub.Admin;
using BookClub.Logic;
using BookClub.Customer;

namespace BookClub
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработик событий, переход на окно гостя
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
        /// Обработик событий, авторизация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = loginBox.Text;
            string password = psswdBox.Password;

            List<User> users = BookClubEntities.GetContext().User.ToList();
            bool trueLogin = false;
            foreach (User user in users)
            {
                if (user.login == login)
                {
                    trueLogin = true;
                    if (user.password == password)
                    {
                        UserInfo.idUser = user.id;
                        UserInfo.authationType = AuthorizationType.autorization;
                        if (user.idTypeUser == 2)
                        {
                            ManagerWindow mw = new ManagerWindow();
                            mw.Show();
                            this.Close();
                        }
                        if (user.idTypeUser == 3)
                        {
                            AuthorizedWindow aw = new AuthorizedWindow();
                            aw.Show();
                            this.Close();
                        }
                        if (user.idTypeUser == 1)
                        {
                            AdminWindow mw = new AdminWindow();
                            mw.Show();
                            this.Close();
                        }
                    }
                    else MessageBox.Show("Неправильный пароль");
                }
            }
            if (trueLogin == false)
                MessageBox.Show("Неверно введены данные");

        }
    }
}
