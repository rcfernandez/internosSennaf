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
    public class DependenciaController : Controller
    {
        private internosSenafEntities db = new internosSenafEntities();

        // GET: Dependencia
        public ActionResult Index()
        {
            return View(db.Dependencia.ToList());
        }

        // GET: Dependencia Sarmiento
        public ActionResult ListadoSarmiento()
        {

            ViewBag.Title = "Home Page";

            var listado = (from i in db.Interno
                           join s in db.Sector on i.idSector equals s.id
                           join sp in db.Sector_Piso on s.id equals sp.idSector
                           join a in db.Area on s.idArea equals a.id
                           join da in db.Dependencia_Area on a.id equals da.idArea
                           where da.idDependencia == 5           // 5 = SARMIENTO   
                           group s by s.descripcion into listadoAgrupado
                           select new Group<string, Sector> { Key = listadoAgrupado.Key, Values = listadoAgrupado })
                          .ToList();

            return View(listado);
        }


        // action que genera el listado en formato PDF
        public ActionResult generadorPDF_Sarmiento()
        {
            DateTime fecha = DateTime.Now;

            var listado = (from i in db.Interno
                           join s in db.Sector on i.idSector equals s.id
                           join sp in db.Sector_Piso on s.id equals sp.idSector
                           join a in db.Area on s.idArea equals a.id
                           join da in db.Dependencia_Area on a.id equals da.idArea
                           where da.idDependencia == 5           // 5 = SARMIENTO   
                           group s by s.descripcion into listadoAgrupado
                           select new Group<string, Sector> { Key = listadoAgrupado.Key, Values = listadoAgrupado })
                          .ToList();

            string headerFooter = "--print-media-type " +
                                    "--header-center \"-  I N T E R N O S    S . E . N . N . A . F .  -\" --header-font-size \"9\" --header-spacing 3 --header-font-name \"calibri light\" " +
                                    "--footer-center \"Pag.: [page]/[toPage] - Fecha: " + fecha.ToShortDateString() + "\" --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\" ";

            return new Rotativa.PartialViewAsPdf("ListadoSarmiento", listado)
            {
                FileName = "Internos_SENNAF_Sarmiento_" + fecha.ToShortDateString() + ".pdf",
                PageMargins = new Rotativa.Options.Margins(15, 10, 15, 10),
                CustomSwitches = headerFooter
            };
        }



        // GET: Dependencia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dependencia dependencia = db.Dependencia.Find(id);
            if (dependencia == null)
            {
                return HttpNotFound();
            }
            return View(dependencia);
        }

        // GET: Dependencia/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dependencia/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] Dependencia dependencia)
        {
            if (ModelState.IsValid)
            {
                db.Dependencia.Add(dependencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dependencia);
        }

        // GET: Dependencia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dependencia dependencia = db.Dependencia.Find(id);
            if (dependencia == null)
            {
                return HttpNotFound();
            }
            return View(dependencia);
        }

        // POST: Dependencia/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] Dependencia dependencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dependencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dependencia);
        }

        // GET: Dependencia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dependencia dependencia = db.Dependencia.Find(id);
            if (dependencia == null)
            {
                return HttpNotFound();
            }
            return View(dependencia);
        }

        // POST: Dependencia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dependencia dependencia = db.Dependencia.Find(id);
            db.Dependencia.Remove(dependencia);
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
