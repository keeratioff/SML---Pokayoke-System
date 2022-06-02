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
using excel = Microsoft.Office.Interop.Excel;

namespace SML___Pokayoke_System
{
    public partial class FormSettingEditStock : Form
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
        public string emp_level = Properties.Settings.Default.user_level.ToString();

        //Location Config file temp
        public static string Location_File_Tmp;

        public FormSettingEditStock()
        {
            InitializeComponent();
        }

        private void FormSettingEditStock_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file local.txt");
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

        #region "Create table"
        private void Create_table()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid1.ColumnCount = 4;
            metroGrid1.Columns[0].HeaderText = "Part no";
            metroGrid1.Columns[1].HeaderText = "Part name";
            metroGrid1.Columns[2].HeaderText = "spine_code";
            metroGrid1.Columns[3].HeaderText = "Stock";
        }
        #endregion

        #region "ShowGrid"
        private void ShowGrid()
        {
            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $"Select * from data_part_local where working = '1'";
                    var sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    metroGrid1.Rows.Add(dr["part_no"], dr["part_name"], dr["spine_code"], dr["part_qty"]);
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Button Add"
        private void Buttonadd_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการเพิ่มโมเดลนี้ในฐานข้อมูล?";
                const string caption = "Add Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (textBoxeditpartno.Text == "" || textBoxeditpartno.Text == null || textBoxeditpartname.Text == "" || textBoxeditpartname.Text == null || textBoxeditstock.Text == "" || textBoxeditstock.Text == null)
                    {
                        MessageBox.Show(" กรุณากรอกข้อมูลทั้งหมดก่อนเริ่มกระบวนการต่อไป", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"select * from data_part_local where part_no = '{textBoxeditpartno.Text}' and working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int count_row = dt.Rows.Count;
                        if (count_row >= 1)
                        {
                            MessageBox.Show("พบข้อมูลเหมือนกันกับในฐานข้อมูล, โปรดตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBoxeditpartno.Text = "";
                            textBoxeditpartname.Text = "";
                            textBoxeditstock.Text = "";
                            textBoxeditspine_code.Text = "";
                            Create_table();
                            ShowGrid();
                        }
                        else
                        {
                            string part_no_add = textBoxeditpartno.Text;
                            string part_name_add = textBoxeditpartname.Text;
                            string spine_code_add = textBoxeditspine_code.Text;
                            int qty_stock_add = Convert.ToInt32(textBoxeditstock.Text);
                            var cmd = $"Insert into data_part_local (part_no, part_name, spine_code, part_qty, working) " +
                                $"Values ('{part_no_add}', '{part_name_add}', '{spine_code_add}', '{qty_stock_add}', '1')";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Add"))
                            {
                                MessageBox.Show("เพิ่มข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                textBoxeditpartno.Text = "";
                                textBoxeditpartname.Text = "";
                                textBoxeditstock.Text = "";
                                textBoxeditspine_code.Text = "";
                                Create_table();
                                ShowGrid();
                            }
                            else
                            {
                                MessageBox.Show(" ไม่สามารถเพิ่มข้อมูลได้, กรุณาลองใหม่อีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
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

        #region "Button Update"
        private void Buttonupdate_Click(object sender, EventArgs e)
        {
            ////////////////////////
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการอัพเดตโมเดลนี้ในฐานข้อมูล?";
                const string caption = "Update Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (textBoxeditpartno.Text == "" || textBoxeditpartno.Text == null || textBoxeditpartname.Text == "" || textBoxeditpartname.Text == null || textBoxeditstock.Text == "" || textBoxeditstock.Text == null)
                    {
                        MessageBox.Show(" กรุณากรอกข้อมูลทั้งหมดก่อนเริ่มกระบวนการต่อไป", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        //string part_no_ed = textBoxeditpartno.Text;
                        //string part_name_ed = textBoxeditpartname.Text;
                        //string spine_code_ed = textBoxeditspine_code.Text;
                        int qtystock = Convert.ToInt32(textBoxeditstock.Text);
                        var cmd = $"Update data_part_local " +
                            $"Set part_name = '{textBoxeditpartname.Text}', spine_code = '{textBoxeditspine_code.Text}', part_qty = {qtystock}" +
                            $"Where part_no = '{textBoxeditpartno.Text}'";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                        {
                            MessageBox.Show("อัพเดทข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBoxeditpartno.Text = "";
                            textBoxeditpartname.Text = "";
                            textBoxeditstock.Text = "";
                            textBoxeditspine_code.Text = "";
                            Create_table();
                            ShowGrid();
                        }
                        else
                        {
                            MessageBox.Show(" ไม่สามารถอัพเดทได้, กรุณาลองใหม่อีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        #region "Button Clear"
        private void Buttoneditclear_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxeditpartno.Text = "";
                textBoxeditpartname.Text = "";
                textBoxeditstock.Text = "";
                textBoxeditspine_code.Text = "";
                Create_table();
                ShowGrid();
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Button Delete"
        private void Buttondelete_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = "Are you sure that you would like to delete of this model?";
                const string caption = "Delete model from database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxeditpartno.Text == "" || textBoxeditpartno.Text == null || textBoxeditpartname.Text == "" || textBoxeditpartname.Text == null || textBoxeditstock.Text == "" || textBoxeditstock.Text == null)
                    {
                        MessageBox.Show(" กรุณากรอกข้อมูลทั้งหมดก่อนเริ่มกระบวนการต่อไป", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var cmd = $"Update data_part_local " +
                             $"Set working = '0' " +
                              $"Where part_no = '{textBoxeditpartno.Text}'";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Delete"))
                        {
                            textBoxeditpartno.Text = "";
                            textBoxeditpartname.Text = "";
                            textBoxeditstock.Text = "";
                            textBoxeditspine_code.Text = "";
                            Create_table();
                            ShowGrid();
                        }
                        else
                        {
                            MessageBox.Show(" ไม่สามารถลบได้, กรุณาลองใหม่อีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void metroGrid1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                //textBoxstockpartnumber.Text = metroGrid1.Rows[e.RowIndex].Cells[0].Value.ToString();
                //textBoxstockqty.Text = metroGrid1.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBoxeditpartno.Text = metroGrid1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBoxeditpartname.Text = metroGrid1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBoxeditspine_code.Text = metroGrid1.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBoxeditstock.Text = metroGrid1.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }

        private void textBoxsearchpartnumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxsearchpartnumber.TextLength > 1)
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = "Select * from data_part_local where part_no Like '%" + textBoxsearchpartnumber.Text + "%' and working = '1'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    Create_table();
                    foreach (DataRow dr in dt.Rows)
                    {
                        metroGrid1.Rows.Add(dr["part_no"], dr["part_name"], dr["part_qty"]);
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }

        private void textBoxsearchpartnumber_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxsearchpartnumber.Text = "";
        }
    }
}
