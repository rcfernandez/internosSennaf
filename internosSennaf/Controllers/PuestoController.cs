using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using internosSennaf.Models;

namespace internosSennaf.Controllers
{
    [Authorize(Roles = "administrador")]
    public class PuestoController : Controller
    {
        private internosSenafEntities db = new internosSenafEntities();

        // GET: Puesto
        public ActionResult Index()
        {
            return View(db.Puesto.ToList());
        }

        // GET: Puesto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Puesto puesto = db.Puesto.Find(id);
            if (puesto == null)
            {
                return HttpNotFound();
            }
            return View(puesto);
        }

        // GET: Puesto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Puesto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tipo,numero")] Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                db.Puesto.Add(puesto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(puesto);
        }

        // GET: Puesto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Puesto puesto = db.Puesto.Find(id);
            if (puesto == null)
            {
                return HttpNotFound();
            }
            return View(puesto);
        }

        // POST: Puesto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tipo,numero")] Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(puesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(puesto);
        }

        // GET: Puesto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Puesto puesto = db.Puesto.Find(id);
            if (puesto == null)
            {
                return HttpNotFound();
            }
            return View(puesto);
        }

        // POST: Puesto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Puesto puesto = db.Puesto.Find(id);
            db.Puesto.Remove(puesto);
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
