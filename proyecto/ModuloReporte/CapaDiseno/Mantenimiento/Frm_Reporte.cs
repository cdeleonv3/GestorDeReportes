﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using capaDatoRpt.Entity;
using CapaControlRpt.Control;
using CapaDisenoRpt.Dialogos;
using CapaDisenoRpt.Procesos;
using CapaDisenoRpt;

namespace CapaDisenoRpt.Mantenimiento
{
    public partial class Frm_Reporte : Form
    {
        private ReporteControl reporteControl = new ReporteControl();
        private Reporte reporte;
        private string accion;
        private String fileUpload;

        public Frm_Reporte()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            llenarDgv();
            llenarCmbConfiguracion();
        }

        private void llenarDgv()
        {
            int fila = 0;
            Dgv_Consulta.Rows.Clear();
            foreach (Reporte reporteTmp in reporteControl.obtenerAllReporte())
            {
                Dgv_Consulta.Rows.Add();
                Dgv_Consulta.Rows[fila].Cells[0].Value = reporteTmp.REPORTE.ToString();
                Dgv_Consulta.Rows[fila].Cells[1].Value = reporteTmp.NOMBRE;
            
                Dgv_Consulta.Rows[fila].Cells[2].Value = reporteTmp.FILENAME;
                Dgv_Consulta.Rows[fila].Cells[3].Value = reporteTmp.ESTADO.ToString();
                fila++;
            }
        }

        private void establecerID()
        {
            int reporte = reporteControl.obtenerUltimoId();
            Txt_Codigo.Text = reporte + 1 + "";
            Txt_Codigo.Enabled = false;
        }

        private void iniciazliarTbpConsulta()
        {
            habilitarCampos();

            Txt_Codigo.Text = "";
            Txt_Nombre.Text = "";
            //Cmb_Configuracion.SelectedIndex = 0;
            Txt_Archivo.Text = "";
            Txt_Estado.Text = "1";
        }

        private Reporte llenarReporte()
        {
            Reporte rptTmp = new Reporte();
            rptTmp.REPORTE = int.Parse(Txt_Codigo.Text);
            rptTmp.NOMBRE = Txt_Nombre.Text;    
           
            rptTmp.FILENAME = Txt_Archivo.Text;
            rptTmp.ESTADO = int.Parse(Txt_Estado.Text);
            
            return rptTmp;
        }

        private void deshabilitarCampos()
        {
            Txt_Codigo.Enabled = false;
            Txt_Nombre.Enabled = false;
           
            Txt_Archivo .Enabled = false;
            Txt_Estado.Enabled = false;
            Btn_SeleccionarFile.Enabled = false;
        }

        private void habilitarCampos()
        {
            Txt_Codigo.Enabled = false;//Deshabilitado ya que no se puede cambiar la llave primaria 
            Txt_Nombre.Enabled = true;
           
            Txt_Archivo.Enabled = true;
            Txt_Estado.Enabled = false;//Deshabilitado
            Btn_SeleccionarFile.Enabled = true;
        }

        private void llenarTbpDato(Reporte reporte)
        {

            deshabilitarCampos();
            Txt_Codigo.Text = reporte.REPORTE.ToString();
            Txt_Nombre.Text = reporte.NOMBRE;
            //      Cmb_Configuracion.SelectedItem = Cmb_Configuracion.Items[reporte.CONFIGURACION.CONFIGURACION];
        

            
          
            
            Txt_Archivo.Text = reporte.FILENAME;
            Txt_Estado.Text = reporte.ESTADO.ToString();
        }

        /*
         * Llenar combobox configuracion.
         */

        public void llenarCmbConfiguracion()
        {


            
           
        }

        /*
         * Buscar archivo.
         */
                 
        private void Btn_SeleccionarFile_Click(object sender, EventArgs e)
        {
            String[] nombre;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "allfiles|*.rpt";
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.fileUpload = openFileDialog.FileName.ToString();
                nombre = this.fileUpload.Split('\\');
                Txt_Archivo.Text = nombre[nombre.Length - 1];
            }
        }

        /*
         * Programacion botones.
         */

        private void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            iniciazliarTbpConsulta();
            establecerID();
            this.reporte = new Reporte();
            Tbc_Reporte.SelectedTab = Tbp_Datos;
            this.accion = "nuevo";
        }

        private void Btn_Cancelar_Click(object sender, EventArgs e)
        {
            iniciazliarTbpConsulta();
            Tbc_Reporte.SelectedTab = Tbp_Consulta;
            this.reporte = new Reporte();
            llenarDgv();
        }

        private void Btn_Guardar_Click(object sender, EventArgs e)
        {
            this.reporte = llenarReporte();

            Dialogo dialogo = new Dialogo();
            bool confirmacion = dialogo.dialogoSiNo("Confirmacion", "Desea guardar?");
            
            if (confirmacion)
            {
                try
                {
                    //UploadFile upload;
                    if (this.accion == "nuevo")
                    {
                        reporteControl.insertarReporte(this.reporte);
                        
                    }
                    else if (this.accion == "modificar")
                    {
                        reporteControl.actualizarReporte(this.reporte);
                        
                    }
                }
                catch
                {
                    MessageBox.Show("No se subio archivo.");
                }

                iniciazliarTbpConsulta();
                Tbc_Reporte.SelectedTab = Tbp_Consulta;
                this.reporte = new Reporte();
                llenarDgv();
            }
        }

        private void Btn_Modificar_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            this.reporte = llenarReporte();
            this.accion = "modificar";
        }

        private void Btn_Borrar_Click(object sender, EventArgs e)
        {
            this.accion = null;
            Dialogo dialogo = new Dialogo();
            bool confirmacion = dialogo.dialogoSiNo("Confirmacion", "Desea eliminar?");

            if (confirmacion)
            {
                reporteControl.eliminarReporte(this.reporte.REPORTE);
                this.reporte = new Reporte();

                iniciazliarTbpConsulta();
                Tbc_Reporte.SelectedTab = Tbp_Consulta;
                llenarDgv();
            }
        }

        /*
         * Seleccionar registro dgv.
         */

        private void seleccionarRegistro(object sender, DataGridViewCellEventArgs e)
        {
            int fila = Dgv_Consulta.CurrentCell.RowIndex;
            String codigoRpt = Dgv_Consulta.Rows[fila].Cells[0].Value.ToString();
            this.reporte = reporteControl.obtenerReporte(Int32.Parse(codigoRpt));
            llenarTbpDato(this.reporte);
            Tbc_Reporte.SelectedTab = Tbp_Datos;
        }

        private void Btn_PreView_Click(object sender, EventArgs e)
        {
            string pathFile = this.reporte.FILENAME;
            Frm_VistaReporte frmVistaRpt = new Frm_VistaReporte(pathFile);
            frmVistaRpt.Show();
            
            //ImprimirReporte imp = new ImprimirReporte(4, 2);
            
        }
    }
}
