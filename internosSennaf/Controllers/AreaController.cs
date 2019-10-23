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
    public class AreaController : Controller
    {

        private internosSenafEntities db = new internosSenafEntities();


        // GET: AreaXDependencia
        public ActionResult AreasXDependencia(int id)
        {
            var areasXDepe = (from a in db.Area
                              where a.Dependencia_Area.Select(da => da.idDependencia).FirstOrDefault() == id
                              select a)
                             .ToList();

            return PartialView("_Listado", areasXDepe);
        }


        // GET: Area
        public ActionResult Index()
        {
            return View(db.Area.ToList());
        }

        // GET: Area/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Area.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // GET: Area/Create
        public ActionResult Create()
        {
            ViewBag.listadoDependencias = db.Dependencia.ToList();

            return View();
        }

        // POST: Area/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] Area area, int idDependencia)
        {
            if (ModelState.IsValid)
            {

                db.Area.Add(area);
                db.SaveChanges();

                Dependencia_Area da = new Dependencia_Area();
                da.idArea = area.id;
                da.idDependencia = idDependencia;
                db.Dependencia_Area.Add(da);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(area);
        }

        // GET: Area/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Area area = db.Area.Find(id);

            ViewBag.listadoDependencias = db.Dependencia.ToList();      // listado de dependencias
            ViewBag.idDepeSeleccionado = db.Dependencia.Find(area.Dependencia_Area.Select(da => da.idDependencia).FirstOrDefault());        // dependencia seleccionado

            if (area == null)
            {
                return HttpNotFound();
            }


            return View(area);
        }

        // POST: Area/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] Area area, int idDependencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(area).State = EntityState.Modified;
                db.SaveChanges();

                var da_encontrado = (from depar in db.Dependencia_Area
                                     where depar.idArea == area.id
                                     select depar).FirstOrDefault();

                if (da_encontrado != null)
                {
                    da_encontrado.idDependencia = idDependencia;
                    db.Entry(da_encontrado).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                Dependencia_Area da = new Dependencia_Area();
                da.idArea = area.id;
                da.idDependencia = idDependencia;
                db.Dependencia_Area.Add(da);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(area);
        }

        // GET: Area/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Area area = db.Area.Find(id);

            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }


        // POST: Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Area area = db.Area.Find(id);
            db.Area.Remove(area);

            var DAbuscado = (from da in db.Dependencia_Area
                             where da.idArea == id
                             select da).FirstOrDefault();

            if (DAbuscado != null)
            {
                db.Dependencia_Area.Remove(DAbuscado);
            }

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
