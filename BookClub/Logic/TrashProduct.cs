using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BookClub.Logic
{
    /// <summary>
    /// Класс нужный для вывода продуктов из корзины
    /// </summary>
    public class TrashProduct
    {
        public int id { get; set; }
        public int idOrder { get; set; }
        public string name { get; set; }
        public BitmapImage image { get; set; }
        public string description { get; set; }
        public int idManufacturer { get; set; }
        public int price { get; set; }
        public Nullable<double> discount { get; set; }
        public int[] amountArr { get; set; }
        public int amount { get; set; }

        public TrashProduct(int id, int idOrder, string name, byte[] image, string description, int idManafacturer,
            int price, Nullable<double> discount, int amount) 
        {
            this.id = id;
            this.idOrder = idOrder;
            this.name = name;
            this.image = ConvertToByteImage(image);
            this.description = description;
            this.idManufacturer = idManafacturer;
            this.price = price;
            this.discount = discount;
            this.amountArr = ConvertAmountToArray(id);
            this.amount = amount;
        }

        /// <summary>
        /// Метод, конвертирующий количество товара в массив
        /// </summary>
        /// <param name="idProd"></param>
        /// <returns></returns>
        private int[] ConvertAmountToArray(int idProd)
        {
            var maxAmount = BookClubEntities.GetContext().Product
                .Where(b => b.id == idProd)
                .Select(b => b.amount)
                .Single();

                maxAmount += 1;
                int[] array = new int[maxAmount];
                for (int i = 0; i < maxAmount; i++)
                {
                    array[i] = i;
                    if (maxAmount == i)
                    {
                        return array;
                    }
                }
                return array;
        }

        /// <summary>
        /// Метод, конвертирует байтовый массив в картинку
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private BitmapImage ConvertToByteImage(byte[] array)
        {
            var image = new BitmapImage();
            if (array == null)
                return image;

            
            using (var mem = new MemoryStream(array))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();

            return image;
        }
    }
}
