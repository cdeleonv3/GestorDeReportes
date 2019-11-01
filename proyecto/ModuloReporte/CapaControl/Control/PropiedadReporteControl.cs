﻿using System;
using System.Collections.Generic;
using capaDatoRpt.Conexion;
using capaDatoRpt.Entity;
using System.Data.Odbc;
using System.Windows.Forms;

namespace CapaControlRpt.Control
{
    class PropiedadReporteControl
    {
        private Transaccion transaccion = new Transaccion();

        public void insertarPropiedadReporte(PropiedadReporte propiedad)
        {
            try
            {
                String sComando = String.Format("INSERT INTO Tbl_Propiedad_Rpt VALUES ({0}, '{1}', {2}, {3}, {4}, {5}); ",
                    propiedad.REPORTE.REPORTE.ToString(), propiedad.USUARIO.USUARIO, propiedad.APLICACION.APLICACION,
                    propiedad.MODULO.MODULO.ToString(), propiedad.IMPRIMIR.ToString(), propiedad.ESTADO.ToString());
                this.transaccion.insertarDatos(sComando);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Error al insertar configuracion para PROPIEDADES.");
            }
        }

        public void modificarPropiedadReporte(PropiedadReporte propiedad)
        {
            try
            {
                String sComando = String.Format("UPDATE Tbl_Propiedad_Rpt " +
                    "SET PK_id_usuario = '{1}', PK_id_aplicacion = {2}, PK_id_modulo = {3}, imprimir = {4}, estado = {5}" +
                    "WHERE PK_id_reporte = {0}; ",
                   propiedad.REPORTE.REPORTE.ToString(), propiedad.USUARIO.USUARIO, propiedad.APLICACION.APLICACION,
                   propiedad.MODULO.MODULO.ToString(), propiedad.IMPRIMIR.ToString(), propiedad.ESTADO.ToString());
                this.transaccion.insertarDatos(sComando);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error al insertar configuracion para PROPIEDADES.");
            }
        }

        public PropiedadReporte obtenerPropiedadPorUsuarioAplicacion(int reporte, string usuario, int aplicacion, int modulo)
        {
            PropiedadReporte propiedad = new PropiedadReporte();
            ReporteControl rpt = new ReporteControl();
            Usuario usu = new Usuario();
            AplicacionControl app = new AplicacionControl();
            ModuloControl mdl = new ModuloControl();
            try
            {
                String sComando = String.Format("SELECT PK_id_reporte, PK_id_usuario, PK_id_aplicacion, PK_id_modulo, imprimir, estado FROM Tbl_Propiedad_Rpt " +
                    "WHERE PK_id_reporte={0} AND PK_id_usuario = {1} AND PK_id_aplicacion = {2} AND PK_id_modulo = {3} AND estado <> 0;",
                   reporte, usuario, aplicacion,modulo);

                OdbcDataReader reader = transaccion.ConsultarDatos(sComando);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        propiedad.REPORTE = rpt.obtenerReporte(reader.GetInt32(0));
                        propiedad.USUARIO.USUARIO = reader.GetString(1);
                        propiedad.APLICACION = app.obtenerAplicacion(reader.GetInt32(2), reader.GetInt32(3));
                        propiedad.MODULO = mdl.obtenerModulo(reader.GetInt32(3));
                        propiedad.IMPRIMIR = reader.GetInt32(4);
                        propiedad.ESTADO = reader.GetInt32(5);
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error al insertar configuracion para PROPIEDADES.");
            }
            return propiedad;
        }

    }
}