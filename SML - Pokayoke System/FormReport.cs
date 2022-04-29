using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.FileIO;
using System.Data.SqlClient;
using excel = Microsoft.Office.Interop.Excel;

namespace SML___Pokayoke_System
{
    public partial class FormReport : Form
    {
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;
        public static string Location_File_Tmp;

        public FormReport()
        {
            InitializeComponent();
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file local.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            InitializeDataGridView();
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

        #region "InitializeDataGridView"
        private void InitializeDataGridView()
        {
            metroGridReport.BorderStyle = BorderStyle.Fixed3D;
            metroGridReport.AllowUserToAddRows = false;
            metroGridReport.AllowUserToDeleteRows = false;
            metroGridReport.AllowUserToOrderColumns = true;
            metroGridReport.ReadOnly = true;
            metroGridReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            metroGridReport.MultiSelect = false;
            metroGridReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            metroGridReport.AllowUserToResizeColumns = false;
            metroGridReport.AllowUserToResizeRows = false;
            metroGridReport.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            metroGridReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        #endregion

        #region "Create_table"
        private void Create_table()
        {
            metroGridReport.Columns.Clear();
            metroGridReport.Rows.Clear();
            metroGridReport.ColumnCount = 6;
            metroGridReport.Columns[0].HeaderText = "Employee";
            metroGridReport.Columns[1].HeaderText = "Vin No.";
            metroGridReport.Columns[2].HeaderText = "Model Code";
            metroGridReport.Columns[3].HeaderText = "Part list";
            metroGridReport.Columns[4].HeaderText = "Date Time";
            metroGridReport.Columns[5].HeaderText = "Status";
        }
        #endregion

        #region "ShowGrid"
        private void ShowGrid()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(Local_Conn))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "Select * from data_log_local";
                var sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            foreach (DataRow dr in dt.Rows)
            {
                metroGridReport.Rows.Add(dr["name"], dr["vin_no"], dr["18_dig"], dr["part_list"], dr["date"], dr["status"]);
            }
        }
        #endregion

        #region "Button Submit"
        private void Buttonsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Create_table();
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var cmd = conn.CreateCommand();
                    var sda = new SqlDataAdapter("Select * from data_log_local where date between '" + metroDateTimestart.Value.ToString() + "' and '" + metroDateTimeend.Value.ToString() + "'", conn);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    metroGridReport.Rows.Add(dr["name"], dr["vin_no"], dr["18_dig"], dr["part_list"], dr["date"], dr["status"]);
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Button Clear"
        private void Buttonclear_Click(object sender, EventArgs e)
        {
            Create_table();
            ShowGrid();
        }
        #endregion

        #region "TextChange"
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Create_table();
            var dt = new DataTable();
            using (var conn = new SqlConnection(Local_Conn))
            {
                var check = conn.CreateCommand();
                check.CommandText = $"Select * From data_log_local Where vin_no = '{textBox1.Text}'";
                var sda = new SqlDataAdapter(check);
                sda.Fill(dt);
            }
            foreach (DataRow dr in dt.Rows)
            {
                metroGridReport.Rows.Add(dr["name"], dr["vin_no"], dr["18_dig"], dr["part_list"], dr["date"], dr["status"]);
            }
        }
        #endregion

        #region "Button Export Excel"
        private void Buttonexport_Click(object sender, EventArgs e)
        {
            try
            {
                excel.Application app = new excel.Application();
                excel.Workbook workbook = app.Workbooks.Add();
                excel.Worksheet worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                for (int i = 0; i < metroGridReport.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = metroGridReport.Columns[i].HeaderText;
                }
                for (int j = 0; j < metroGridReport.Rows.Count - 1; j++)
                {
                    for (int i = 0; i < metroGridReport.Columns.Count; i++)
                    {
                        worksheet.Cells[j + 2, i + 1] = metroGridReport.Rows[j].Cells[i].Value.ToString();
                    }
                }
                worksheet.Columns.AutoFit();
                var saveFileDialoge = new SaveFileDialog();
                saveFileDialoge.FileName = "Master Store" + "_" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
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
    }
}
