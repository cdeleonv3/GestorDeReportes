﻿using System;
using System.Windows.Forms;
using capaDatoRpt.Entity;
using CapaControlRpt.Control;
using CapaDisenoRpt.Dialogos;
using System.Collections.Generic;
using CapaDatos;

namespace CapaDisenoRpt.Mantenimiento
{
    public partial class Frm_RptMdl : Form
    {
        private ReporteModuloControl reporteMdlControl = new ReporteModuloControl();
        private PropiedadReporteControl propiedadReporteControl = new PropiedadReporteControl();
        private ReporteModulo reporteMdl;
        private PropiedadReporte propiedadReporte;
        private string accion;
        public string usuario;
        

        public Frm_RptMdl(string sidUsuario)
        {
            InitializeComponent();
            deshabilitarBotones();
            this.StartPosition = FormStartPosition.CenterScreen;
            llenarDgv();
            iniciazliarTbpConsulta();
            this.usuario = sidUsuario;
        }

        private void habilitarBotones()
        {
            Btn_Borrar.Enabled = true;
            Btn_Cancelar.Enabled = true;
            Btn_Guardar.Enabled = true;
            Btn_Modificar.Enabled = true;
            Btn_Nuevo.Enabled = false;
        }
        private void deshabilitarBotones()
        {
            Btn_Borrar.Enabled = false;
            Btn_Cancelar.Enabled = false;
            Btn_Guardar.Enabled = false;
            Btn_Modificar.Enabled = false;
            Btn_Nuevo.Enabled = true;
        }
        private void llenarDgv()
        {
            int fila = 0;
            Dgv_Consulta.Rows.Clear();
            foreach (ReporteModulo reporteMdlTmp in reporteMdlControl.obtenerAllReporteMdl())
            {
                Dgv_Consulta.Rows.Add();
                Dgv_Consulta.Rows[fila].Cells[0].Value = reporteMdlTmp.REPORTE.REPORTE;
                Dgv_Consulta.Rows[fila].Cells[1].Value = reporteMdlTmp.REPORTE.NOMBRE;
                Dgv_Consulta.Rows[fila].Cells[2].Value = reporteMdlTmp.MODULO.MODULO;
                Dgv_Consulta.Rows[fila].Cells[3].Value = reporteMdlTmp.MODULO.NOMBRE;
                Dgv_Consulta.Rows[fila].Cells[4].Value = reporteMdlTmp.ESTADO.ToString();
                fila++;
            }
        }
        public void llenarCmbReporte()
        {
            ReporteControl reporteControl = new ReporteControl();
            List<Reporte> reporteList = reporteControl.obtenerAllReporte();

            Cmb_Reporte.ValueMember = "REPORTE";
            Cmb_Reporte.DisplayMember = "NOMBRE";
            Cmb_Reporte.DataSource = reporteList;
        }

        public void llenarCmbModulo()
        {
            ModuloControl moduloControl = new ModuloControl();
            List<Modulo> moduloList = moduloControl.obtenerAllModulo();

            Cmb_Modulo.ValueMember = "MODULO";
            Cmb_Modulo.DisplayMember = "NOMBRE";
            Cmb_Modulo.DataSource = moduloList;
        }

        private void habilitarCampos()
        {
            Cmb_Reporte.Enabled = true;
            Cmb_Modulo.Enabled = true;
            Txt_Estado.Enabled = true;
        }

        private void deshabilitarCampos()
        {
            Cmb_Reporte.Enabled = false;
            Cmb_Modulo.Enabled = false;
            Txt_Estado.Enabled = false;
        }

        private void iniciazliarTbpConsulta()
        {
            habilitarCampos();
            llenarCmbReporte();
            llenarCmbModulo();
            Txt_Estado.Text = "1";
        }

        private PropiedadReporte llenarPropiedadRpt()
        {
            PropiedadReporte propiedadReporte = new PropiedadReporte();
            Usuario usu = new Usuario();
            usu.USUARIO = usuario;

            propiedadReporte.USUARIO = usu;
            propiedadReporte.REPORTE = (Reporte)Cmb_Reporte.SelectedItem;
            propiedadReporte.MODULO = (Modulo)Cmb_Modulo.SelectedItem;
            propiedadReporte.APLICACION = null;
            propiedadReporte.ESTADO = 1;

            return propiedadReporte;
        }
        private ReporteModulo llenarReporteMdl()
        {
            ReporteModulo reporteMdlTmp = new ReporteModulo();
            reporteMdlTmp.REPORTE = (Reporte)Cmb_Reporte.SelectedItem;
            reporteMdlTmp.MODULO = (Modulo)Cmb_Modulo.SelectedItem;
            reporteMdlTmp.ESTADO = int.Parse(Txt_Estado.Text);

            return reporteMdlTmp;
        }

