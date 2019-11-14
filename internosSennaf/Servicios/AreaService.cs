using internosSennaf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace internosSennaf.Servicios
{
    public class AreaService
    {

        internosSenafEntities db = new internosSenafEntities();

        public List<Area> areaXDependencia( int idDepe)
        {
            var listadoAreas = from a in db.Area
                               join da in db.Dependencia_Area on a.id equals da.idArea
                               join d in db.Dependencia on da.idDependencia equals d.id
                               select a;

            return listadoAreas.ToList();

        }

    }
}