using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDV_Carlos.Models;

namespace TDV_Carlos.Controllers
{
    public class CarroController : Controller
    {
        // GET: Carro
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agregar(int id)
        {
            ProdCarro carro = new ProdCarro();
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                Productos p = carro.find(id);
                string nam = p.nombre;
                cart.Add(new Item { Product = carro.find(id), Cantidad = 1 });
                Session["cart"] = cart;
                Session["itemTotal"] = 1;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Cantidad++;
                }
                else
                {
                    Productos p = carro.find(id);
                    string name = p.nombre;
                    cart.Add(new Item { Product = carro.find(id), Cantidad = 1 });
                    Session["itemTotal"] = (Int32.Parse( Session["itemTotal"].ToString())) + 1;
                }
                Session["cart"] = cart;
            }
            return RedirectToAction("Index");
            //return View();
        }

        public ActionResult Quitar(int id)
        {
            Session["itemTotal"] = (Int32.Parse(Session["itemTotal"].ToString())) - 1;
            List<Item> cart = (List<Item>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.id_producto.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

    }
}