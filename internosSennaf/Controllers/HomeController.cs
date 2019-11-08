using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using internosSennaf.Models;
using internosSennaf.Servicios;

namespace internosSennaf.Controllers
{
    public class HomeController : Controller
    {

        private internosSenafEntities db = new internosSenafEntities();

        private string versionListado = "2019.09.17";


        public ActionResult Index()
        {
            ViewBag.Title = "Internos SENNAF";

            return View();
        }


        // GET: Home/MostrarListadoInternos/5
        public PartialViewResult MostrarListadoInternos(int id)
        {
            internoService iserv = new internoService();
            var listadoInternos = iserv.listadoGeneral(id);

            if (id == 4)
            {
                return PartialView("_ListadoInternos3", listadoInternos.ToList());
            }

            return PartialView("_ListadoInternos2", listadoInternos.ToList());
        }


        // action que genera el listado en formato PDF
        public ActionResult generadorPDF(int idDepe)
        {
            ViewBag.version = versionListado;
            DateTime fecha = DateTime.Now;

            internoService iserv = new internoService();
            var listado = iserv.listadoGeneral(idDepe);

            var sede = db.Dependencia.Find(idDepe).descripcion;

            string headerFooter = "--print-media-type " +
                                    "--header-center \"-  I N T E R N O S    S . E . N . N . A . F .  :   " + sede + "  -\" --header-font-size \"9\" --header-spacing 3 --header-font-name \"calibri light\" " +
                                    "--footer-center \"Pag.: [page]/[toPage] - Fecha: " + fecha.ToShortDateString() + "\" --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\" ";

            return new Rotativa.PartialViewAsPdf("_ListadoInternos2", listado)
            {
                FileName = "ListadoDeInternosSENNAF_" + sede + "_" + fecha.ToShortDateString() + ".pdf",
                PageMargins = new Rotativa.Options.Margins(15, 10, 15, 10),
                CustomSwitches = headerFooter
            };
        }





    }
}