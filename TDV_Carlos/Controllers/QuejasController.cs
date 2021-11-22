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
    public class QuejasController : Controller
    {
        private Tienda_RestEntities db = new Tienda_RestEntities();

        // GET: Quejas
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
                if (rol == "Chateador")
                {
                    var comentario = db.Comentario.Include(c => c.Usuarios).Include(c => c.Venta);
                    return View(comentario.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });
                }
            }
            return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });
        }

        // GET: Quejas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            return View(comentario);
        }

        // GET: Quejas/Create
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
                if (rol == "Chateador")
                {
                    ViewBag.id_usuario = new SelectList(db.Usuarios, "id_usuario", "nombre");
                    ViewBag.id_venta = new SelectList(db.Venta, "id_venta", "id_venta");
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });
                }
            }
            return RedirectToAction("Index", "Usuario", routeValues: new { email = correo });            
        }

        // POST: Quejas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_cometario,titulo,descripcion,fecha,respuesta,id_usuario,id_venta,estatus")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                db.Comentario.Add(comentario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_usuario = new SelectList(db.Usuarios, "id_usuario", "nombre", comentario.id_usuario);
            ViewBag.id_venta = new SelectList(db.Venta, "id_venta", "id_venta", comentario.id_venta);
            return View(comentario);
        }

        // GET: Quejas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_usuario = new SelectList(db.Usuarios, "id_usuario", "nombre", comentario.id_usuario);
            ViewBag.id_venta = new SelectList(db.Venta, "id_venta", "id_venta", comentario.id_venta);
            return View(comentario);
        }

        // POST: Quejas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_cometario,titulo,descripcion,fecha,respuesta,id_usuario,id_venta,estatus")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                comentario.estatus = 1;
                db.Entry(comentario).State = EntityState.Modified;                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_usuario = new SelectList(db.Usuarios, "id_usuario", "nombre", comentario.id_usuario);
            ViewBag.id_venta = new SelectList(db.Venta, "id_venta", "id_venta", comentario.id_venta);
            return View(comentario);
        }

        // GET: Quejas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            return View(comentario);
        }

        // POST: Quejas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comentario comentario = db.Comentario.Find(id);
            db.Comentario.Remove(comentario);
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
