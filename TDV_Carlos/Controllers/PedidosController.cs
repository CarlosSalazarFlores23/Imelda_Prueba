using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDV_Carlos.Models;

namespace TDV_Carlos.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        Tienda_RestEntities db = new Tienda_RestEntities();
        // GET: Pedidos
        public ActionResult Index()
        {
            string correo = User.Identity.Name;
            Clientes cl = (from c in db.Clientes
                           where c.email == correo
                           select c
                           ).ToList().FirstOrDefault();

            int id = cl.Id_cliente;

            var query = from o in db.orden
                        where o.id_cliente == id
                        orderby o.fecha_creacion ascending
                        select o;

            List<orden> ordenes = query.ToList();

            List<PedidoCliente> pedidos = new List<PedidoCliente>();
            PedidoCliente pedido;
            List<orden_producto> ordPed;
            List<ItemPedido> itemPed = new List<ItemPedido>();

            ItemPedido iPed;

            foreach (orden o in ordenes)
            {
                pedido = new PedidoCliente();
                pedido.Orden = o;
                pedido.Fecha = o.fecha_creacion.ToShortDateString();
                if (o.fecha_envio.HasValue)
                {
                    pedido.envio = o.fecha_envio.GetValueOrDefault().ToShortDateString();
                }
                else
                {
                    pedido.envio = "Proximanente";
                }
                if (o.fecha_entrega.HasValue)
                {
                    pedido.status = o.fecha_entrega.GetValueOrDefault().ToShortDateString();
                }
                else
                {
                    pedido.status = "Sin entregar";
                }
                pedido.Total = o.total.ToString();
                pedidos.Add(pedido);
                ordPed = (from oP in db.orden_producto
                          where oP.id_orden == o.Id_orden
                          select oP).ToList();
                pedido.orden_Productos = ordPed;
                foreach (orden_producto op in ordPed)
                {
                    iPed = new ItemPedido();
                    iPed.idOrd = op.id_orden;
                    iPed.Product = db.Productos.First(p => p.id_producto == op.id_producto);
                    iPed.Cantidad = op.cantidad;
                    itemPed.Add(iPed);
                }
                Session["misPedidos"] = pedidos;
                Session["Pedido"] = itemPed;
            }
            return View();
        }
    }
}