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
    public partial class FormSetting : Form
    {
        //ChildForm
        private Form ActiveForm;

        public FormSetting()
        {
            InitializeComponent();
        }

        private void FormSetting_Load(object sender, EventArgs e)
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
            panelChildForm.Controls.Add(ChildForm);
            ChildForm.BringToFront();
            ChildForm.Show();
        }
        #endregion

        private void Buttonaccessibility_Click(object sender, EventArgs e)
        {
            Buttonaccessibility.BackColor = Color.Blue;
            //Buttonvinnumber.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonmodelcode.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonpartnumber.BackColor = Color.FromArgb(2, 48, 71);
            OpenChildForm(new FormSettingAccessibility());
        }

        private void Buttonvinnumber_Click(object sender, EventArgs e)
        {
            //Buttonaccessibility.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonvinnumber.BackColor = Color.Blue;
            //Buttonmodelcode.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonpartnumber.BackColor = Color.FromArgb(2, 48, 71);
            OpenChildForm(new FormAddvinno());
        }

        private void Buttonmodelcode_Click(object sender, EventArgs e)
        {
            //Buttonaccessibility.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonvinnumber.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonmodelcode.BackColor = Color.Blue;
            //Buttonpartnumber.BackColor = Color.FromArgb(2, 48, 71);
            OpenChildForm(new FormSettingModel());
        }

        private void Buttonpartnumber_Click(object sender, EventArgs e)
        {
            //Buttonaccessibility.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonvinnumber.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonmodelcode.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonpartnumber.BackColor = Color.Blue;
            OpenChildForm(new FormSettingPartNumber());
        }
    }
}
