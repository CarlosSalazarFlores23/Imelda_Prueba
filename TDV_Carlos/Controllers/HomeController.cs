using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDV_Carlos.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            //Esto lo puse desde la rama "Carlos"
            //Si funciona        

            //return RedirectToAction("Index","Envios");
            //return RedirectToAction("Index", "Pruebas");            
            if (Session["itemTotal"]==null)
            {
                Session["itemTotal"] = 0;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}