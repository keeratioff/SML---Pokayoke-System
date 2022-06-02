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
        //public bool statusport = Properties.Settings.Default.statusport;


        public bool setting = false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            labelid.Text = emp_id;
            labelname.Text = emp_name;
            labellastname.Text = emp_surname;
            #region "Hiden button"
            Buttonaccessibility.Hide();
            ButtonAddpartlists.Hide();
            ButtonAddvinno.Hide();
            Buttonpartnumber_im.Hide();
            Buttonaddstock.Hide();
            rjButton1.Hide();
            rjButton2.Hide();
            rjButton3.Hide();
            rjButton4.Hide();
            rjButton5.Hide();
            #endregion
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
            Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
            Buttonaccessibility.BackColor = Color.Snow;
            ButtonAddpartlists.BackColor = Color.Snow;
            ButtonAddvinno.BackColor = Color.Snow;
            Buttonpartnumber_im.BackColor = Color.Snow;
            Buttonaddstock.BackColor = Color.Snow;
            Buttonaccessibility.Hide();
            ButtonAddpartlists.Hide();
            ButtonAddvinno.Hide();
            Buttonpartnumber_im.Hide();
            Buttonaddstock.Hide();
            rjButton1.Hide();
            rjButton2.Hide();
            rjButton3.Hide();
            rjButton4.Hide();
            rjButton5.Hide();
            setting = false;
            labelsubject.Text = "Master Store";
            OpenChildForm(new FormStore());
        }

        private void Buttonsetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (setting == false)
                {
                    Buttonaccessibility.Show();
                    ButtonAddpartlists.Show();
                    ButtonAddvinno.Show();
                    Buttonpartnumber_im.Show();
                    Buttonaddstock.Show();
                    rjButton1.Show();
                    rjButton2.Show();
                    rjButton3.Show();
                    rjButton4.Show();
                    rjButton5.Show();
                    Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonsetting.BackColor = Color.Blue;
                    Buttonaccessibility.BackColor = Color.Snow;
                    ButtonAddpartlists.BackColor = Color.Snow;
                    ButtonAddvinno.BackColor = Color.Snow;
                    Buttonpartnumber_im.BackColor = Color.Snow;
                    Buttonaddstock.BackColor = Color.Snow;
                    labelsubject.Text = "Setting";
                    setting = true;
                }
                else if (setting == true)
                {
                    Buttonaccessibility.Hide();
                    ButtonAddpartlists.Hide();
                    ButtonAddvinno.Hide();
                    Buttonpartnumber_im.Hide();
                    Buttonaddstock.Hide();
                    rjButton1.Hide();
                    rjButton2.Hide();
                    rjButton3.Hide();
                    rjButton4.Hide();
                    rjButton5.Hide();
                    Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonsetting.BackColor = Color.FromArgb(2, 48, 71);
                    Buttonaccessibility.BackColor = Color.Snow;
                    ButtonAddpartlists.BackColor = Color.Snow;
                    ButtonAddvinno.BackColor = Color.Snow;
                    Buttonpartnumber_im.BackColor = Color.Snow;
                    Buttonaddstock.BackColor = Color.Snow;
                    setting = false;
                }

                //if (emp_level == "User")
                //{
                //    if (setting == false)
                //    {
                //        Buttonaccessibility.Show();
                //        ButtonAddpartlists.Show();
                //        ButtonAddvinno.Show();
                //        Buttonpartnumber_im.Show();
                //        Buttonaddstock.Show();
                //        rjButton1.Show();
                //        rjButton2.Show();
                //        rjButton3.Show();
                //        rjButton4.Show();
                //        rjButton5.Show();
                //        Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
                //        Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
                //        Buttonsetting.BackColor = Color.Blue;
                //        Buttonaccessibility.BackColor = Color.Snow;
                //        ButtonAddpartlists.BackColor = Color.Snow;
                //        ButtonAddvinno.BackColor = Color.Snow;
                //        Buttonpartnumber_im.BackColor = Color.Snow;
                //        Buttonaddstock.BackColor = Color.Snow;
                //        labelsubject.Text = "Setting";
                //        setting = true;
                //    }
                //    else if (setting == true)
                //    {
                //        Buttonaccessibility.Hide();
                //        ButtonAddpartlists.Hide();
                //        ButtonAddvinno.Hide();
                //        Buttonpartnumber_im.Hide();
                //        Buttonaddstock.Hide();
                //        rjButton1.Hide();
                //        rjButton2.Hide();
                //        rjButton3.Hide();
                //        rjButton4.Hide();
                //        rjButton5.Hide();
                //        Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
                //        Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
                //        Buttonsetting.BackColor = Color.FromArgb(2, 48, 71);
                //        Buttonaccessibility.BackColor = Color.Snow;
                //        ButtonAddpartlists.BackColor = Color.Snow;
                //        ButtonAddvinno.BackColor = Color.Snow;
                //        Buttonpartnumber_im.BackColor = Color.Snow;
                //        Buttonaddstock.BackColor = Color.Snow;
                //        setting = false;
                //    }
                //}
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
            Buttonreport.BackColor = Color.Blue;
            Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
            Buttonsetting.BackColor = Color.FromArgb(2, 48, 71);
            Buttonaccessibility.BackColor = Color.Snow;
            ButtonAddpartlists.BackColor = Color.Snow;
            ButtonAddvinno.BackColor = Color.Snow;
            Buttonpartnumber_im.BackColor = Color.Snow;
            Buttonaddstock.BackColor = Color.Snow;

            Buttonaccessibility.Hide();
            ButtonAddpartlists.Hide();
            ButtonAddvinno.Hide();
            Buttonpartnumber_im.Hide();
            Buttonaddstock.Hide();
            rjButton1.Hide();
            rjButton2.Hide();
            rjButton3.Hide();
            rjButton4.Hide();
            rjButton5.Hide();
            setting = false;
            labelsubject.Text = "Report";
            //Properties.Settings.Default.statusport = true;
            //Properties.Settings.Default.Save();
            //OpenChildForm(new FormReport());
            OpenChildForm(new FormMainReport());
        }
        #endregion

        #region "Button OpenChidForm Setting"
        private void Buttonaccessibility_Click(object sender, EventArgs e)
        {
            //Buttonaccessibility.BackColor = Color.Blue;
            //ButtonAddpartlists.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonpartnumber_im.BackColor = Color.FromArgb(2, 48, 71);
            //ButtonAddvinno.BackColor = Color.FromArgb(2, 48, 71);


            Buttonaccessibility.BackColor = Color.LightSteelBlue;
            ButtonAddpartlists.BackColor = Color.Snow;
            Buttonpartnumber_im.BackColor = Color.Snow;
            ButtonAddvinno.BackColor = Color.Snow;
            Buttonaddstock.BackColor = Color.Snow;
            OpenChildForm(new FormSettingAccessibility());
        }

        private void ButtonAddpartlists_Click(object sender, EventArgs e)
        {
            //Buttonaccessibility.BackColor = Color.FromArgb(2, 48, 71);
            //ButtonAddpartlists.BackColor = Color.Blue;
            //Buttonpartnumber_im.BackColor = Color.FromArgb(2, 48, 71);
            //ButtonAddvinno.BackColor = Color.FromArgb(2, 48, 71);

            Buttonaccessibility.BackColor = Color.Snow;
            ButtonAddpartlists.BackColor = Color.LightSteelBlue;
            Buttonpartnumber_im.BackColor = Color.Snow;
            ButtonAddvinno.BackColor = Color.Snow;
            Buttonaddstock.BackColor = Color.Snow;
            OpenChildForm(new FormAddpastlists());
            #region "Hiden Code"
            //try
            //{
            //    if (emp_level == "Admin")
            //    {
            //        Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
            //        Buttonsetting.BackColor = Color.FromArgb(2, 48, 71);
            //        ButtonAddvinno.BackColor = Color.FromArgb(2, 48, 71);
            //        ButtonAddpartlists.BackColor = Color.Blue;
            //        Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
            //        labelsubject.Text = "Add Part lists";
            //        OpenChildForm(new FormAddpastlists());
            //    }
            //    else
            //    {
            //        MessageBox.Show("คุณไม่มีสิทธิในการเข้าถึงข้อมูล โปรดติดต่อผู้ดูแลระบบ", "Warning");
            //    }
            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            //}
            #endregion
        }

        private void Buttonpartnumber_im_Click(object sender, EventArgs e)
        {
            //Buttonaccessibility.BackColor = Color.FromArgb(2, 48, 71);
            //ButtonAddpartlists.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonpartnumber_im.BackColor = Color.Blue;
            //ButtonAddvinno.BackColor = Color.FromArgb(2, 48, 71);

            Buttonaccessibility.BackColor = Color.Snow;
            ButtonAddpartlists.BackColor = Color.Snow;
            Buttonpartnumber_im.BackColor = Color.LightSteelBlue;
            ButtonAddvinno.BackColor = Color.Snow;
            Buttonaddstock.BackColor = Color.Snow;
            OpenChildForm(new FormSettingPartNumber());
        }

        private void ButtonAddvinno_Click(object sender, EventArgs e)
        {
            //Buttonaccessibility.BackColor = Color.FromArgb(2, 48, 71);
            //ButtonAddpartlists.BackColor = Color.FromArgb(2, 48, 71);
            //Buttonpartnumber_im.BackColor = Color.FromArgb(2, 48, 71);
            //ButtonAddvinno.BackColor = Color.Blue;

            Buttonaccessibility.BackColor = Color.Snow;
            ButtonAddpartlists.BackColor = Color.Snow;
            Buttonpartnumber_im.BackColor = Color.Snow;
            ButtonAddvinno.BackColor = Color.LightSteelBlue;
            Buttonaddstock.BackColor = Color.Snow;
            OpenChildForm(new FormAddvinno());
            #region "Hiden Code"
            //try
            //{
            //    if (emp_level == "Admin")
            //    {
            //        Buttonstore.BackColor = Color.FromArgb(2, 48, 71);
            //        Buttonsetting.BackColor = Color.FromArgb(2, 48, 71);
            //        ButtonAddvinno.BackColor = Color.Blue;
            //        ButtonAddpartlists.BackColor = Color.FromArgb(2, 48, 71);
            //        Buttonreport.BackColor = Color.FromArgb(2, 48, 71);
            //        labelsubject.Text = "Add Vin Number";
            //        OpenChildForm(new FormAddvinno());
            //    }
            //    else
            //    {
            //        MessageBox.Show("คุณไม่มีสิทธิในการเข้าถึงข้อมูล โปรดติดต่อผู้ดูแลระบบ", "Warning");
            //    }
            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            //}
            #endregion
        }
        #endregion

        private void Buttonpartnumber_im_Click_1(object sender, EventArgs e)
        {
            Buttonaccessibility.BackColor = Color.Snow;
            ButtonAddpartlists.BackColor = Color.Snow;
            Buttonpartnumber_im.BackColor = Color.LightSteelBlue;
            ButtonAddvinno.BackColor = Color.Snow;
            Buttonaddstock.BackColor = Color.Snow;
            OpenChildForm(new FormSettingPartNumber());
        }

        private void Buttonaddstock_Click(object sender, EventArgs e)
        {
            Buttonaccessibility.BackColor = Color.Snow;
            ButtonAddpartlists.BackColor = Color.Snow;
            Buttonpartnumber_im.BackColor = Color.Snow;
            ButtonAddvinno.BackColor = Color.Snow;
            Buttonaddstock.BackColor = Color.LightSteelBlue;
            //OpenChildForm(new FormSettingAddStock());
            OpenChildForm(new FormMainSettingAddStock());
        }
    }
}
