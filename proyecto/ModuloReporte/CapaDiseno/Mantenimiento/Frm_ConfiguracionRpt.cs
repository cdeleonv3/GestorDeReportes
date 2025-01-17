﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaDisenoRpt.Mantenimiento
{
    public partial class Frm_ConfiguracionRpt : Form
    {
        string usuario;
        public Frm_ConfiguracionRpt()
        {
            InitializeComponent();
            inicializarNavegador();
        }

        private void inicializarNavegador()
        {
            //incializacion de navegador
            usuario = "admin";
            string[] alias = { "Cod", "Configuracion", "Ruta", "Estado"};
            navegador1.asignarAlias(alias);
            navegador1.asignarAyuda("1");
            navegador1.asignarSalida(this);
            navegador1.asignarColorFondo(Color.White);
            navegador1.asignarColorFuente(Color.Black);
            navegador1.asignarTabla("Tbl_configuracion_rpt");
            navegador1.asignarNombreForm("BODEGA");
            navegador1.ObtenerIdAplicacion("1");
        }

        private void Navegador1_Load(object sender, EventArgs e)
        {
            string aplicacionActiva = "1";
            navegador1.ObtenerIdUsuario(usuario);
            navegador1.botonesYPermisosInicial(usuario, aplicacionActiva);
            navegador1.ObtenerIdAplicacion(aplicacionActiva);
        }
    }
}
