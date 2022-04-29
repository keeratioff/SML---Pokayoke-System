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

namespace SML___Pokayoke_System
{
    public partial class FormSettingModel : Form
    {
        //Database
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;

        //Location Config file temp
        public static string Location_File_Tmp;
        public static bool status_sql;

        public FormSettingModel()
        {
            InitializeComponent();
        }

        private void FormSettingModel_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            Create_table();
            ShowGrid();
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
            metroGrid1.ColumnCount = 6;
            metroGrid1.Columns[0].HeaderText = "Model Base";
            metroGrid1.Columns[1].HeaderText = "Model Code";
            metroGrid1.Columns[2].HeaderText = "Buyer Code";
            metroGrid1.Columns[3].HeaderText = "Part Number";
            metroGrid1.Columns[4].HeaderText = "Qty";
            metroGrid1.Columns[5].HeaderText = "Operation Name";
        }
        #endregion

        #region "Function ShowGrid"
        private void ShowGrid()
        {
            metroGrid1.Rows.Clear();
            var dt = new DataTable();
            using (var conn = new SqlConnection(Local_Conn))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "Select * from data_master_list_local Where working = '1'";
                var sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            foreach (DataRow dr in dt.Rows)
            {
                metroGrid1.Rows.Add(dr["model_base"], dr["model_code"], dr["buyer_code"], dr["part_no_sync"], dr["part_qty"], dr["operation_name"]);
            }
        }
        #endregion

        private void Buttonadd_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    const string message = "คุณแน่ใจหรือไม่ ว่าต้องการเพิ่มโมเดลนี้ในฐานข้อมูล?";
            //    const string caption = "Add Model to Database";
            //    var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (result == DialogResult.Yes)
            //    {
            //        if (textBoxmodelbase.Text == "" || textBoxmodelbase.Text == null || textBoxmodelcode.Text == "" || textBoxmodelcode.Text == null || textBoxbuyercode.Text == "" || textBoxbuyercode.Text == null || textBoxpartnumber.Text == "" || textBoxpartnumber.Text == null || textBoxoperationname.Text == "" || textBoxoperationname.Text == null || textBoxqty.Text == "" || textBoxqty.Text == null)
            //        {
            //            MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        }
            //        else
            //        {
            //            string add_model_base = textBoxmodelbase.Text;
            //            string add_model_code = textBoxmodelcode.Text;
            //            string add_buyer_code = textBoxbuyercode.Text;
            //            string add_part_number = textBoxpartnumber.Text;
            //            string add_operationname = textBoxoperationname.Text;
            //            string add_qty = textBoxqty.Text;

            //            var dt = new DataTable();
            //            using (var conn = new SqlConnection(Local_Conn))
            //            {
            //                var check = conn.CreateCommand();
            //                check.CommandText = $"Select * from user_local where employee_id = '{textBoxusername.Text}' and working = '1'";
            //                var sda = new SqlDataAdapter(check);
            //                sda.Fill(dt);
            //            }
            //            int count_rows = dt.Rows.Count;
            //            if (count_rows >= 1)
            //            {
            //                MessageBox.Show("พบ ID พนักงานเหมืยนกันกับในฐานข้อมูล โปรดอัปเดตหรือใช้ ID ใหม่เพื่อเพิ่ม", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                textBoxfirstname.Text = "";
            //                textBoxlastname.Text = "";
            //                textBoxusername.Text = "";
            //                textBoxpassword.Text = "";
            //                radioButtonadmin.Checked = false;
            //                radioButtonuser.Checked = false;
            //                Create_table();
            //                ShowGrid();
            //            }
            //            else
            //            {
            //                var cmd = $"Insert into user_local (employee_id, password, name, surname, level, time_register, time_update, comment, working) " +
            //                    $"Values ('{add_username}', '{add_password}', '{add_firstname}', '{add_lastname}', '{Permission}', 'Getdate()', 'Getdate()', 'System', '1')";
            //                if (ExecuteSqlTransaction(cmd, Local_Conn, "Add"))
            //                {
            //                    MessageBox.Show("เพิ่มข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    textBoxfirstname.Text = "";
            //                    textBoxlastname.Text = "";
            //                    textBoxusername.Text = "";
            //                    textBoxpassword.Text = "";
            //                    radioButtonadmin.Checked = false;
            //                    radioButtonuser.Checked = false;
            //                    Create_table();
            //                    ShowGrid();
            //                }
            //                else
            //                {
            //                    MessageBox.Show(" ไม่สามารถเพิ่ม ID พนักงานได้, กรุณาลองใหม่อีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            //}
        }

        private void Buttonupdate_Click(object sender, EventArgs e)
        {

        }


        private void Buttondelete_Click(object sender, EventArgs e)
        {

        }

        private void Buttonclear_Click(object sender, EventArgs e)
        {
            textBoxmodelbase.Text = "";
            textBoxmodelcode.Text = "";
            textBoxbuyercode.Text = "";
            textBoxpartnumber.Text = "";
            textBoxoperationname.Text = "";
            textBoxqty.Text = "";
            Create_table();
            ShowGrid();
        }
    }
}
