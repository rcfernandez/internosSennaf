using internosSennaf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace internosSennaf.Servicios
{
    public class SectorService
    {

        private internosSenafEntities db = new internosSenafEntities();

        // DEVUELVE UN LISTADO DE SECTORES MEDIANTE UN ID DE DEPENDENCIA
        public List<Sector> sectoresXDependencia(int? depe)
        {

            var sectores = from s in db.Sector
                           join a in db.Area on s.idArea equals a.id
                           join da in db.Dependencia_Area on a.id equals da.idArea
                           join d in db.Dependencia on da.idDependencia equals d.id
                           where d.id == depe
                           orderby s.descripcion
                           select s;

            return sectores.ToList();

        }
        



    }
}