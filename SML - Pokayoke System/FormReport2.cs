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
    public partial class FormReport2 : Form
    {

        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;
        public static string Location_File_Tmp;

        public FormReport2()
        {
            InitializeComponent();
        }

        private void FormReport2_Load(object sender, EventArgs e)
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

        #region "Create tabel"
        private void Create_table()
        {
            metroGridReport.Columns.Clear();
            metroGridReport.Rows.Clear();
            metroGridReport.ColumnCount = 7;
            metroGridReport.Columns[0].HeaderText = "Name";
            metroGridReport.Columns[1].HeaderText = "Part no";
            metroGridReport.Columns[2].HeaderText = "Part name";
            metroGridReport.Columns[3].HeaderText = "Spine Code";
            metroGridReport.Columns[4].HeaderText = "Vendor";
            metroGridReport.Columns[5].HeaderText = "Withdraw qty.";
            metroGridReport.Columns[6].HeaderText = "Date Time";
        }
        #endregion

        #region "Show Grid"
        private void ShowGrid()
        {
            var dt = new DataTable();
            using (var conn = new SqlConnection(Local_Conn))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = "Select * from data_log_stock_local";
                var sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            foreach (DataRow dr in dt.Rows)
            {
                metroGridReport.Rows.Add(dr["name"], dr["part_no"], dr["part_name"], dr["spine_code"], dr["vendor"], dr["part_withdraw_qty"], dr["date"]);
            }
        }
        #endregion

        #region "Button Submit"
        private void Buttonsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string start_date_ = metroDateTimestart.Value.ToString("yyyyMMdd");
                string end_date_ = metroDateTimeend.Value.ToString("yyyyMMdd");

                string[] start_date = start_date_.Split(' ');
                string[] end_date = end_date_.Split(' ');

                Create_table();
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var cmd = conn.CreateCommand();
                    var sda = new SqlDataAdapter("Select * from data_log_stock_local where date between '" + start_date[0] + "' and '" + end_date[0] + "'", conn);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    metroGridReport.Rows.Add(dr["name"], dr["part_no"], dr["part_name"], dr["spine_code"], dr["vendor"], dr["part_withdraw_qty"], dr["date"]);
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($"   Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Button Clear"
        private void Buttonclear_Click(object sender, EventArgs e)
        {
            try
            {
                Create_table();
                ShowGrid();
                textBox1.Text = "Search ( Vin.No )";
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($"   Message: {0}, {error.Message}");
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
                for (int i = 0; i < metroGridReport.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = metroGridReport.Columns[i].HeaderText;
                }
                for (int j = 0; j <= metroGridReport.Rows.Count - 1; j++)
                {
                    for (int i = 0; i < metroGridReport.Columns.Count; i++)
                    {
                        worksheet.Cells[j + 2, i + 1] = metroGridReport.Rows[j].Cells[i].Value.ToString();
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

    }
}
