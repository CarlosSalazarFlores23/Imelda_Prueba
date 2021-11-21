using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDV_Carlos.Models
{
    public class PedidoCliente
    {
        private Tienda_RestEntities db = new Tienda_RestEntities();
        private List<orden_producto> detalle_orden;

        public PedidoCliente()
        {
            detalle_orden = db.orden_producto.ToList();
        }

        public orden Orden
        {
            get;
            set;
        }

        public string Fecha
        {
            get;
            set;
        }

        public string envio
        {
            get;
            set;
        }

        public string status
        {
            get;
            set;
        }

        public string Total
        {
            get;
            set;
        }

        public List<orden_producto> orden_Productos
        {
            get;
            set;
        }
    }
}