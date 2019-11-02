using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using internosSennaf.Models;


namespace internosSennaf.Controllers
{
    public class HomeController : Controller
    {

        private internosSenafEntities db = new internosSenafEntities();

        // version del listado
        string versionListado = "2019.09.17";



        public ActionResult Index()
        {
            ViewBag.Title = "Internos SENNAF";

            return View();
        }


        // GET: Home/MostrarListadoInternos/5
        //public PartialViewResult MostrarListadoInternos(int id)
        //{
        //    var listadoInternos = (from i in db.Interno
        //                           join s in db.Sector on i.idSector equals s.id
        //                           join sp in db.Sector_Piso on s.id equals sp.idSector
        //                           join da in db.Dependencia_Area on s.idArea equals da.idArea
        //                           where da.idDependencia == id
        //                           group s by s.descripcion into listadoAgrupado
        //                           select new Group<string, Sector> { Key = listadoAgrupado.Key, Values = listadoAgrupado })
        //                          .ToList();

        //    return PartialView("_ListadoInternos", listadoInternos);
        //}


        // GET: Home/MostrarListadoInternos/5
        public PartialViewResult MostrarListadoInternos(int id)
        {
            var listadoInternos = from i in db.Interno
                                  join s in db.Sector on i.idSector equals s.id
                                  join sp in db.Sector_Piso on s.id equals sp.idSector
                                  join da in db.Dependencia_Area on s.idArea equals da.idArea
                                  where da.idDependencia == id
                                  group s by s.descripcion into listadoAgrupado
                                  orderby listadoAgrupado.Select(xx => xx.Sector_Piso.Select(spsp => spsp.Piso.id).FirstOrDefault()).FirstOrDefault(), listadoAgrupado.Key
                                  select new ListInterno()
                                  {
                                      idDependencia = id,
                                      Piso = listadoAgrupado.Select(x => x.Sector_Piso.Select(sp => sp.Piso.numero).FirstOrDefault()).FirstOrDefault(),
                                      Sector = listadoAgrupado.Key,
                                      Referente = listadoAgrupado.Select(referente => referente.referente).FirstOrDefault(),
                                      Internos = listadoAgrupado.Select(pp => pp.Interno).FirstOrDefault()
                                  };


            return PartialView("_ListadoInternos2", listadoInternos.ToList());
        }


        // action que genera el listado en formato PDF
        public ActionResult generadorPDF(int idDepe)
        {
            ViewBag.version = versionListado;
            DateTime fecha = DateTime.Now;

            var listado = (from i in db.Interno
                           join s in db.Sector on i.idSector equals s.id
                           join sp in db.Sector_Piso on s.id equals sp.idSector
                           join da in db.Dependencia_Area on s.idArea equals da.idArea
                           where da.idDependencia == idDepe
                           group s by s.descripcion into listadoAgrupado
                           select new Group<string, Sector> { Key = listadoAgrupado.Key, Values = listadoAgrupado })
                          .ToList();

            var sede = listado.Select(list => list.Values
                            .Select(val => val.Area.Dependencia_Area
                                .Select(da => da.Dependencia.descripcion).FirstOrDefault()
                            ).FirstOrDefault()
                        ).FirstOrDefault();

            string headerFooter = "--print-media-type " +
                                    "--header-center \"-  I N T E R N O S    S . E . N . N . A . F .  :   " + sede + "  -\" --header-font-size \"9\" --header-spacing 3 --header-font-name \"calibri light\" " +
                                    "--footer-center \"Pag.: [page]/[toPage] - Fecha: " + fecha.ToShortDateString() + "\" --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\" ";

            return new Rotativa.PartialViewAsPdf("_ListadoInternos", listado)
            {
                FileName = "ListadoDeInternosSENNAF_" + sede + "_" + fecha.ToShortDateString() + ".pdf",
                PageMargins = new Rotativa.Options.Margins(15, 10, 15, 10),
                CustomSwitches = headerFooter
            };
        }







    }
}