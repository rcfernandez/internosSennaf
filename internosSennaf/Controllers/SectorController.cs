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
    public class SectorController : Controller
    {
        private internosSenafEntities db = new internosSenafEntities();


        // GET: Interno/ListadoXDependencia/5
        public PartialViewResult SectoresXDependencia(int id)
        {
            var sectoresXDepe = (from s in db.Sector
                                  join da in db.Dependencia_Area on s.idArea equals da.idArea
                                  where da.idDependencia == id
                                  orderby s.Sector_Piso.Select(sp => sp.idPiso).FirstOrDefault()
                                  select s)
                                .ToList();

            return PartialView("_Listado", sectoresXDepe);
        }



        // GET: Sector
        public ActionResult Index()
        {
            return View();
        }


        // GET: Sector/Listado
        public PartialViewResult MostrarListado(int id)
        {
            var sectorList = (from s in db.Sector
                              join da in db.Dependencia_Area on s.idArea equals da.idArea
                              where da.idDependencia == id
                              orderby s.Sector_Piso.Select(sp => sp.idPiso).FirstOrDefault()
                              select s)
                            .ToList();

            return PartialView("_Listado", sectorList);
        }



        // GET: Sector/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sector sector = db.Sector.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        // GET: Sector/Create
        public ActionResult Create()
        {
            ViewBag.idArea = new SelectList(db.Area, "id", "descripcion");
            ViewBag.idPiso = new SelectList(db.Piso, "id", "numero");

            return View();
        }

        // POST: Sector/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion,referente,idArea")] Sector sector, int idPiso)
        {
            if (ModelState.IsValid)
            {
                Sector_Piso sp = new Sector_Piso();
                sp.idSector = sector.id;
                sp.idPiso = idPiso;

                db.Sector.Add(sector);
                db.Sector_Piso.Add(sp);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.idArea = new SelectList(db.Area, "id", "descripcion", sector.idArea); // esto creo que se puede sacar
            //ViewBag.idPiso = new SelectList(db.Sector_Piso, "id", "descripcion", sector.Sector_Piso.Select(sp => sp.idPiso).FirstOrDefault());

            return View(sector);
        }


        // GET: Sector/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Sector sector = db.Sector.Find(id);
            //Sector_Piso sp = db.Sector_Piso.Find(sector.id);

            if (sector == null)
            {
                return HttpNotFound();
            }

            ViewBag.idArea = new SelectList(db.Area, "id", "descripcion", sector.idArea);
            ViewBag.idPiso = new SelectList(db.Piso, "id", "numero", sector.Sector_Piso.Select(sp => sp.Piso.id).FirstOrDefault());

            return View(sector);
        }


        // POST: Sector/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion,referente,idArea")] Sector sector, int idPiso)
        {
            if (ModelState.IsValid)
            {

                db.Entry(sector).State = EntityState.Modified;

                Sector_Piso sp = (from spdb in db.Sector_Piso
                                  where spdb.idSector == sector.id
                                  select spdb).FirstOrDefault();

                sp.idPiso = idPiso;
                db.Entry(sp).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.idArea = new SelectList(db.Area, "id", "descripcion", sector.idArea);

            return View(sector);
        }

        // GET: Sector/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sector sector = db.Sector.Find(id);
            if (sector == null)
            {
                return HttpNotFound();
            }
            return View(sector);
        }

        // POST: Sector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sector sector = db.Sector.Find(id);
            db.Sector.Remove(sector);
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
