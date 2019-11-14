using internosSennaf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace internosSennaf.Servicios
{

    public class DependenciaService
    {

        private internosSenafEntities db = new internosSenafEntities();

        public int? dependenciaXinterno(int? idInterno)
        {

            var depe = (from da in db.Dependencia_Area
                        join a in db.Area on da.idArea equals a.id
                        join s in db.Sector on a.id equals s.idArea
                        join i in db.Interno on s.id equals i.idSector
                        where i.id == idInterno
                        select da.idDependencia).FirstOrDefault();

            return depe;
        }


        public int? dependenciaXSector(int? idSector)
        {

            var depe = (from da in db.Dependencia_Area
                        join a in db.Area on da.idArea equals a.id
                        join s in db.Sector on a.id equals s.idArea
                        where s.id == idSector
                        select da.idDependencia).FirstOrDefault();

            return depe;
        }


    }
}