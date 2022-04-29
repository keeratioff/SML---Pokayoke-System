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
using Microsoft.VisualBasic.FileIO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using ExcelDataReader;

namespace SML___Pokayoke_System
{
    public partial class FormRegister : Form
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

        public static bool status_sql;

        //Location Config file temp
        public static string Location_File_Tmp;

        public FormRegister()
        {
            InitializeComponent();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file local.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
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

        #region "Transaction"
        private static bool ExecuteSqlTransaction(string cmd, string connectionString, string Transaction)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction(Transaction);
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = cmd;
                    status_sql = Convert.ToBoolean(command.ExecuteNonQuery());
                    transaction.Commit();
                    _ = new LogWriter.LogWriter("Data record are written to database.");
                    return true;
                }
                catch (Exception ex)
                {
                    _ = new LogWriter.LogWriter($"Commit Exeption Type: {0}, {ex.GetType()}");
                    _ = new LogWriter.LogWriter($"   Message: {0}, {ex.Message}");
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        _ = new LogWriter.LogWriter($"Rollback Exception Type: {0}, {ex2.GetType()}");
                        _ = new LogWriter.LogWriter($"  Message: {0}, {ex2.Message}");
                    }
                    return false;
                }
            }
        }
        #endregion

        #region "Button Close"
        private void Buttonclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region "Button Signup"
        private void Buttonsignup_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxfirstname.Text == "" || textBoxfirstname.Text == null || textBoxlastname.Text == "" || textBoxlastname.Text == null || textBoxusername.Text == "" || textBoxusername.Text == null || textBoxpassword.Text == "" || textBoxpassword.Text == null)
                {
                    MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From user_local where employee_id = '{textBoxusername}' and working = 1";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_rows = dt.Rows.Count;
                    if (count_rows >= 1)
                    {
                        MessageBox.Show("User : " + textBoxusername.Text + " มีในระบบแล้ว, กรุณาตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var cmd = $"Insert into user_local (employee_id, password, name, surname, level, time_register, time_update, comment, working) " +
                            $"Values ('{textBoxusername.Text}', '{textBoxpassword.Text}', '{textBoxfirstname.Text}', '{textBoxlastname.Text}', 'User', Getdate(), '{null}', '{null}', '1')";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Add"))
                        {
                            MessageBox.Show("บันทึกสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBoxfirstname.Text = "First Name";
                            textBoxlastname.Text = "Last Name";
                            textBoxusername.Text = "User Name";
                            textBoxpassword.Text = "Password";
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        private void textBoxfirstname_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxfirstname.Text = "";
        }

        private void textBoxlastname_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxlastname.Text = "";
        }

        private void textBoxusername_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxusername.Text = "";
        }

        private void textBoxpassword_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxpassword.Text = "";
        }
    }
}
