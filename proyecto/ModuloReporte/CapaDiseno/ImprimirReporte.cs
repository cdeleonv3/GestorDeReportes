﻿using System;
using System.Collections.Generic;
using CapaControl.Control;
using capaDato.Entity;
using CapaDiseno.Dialogos;
using CapaDiseno.Procesos;

namespace CapaDiseno
{
    public class ImprimirReporte
    {
        public void imprimirReporteAplicacion(int Codmodulo, int Codaplicacion)
        {

            ReporteAplicacionControl datos = new ReporteAplicacionControl();

            ReporteAplicacion repApp = datos.obtenerReporteApp(Codmodulo, Codaplicacion);
            Reporte rpt = repApp.REPORTE;

            string pathFile = rpt.CONFIGURACION.RUTA + rpt.FILENAME;
            Frm_VistaReporte frmVistaRpt = new Frm_VistaReporte(pathFile);
            frmVistaRpt.Show();

        }

        public void imprimirReporteModulo(int codModulo, int codReporte)
        {
            ReporteModuloControl reporteMdlControl = new ReporteModuloControl();

            try
            {
                ReporteModulo reporteMdl = reporteMdlControl.obtenerReporteMdl(codModulo, codReporte);
                Reporte rpt = reporteMdl.REPORTE;

                string pathFile = rpt.CONFIGURACION.RUTA + rpt.FILENAME;
                Frm_VistaReporte frmVistaRpt = new Frm_VistaReporte(pathFile);
                frmVistaRpt.Show();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString() + "Error en impresion, no se pudo obtener reporte.");
            }
        }
    }
}
