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
    public class PisoController : Controller
    {
        private internosSenafEntities db = new internosSenafEntities();

        // GET: Piso
        public ActionResult Index()
        {
            return View(db.Piso.ToList());
        }

        // GET: Piso/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piso piso = db.Piso.Find(id);
            if (piso == null)
            {
                return HttpNotFound();
            }
            return View(piso);
        }

        // GET: Piso/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Piso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,numero")] Piso piso)
        {
            if (ModelState.IsValid)
            {
                db.Piso.Add(piso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(piso);
        }

        // GET: Piso/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piso piso = db.Piso.Find(id);
            if (piso == null)
            {
                return HttpNotFound();
            }
            return View(piso);
        }

        // POST: Piso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,numero")] Piso piso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(piso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(piso);
        }

        // GET: Piso/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Piso piso = db.Piso.Find(id);
            if (piso == null)
            {
                return HttpNotFound();
            }
            return View(piso);
        }

        // POST: Piso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Piso piso = db.Piso.Find(id);
            db.Piso.Remove(piso);
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
