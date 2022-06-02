using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SML___Pokayoke_System
{
    public partial class FormMainSettingAddStock : Form
    {

        //ChildForm
        private Form ActiveForm;

        public FormMainSettingAddStock()
        {
            InitializeComponent();
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

        private void Buttonstockon_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormSettingAddStock());
        }

        private void Buttoneditdatabaseon_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormSettingEditStock());
        }
    }
}
