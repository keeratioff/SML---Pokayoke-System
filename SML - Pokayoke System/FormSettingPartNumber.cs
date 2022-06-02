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
    public partial class FormSettingPartNumber : Form
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

        DataTableCollection tableCollection;

        public FormSettingPartNumber()
        {
            InitializeComponent();
        }

        private void FormSettingPartNumber_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file local.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            InitializeDataGridView();
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

        #region "InitializeDataGridView"
        private void InitializeDataGridView()
        {
            metroGrid1.BorderStyle = BorderStyle.Fixed3D;
            metroGrid1.AllowUserToAddRows = false;
            metroGrid1.AllowUserToDeleteRows = false;
            metroGrid1.AllowUserToOrderColumns = true;
            metroGrid1.ReadOnly = true;
            metroGrid1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            metroGrid1.MultiSelect = false;
            metroGrid1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            metroGrid1.AllowUserToResizeColumns = false;
            metroGrid1.AllowUserToResizeRows = false;
            metroGrid1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            metroGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        #endregion

        #region "Combobox"
        private void comboBoxsheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = tableCollection[comboBoxsheet.SelectedItem.ToString()];
            metroGrid1.DataSource = dt;
        }
        #endregion

        #region "Button Browse File"
        private void buttonbrowse1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel 97-2003 Workbook |*.xls" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxfilename.Text = openFileDialog.FileName;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataReader.ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            tableCollection = result.Tables;
                            comboBoxsheet.Items.Clear();
                            foreach (DataTable table in tableCollection)
                                comboBoxsheet.Items.Add(table.TableName);
                        }
                    }
                }
            }
        }

        #endregion

        #region "Button Save"
        private void Buttonsave_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = " คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล? ";
                const string caption = "Add Partlist to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int count_row = metroGrid1.Rows.Count;
                    for (int ii = 0; ii <= count_row;)
                    {
                        string model_base = metroGrid1.Rows[ii].Cells[1].Value.ToString();
                        string model_code = metroGrid1.Rows[ii].Cells[2].Value.ToString();
                        string buyer_code = metroGrid1.Rows[ii].Cells[3].Value.ToString();
                        string part_no = metroGrid1.Rows[ii].Cells[4].Value.ToString();
                        string part_qty = metroGrid1.Rows[ii].Cells[5].Value.ToString();
                        string operation_name = metroGrid1.Rows[ii].Cells[6].Value.ToString();
                        string image = metroGrid1.Rows[ii].Cells[7].Value.ToString();
                        var cmd = $"Insert into data_master_list_local (model_base, model_code, buyer_code, part_no_sync, part_qty, operation_name, image, working) " +
                            $"Values ('{model_base}', '{model_code}', '{buyer_code}', '{part_no}', '1', '{operation_name}', 'null','1')";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "ADD"))
                        {
                            ii++;
                        }
                        else
                        {
                            MessageBox.Show("ไม่สามารถบันทึกได้, กรุณาตรวจสอบอีกครั้ง.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }





                        //string modelbase_check = metroGrid1.Rows[ii].Cells[1].Value.ToString();
                        //string modelcode_check = metroGrid1.Rows[ii].Cells[2].Value.ToString();
                        //string buyercode_check = metroGrid1.Rows[ii].Cells[3].Value.ToString();
                        //string operationname_check = metroGrid1.Rows[ii].Cells[6].Value.ToString();
                        //var dt = new DataTable();
                        //using (var conn = new SqlConnection(Local_Conn))
                        //{
                        //    var check = conn.CreateCommand();
                        //    check.CommandText = $"Select * from data_master_list_local where model_base = '{modelbase_check}' and model_code = '{modelcode_check}' and buyer_code = '{buyercode_check}' and operation_name = '{operationname_check}'";
                        //    var sda = new SqlDataAdapter(check);
                        //    sda.Fill(dt);
                        //}
                        //int count_row_check = dt.Rows.Count;
                        //if (count_row_check >= 1)
                        //{
                        //    ii++;
                        //}
                        //else if (count_row_check == 0)
                        //{
                        //    string model_base = metroGrid1.Rows[ii].Cells[1].Value.ToString();
                        //    string model_code = metroGrid1.Rows[ii].Cells[2].Value.ToString();
                        //    string buyer_code = metroGrid1.Rows[ii].Cells[3].Value.ToString();
                        //    string part_no = metroGrid1.Rows[ii].Cells[4].Value.ToString();
                        //    string part_qty = metroGrid1.Rows[ii].Cells[5].Value.ToString();
                        //    string operation_name = metroGrid1.Rows[ii].Cells[6].Value.ToString();
                        //    string image = metroGrid1.Rows[ii].Cells[7].Value.ToString();
                        //    var cmd = $"Insert into data_master_list_local (model_base, model_code, buyer_code, part_no_sync, part_qty, operation_name, image, working) " +
                        //        $"Values ('{model_base}', '{model_code}', '{buyer_code}', '{part_no}', '1', '{operation_name}', 'null','1')";
                        //    if (ExecuteSqlTransaction(cmd, Local_Conn, "ADD"))
                        //    {
                        //        ii++;
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show("ไม่สามารถบันทึกได้, กรุณาตรวจสอบอีกครั้ง.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //        return;
                        //    }
                        //}
                    }
                    MessageBox.Show("บันทึกสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Button Clear"
        private void Buttonclear_Click(object sender, EventArgs e)
        {
            textBoxfilename.Text = "";
            comboBoxsheet.Text = "";
            metroGrid1.DataSource = null;
        }
        #endregion

        
        private void buttoncleardatabase_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = " คุณแน่ใจหรือไม่ ว่าต้องการลบข้อมูลทั้งหมด? ";
                const string caption = "Add Partlist to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var cmd = $"Delete from data_master_list_local";
                    if (ExecuteSqlTransaction(cmd, Local_Conn, "Delete"))
                    {

                    }
                    MessageBox.Show("ลบข้อมูลทั้งหมดสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error FormWeight Message: {0}, {error.Message}");
            }
        }
    }
}
