using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGC.UI
{
    public partial class FormAtencion : Form
    {
        public FormAtencion()
        {
            InitializeComponent();
            DtpFecha.Format = DateTimePickerFormat.Custom; // 1. Le decimos al control que vamos a usar un formato personalizado

            DtpFecha.CustomFormat = "dd/MM/yyyy"; // 2. Le decimos al control que el formato personalizado es "dd/MM/yyyy"

            DtpFecha.Value = DateTime.Now; // Esta línea setea el control con la fecha de hoy automáticamente
        }

        private void DtpFecha_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void FormAtencion_Load(object sender, EventArgs e)
        {

        }

        private void lblSello_Click(object sender, EventArgs e)
        {

        }

        private void FormAtencion_Load_1(object sender, EventArgs e)
        {

        }
    }
}
