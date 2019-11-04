using internosSennaf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace internosSennaf.Servicios
{
    public class internoService
    {

        private internosSenafEntities db = new internosSenafEntities();

        public List<ListInterno> listadoGeneral(int idDependencia)
        {
            var listado = from i in db.Interno
                                  join s in db.Sector on i.idSector equals s.id
                                  join sp in db.Sector_Piso on s.id equals sp.idSector
                                  join da in db.Dependencia_Area on s.idArea equals da.idArea
                                  where da.idDependencia == idDependencia
                                  group s by s.descripcion into listadoAgrupado
                                  orderby listadoAgrupado.Select(xx => xx.Sector_Piso.Select(spsp => spsp.Piso.id).FirstOrDefault()).FirstOrDefault(), listadoAgrupado.Key
                                  select new ListInterno()
                                  {
                                      idDependencia = idDependencia,
                                      Piso = listadoAgrupado.Select(x => x.Sector_Piso.Select(sp => sp.Piso.numero).FirstOrDefault()).FirstOrDefault(),
                                      Sector = listadoAgrupado.Key,
                                      Referente = listadoAgrupado.Select(referente => referente.referente).FirstOrDefault(),
                                      Internos = listadoAgrupado.Select(pp => pp.Interno).FirstOrDefault()
                                  };

            return listado.ToList();

        }

        

    }
}