using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDV_Carlos.Models;

namespace TDV_Carlos.Controllers
{
    public class PruebasController : Controller
    {
        private Tienda_RestEntities db = new Tienda_RestEntities();
        // GET: Pruebas
        public ActionResult Index()
        {
            using (db)
            {
                var query = from st in db.Productos
                            select st;

                var listProd = query.ToList();
                ViewBag.Listado = listProd;
            }
            return View();
        }
    }
}