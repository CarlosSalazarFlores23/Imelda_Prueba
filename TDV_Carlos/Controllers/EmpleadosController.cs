using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TDV_Carlos.Models;

namespace TDV_Carlos.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private Tienda_RestEntities db = new Tienda_RestEntities();

        // GET: Empleados
        public ActionResult Index()
        {
            Tienda_RestEntities bd = new Tienda_RestEntities();
            string correo = "";
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
                    return View(db.Usuarios.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });
                }
            }
            return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });            
        }

        // GET: Empleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: Empleados/Create
        public ActionResult Create()
        {
            Tienda_RestEntities bd = new Tienda_RestEntities();
            string correo = "";
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

        // POST: Empleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_usuario,nombre,apellidos,email,telefono,rol,estatus")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuarios);
        }

        // GET: Empleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Empleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_usuario,nombre,apellidos,email,telefono,rol,estatus")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuarios);
        }

        // GET: Empleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuarios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
