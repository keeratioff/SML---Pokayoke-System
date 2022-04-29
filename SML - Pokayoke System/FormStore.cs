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
    public partial class FormStore : Form
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

        //Login
        public string emp_id = Properties.Settings.Default.user_employee_id.ToString();
        public string emp_password = Properties.Settings.Default.user_password.ToString();
        public string emp_name = Properties.Settings.Default.user_name.ToString();
        public string emp_surname = Properties.Settings.Default.user_surname.ToString();

        //Serial Port
        string Serial_DataIn;

        public string partlist_scan;
        public int partlist_scan_count = 0;

        //Sound
        public bool flag_sound;

        public FormStore()
        {
            InitializeComponent();
        }

        private void FormStore_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file local.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            textBoxname.Text = emp_name + " " + emp_surname;
            textBoxdate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            InitializeDataGridView();
            GetSerialPort();
        }

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

        #region "Serial Port"
        public void GetSerialPort()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                SerialPort myserialport = new SerialPort(ports[0]);

                myserialport.BaudRate = 9600;
                myserialport.Parity = Parity.None;
                myserialport.StopBits = StopBits.One;
                myserialport.DataBits = 8;
                myserialport.Handshake = Handshake.None;
                myserialport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                myserialport.Open();
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "DataReceived"
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                Serial_DataIn = sp.ReadExisting();
                this.Invoke(new EventHandler(ShowData));
                Serial_DataIn = "";
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
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

        #region "ShowData of Scanner"
        private void ShowData(object sender, EventArgs e)
        {
            try
            {
                string[] textcode = Serial_DataIn.Split('\r');
                if (textBoxmodel.TextLength == 0)
                {
                    textBoxvinno.Text = textcode[0];
                }
                else if (textBoxvinno.TextLength >= 1)
                {
                    //textBoxmodel.Text = textcode[0];
                    partlist_scan = textcode[0];
                    ScanPartlists();
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Function Scan Part list"

        public void ScanPartlists()
        {
            //try
            //{
            //    int check_count_rows = metroGrid1.Rows.Count;
            //    int check_count_rows_result = check_count_rows - 1;
            //    if (partlist_scan_count <= check_count_rows_result)
            //    {
            //        string partlist = partlist_scan;
            //        partlist_scan = "";
            //        metroGrid2.Rows.Add(partlist, "1");
            //        CheckPartlist();
            //    }

            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            //}

            int check_count_rows = metroGrid1.Rows.Count;
            int check_count_rows_result = check_count_rows - 1;
            string partlist = partlist_scan;
            partlist_scan = "";
            //select part => m1

            var dt = new DataTable();
            using (var conn = new SqlConnection(Local_Conn))
            {
                var check = conn.CreateCommand();
                check.CommandText = $"Select * from data_master_list_local where part_no_sync = '{partlist}' and model_code = '{textBoxmodel.Text}'";
                var sda = new SqlDataAdapter(check);
                sda.Fill(dt);
            }
            int cout_row = dt.Rows.Count;
            if (cout_row == 0)
            {
                sound y = new sound();
                y.alarm_sounnd(false);
                textBoxalarm.Text = "Part No : " + partlist + " ,ไม่มีในระบบ";
                return;
            }
            else
            {
                for (int i = 0; i < check_count_rows_result; i++)
                {
                    for (int ii = 0; ii <= check_count_rows_result;)
                    {
                        string check_rows_m1 = metroGrid1.Rows[ii].Cells[0].Value.ToString();
                        if (check_rows_m1 == partlist)
                        {
                            sound y = new sound();
                            y.alarm_sounnd(true);
                            metroGrid2.Rows.Add(partlist, "1");
                            metroGrid1.Rows[ii].DefaultCellStyle.SelectionBackColor = Color.Green;
                            metroGrid1.Rows[ii].DefaultCellStyle.BackColor = Color.Green;
                            metroGrid1.Rows[ii].DefaultCellStyle.ForeColor = Color.Black;
                            textBoxalarm.Text = "";
                            partlist_scan_count++;
                            return;
                        }
                        else
                        {
                            ii++;
                        }
                    }
                }
            }
        }
        #endregion

        #region "Function Check Part List"
        public void CheckPartlist()
        {
            //try
            //{
            //    int check_count_rows = metroGrid1.Rows.Count;
            //    int check_count_rows_result = check_count_rows - 1;
            //    string check_rows_m2 = metroGrid2.Rows[partlist_scan_count].Cells[0].Value.ToString();

            //    for (int i = 0; i < check_count_rows_result; i++)
            //    {
            //        for (int ii = 0; ii <= check_count_rows_result;)
            //        {
            //            string check_rows_m1 = metroGrid1.Rows[ii].Cells[0].Value.ToString();
            //            if ( check_rows_m2 == check_rows_m1)
            //            {
            //                sound y = new sound();
            //                y.alarm_sounnd(true);
            //                metroGrid1.Rows[ii].DefaultCellStyle.SelectionBackColor = Color.Green;
            //                metroGrid1.Rows[ii].DefaultCellStyle.BackColor = Color.Green;
            //                metroGrid1.Rows[ii].DefaultCellStyle.ForeColor = Color.Black;
            //                partlist_scan_count++;
            //                return;
            //            }
            //            else
            //            {
            //                ii++;
            //            }
            //        }
            //    }
            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            //}

            
        }
        #endregion

        #region "Function Create table"
        private void Create_table()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid1.ColumnCount = 2;
            metroGrid1.Columns[0].HeaderText = "Booklet part No.";
            metroGrid1.Columns[1].HeaderText = "Quantity";

            metroGrid2.Columns.Clear();
            metroGrid2.Rows.Clear();
            metroGrid2.ColumnCount = 2;
            metroGrid2.Columns[0].HeaderText = "Booklet part No.";
            metroGrid2.Columns[1].HeaderText = "Quantity";
        }
        #endregion

        #region "TextChanged is Vin No"
        private void textBoxvinno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxvinno.TextLength >= 17)
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * from data_vlm_local where vin_no = '{textBoxvinno.Text}'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int cout_row = dt.Rows.Count;
                    if (cout_row == 0)
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
                        textBoxmodel.Text = model_result;
                        textBoxbuyer.Text = dt.Rows[0]["buyer_code"].ToString();

                        var dt_part = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_part);
                        }
                        string buyer_vlm_str = textBoxbuyer.Text;
                        string buyer_sub = dt_part.Rows[0]["buyer_code"].ToString();
                        bool result_contains = (buyer_vlm_str.Contains(buyer_sub));
                        if (result_contains == true)
                        {
                            // contains str = sub
                            var dt_true = new DataTable();
                            using (var conn = new SqlConnection(Local_Conn))
                            {
                                var check = conn.CreateCommand();
                                check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code = '{buyer_sub}'";
                                var sda = new SqlDataAdapter(check);
                                sda.Fill(dt_true);
                            }
                            int count_row_true = dt_true.Rows.Count;
                            Create_table();
                            foreach (DataRow dr in dt_true.Rows)
                            {
                                metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
                            }

                        }
                        else if (result_contains == false)
                        {
                            // contains str != sub
                            var dt_false = new DataTable();
                            using (var conn = new SqlConnection(Local_Conn))
                            {
                                var check = conn.CreateCommand();
                                check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code != '{buyer_sub}'";
                                var sda = new SqlDataAdapter(check);
                                sda.Fill(dt_false);
                            }
                            int count_row_false = dt_false.Rows.Count;
                            Create_table();
                            foreach (DataRow dr in dt_false.Rows)
                            {
                                metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
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

        #region "Button Clear"
        private void Buttonclear_Click(object sender, EventArgs e)
        {
            textBoxvinno.Text = "";
            textBoxmodel.Text = "";
            textBoxbuyer.Text = "";
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid2.Columns.Clear();
            metroGrid2.Rows.Clear();
        }
        #endregion

        #region "Button Save"
        private void Buttonsave_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการบันทึกข้อมูลนี้ในฐานข้อมูล?";
                const string caption = "Add Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (textBoxname.Text == "" || textBoxname.Text == null || textBoxdate.Text == "" || textBoxdate.Text == null || textBoxvinno.Text == "" || textBoxvinno.Text == null || textBoxmodel.Text == "" || textBoxmodel.Text == null || textBoxbuyer.Text == "" || textBoxbuyer.Text == null)
                    {
                        MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        int count_m1 = metroGrid1.Rows.Count;
                        int count_m2 = metroGrid2.Rows.Count;
                        if (count_m1 == count_m2)
                        {
                            for (int i = 1;i >= count_m2;)
                            {
                                string partlist_rows = metroGrid2.Rows[i].Cells[0].Value.ToString();
                                var cmd = $"Insert into data_log_local (name, vin_no, 18_dig, part_list, date, status) " +
                                $"Values ('{textBoxname.Text}', '{textBoxvinno.Text}', '{textBoxmodel.Text}', '{partlist_rows}', 'Getdate()', 'Complete')";
                                if (ExecuteSqlTransaction(cmd, Local_Conn, "Add"))
                                {
                                    i++;
                                }
                            }
                            MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBoxvinno.Text = "";
                            textBoxmodel.Text = "";
                            textBoxbuyer.Text = "";
                            metroGrid1.Columns.Clear();
                            metroGrid1.Rows.Clear();
                            metroGrid2.Columns.Clear();
                            metroGrid2.Rows.Clear();
                        }
                        else
                        {
                            MessageBox.Show("ไม่สามารถบันทึกข้อมูลได้, โปรดติดต่อผู้ดูแลระบบ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
