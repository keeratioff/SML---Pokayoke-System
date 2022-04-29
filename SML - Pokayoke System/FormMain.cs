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
    public partial class FormMain : Form
    {

        //Connect Database Local
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;
        SqlConnection conn;
        SqlCommand com;
        DataTable dt;
        SqlDataAdapter adpt;

        //ChildForm
        private Form ActiveForm;

        //Login
        public string emp_id = Properties.Settings.Default.user_employee_id.ToString();
        public string emp_password = Properties.Settings.Default.user_password.ToString();
        public string emp_name = Properties.Settings.Default.user_name.ToString();
        public string emp_surname = Properties.Settings.Default.user_surname.ToString();
        public string emp_level = Properties.Settings.Default.user_level.ToString();

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            labelid.Text = emp_id;
            labelname.Text = emp_name;
            labellastname.Text = emp_surname;
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

        #region "Button Log Out"
        private void Buttonlogout_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.user_employee_id = Convert.ToInt32(null);
            Properties.Settings.Default.user_password = null;
            Properties.Settings.Default.user_name = null;
            Properties.Settings.Default.user_surname = null;
            Properties.Settings.Default.Save();
            this.Close();
        }
        #endregion

        #region "Button OpenChildForm"
        private void Buttonstore_Click(object sender, EventArgs e)
        {
            Buttonstore.BackColor = Color.Blue;
            Buttonsetting.BackColor = Color.FromArgb(2, 48, 71);
            ButtonAddvinno.BackColor = Color.FromArgb(2, 48, 71);
            ButtonAddpartlists.BackColor = Color.FromArgb(2, 48, 71);
            Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
            labelsubject.Text = "Master Store";
            OpenChildForm(new FormStore());
        }

        private void Buttonsetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (emp_level == "Admin")
                {
                    Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonsetting.BackColor = Color.Blue;
                    ButtonAddvinno.BackColor = Color.FromArgb(2, 48, 71);
                    ButtonAddpartlists.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
                    labelsubject.Text = "Setting";
                    OpenChildForm(new FormSetting());
                }
                else
                {
                    MessageBox.Show("คุณไม่มีสิทธิในการเข้าถึงข้อมูล โปรดติดต่อผู้ดูแลระบบ", "Warning");
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            }
        }

        private void ButtonAddvinno_Click(object sender, EventArgs e)
        {
            try
            {
                if (emp_level == "Admin")
                {
                    Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonsetting.BackColor = Color.FromArgb(2, 48, 71);
                    ButtonAddvinno.BackColor = Color.Blue;
                    ButtonAddpartlists.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
                    labelsubject.Text = "Add Vin Number";
                    OpenChildForm(new FormAddvinno());
                }
                else
                {
                    MessageBox.Show("คุณไม่มีสิทธิในการเข้าถึงข้อมูล โปรดติดต่อผู้ดูแลระบบ", "Warning");
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            }
        }

        private void ButtonAddpartlists_Click(object sender, EventArgs e)
        {
            try
            {
                if (emp_level == "Admin")
                {
                    Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonsetting.BackColor = Color.FromArgb(2, 48, 71);
                    ButtonAddvinno.BackColor = Color.FromArgb(2, 48, 71);
                    ButtonAddpartlists.BackColor = Color.Blue;
                    Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
                    labelsubject.Text = "Add Part lists";
                    OpenChildForm(new FormAddpastlists());
                }
                else
                {
                    MessageBox.Show("คุณไม่มีสิทธิในการเข้าถึงข้อมูล โปรดติดต่อผู้ดูแลระบบ", "Warning");
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            }
        }

        private void Buttonreport_Click(object sender, EventArgs e)
        {
            Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
            Buttonsetting.BackColor = Color.FromArgb(2, 48, 71);
            ButtonAddvinno.BackColor = Color.FromArgb(2, 48, 71);
            ButtonAddpartlists.BackColor = Color.FromArgb(2, 48, 71);
            Buttonreport.BackColor = Color.Blue;
            labelsubject.Text = "Report";
            OpenChildForm(new FormReport());
        }
        #endregion
    }
}
