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
    public partial class FormSettingAddStock : Form
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

        public FormSettingAddStock()
        {
            InitializeComponent();
        }

        private void FormSettingAddStock_Load(object sender, EventArgs e)
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
            metroGrid1.ColumnCount = 3;
            metroGrid1.Columns[0].HeaderText = "Part number";
            metroGrid1.Columns[1].HeaderText = "Part name";
            metroGrid1.Columns[2].HeaderText = "Stock";
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
                    metroGrid1.Rows.Add(dr["part_no"], dr["part_name"], dr["part_qty"]);
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Grid Cell mouser up"
        private void metroGrid1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                textBoxstockpartnumber.Text = metroGrid1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBoxstockqty.Text = metroGrid1.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }

        #endregion

        #region "Button Stock"
        private void Buttonstockclear_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxstockpartnumber.Text = "";
                textBoxstockqty.Text = "";
                textBoxstockvendor.Text = "";
                textBoxstockadd.Text = "";
                textBoxsearchpartnumber.Text = "Search";
                this.metroGrid1.Rows.Clear();
                ShowGrid();
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }

        private void Buttonaddstock_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล?";
                const string caption = "Update Data Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxstockpartnumber.Text == "" || textBoxsearchpartnumber.Text == null || textBoxstockqty.Text == "" || textBoxstockqty.Text == null || textBoxstockadd.Text == "" || textBoxstockadd.Text == null || textBoxstockvendor.Text == "" || textBoxstockvendor.Text == null)
                    {
                        MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string stock_partlist = textBoxstockpartnumber.Text;
                        int stock_qty = Convert.ToInt32(textBoxstockqty.Text);
                        int stock_add = Convert.ToInt32(textBoxstockadd.Text);
                        string stock_vendor = textBoxstockvendor.Text;
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * from data_part_local where part_no = '{stock_partlist}' and working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int count_rows = dt.Rows.Count;
                        if (count_rows == 0)
                        {
                            MessageBox.Show("ไม่พบ Part number นี้ในระบบ, กรุณาตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBoxstockpartnumber.Text = "";
                            textBoxstockqty.Text = "";
                            textBoxstockadd.Text = "";

                        }
                        else
                        {
                            int stock_result = stock_qty + stock_add;
                            var cmd = $"Update data_part_local " +
                                $"Set part_qty = '{stock_result}' " +
                                $"Where part_no = '{stock_partlist}'";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                            {
                                string emp_name_st = emp_name;
                                string part_no_st = dt.Rows[0]["part_no"].ToString();
                                string part_name_st = dt.Rows[0]["part_name"].ToString();
                                string spine_code_st = dt.Rows[0]["spine_code"].ToString();
                                int part_withdraw_qty_st = stock_add;
                                var cmd1 = $"Insert into data_log_stock_local (name, part_no, part_name, spine_code, vendor, part_withdraw_qty, date) " +
                                 $"Values ('{emp_name_st}', '{part_no_st}', '{part_name_st}', '{spine_code_st}', '{stock_vendor}', '{part_withdraw_qty_st}', Getdate())";
                                if (ExecuteSqlTransaction(cmd1, Local_Conn, "Add"))
                                {
                                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    textBoxstockpartnumber.Text = "";
                                    textBoxstockqty.Text = "";
                                    textBoxstockadd.Text = "";
                                    textBoxsearchpartnumber.Text = "Search";
                                    this.metroGrid1.Rows.Clear();
                                    ShowGrid();
                                }
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

        #region "Text Change for Model Code"
        private void textBoxsearchpartnumber_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxsearchpartnumber.Text = "";
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
        #endregion

        #region "Button Export"
        private void Buttonexport_Click(object sender, EventArgs e)
        {
            try
            {
                excel.Application app = new excel.Application();
                excel.Workbook workbook = app.Workbooks.Add();
                excel.Worksheet worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                for (int i = 0; i < metroGrid1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = metroGrid1.Columns[i].HeaderText;
                }
                for (int j = 0; j <= metroGrid1.Rows.Count - 1; j++)
                {
                    for (int i = 0; i < metroGrid1.Columns.Count; i++)
                    {
                        worksheet.Cells[j + 2, i + 1] = metroGrid1.Rows[j].Cells[i].Value.ToString();
                    }
                }
                worksheet.Columns.AutoFit();
                var saveFileDialoge = new SaveFileDialog();
                saveFileDialoge.FileName = "Report Master Store Stock" + "_" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                saveFileDialoge.Filter = "XLSX|*.xlsx";
                if (saveFileDialoge.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialoge.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    MessageBox.Show("Export is Successful");
                    _ = new LogWriter.LogWriter("Exportfile : " + DateTime.Now);
                    workbook.Close();
                    app.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                }
                else
                {
                    return;
                }

            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($"   Export is fail: ");
                _ = new LogWriter.LogWriter($"   Message: {0}, {error.Message}");
            }
        }
        #endregion

        private void Buttonexcel_Click(object sender, EventArgs e)
        {
            try
            {
                excel.Application app = new excel.Application();
                excel.Workbook workbook = app.Workbooks.Add();
                excel.Worksheet worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                for (int i = 0; i < metroGrid1.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = metroGrid1.Columns[i].HeaderText;
                }
                for (int j = 0; j < metroGrid1.Rows.Count - 1; j++)
                {
                    for (int i = 0; i < metroGrid1.Columns.Count; i++)
                    {
                        worksheet.Cells[j + 2, i + 1] = metroGrid1.Rows[j].Cells[i].Value.ToString();
                    }
                }
                worksheet.Columns.AutoFit();
                var saveFileDialoge = new SaveFileDialog();
                saveFileDialoge.FileName = "Report Master Store Stock" + "_" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                saveFileDialoge.Filter = "XLSX|*.xlsx";
                if (saveFileDialoge.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(saveFileDialoge.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    MessageBox.Show("Export is Successful");
                    _ = new LogWriter.LogWriter("Exportfile : " + DateTime.Now);
                    workbook.Close();
                    app.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                }
                else
                {
                    return;
                }

            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($"   Export is fail: ");
                _ = new LogWriter.LogWriter($"   Message: {0}, {error.Message}");
            }
        }
    }
}
