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
    /// Логика взаимодействия для BuyWindow.xaml
    /// </summary>
    public partial class BuyWindow : Window
    {
        private Order order { get; set; }
        public BuyWindow(Order order)
        {
            InitializeComponent();
            this.order = order;
            PickUpPointBox.ItemsSource = BookClubEntities.GetContext().PickUpPoint
                .Select(b => b.location)
                .ToList();
        }
        
        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (PickUpPointBox.SelectedValue != null)
            {
                Random rnd = new Random();
                var ordr = BookClubEntities.GetContext().Order
                    .Where(b => b.id == this.order.id)
                    .Single();
                ordr.idPickUpPoint = ConvertCityToId(PickUpPointBox.SelectedValue.ToString());
                ordr.orderDate = DateTime.Now;
                ordr.receiptCode = GenerateReceiptCode();
                ordr.idStatusOrder = 2;
                ordr.deliveryTime = rnd.Next(1, 9);

                BookClubEntities.GetContext().SaveChanges();

                MessageBox.Show("Ваш код получения: " + ordr.receiptCode + "\nОжидаемое время доставки: " + ordr.deliveryTime
                    + "\n Дата заказа: " + DateTime.Now);

                AuthorizedWindow aw = new AuthorizedWindow();
                aw.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("Выберите место доставки");
            }
        }

        private int ConvertCityToId(string city)
        {
            int result = 0;
            var cities = BookClubEntities.GetContext().PickUpPoint.ToList();
            foreach (var c in cities)
            {
                if (c.location == city)
                {
                    result = c.id;
                }
            }
            return result;
        }

        private int GenerateReceiptCode()
        {
            int tmp = GenerateNumber();
            bool trueFalse = true;

            while(trueFalse)
            {
                if (CheckReceiptCode(tmp))
                {
                    trueFalse = false;
                    break;
                }
                else
                {
                    tmp = GenerateNumber();
                }
            }
            return tmp;
        }

        private bool CheckReceiptCode(int code)
        {
            bool result = false;
            var ordersReceiptsCode = BookClubEntities.GetContext().Order
                .Select(b => b.receiptCode)
                .ToList();
            int tmp = 0;
            
            foreach (var receiptCode in ordersReceiptsCode)
            {
                if (receiptCode == code)
                {
                    tmp++;
                }
            }

            if (tmp == 0)
                result = true;

            return result;
        }
        private int GenerateNumber()
        {
            Random rnd = new Random();
            string result = "";
            for (int i = 0; i <= 3;i++)
            {
                result += rnd.Next(0, 9); 
            }

            return Convert.ToInt32( result);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizedWindow aw = new AuthorizedWindow();
            aw.Show();
            this.Close();
        }
    }
}
