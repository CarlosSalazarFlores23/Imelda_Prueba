using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDV_Carlos.Controllers
{
    [Authorize]
    public class ComprasController : Controller
    {
        // GET: Compras
        public ActionResult Index()
        {
            return View();
        }
    }
}