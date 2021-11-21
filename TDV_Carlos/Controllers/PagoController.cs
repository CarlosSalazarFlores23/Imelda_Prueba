using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Web.Mvc;
using TDV_Carlos.Models;

namespace TDV_Carlos.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private Tienda_RestEntities db = new Tienda_RestEntities();
        private string NumConfirPago;
        
        // GET: Pago
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult CrearOrden()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Session["CrearOrden"] = "pend";
                return RedirectToAction("Login", "Account");
            }

            string correo = User.Identity.Name;

            //var bd = new Tienda_RestEntities();
            string fechaCreacion = DateTime.Today.ToShortDateString();
            string fechaProbEntrega = DateTime.Today.AddDays(3).ToShortDateString();
            var cliente = (from c in db.Clientes
                           where c.email == correo
                           select c).ToList().FirstOrDefault();

            Session["nom"] = cliente.nombre;
            Session["dirCliente"] = cliente.calle_t + " " + cliente.colonia_t + " " + cliente.estado_t;
            Session["fechaOrden"] = fechaCreacion;
            Session["fPEntreg"] = fechaProbEntrega;

            if (cliente.num_tarj_cred_ppal.StartsWith("4"))
                Session["tTarjeta"] = "1";
            if (cliente.num_tarj_cred_ppal.StartsWith("5"))
                Session["tTarjeta"] = "2";
            if (cliente.num_tarj_cred_ppal.StartsWith("3"))
                Session["tTarjeta"] = "3";

            Session["nTarj"] = cliente.num_tarj_cred_ppal;
            return View();        
        }

        public ActionResult Pagar(string tipoPago)
        {
            string correo = User.Identity.Name;

            DateTime fechaCreacion = DateTime.Today;
            DateTime fechaProbEntrega = fechaCreacion.AddDays(3);
            var cliente = (from c in db.Clientes
                           where c.email == correo
                           select c).ToList().FirstOrDefault();
            int idClient = cliente.Id_cliente;

            if (tipoPago.Equals("T"))
            {
                if (!validaPago(cliente))
                {
                    return RedirectToAction("pagoNoAceptado");
                }
                else{
                    var dirEnt = (from d in db.dirEntrega
                                  where d.id_cliente == cliente.Id_cliente
                                  select d).ToList().FirstOrDefault();

                    int idDir = dirEnt.Id_dirEnt;
                    return RedirectToAction("pagoAceptado", routeValues: new { idC = idClient, idD = idDir });
                }
            }
            if (tipoPago.Equals("P"))
            {
                var dirEnt = (from d in db.dirEntrega
                              where d.id_cliente == cliente.Id_cliente
                              select d).ToList().FirstOrDefault();

                int idDir = dirEnt.Id_dirEnt;
                validaPago(cliente);
                return RedirectToAction("pagoPaypal", routeValues: new { idC = idClient, idD = idDir });
            }
            return View();
        }

        private bool validaPago(Clientes cliente)
        {
            bool retorna = true;
            int randomvalue;

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] val = new byte[6];
                crypto.GetBytes(val);
                randomvalue = BitConverter.ToInt32(val, 1);
            }

            NumConfirPago = Math.Abs(randomvalue * 1000).ToString();
            Session["nConfirma"] = NumConfirPago;
            return retorna;
        }

        public ActionResult pagoNoAceptado()
        {
            return View();
        }

        public ActionResult pagoAceptado(int idC, int idD)
        {
            orden orden_cliente = new orden();
            int id = 0;
            if (!(db.orden.Max(o => (int?)o.Id_orden)==null))
            {
                id = db.orden.Max(o => o.Id_orden);
            }
            else
            {
                id = 0;
            }
            id++;

            orden_cliente.Id_orden = id;
            orden_cliente.fecha_creacion = DateTime.Today;
            orden_cliente.num_confirmacion =Session["nConfirma"].ToString();
            //Se pone el '1'  por defecto para evitar el problema por el NOT NULL, ya que tiene una FK
            orden_cliente.id_paqueteria = 1;
            var carro = Session["cart"] as List<Item>;
            var total = carro.Sum(item => item.Product.precio_venta * item.Cantidad);
            orden_cliente.total = total;
            orden_cliente.id_cliente = idC;
            orden_cliente.id_dirEntrega = idD;
            db.orden.Add(orden_cliente);
            db.SaveChanges();

            orden_producto ordenProd;
            List<orden_producto> listaProdOrd = new List<orden_producto>();
            foreach(Item linea in carro)
            {
                ordenProd = new orden_producto();
                ordenProd.id_orden = orden_cliente.Id_orden;
                ordenProd.id_producto = linea.Product.id_producto;
                ordenProd.cantidad = linea.Cantidad;
                db.orden_producto.Add(ordenProd);
            }
            db.SaveChanges();
            Session["cart"]=null;
            Session["nConfirma"] = null;
            Session["itemTotal"] = 0;
            return View();
        }

        public ActionResult pagoPaypal(int idC, int idD)
        {
            Session["idClient"] = idC;
            Session["idDir"] = idD;

            return View();
        }

        public ActionResult pagandoPaypal(int idC, int idD)
        {
            Session["idClient"] = idC;
            Session["idDir"] = idD;

            return View();
        }
    }
}