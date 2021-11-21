using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDV_Carlos.Models
{
    public class ItemPedido
    {
        public int idOrd
        {
            get;
            set;
        }

        public Productos Product
        {
            get;
            set;
        }

        public int Cantidad
        {
            get;
            set;
        }
    }
}