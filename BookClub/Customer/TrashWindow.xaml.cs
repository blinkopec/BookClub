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

namespace BookClub.Customer
{
    /// <summary>
    /// Логика взаимодействия для TrashWindow.xaml
    /// </summary>
    public partial class TrashWindow : Window
    {
        List<TrashProduct> newprd;
        private int idOrder { get; set; }
        public TrashWindow(int idOrder)
        {
            InitializeComponent();
            
            newprd = new List<TrashProduct>();
            this.idOrder = idOrder;

            if (TempTrash.Products.Count != null)
            {
                GetProductsWithTempTrash();
            }
            else
                GetProducts();
        }

        /// <summary>
        /// Обработчик событий, открывает окно авторизированного пользователя и удаляет пустой заказ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookClubEntities.GetContext().ContentOrder.Where(b=>b.idOrder == this.idOrder).ToList().Count() <= 0 )
            {
                var ordr = BookClubEntities.GetContext().Order
                    .Where(b => b.id == this.idOrder).First();
                BookClubEntities.GetContext().Order.Remove(ordr);
                BookClubEntities.GetContext().SaveChanges();
            }
            AuthorizedWindow aw = new AuthorizedWindow();
            aw.Show();
            this.Close();
        }
        /// <summary>
        /// Обработчик событий, открывает окно покупки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            Order ord = BookClubEntities.GetContext().Order
                .Where(b => b.id == this.idOrder)
                .Single();
            BuyWindow bw = new BuyWindow(ord);
            bw.Show();
            this.Close();
        }

        /// <summary>
        /// Обработчик событий, удаляет товар из корзины
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;
            var item = button.DataContext as TrashProduct;

            if (item == null)
                return;

            var product = BookClubEntities.GetContext().ContentOrder
                .Where(b => b.idProduct == item.id && b.idOrder == this.idOrder)
                .Single();

            BookClubEntities.GetContext().ContentOrder.Remove(product);
            BookClubEntities.GetContext().SaveChanges();
            
            GetProducts();
            
        }

        /// <summary>
        /// Метод, вносит товары гостя в базу данных
        /// </summary>
        private void GetProductsWithTempTrash()
        {
            var contents = BookClubEntities.GetContext().ContentOrder.Where(b => b.idOrder == this.idOrder).ToList();
            foreach (var prd in TempTrash.Products)
            {
                foreach (var content in contents)
                {
                    if (content.idProduct == prd.Product.id)
                    {
                        content.amount += prd.amount;
                        BookClubEntities.GetContext().SaveChanges();
                    }
                    else
                    {
                        ContentOrder c = new ContentOrder() { idOrder=this.idOrder , idProduct = prd.Product.id, amount = prd.amount };
                        BookClubEntities.GetContext().ContentOrder.Add(c);
                        BookClubEntities.GetContext().SaveChanges(); 
                    }
                }
            }

            TempTrash.Products.Clear();
            GetProducts();

        }


        /// <summary>
        /// Метод, генерирует список товаров в корзину и выводит
        /// </summary>
        private void GetProducts()
        {
            itemsControl.ItemsSource = null;
            newprd.Clear();
            var productsInTrash = BookClubEntities.GetContext().ContentOrder
                .Where(b => b.idOrder ==  this.idOrder)
                .ToList();

            if (productsInTrash.Count != 0)
            {
                foreach (var prod in BookClubEntities.GetContext().ContentOrder
                    .Where(b => b.idOrder == this.idOrder).ToList())
                {
                    var prd = prod.Product;
                    newprd.Add(new TrashProduct(prd.id, this.idOrder,prd.name, prd.image,
                                    prd.description, prd.idManufacturer, prd.price, prd.discount, prod.amount));
                }
                itemsControl.ItemsSource = newprd;
            }
            else
            {
                MessageBox.Show("Корзина пуста");
                itemsControl.ItemsSource = null;
            }
            GeneratePrice();
        }

        /// <summary>
        /// Обработчик событий, изменяет количество товара в корзину
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                foreach (var contentOrder in BookClubEntities.GetContext().ContentOrder.Where(b => b.idOrder == this.idOrder
                        && b.idProduct == trashProduct.id).ToList())
                {
                    BookClubEntities.GetContext().ContentOrder.Remove(contentOrder);
                    BookClubEntities.GetContext().SaveChanges();
                    GetProducts();
                }
            }
            else
            {
                foreach (var contentOrder in BookClubEntities.GetContext().ContentOrder.Where(b => b.idOrder == this.idOrder
                        && b.idProduct == trashProduct.id).ToList())
                {
                    contentOrder.amount = value;
                    GeneratePrice();
                }
                BookClubEntities.GetContext().SaveChanges();
            }
           
        }
        
        /// <summary>
        /// Метод, генерирует цену и скидку
        /// </summary>
        private void GeneratePrice()
        {
            // КОЛИЧЕСТВО
            int price = 0;
            float discount = 0;
           
            foreach (var prd in newprd)
            {
                for (int i = 0; i < prd.amount; i++)
                {
                    price += prd.price;
                    if (prd.discount != null)
                        discount += (float)(prd.price * prd.discount) / 100;
                }
            }

            PriceLabel.Content = price + "₽";
            DiscountLabel.Content = discount + "₽";
        }
    }
    
}
    

