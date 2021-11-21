using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDV_Carlos.Models;

namespace TDV_Carlos.Controllers
{
    public class CatalogoController : Controller
    {
        private Tienda_RestEntities db = new Tienda_RestEntities();
        // GET: Catalogo
        public ActionResult Index()
        {
            return View();
        }

         [HttpPost]
         public ActionResult BuscarProd(string nomBuscar)
        {
            ViewBag.SearchKey = nomBuscar;
            using (db)
            {
                var query = from st in db.Productos
                            where st.nombre.Contains(nomBuscar)
                            select st;

                var listProd = query.ToList();
                ViewBag.Listado = listProd;
            }
            //return RedirectToAction("Index","Home");
            return View();
        }

        public ActionResult prodCategoria(int idCat)
        {
            List<Productos> mercancia = null;
            var query = from p in db.Productos
                        where p.id_subcategoria == idCat
                        select p;
            if(idCat == 1)
            {
                List<Productos> platos = query.ToList();
                mercancia = platos;
                ViewBag.Catego = "Platos:";
            }
            if (idCat == 2)
            {
                List<Productos> tazas = query.ToList();
                mercancia = tazas;
                ViewBag.Catego = "Tazas:";
            }
            ViewBag.productos = mercancia;
            return View();
        }
    }
}