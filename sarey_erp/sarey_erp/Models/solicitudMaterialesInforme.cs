using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sarey_erp.Models
{
    public class solicitudMaterialesInforme
    {
        public string numeroSolicitud { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public string nombreSolicitante { get; set; }
        public string faena { get; set; }
        public string partidasPresupuesto { get; set; }
        public string cantidad { get; set; }
        public string unidad { get; set; }
        public string item { get; set; }
        public string dimensiones { get; set; }
        public string partida { get; set; }

        public string nombreRevision { get; set; }
        public string fechaRevision { get; set; }

        public static List<solicitudMaterialesInforme> convertirSolicitudEnSolicitudInforme(solicitud_materiales sol)
        {
            List<solicitudMaterialesInforme> retorno = new List<solicitudMaterialesInforme>();
            List<itemSolicitudMateriales> listaItems = sol.ObtenerItemsDeSolicitudMateriales();

            for (int i = 0; i < listaItems.Count; i++)
            {
                solicitudMaterialesInforme temp = new solicitudMaterialesInforme();
                temp.numeroSolicitud = sol.id_solicitud;
                temp.fechaSolicitud = sol.fecha;
                temp.nombreSolicitante = usuarios.obtenerUsuario(sol.nombre_solicitante).nombreCompleto;
                temp.faena = sol.id_faena;
                temp.partidasPresupuesto = sol.obtenerPresupuestoPartidaFaenaSolicitud();

                char[] caracteres = temp.partidasPresupuesto.ToCharArray();
                temp.partidasPresupuesto = "";
                for (int j = caracteres.Length - 1; j >= 0; j--) 
                {
                    temp.partidasPresupuesto = caracteres[j] + temp.partidasPresupuesto;
                    if (j % 3 == 0 && j != 0) temp.partidasPresupuesto = '.' + temp.partidasPresupuesto;
                }

                temp.cantidad = listaItems[i].cantidad;
                temp.unidad = listaItems[i].unidad;
                temp.item = listaItems[i].nombre_item;
                temp.dimensiones = listaItems[i].dimensiones;
                temp.partida = listaItems[i].id_partida;

                if (sol.nombre_revision_oficina.Equals(""))
                {
                    temp.nombreRevision = "Aún no ha sido revisado";
                    temp.fechaRevision = "";
                }
                else
                {
                    temp.nombreRevision = usuarios.obtenerUsuario(sol.nombre_revision_oficina).nombreCompleto;
                    temp.fechaRevision = sol.fecha_revision_oficina.ToString("d");
                }


                retorno.Add(temp);
            }

            return retorno;
        }
    }

    public class detallePartidas 
    {
        public string nombre { get; set; }
        public string codigo { get; set; }

        public static List<detallePartidas> obtenerDetallePartidasSolicitud(solicitud_materiales solicitud)
        {
            List<detallePartidas> retorno = new List<detallePartidas>();

            List<itemSolicitudMateriales> detalle=solicitud.ObtenerItemsDeSolicitudMateriales();

            for (int i = 0; i < detalle.Count; i++) 
            {
                detallePartidas nueva = new detallePartidas();

                nueva.codigo = detalle[i].id_partida + " - " + detalle[i].numero_item_partida;
                nueva.nombre = detalle[i].nombre_item_partida;

                if(!retorno.Contains(nueva))retorno.Add(nueva);
            }

            return retorno;
        }
    }
}