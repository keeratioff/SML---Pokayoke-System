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
    public partial class FormAddmodel : Form
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

        //Login
        public string emp_id = Properties.Settings.Default.user_employee_id.ToString();
        public string emp_password = Properties.Settings.Default.user_password.ToString();
        public string emp_name = Properties.Settings.Default.user_name.ToString();
        public string emp_surname = Properties.Settings.Default.user_surname.ToString();

        //Location Config file temp
        public static string Location_File_Tmp;

        public int count_row_button = 0;

        public FormAddmodel()
        {
            InitializeComponent();
        }

        private void FormAddmodel_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file local.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            Create_table();
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

        #region "Function Create table"
        private void Create_table()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid1.ColumnCount = 2;

            metroGrid1.Columns[0].HeaderText = "Part no";
            metroGrid1.Columns[1].HeaderText = "Quantity";
        }
        #endregion

        #region "Text Changed Vin no"
        private void textBoxvinno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxvinno.TextLength >= 11)
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * from data_vlm_local where vin_no = '{textBoxvinno.Text}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row == 0)
                    {
                        textBoxvinno.Text = "";
                        MessageBox.Show(" ไม่พบ Vin No : " + textBoxvinno.Text + " โปรดตรวจสอบอีกครั้ง");
                        return;
                    }
                    else
                    {
                        string model_prefix = dt.Rows[0]["model_prefix"].ToString();
                        string model_base = dt.Rows[0]["model_base"].ToString();
                        string model_suffix = dt.Rows[0]["model_suffix"].ToString();
                        string model_result = (model_prefix + model_base + model_suffix);
                        textBoxmodelcode.Text = model_result;
                        textBoxbuyercode.Text = dt.Rows[0]["buyer_code"].ToString();
                        textBoxmodelprefix.Text = model_prefix;
                        textBoxmodelbase.Text = model_base;
                        textBoxmodelsuffix.Text = model_suffix;
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($"  Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Button Clear"
        private void Buttonclear_Click(object sender, EventArgs e)
        {
            textBoxvinno.Text = "";
            textBoxmodelprefix.Text = "";
            textBoxmodelbase.Text = "";
            textBoxmodelsuffix.Text = "";
            textBoxmodelcode.Text = "";
            textBoxbuyercode.Text = "";
            textBoxpartno.Text = "";
        }
        #endregion

        private void buttonaddpartno_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxvinno.Text == "" || textBoxvinno.Text == null || textBoxmodelprefix.Text == "" || textBoxmodelprefix.Text == null || textBoxmodelbase.Text == "" || textBoxmodelbase.Text == null || textBoxmodelsuffix.Text == "" || textBoxmodelsuffix.Text == null || textBoxmodelcode.Text == "" || textBoxmodelcode.Text == null || textBoxbuyercode.Text == "" || textBoxbuyercode.Text == null || textBoxpartno.Text == "" || textBoxpartno.Text == null)
                {
                    MessageBox.Show("กรุณาใส่ข้อมูลให้ครบ, โปรดตรวจสอบอีกครั้ง");
                }
                else
                {
                    metroGrid1.Rows[count_row_button].Cells[0].Value = textBoxpartno.Text;
                    metroGrid1.Rows[count_row_button].Cells[1].Value = "1";
                    count_row_button++;
                }
            }
            catch
            {

            }

        }

        private void Buttonsave_Click(object sender, EventArgs e)
        {

        }
    }
}
