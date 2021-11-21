using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDV_Carlos.Models;

namespace TDV_Carlos.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private Tienda_RestEntities bd = new Tienda_RestEntities();

        // GET: Usuario
        public ActionResult Index(string email)
        {
            if (User.Identity.IsAuthenticated)
            {
                string correo = email;
                string rol = "";

                using (bd)
                {
                    var query = from st in bd.Usuarios
                                  where st.email == correo
                                  select st;
                    var lista = query.ToList();
                    if (lista.Count > 0)
                    {
                        var empleado = query.FirstOrDefault<Usuarios>();
                        string[] nombres = empleado.nombre.ToString().Split(' ');
                        Session["name"] = nombres[0];
                        Session["usr"] = empleado.nombre;                        
                        rol = empleado.rol.ToString().TrimEnd();
                    }
                    else
                    {
                        var consu = from st in bd.Clientes
                                    where st.email == correo
                                    select st;
                        var regis = query.ToList();

                        var cliente = consu.FirstOrDefault<Clientes>();
                        string[] nombres = cliente.nombre.ToString().Split(' ');
                        Session["name"] = nombres[0];
                        Session["usr"] = cliente.nombre;
                        rol = "Cliente";
                    }
                }

                if (rol=="Comprador")
                {
                    return RedirectToAction("Index","Compras");
                }
                else if (rol == "Enviador")
                {
                    return RedirectToAction("Index", "Envios");
                }
                else if (rol == "Chateador")
                {
                    return RedirectToAction("Index", "Chat");
                }
                else if (rol == "Cliente")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (rol == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
            //return View();
        }
    }
}