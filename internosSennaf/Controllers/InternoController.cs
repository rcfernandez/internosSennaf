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
    public class InternoController : Controller
    {
        private internosSenafEntities db = new internosSenafEntities();


        // GET: Interno/ListadoXDependencia/5
        public PartialViewResult InternosXDependencia(int id)
        {
            var interDepe = (from i in db.Interno
                             where i.Sector.Area.Dependencia_Area.Select(da => da.idDependencia).FirstOrDefault() == id
                             select i)
                            .ToList();

            return PartialView("_Listado", interDepe);
        }


        // GET: InternosMVC
        public ActionResult Index()
        {
            //var internos = (from i in db.Interno
            //                orderby i.numero
            //                select i).ToList();

            return View();
        }

        // GET: Interno/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interno interno = db.Interno.Find(id);
            if (interno == null)
            {
                return HttpNotFound();
            }
            return View(interno);
        }

        // GET: Interno/Create
        public ActionResult Create()
        {
            ViewBag.idDependencia = new SelectList(db.Dependencia, "id", "descripcion");

            ViewBag.tipo = new List<SelectListItem>
            {
                new SelectListItem() {  Value = "s/n", Text = "s/n"  },
                new SelectListItem() {  Value = "analogico", Text = "Analógico"  },
                new SelectListItem() {  Value = "digital", Text = "Digital"  },
                new SelectListItem() {  Value = "linea", Text = "Línea"  },
            };


            ViewBag.estado = new List<SelectListItem>
            {
                new SelectListItem() {  Value = "usado", Text = "Usado"  },
                new SelectListItem() {  Value = "libre", Text = "Libre"  },
                new SelectListItem() {  Value = "chequear", Text = "Chequear"  },
                new SelectListItem() {  Value = "no funciona", Text = "No Funciona"  },
            };

            return View();
        }

        // POST: Interno/TraerSectores/5
        public JsonResult TraerSectores(int? idDependencia)
        {
            var listadoSectores = (from s in db.Sector
                                   where s.Area.Dependencia_Area.Select(da => da.idDependencia).FirstOrDefault() == idDependencia
                                   select new { identi = s.id, descri = s.descripcion })
                                    .ToList();

            return Json(listadoSectores, JsonRequestBehavior.AllowGet);
        }

        // POST: Interno/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,numero,tipo,tn,estado,ocultar,observacion,idSector")] Interno interno)
        {
            if (ModelState.IsValid)
            {
                db.Interno.Add(interno);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(interno);
        }

        // GET: Interno/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interno interno = db.Interno.Find(id);
            if (interno == null)
            {
                return HttpNotFound();
            }

            // listado de tipo de interno
            ViewBag.opcionesTipos = new List<SelectListItem>
            {
                new SelectListItem() { Value = "s/n", Text = "s/n" },
                new SelectListItem() { Value = "analogico", Text = "Analógico" },
                new SelectListItem() { Value = "digital", Text = "Digital" },
                new SelectListItem() { Value = "linea", Text = "Línea" },
            };

            ViewBag.idSector = new SelectList(db.Sector, "id", "descripcion", interno.idSector);
            return View(interno);
        }

        // POST: Interno/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,numero,tipo,tn,estado,ocultar,observacion,idSector")] Interno interno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idSector = new SelectList(db.Sector, "id", "descripcion", interno.idSector);
            return View(interno);
        }

        // GET: Interno/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interno interno = db.Interno.Find(id);
            if (interno == null)
            {
                return HttpNotFound();
            }
            return View(interno);
        }

        // POST: Interno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interno interno = db.Interno.Find(id);
            db.Interno.Remove(interno);
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
