using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TDV_Carlos.Models;

namespace TDV_Carlos.Controllers
{
    public class ClientesController : Controller
    {
        private Tienda_RestEntities db = new Tienda_RestEntities();

        // GET: Clientes
        public ActionResult Index()
        {
            return View(db.Clientes.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id_cliente,nombre,email,calle_t,colonia_t,estado_t,num_tarj_cred_ppal")] Clientes clientes)
        public ActionResult Create(string nombre, string email, string calle_t, string colonia_t, string estado_t, string num_tarj_cred_ppal, string TipoTarj, string Mes, string Anio, string CVV)
        {
            Clientes cliente = new Clientes();            
            if (Tarjeta(num_tarj_cred_ppal, TipoTarj, Mes, Anio, CVV))
            {
                // comunicarse con el sistema de pago
                if (validaPago(nombre, calle_t, colonia_t, estado_t, num_tarj_cred_ppal, Mes, Anio, CVV))
                {
                    //El id puede ir null porque es AUTO_INCREMENT                    
                    //cliente.Id_cliente = id;

                    cliente.nombre = nombre;
                    cliente.email = Session["correo"].ToString();
                    cliente.calle_t = calle_t;
                    cliente.colonia_t = colonia_t;
                    cliente.estado_t = estado_t;
                    cliente.num_tarj_cred_ppal = num_tarj_cred_ppal;

                    db.Clientes.Add(cliente);
                    db.SaveChanges();

                    int? intIdt = db.Clientes.Max(c => (int?)c.Id_cliente);
                    int id = (int)intIdt;

                    System.Diagnostics.Debug.WriteLine("Si estas leyendo esto es porque funciona. Y sabes a lo que me refiero");
                    System.Diagnostics.Debug.WriteLine(id);

                    dirEntrega dir = new dirEntrega();
                    dir.Calle = calle_t;
                    dir.Colonia = colonia_t;
                    dir.Estado = estado_t;
                    dir.id_cliente = id;
                    
                    db.dirEntrega.Add(dir);
                    db.SaveChanges();

                    string[] nombres = cliente.nombre.ToString().Split(' ');
                    Session["name"] = nombres[0];
                    Session["usr"] = cliente.nombre;

                    if(Session["CrearOrden"] != null)
                    {
                        if (Session["CrearOrden"].Equals("pend"))
                        {
                            return RedirectToAction("CrearOrden", "Pago");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Invalida");
                }
            }
            else
            {
                return RedirectToAction("Invalida");
            }
            return View(cliente);
        }

        private bool Tarjeta(string tarj, string tipo, string mes, string anio, string cvv)
        {
            bool retorna = validaTarj(tarj);
            if (retorna)
            {
                if (tarj.StartsWith("4") && tipo.Equals("Visa"))
                {
                    retorna = true;
                }
                else
                {
                    if (tarj.StartsWith("5") && tipo.Equals("Mastercard"))
                    {
                        retorna = true;
                    }
                    else
                    {
                        if(tarj.StartsWith("3") && tipo.Equals("American"))
                        {
                            retorna = true;
                        }
                        else
                        {
                            retorna = false;
                        }
                    }
                }
                DateTime hoy = new DateTime();
                if(Convert.ToInt32(anio) >= hoy.Year)
                {
                    if (Convert.ToInt32(mes) >  hoy.Month)
                    {
                        retorna = true;
                    }
                    else
                    {
                        retorna = false;
                    }
                }
                else
                {
                    retorna = false;
                }
            }
            return retorna;
        }

        private bool validaTarj(string tarj)
        {
            bool retorna = true;
            StringBuilder digitsOnly = new StringBuilder();
            foreach(Char c in tarj)
            {
                if (Char.IsDigit(c)) digitsOnly.Append(c);
            };

            if (digitsOnly.Length > 18 || digitsOnly.Length < 15) return false;

            int sum = 0;
            int digit = 0;
            int addend = 0;
            bool timesTwo = false;

            for (int i =digitsOnly.Length-1; i>=0; i--)
            {
                digit = Int32.Parse(digitsOnly.ToString(i, 1));
                if (timesTwo)
                {
                    addend = digit * 2;
                    if (addend > 9)
                        addend -= 9;
                }
                else
                {
                    addend = digit;
                }
                sum += addend;
                timesTwo = !timesTwo;
            }
            retorna = ((sum % 10) == 0);

            return retorna;
        }

        private bool validaPago (string nombre, string calle_t, string colonia_t, string estado_t, string tarj, string mes, string anio, string cvv)
        {
            bool retorna = true;
            //En esta parte se comunica con el sistema de pago para validad la tarjeta y poder acreditar el pago. Esta parte aqui solo se esta simulando
            return retorna;
        }

        public ActionResult Invalida()
        {
            return View();
        }

        public ActionResult BorrarUser()
        {
            string idUser = User.Identity.GetUserId();
            return RedirectToAction("Delete", "Account", routeValues: new { id = idUser });
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_cliente,nombre,email,calle_t,colonia_t,estado_t,num_tarj_cred_ppal")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clientes clientes = db.Clientes.Find(id);
            db.Clientes.Remove(clientes);
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
