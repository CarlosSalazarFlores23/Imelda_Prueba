using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDV_Carlos.Models;

namespace TDV_Carlos.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            Tienda_RestEntities bd = new Tienda_RestEntities();
            string correo="";
            if (User.Identity.IsAuthenticated)
            {
                correo = User.Identity.Name;
                string rol = "";

                var query = from st in bd.Usuarios
                            where st.email == correo
                            select st;
                var lista = query.ToList();
                if (lista.Count > 0)
                {
                    rol = lista.FirstOrDefault().rol;
                }
                if (rol == "Admin")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });
                }                
            }
            return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });
        }
    }
}