        private void llenarTbpDato(ReporteModulo reporteMdl)
        {
            deshabilitarCampos();

            ConfiguracionRptControl confControl = new ConfiguracionRptControl();

            Cmb_Reporte.Text = reporteMdl.REPORTE.NOMBRE;
            Cmb_Reporte.SelectedText = Cmb_Reporte.Text;

            Cmb_Modulo.Text = reporteMdl.MODULO.NOMBRE;
            Cmb_Modulo.SelectedText = Cmb_Modulo.Text;

            Txt_Estado.Text = reporteMdl.ESTADO.ToString();
        }

        /*
         * Programacion botones.
         */

        private void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            habilitarBotones();
            iniciazliarTbpConsulta();
            this.reporteMdl = new ReporteModulo();
            Tbc_RptMdl.SelectedTab = Tbp_Datos;
            this.accion = "nuevo";
            sentencia s = new sentencia(usuario);
            s.insertarBitacora(usuario, "Creacion reporte modulo", "Tbl_Modulo");
        }

        private void Btn_Modificar_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            this.reporteMdl = llenarReporteMdl();
            this.accion = "modificar";
            sentencia s = new sentencia(usuario);
            s.insertarBitacora(usuario, "Modificacion reporte modulo", "Tbl_Modulo");
        }

        private void Btn_Guardar_Click(object sender, EventArgs e)
        {

            deshabilitarBotones();
            this.reporteMdl = llenarReporteMdl();
            this.propiedadReporte = llenarPropiedadRpt();
            sentencia s = new sentencia(usuario);
            Dialogo dialogo = new Dialogo();
            bool confirmacion = dialogo.dialogoSiNo("Confirmacion", "Desea guardar?");
            if (confirmacion)
            {
                if (this.accion == "nuevo")
                {
                    reporteMdlControl.insertarReporteMdl(this.reporteMdl);
                    propiedadReporteControl.insertarPropiedadReporteSinApp(this.propiedadReporte);
                }
                else if (this.accion == "modificar")
                {
                    reporteMdlControl.actualizarReporteMdl(this.reporteMdl);
                }

                iniciazliarTbpConsulta();
                Tbc_RptMdl.SelectedTab = Tbp_Consulta;
                this.reporteMdl = new ReporteModulo();
                llenarDgv();
                s.insertarBitacora(usuario, "Guardar reporte modulo", "Tbl_Modulo");
            }
        }

        private void Btn_Borrar_Click(object sender, EventArgs e)
        {
            deshabilitarBotones();
            this.propiedadReporte = llenarPropiedadRpt();
            propiedadReporte.ESTADO = 0;
            this.accion = null;
            Dialogo dialogo = new Dialogo();
            bool confirmacion = dialogo.dialogoSiNo("Confirmacion", "Desea eliminar?");

            if (confirmacion)
            {
                propiedadReporteControl.modificarPropiedadReporteSinApp(propiedadReporte);
                reporteMdlControl.eliminarReporteMdl(this.reporteMdl.MODULO.MODULO, this.reporteMdl.REPORTE.REPORTE);
                this.reporteMdl = new ReporteModulo();

                iniciazliarTbpConsulta();
                Tbc_RptMdl.SelectedTab = Tbp_Consulta;
                llenarDgv();
            }
        }

        private void Btn_Cancelar_Click(object sender, EventArgs e)
        {
            deshabilitarBotones();
            iniciazliarTbpConsulta();
            Tbc_RptMdl.SelectedTab = Tbp_Consulta;
            this.reporteMdl = new ReporteModulo();
            llenarDgv();
        }

        /*
         * Seleccionar registro.
         */

        private void Dgv_Consulta_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            habilitarBotones();
            int fila = Dgv_Consulta.CurrentCell.RowIndex;
            String codigoRpt = Dgv_Consulta.Rows[fila].Cells[0].Value.ToString();
            String codigoMdl = Dgv_Consulta.Rows[fila].Cells[2].Value.ToString();
            this.reporteMdl = reporteMdlControl.obtenerReporteMdl(Int32.Parse(codigoMdl), Int32.Parse(codigoRpt));
            llenarTbpDato(this.reporteMdl);
            Tbc_RptMdl.SelectedTab = Tbp_Datos;
        }

    }
}
