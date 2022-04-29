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
using System.IO.Ports;

namespace SML___Pokayoke_System
{

    public partial class Form1 : Form
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

        //Transaction
        public static bool status_sql;

        //Location Config file temp
        public static string Location_File_Tmp;

        public static string emp_id;
        public static string emp_password;
        public static string emp_name;
        public static string emp_surname;
        public static string emp_level;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file local.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            textBoxusername.Text = "USERNAME";
            textBoxpassword.Text = "PASSWORD";
        }

        #region "Read System File"
        public static void Read_Systemfile(string Location)
        {
            string[] currentrow;
            try
            {
                TextFieldParser parser = new TextFieldParser(Location, Encoding.GetEncoding("utf-8"))
                {
                    TextFieldType = FieldType.Delimited
                };
                parser.SetDelimiters(";");
                while (parser.EndOfData == false)
                {
                    currentrow = parser.ReadFields();
                    Ip_Addr_Local = currentrow[1];
                    currentrow = parser.ReadFields();
                    Catalog_Local = currentrow[1];
                    currentrow = parser.ReadFields();
                    Sql_usr_Local = currentrow[1];
                    currentrow = parser.ReadFields();
                    Sql_pw_Local = currentrow[1];
                }
            }
            catch
            {
                _ = new LogWriter.LogWriter("Application can't open System_Local.txt, Maybe lost");
                Environment.Exit(0);
            }
        }
        #endregion

        #region "Button Exit"
        private void Buttonexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region "Button/Label Register"
        private void labelregister_Click(object sender, EventArgs e)
        {
            FormRegister formregister = new FormRegister();
            formregister.ShowDialog();
        }
        #endregion

        #region "TextBox User Name"
        private void textBoxusername_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxusername.Text = "";
        }
        #endregion



        #region "Button Login"
        private void Buttonlogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxusername.Text == "" || textBoxusername.Text == null || textBoxpassword.Text == "" || textBoxpassword.Text == null)
                {
                    MessageBox.Show("กรุณาใส่ข้อมูลในช่องว่างให้ครบ เพื่อดำเนินการต่อไป", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxusername.Text = "USERNAME";
                    textBoxpassword.Text = "PASSWORD";
                    return;
                }
                else
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"select * from user_local where employee_id = '{textBoxusername.Text}' And password = '{textBoxpassword.Text}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row == 0)
                    {
                        MessageBox.Show(" ไม่พบชื่อในระบบ โปรดตรวจสอบอีกครั้ง");
                        textBoxusername.Text = "USERNAME";
                        textBoxpassword.Text = "PASSWORD";
                        return;
                    }
                    else
                    {
                        emp_id = dt.Rows[0]["employee_id"].ToString();
                        emp_password = dt.Rows[0]["password"].ToString();
                        emp_name = dt.Rows[0]["name"].ToString();
                        emp_surname = dt.Rows[0]["surname"].ToString();
                        emp_level = dt.Rows[0]["level"].ToString();
                        Properties.Settings.Default.user_employee_id = Convert.ToInt32(emp_id);
                        Properties.Settings.Default.user_password = emp_password;
                        Properties.Settings.Default.user_name = emp_name;
                        Properties.Settings.Default.user_surname = emp_surname;
                        Properties.Settings.Default.user_level = emp_level;
                        Properties.Settings.Default.Save();
                        textBoxusername.Text = "USERNAME";
                        textBoxpassword.Text = "PASSWORD";
                        FormMain formmain = new FormMain();
                        formmain.ShowDialog();
                        _ = new LogWriter.LogWriter("ID: " + emp_id + " " + emp_name + " " + emp_surname + " Login Successful!" + " " + "DateTime: " + DateTime.Now);
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error FormWeight Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Key Press Enter"
        private void textBoxpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                Buttonlogin.PerformClick();
            }
        }

        #endregion

        private void textBoxpassword_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxpassword.UseSystemPasswordChar = true;
            textBoxpassword.Text = "";
        }
    }
}
