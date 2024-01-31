﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace BookClub
{
    public class EditableOrder
    {
        public int id {  get; set; }
        public int idUser { get; set; }
        public string StatusOrder { get; set; }
        public List<TrashProduct> Products { get; set; }

        public EditableOrder(int id, int idUser, int idStatusOrder, List<TrashProduct> Products)
        {
            this.id = id;
            this.idUser = idUser;
            this.StatusOrder = ConvertStatusOrderId(idStatusOrder);
            this.Products = Products;
        }

        public string ConvertStatusOrderId(int id)
        {
            return BookClubEntities.GetContext().StatusOrder.Where(b => b.id == id).Select(b=>b.name).Single();
        }
    }
}
