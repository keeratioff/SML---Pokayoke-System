using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace SML___Pokayoke_System
{
    public partial class FormMainReport : Form
    {

        //ChildForm
        private Form ActiveForm;

        public FormMainReport()
        {
            InitializeComponent();
        }

        private void FormMainReport_Load(object sender, EventArgs e)
        {

        }

        #region "Function OpenChildForm"
        private void OpenChildForm(System.Windows.Forms.Form ChildForm)
        {
            if (ActiveForm != null)
            {
                ActiveForm.Close();
            }
            ActiveForm = ChildForm;
            ChildForm.TopLevel = false;
            ChildForm.FormBorderStyle = FormBorderStyle.None;
            ChildForm.Dock = DockStyle.Fill;
            panel2.Controls.Add(ChildForm);
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        #endregion

        private void pictureBoxwithdraw_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormReport());
        }

        private void pictureBoxstock_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormReport2());
        }


    }
}
