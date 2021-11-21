using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TDV_Carlos.Models
{
    public class ProdCarro
    {
        private Tienda_RestEntities db = new Tienda_RestEntities();
        private List<Productos> products;
        public ProdCarro()
        {
            products = db.Productos.ToList();
        }

        public List<Productos> findAll()
        {
            return this.products;
        }

        public Productos find( int id)
        {
            Productos pp = this.products.Single(p => p.id_producto.Equals(id));
            return pp;
        }
    }
}