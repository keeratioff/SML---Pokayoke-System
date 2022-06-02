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
        public static SerialPort myserialport;

        public string partlist_scan;
        public int partlist_scan_count = 0;

        //Sound
        public bool flag_sound;

        //ALL EX
        public bool Text_G_1 = false;
        public bool Text_G_2 = false;
        public bool Text_G_3 = false;
        public bool Text_G_4 = false;
        public bool Text_G_5 = false;


        public bool status_check_partlist = false;
        public bool status_checl_partlist_scan = false;


        public int switch_C = 0;
        public int count_scan = 0;
        public int count_switch = 0;

        public FormStore()
        {
            InitializeComponent();
        }

        private void FormStore_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file local.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            //textBoxname.Text = emp_name + " " + emp_surname;
            textBoxdate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            //Create_table();
            InitializeDataGridView();
            CloseSerialPort();
            GetSerialPort();
            Check_stock();

            //////////
            ///////
            ///
            //labeltest.Hide();
            //textBoxname.Text = "Siam Smart Solutions";
            //timer1.Enabled = true;
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
            //metroGrid1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
                //string[] ports = SerialPort.GetPortNames();
                //SerialPort myserialport = new SerialPort(ports[0]);
                myserialport = new SerialPort("COM1");


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

        #region "Close Serial Port"
        public void CloseSerialPort()
        {
            try
            {
                if (myserialport.IsOpen == true)
                {
                    myserialport.Close();
                }
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
                //string[] textcode = Serial_DataIn.Split('\r');
                ////string[] textcode = Serial_DataIn.ToString();
                //if (textBoxmodel.TextLength == 0)
                //{
                //    textBoxvinno.Text = textcode[0];
                //    Serial_DataIn = "";
                //}
                //else if (textBoxvinno.TextLength >= 1)
                //{
                //    //textBoxmodel.Text = textcode[0];
                //    partlist_scan = textcode[0];
                //    Serial_DataIn = "";
                //    ScanPartlists();
                //}
                //else if (textBoxname.TextLength == 0 && textBoxvinno.TextLength == 0 && textBoxmodel.TextLength == 0)
                //{

                //}

                string[] textcode = Serial_DataIn.Split('\r');
                if (textBoxname.TextLength == 0 && textBoxvinno.TextLength == 0 && textBoxmodel.TextLength == 0)
                {
                    textBoxname.Text = textcode[0];
                    Serial_DataIn = "";
                }
                else if (textBoxname.TextLength > 1 && textBoxvinno.TextLength == 0 && textBoxmodel.TextLength == 0)
                {
                    textBoxvinno.Text = textcode[0];
                    Serial_DataIn = "";
                }
                else if (textBoxname.TextLength > 1 && textBoxvinno.TextLength > 1 && textBoxmodel.TextLength > 1)
                {
                    //textBox1.Text = textcode[0];
                    partlist_scan = textcode[0];
                    Serial_DataIn = "";
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
            #region "Hoden Code"
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
            #endregion

            //int check_count_rows = metroGrid1.Rows.Count;
            //int check_count_rows_result = check_count_rows - 1;
            //string partlist = partlist_scan;
            //partlist_scan = "";
            ////select part => m1

            //var dt = new DataTable();
            //using (var conn = new SqlConnection(Local_Conn))
            //{
            //    var check = conn.CreateCommand();
            //    check.CommandText = $"Select * from data_master_list_local where part_no_sync = '{partlist}' and model_code = '{textBoxmodel.Text}' and working = '1'";
            //    var sda = new SqlDataAdapter(check);
            //    sda.Fill(dt);
            //}
            //int cout_row = dt.Rows.Count;
            //if (cout_row == 0)
            //{
            //    sound y = new sound();
            //    y.alarm_sounnd(false);
            //    textBoxalarm.Text = "Part No : " + partlist + " ,ไม่มีในระบบ";
            //    return;
            //}
            //else
            //{
            //    for (int i = 0; i < check_count_rows_result; i++)
            //    {
            //        for (int ii = 0; ii <= check_count_rows_result;)
            //        {
            //            string check_rows_m1 = metroGrid1.Rows[ii].Cells[0].Value.ToString();
            //            if (check_rows_m1 == partlist)
            //            {
            //                sound y = new sound();
            //                y.alarm_sounnd(true);
            //                metroGrid2.Rows.Add(partlist, "1");
            //                metroGrid1.Rows[ii].DefaultCellStyle.SelectionBackColor = Color.Green;
            //                metroGrid1.Rows[ii].DefaultCellStyle.BackColor = Color.Green;
            //                metroGrid1.Rows[ii].DefaultCellStyle.ForeColor = Color.Black;
            //                textBoxalarm.Text = "";
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


            try
            {
                int check_count_rows = metroGrid1.Rows.Count;
                //int check_count_rows_result = check_count_rows - 1;
                string partlist = partlist_scan;
                partlist_scan = "";
                //select part => m1

                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check = conn.CreateCommand();
                    check.CommandText = $"Select * from data_master_list_local where part_no_sync = '{partlist}' and model_code = '{textBoxmodel.Text}' and working = '1'";
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
                    int check_count_rows_m2 = metroGrid2.Rows.Count;
                    int check_count_rows_m2_result = check_count_rows_m2 - 1;
                    if (check_count_rows_m2_result == 0)
                    {
                        for (int ii = 0; ii <= check_count_rows;)
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
                    else if (check_count_rows_m2_result > 0)
                    {
                        //Check M2
                        for (int j = 0; j + 1 <= check_count_rows_m2_result;)
                        {
                            string check_rows_m2 = metroGrid2.Rows[j].Cells[0].Value.ToString();
                            if (check_rows_m2 == partlist)
                            {
                                status_checl_partlist_scan = true;
                                MessageBox.Show("Part No : " + partlist + " ถูกสแกนแล้ว");
                                return;
                            }
                            else
                            {
                                status_checl_partlist_scan = false;
                                j++;
                            }
                        }
                        if (status_checl_partlist_scan == false)
                        {
                            for (int ii = 0; ii <= check_count_rows;)
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
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
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

        #region "Function Check Stock"
        private void Check_stock()
        {
            try
            {
                var dt_check_stock = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check_stock = conn.CreateCommand();
                    check_stock.CommandText = $"Select * from data_part_local where part_qty <= 1 and working = '1'";
                    var sda = new SqlDataAdapter(check_stock);
                    sda.Fill(dt_check_stock);
                }
                int count_check_stock = dt_check_stock.Rows.Count;
                if (count_check_stock >= 1)
                {
                    string part_name = dt_check_stock.Rows[0]["part_no"].ToString();
                    string part_qty = dt_check_stock.Rows[0]["part_qty"].ToString();
                    textBoxalarm.Text = part_name + " เหลือจำนวน : " + part_qty;
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Function Create table"
        private void Create_table()
        {
            try
            {
                metroGrid1.Columns.Clear();
                metroGrid1.Rows.Clear();
                metroGrid1.ColumnCount = 4;
                metroGrid1.Columns[0].HeaderText = "Booklet part no.   ";
                metroGrid1.Columns[1].HeaderText = "Spine_code";
                metroGrid1.Columns[2].HeaderText = "Quantity";
                metroGrid1.Columns[3].HeaderText = "Stock";

                metroGrid2.Columns.Clear();
                metroGrid2.Rows.Clear();
                metroGrid2.ColumnCount = 2;
                metroGrid2.Columns[0].HeaderText = "Booklet part no.";
                metroGrid2.Columns[1].HeaderText = "Quantity";
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "TextChanged is Vin No"
        private void textBoxvinno_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxvinno.TextLength > 15)
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * from data_vlm_local where vin_no = '{textBoxvinno.Text}' and working = '1'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_row = dt.Rows.Count;
                    if (count_row == 0)
                    {
                        MessageBox.Show(" ไม่พบ Vin No : " + textBoxvinno.Text + " โปรดตรวจสอบอีกครั้ง ");
                        return;
                    }
                    else
                    {
                        Create_table();
                        string model_prefix = dt.Rows[0]["model_prefix"].ToString();
                        string model_base = dt.Rows[0]["model_base"].ToString();
                        string model_suffix = dt.Rows[0]["model_suffix"].ToString();
                        string model_result = (model_prefix + model_base + model_suffix);
                        textBoxmodel.Text = model_result;

                        string opname = dt.Rows[0]["operation_name"].ToString();
                        string operation_name = dt.Rows[0]["operation_name"].ToString();
                        textBoxoperationname.Text = operation_name;


                        string buyer_code_vin = dt.Rows[0]["buyer_code"].ToString();
                        textBoxbuyer.Text = buyer_code_vin;

                        var dt_select = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * from data_master_list_local where model_base = '{model_base}' and model_code = '{model_result}' and operation_name = '{operation_name}' and working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_select);
                        }
                        int count_row_model = dt_select.Rows.Count;
                        count_switch = count_row_model;
                        if (count_row_model == 0)
                        {
                            MessageBox.Show(" ไม่พบ Model code : " + textBoxmodel.Text + " โปรดตรวจสอบอีกครั้ง ");
                            return;
                        }
                        else
                        {
                            for (int i = 0; i <= count_row_model;)
                            {
                                string buyer_tbl = dt_select.Rows[i]["buyer_code"].ToString();
                                string model_tbl = dt_select.Rows[i]["model_code"].ToString();
                                string part_tbl = dt_select.Rows[i]["part_no_sync"].ToString();
                                if (buyer_tbl.StartsWith("A"))
                                {
                                    string[] text_sp = buyer_tbl.Split('_');
                                    int text_count = text_sp.Count();
                                    bool buyer_code_vin_con = (buyer_code_vin.Contains(text_sp[2]));
                                    if (text_count == 3)
                                    {
                                        if (buyer_code_vin_con == false)
                                        {
                                            metroGrid1.Rows.Add(part_tbl, null, "1", null);
                                            //Cut Stock
                                            int count_m11 = metroGrid1.Rows.Count;
                                            int count_m11_result = count_m11 - 1;
                                            var dt_stock = new DataTable();
                                            using (var conn = new SqlConnection(Local_Conn))
                                            {
                                                var check = conn.CreateCommand();
                                                check.CommandText = $"Select * from data_part_local where part_no = '{part_tbl}' and working = '1'";
                                                var sda = new SqlDataAdapter(check);
                                                sda.Fill(dt_stock);
                                            }
                                            string part_stock = dt_stock.Rows[i]["part_qty"].ToString();
                                            string spine_code = dt_stock.Rows[i]["spine_code"].ToString();
                                            metroGrid1.Rows[count_m11_result].Cells[1].Value = spine_code;
                                            metroGrid1.Rows[count_m11_result].Cells[3].Value = part_stock;
                                        }
                                        #region "Code old"
                                        //if (text_sp[2] != buyer_code_vin)
                                        //{
                                        //    metroGrid1.Rows.Add(part_tbl, "1", null);
                                        //    //Cut Stock
                                        //    int count_m11 = metroGrid1.Rows.Count;
                                        //    int count_m11_result = count_m11 - 1;
                                        //    var dt_stock = new DataTable();
                                        //    using (var conn = new SqlConnection(Local_Conn))
                                        //    {
                                        //        var check = conn.CreateCommand();
                                        //        check.CommandText = $"Select * from data_part_local where part_no = '{part_tbl}' and working = '1'";
                                        //        var sda = new SqlDataAdapter(check);
                                        //        sda.Fill(dt_stock);
                                        //    }
                                        //    string part_stock = dt_stock.Rows[i]["part_qty"].ToString();

                                        //    metroGrid1.Rows[count_m11_result].Cells[2].Value = part_stock;
                                        //}
                                        #endregion
                                    }
                                    if (text_count > 3)
                                    {
                                        for (int ii = 3; ii <= text_count;)
                                        {
                                            bool buyer_contains = (buyer_code_vin.Contains(text_sp[ii]));
                                            if (buyer_contains == true)
                                            {
                                                return;
                                            }
                                            if (buyer_contains == false)
                                            {
                                                if (ii + 1 == text_count)
                                                {
                                                    metroGrid1.Rows.Add(part_tbl, null, "1", null);
                                                    //Cut Stock
                                                    int count_m11 = metroGrid1.Rows.Count;
                                                    int count_m11_result = count_m11 - 1;
                                                    var dt_stock = new DataTable();
                                                    using (var conn = new SqlConnection(Local_Conn))
                                                    {
                                                        var check = conn.CreateCommand();
                                                        check.CommandText = $"Select * from data_part_local where part_no = '{part_tbl}' and working = '1'";
                                                        var sda = new SqlDataAdapter(check);
                                                        sda.Fill(dt_stock);
                                                    }
                                                    string part_stock = dt_stock.Rows[0]["part_qty"].ToString();
                                                    string spine_code = dt_stock.Rows[0]["spine_code"].ToString();
                                                    metroGrid1.Rows[count_m11_result].Cells[1].Value = spine_code;
                                                    metroGrid1.Rows[count_m11_result].Cells[3].Value = part_stock;
                                                }
                                                ii++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool buyer_contains = (buyer_code_vin.Contains(buyer_tbl));
                                    if (buyer_contains == true)
                                    {
                                        //selsct
                                        var dt_true = new DataTable();
                                        using (var conn = new SqlConnection(Local_Conn))
                                        {
                                            var check = conn.CreateCommand();
                                            check.CommandText = $"Select * from data_master_list_local where model_code = '{model_tbl}' and buyer_code = '{buyer_tbl}' and operation_name = '{operation_name}' and working = '1'";
                                            var sda = new SqlDataAdapter(check);
                                            sda.Fill(dt_true);
                                        }
                                        metroGrid1.Rows.Add(part_tbl, null, "1", null);
                                        //Cut Stock
                                        int count_m11 = metroGrid1.Rows.Count;
                                        int count_m11_result = count_m11 - 1;
                                        var dt_stock = new DataTable();
                                        using (var conn = new SqlConnection(Local_Conn))
                                        {
                                            var check = conn.CreateCommand();
                                            check.CommandText = $"Select * from data_part_local where part_no = '{part_tbl}' and working = '1'";
                                            var sda = new SqlDataAdapter(check);
                                            sda.Fill(dt_stock);
                                        }
                                        string part_stock = Convert.ToString(dt_stock.Rows[0]["part_qty"]).ToString();
                                        string spine_code = dt_stock.Rows[0]["spine_code"].ToString();
                                        metroGrid1.Rows[count_m11_result].Cells[1].Value = spine_code;
                                        metroGrid1.Rows[count_m11_result].Cells[3].Value = part_stock;
                                    }
                                }
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }


            #region "Hiden Code1"
            //try
            //{
            //    if (textBoxvinno.TextLength >= 14)
            //    {
            //        Create_table();
            //        var dt = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * from data_vlm_local where vin_no = '{textBoxvinno.Text}' and working = '1'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt);
            //        }
            //        int cout_row = dt.Rows.Count;
            //        if (cout_row == 0)
            //        {
            //            textBoxvinno.Text = "";
            //            MessageBox.Show(" ไม่พบ Vin No : " + textBoxvinno.Text + " โปรดตรวจสอบอีกครั้ง");
            //            return;
            //        }
            //        else
            //        {
            //            string model_prefix = dt.Rows[0]["model_prefix"].ToString();
            //            string model_base = dt.Rows[0]["model_base"].ToString();
            //            string model_suffix = dt.Rows[0]["model_suffix"].ToString();
            //            string model_result = (model_prefix + model_base + model_suffix);
            //            string operation_name = dt.Rows[0]["operation_name"].ToString();
            //            textBoxmodel.Text = model_result;
            //            textBoxbuyer.Text = dt.Rows[0]["buyer_code"].ToString();
            //            textBoxoperationname.Text = operation_name;

            //            if (model_base == "D23")
            //            {
            //                var dt_part = new DataTable();
            //                using (var conn = new SqlConnection(Local_Conn))
            //                {
            //                    var check = conn.CreateCommand();
            //                    check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and operation_name = '{operation_name}' and working = '1'";
            //                    var sda = new SqlDataAdapter(check);
            //                    sda.Fill(dt_part);
            //                }
            //                //
            //                string buyer_vlm_str = textBoxbuyer.Text;
            //                string buyer_sub = dt_part.Rows[0]["buyer_code"].ToString(); // All_EXCEPT_THI
            //                bool result_contains = (buyer_vlm_str.Contains(buyer_sub)); // (24THI) (THI)
            //                if (result_contains == true)
            //                {
            //                    // contains str = sub
            //                    var dt_true = new DataTable();
            //                    using (var conn = new SqlConnection(Local_Conn))
            //                    {
            //                        var check = conn.CreateCommand();
            //                        check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code = '{buyer_sub}' and operation_name = '{operation_name}' and working = '1'";
            //                        var sda = new SqlDataAdapter(check);
            //                        sda.Fill(dt_true);
            //                    }
            //                    int count_row_true = dt_true.Rows.Count;
            //                    foreach (DataRow dr in dt_true.Rows)
            //                    {
            //                        metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                    }
            //                }
            //                else if (result_contains == false)
            //                {
            //                    if (buyer_sub.StartsWith("A"))
            //                    {
            //                        string[] text = buyer_sub.Split('_');
            //                        int counttext = text.Count();
            //                        if (counttext == 3)
            //                        {
            //                            if (text[2] != buyer_vlm_str)
            //                            {
            //                                var dt_false = new DataTable();
            //                                using (var conn = new SqlConnection(Local_Conn))
            //                                {
            //                                    var check = conn.CreateCommand();
            //                                    check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code = '{buyer_sub}' and operation_name = '{operation_name}' and working = '1'";
            //                                    var sda = new SqlDataAdapter(check);
            //                                    sda.Fill(dt_false);
            //                                }
            //                                int count_row_false = dt_false.Rows.Count;
            //                                foreach (DataRow dr in dt_false.Rows)
            //                                {
            //                                    metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                                }
            //                            }

            //                        }
            //                        if (counttext > 3)
            //                        {
            //                            for (int i = 3; i <= counttext;)
            //                            {
            //                                if (text[i - 1] != buyer_vlm_str)
            //                                {
            //                                    i++;
            //                                }
            //                                if (i - 1 == counttext)
            //                                {
            //                                    var dt_false = new DataTable();
            //                                    using (var conn = new SqlConnection(Local_Conn))
            //                                    {
            //                                        var check = conn.CreateCommand();
            //                                        check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code = '{buyer_sub}' and operation_name = '{operation_name}' and working = '1'";
            //                                        var sda = new SqlDataAdapter(check);
            //                                        sda.Fill(dt_false);
            //                                    }
            //                                    int count_row_false = dt_false.Rows.Count;
            //                                    foreach (DataRow dr in dt_false.Rows)
            //                                    {
            //                                        metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        //contains str != sub
            //                        var dt_false = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code == '{buyer_sub}' and operation_name = '{operation_name}' and working = '1'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_false);
            //                        }
            //                        int count_row_false = dt_false.Rows.Count;
            //                        foreach (DataRow dr in dt_false.Rows)
            //                        {
            //                            metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                var dt_part = new DataTable();
            //                using (var conn = new SqlConnection(Local_Conn))
            //                {
            //                    var check = conn.CreateCommand();
            //                    check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and operation_name = '{operation_name}' and working = '1'";
            //                    var sda = new SqlDataAdapter(check);
            //                    sda.Fill(dt_part);
            //                }
            //                //
            //                string buyer_vlm_str = textBoxbuyer.Text;
            //                string buyer_sub = dt_part.Rows[0]["buyer_code"].ToString(); // All_EXCEPT_THI
            //                bool result_contains = (buyer_vlm_str.Contains(buyer_sub)); // (24THI) (THI)
            //                if (result_contains == true)
            //                {
            //                    // contains str = sub
            //                    var dt_true = new DataTable();
            //                    using (var conn = new SqlConnection(Local_Conn))
            //                    {
            //                        var check = conn.CreateCommand();
            //                        check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code = '{buyer_sub}' and operation_name = '{operation_name}' and working = '1'";
            //                        var sda = new SqlDataAdapter(check);
            //                        sda.Fill(dt_true);
            //                    }
            //                    int count_row_true = dt_true.Rows.Count;
            //                    foreach (DataRow dr in dt_true.Rows)
            //                    {
            //                        metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                    }
            //                }
            //                else if (result_contains == false)
            //                {
            //                    if (buyer_sub.StartsWith("A"))
            //                    {
            //                        string[] text = buyer_sub.Split('_');
            //                        int counttext = text.Count();
            //                        if (counttext == 3)
            //                        {
            //                            if (text[2] != buyer_vlm_str)
            //                            {
            //                                var dt_false = new DataTable();
            //                                using (var conn = new SqlConnection(Local_Conn))
            //                                {
            //                                    var check = conn.CreateCommand();
            //                                    check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code = '{buyer_sub}' and operation_name = '{operation_name}' and working = '1'";
            //                                    var sda = new SqlDataAdapter(check);
            //                                    sda.Fill(dt_false);
            //                                }
            //                                int count_row_false = dt_false.Rows.Count;
            //                                foreach (DataRow dr in dt_false.Rows)
            //                                {
            //                                    metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                                }
            //                            }

            //                        }
            //                        if (counttext > 3)
            //                        {
            //                            for (int i = 3; i <= counttext;)
            //                            {
            //                                if (text[i - 1] != buyer_vlm_str)
            //                                {
            //                                    i++;
            //                                }
            //                                if (i - 1 == counttext)
            //                                {
            //                                    var dt_false = new DataTable();
            //                                    using (var conn = new SqlConnection(Local_Conn))
            //                                    {
            //                                        var check = conn.CreateCommand();
            //                                        check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code = '{buyer_sub}' and operation_name = '{operation_name}' and working = '1'";
            //                                        var sda = new SqlDataAdapter(check);
            //                                        sda.Fill(dt_false);
            //                                    }
            //                                    int count_row_false = dt_false.Rows.Count;
            //                                    foreach (DataRow dr in dt_false.Rows)
            //                                    {
            //                                        metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        // contains str != sub
            //                        var dt_false = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code == '{buyer_sub}' and operation_name = '{operation_name}' and working = '1'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_false);
            //                        }
            //                        int count_row_false = dt_false.Rows.Count;
            //                        foreach (DataRow dr in dt_false.Rows)
            //                        {
            //                            metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");

            //}
            #endregion

            #region "Hiden Code"
            //try
            //{
            //    if (textBoxvinno.TextLength > 14)
            //    {
            //        var dt = new DataTable();
            //        using (var conn = new SqlConnection(Local_Conn))
            //        {
            //            var check = conn.CreateCommand();
            //            check.CommandText = $"Select * from data_vlm_local where vin_no = '{textBoxvinno.Text}' and working = '1'";
            //            var sda = new SqlDataAdapter(check);
            //            sda.Fill(dt);
            //        }
            //        int cout_row = dt.Rows.Count;
            //        if (cout_row == 0)
            //        {
            //            textBoxvinno.Text = "";
            //            MessageBox.Show(" ไม่พบ Vin No : " + textBoxvinno.Text + " โปรดตรวจสอบอีกครั้ง");
            //            return;
            //        }
            //        else
            //        {
            //            string model_prefix = dt.Rows[0]["model_prefix"].ToString();
            //            string model_base = dt.Rows[0]["model_base"].ToString();
            //            string model_suffix = dt.Rows[0]["model_suffix"].ToString();
            //            string model_result = (model_prefix + model_base + model_suffix);
            //            textBoxmodel.Text = model_result;
            //            textBoxbuyer.Text = dt.Rows[0]["buyer_code"].ToString();


            //            var dt_part = new DataTable();
            //            using (var conn = new SqlConnection(Local_Conn))
            //            {
            //                var check = conn.CreateCommand();
            //                check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}'";
            //                var sda = new SqlDataAdapter(check);
            //                sda.Fill(dt_part);
            //            }
            //            string buyer_vlm_str = textBoxbuyer.Text;
            //            string buyer_sub = dt_part.Rows[0]["buyer_code"].ToString();
            //            bool result_contains = (buyer_vlm_str.Contains(buyer_sub));
            //            if (result_contains == true)
            //            {
            //                // contains str = sub
            //                var dt_true = new DataTable();
            //                using (var conn = new SqlConnection(Local_Conn))
            //                {
            //                    var check = conn.CreateCommand();
            //                    check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code = '{buyer_sub}'";
            //                    var sda = new SqlDataAdapter(check);
            //                    sda.Fill(dt_true);
            //                }
            //                int count_row_true = dt_true.Rows.Count;
            //                Create_table();
            //                foreach (DataRow dr in dt_true.Rows)
            //                {
            //                    metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                }
            //            }
            //            else if (result_contains == false)
            //            {
            //                // contains str != sub
            //                var dt_false = new DataTable();
            //                using (var conn = new SqlConnection(Local_Conn))
            //                {
            //                    var check = conn.CreateCommand();
            //                    check.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodel.Text}' and model_base = '{model_base}' and buyer_code != '{buyer_sub}'";
            //                    var sda = new SqlDataAdapter(check);
            //                    sda.Fill(dt_false);
            //                }
            //                int count_row_false = dt_false.Rows.Count;
            //                Create_table();
            //                foreach (DataRow dr in dt_false.Rows)
            //                {
            //                    metroGrid1.Rows.Add(dr["part_no_sync"], dr["part_qty"]);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            //}
            #endregion
        }

        #endregion

        #region "Button Clear"
        private void Buttonclear_Click(object sender, EventArgs e)
        {
            //////
            ///
            //
            //switch_C = 0;
            //labeltest.Text = "";
            //labeltest.Hide();
            
            //timer1.Enabled = true;
            //timer2.Enabled = false;
            /////
            ///

            textBoxvinno.Text = "";
            textBoxmodel.Text = "";
            textBoxbuyer.Text = "";
            textBoxoperationname.Text = "";
            textBoxalarm.Text = "";
            textBoxname.Text = "";
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid2.Columns.Clear();
            metroGrid2.Rows.Clear();
            status_check_partlist = false;
            status_checl_partlist_scan = false;
            labeltest.Hide();
            textBox1.Text = "";


            /////
            ///
            //
            //textBoxname.Text = "Siam Smart Solutions";
            ///////
            ///
            ///

            //GetSerialPort();
        }
        #endregion

        #region "Button Save"
        private void Buttonsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxname.Text == "" || textBoxname.Text == null || textBoxdate.Text == "" || textBoxdate.Text == null || textBoxvinno.Text == "" || textBoxvinno.Text == null || textBoxmodel.Text == "" || textBoxmodel.Text == null)
                {
                    MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    int count_m1 = metroGrid1.Rows.Count;
                    int count_mm2 = metroGrid2.Rows.Count;
                    int count_m2 = count_mm2 - 1;
                    for (int ii = 0; ii + 1 <= count_m2;)
                    {
                        string partlist_rows_check = metroGrid2.Rows[ii].Cells[0].Value.ToString();
                        var dt_check = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * from data_part_local where part_no = '{partlist_rows_check}' and working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt_check);
                        }
                        int count_check = dt_check.Rows.Count;
                        if (count_check >= 1)
                        {
                            status_check_partlist = false;
                            ii++;
                        }
                        else
                        {
                            status_check_partlist = true;
                            MessageBox.Show("ไม่มี Partlist : " + partlist_rows_check + " ,โปรดตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    if (status_check_partlist == false)
                    {
                        if (count_m1 == count_m2)
                        {
                            for (int i = 0; i < count_m2;)
                            {
                                string partlist_rows = metroGrid2.Rows[i].Cells[0].Value.ToString();
                                int cutstock = Convert.ToInt32(metroGrid2.Rows[i].Cells[1].Value.ToString());
                                var cmd = $"Insert into data_log_local (name, vin_no, model_code, part_list, date, status) " +
                                $"Values ('{textBoxname.Text}', '{textBoxvinno.Text}', '{textBoxmodel.Text}', '{partlist_rows}', Getdate(), 'Complete')";
                                if (ExecuteSqlTransaction(cmd, Local_Conn, "Add"))
                                {
                                    var dt_true = new DataTable();
                                    using (var conn = new SqlConnection(Local_Conn))
                                    {
                                        var check = conn.CreateCommand();
                                        check.CommandText = $"Select * from data_part_local where part_no = '{partlist_rows}' and working = '1'";
                                        var sda = new SqlDataAdapter(check);
                                        sda.Fill(dt_true);
                                    }
                                    int stock = Convert.ToInt32(dt_true.Rows[0]["part_qty"].ToString());
                                    int cutstock_result = stock - cutstock;
                                    if (stock >= 0)
                                    {
                                        var cmd1 = $"Update data_part_local " +
                                            $"Set part_qty = '{cutstock_result}' " +
                                            $"Where part_no = '{partlist_rows}'";
                                        if (ExecuteSqlTransaction(cmd1, Local_Conn, "Update"))
                                        {
                                            //i++;
                                        }
                                    }
                                    i++;
                                }
                                else
                                {
                                    MessageBox.Show("บันทึกข้อมูลไม่สำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            //MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            var dt_cut_vin = new DataTable();
                            using (var conn = new SqlConnection(Local_Conn))
                            {
                                var check_cut_vin = conn.CreateCommand();
                                check_cut_vin.CommandText = $"Select * from data_vlm_local where vin_no = '{textBoxvinno.Text}' and working = '1'";
                                var sda = new SqlDataAdapter(check_cut_vin);
                                sda.Fill(dt_cut_vin);
                            }
                            int count_cut_vin = dt_cut_vin.Rows.Count;
                            if (count_cut_vin >= 1)
                            {
                                var cmd_cut_vin = $"Update data_vlm_local " +
                                    $"Set working = '0'" +
                                    $"Where vin_no = '{textBoxvinno.Text}'";
                                if (ExecuteSqlTransaction(cmd_cut_vin, Local_Conn, "Update"))
                                {
                                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("บันทึกข้อมูลไม่สำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            textBoxvinno.Text = "";
                            textBoxmodel.Text = "";
                            textBoxbuyer.Text = "";
                            textBoxoperationname.Text = "";
                            textBoxname.Text = "";
                            metroGrid1.Columns.Clear();
                            metroGrid1.Rows.Clear();
                            metroGrid2.Columns.Clear();
                            metroGrid2.Rows.Clear();
                            status_check_partlist = false;
                            status_checl_partlist_scan = false;
                            Check_stock();
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








            //try
            //{
            //    const string message = "คุณแน่ใจหรือไม่ ว่าต้องการบันทึกข้อมูลนี้ในฐานข้อมูล?";
            //    const string caption = "Add Model to Database";
            //    var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (result == DialogResult.Yes)
            //    {
            //        if (textBoxname.Text == "" || textBoxname.Text == null || textBoxdate.Text == "" || textBoxdate.Text == null || textBoxvinno.Text == "" || textBoxvinno.Text == null || textBoxmodel.Text == "" || textBoxmodel.Text == null || textBoxbuyer.Text == "" || textBoxbuyer.Text == null)
            //        {
            //            MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        }
            //        else
            //        {
            //            int count_m1 = metroGrid1.Rows.Count;
            //            int count_m2 = metroGrid2.Rows.Count;
            //            for (int ii = 1; ii >= count_m2;)
            //            {
            //                string partlist_rows_check = metroGrid2.Rows[ii].Cells[0].Value.ToString();
            //                var dt_check = new DataTable();
            //                using (var conn = new SqlConnection(Local_Conn))
            //                {
            //                    var check = conn.CreateCommand();
            //                    check.CommandText = $"Select * from data_part_local where model_code = '{partlist_rows_check}' and working = '1'";
            //                    var sda = new SqlDataAdapter(check);
            //                    sda.Fill(dt_check);
            //                }
            //                int count_check = dt_check.Rows.Count;
            //                if (count_check >= 1)
            //                {
            //                    status_check_partlist = false;
            //                    ii++;
            //                }
            //                else
            //                {
            //                    status_check_partlist = true;
            //                    MessageBox.Show("ไม่มี Partlist : " + partlist_rows_check + " ,โปรดตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                    return;
            //                }
            //            }
            //            if (status_check_partlist == false)
            //            {
            //                if (count_m1 == count_m2)
            //                {
            //                    for (int i = 1; i >= count_m2;)
            //                    {
            //                        string partlist_rows = metroGrid2.Rows[i].Cells[0].Value.ToString();
            //                        int cutstock = Convert.ToInt32(metroGrid2.Rows[i].Cells[1].Value.ToString());
            //                        var cmd = $"Insert into data_log_local (name, vin_no, 18_dig, part_list, date, status) " +
            //                        $"Values ('{textBoxname.Text}', '{textBoxvinno.Text}', '{textBoxmodel.Text}', '{partlist_rows}', 'Getdate()', 'Complete')";
            //                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Add"))
            //                        {
            //                            i++;
            //                        }
            //                        var dt_true = new DataTable();
            //                        using (var conn = new SqlConnection(Local_Conn))
            //                        {
            //                            var check = conn.CreateCommand();
            //                            check.CommandText = $"Select * from data_part_local where model_code = '{partlist_rows}' and working = '1'";
            //                            var sda = new SqlDataAdapter(check);
            //                            sda.Fill(dt_true);
            //                        }
            //                        int stock = Convert.ToInt32(dt.Rows[0]["part_qty"].ToString());
            //                        int cutstock_result = stock - cutstock;
            //                        if (stock >= 0)
            //                        {
            //                            var cmd1 = $"Update data_part_local " +
            //                                $"Set part_qty = '{cutstock_result}' " +
            //                                $"Where part_no = '{partlist_rows}'";
            //                            if (ExecuteSqlTransaction(cmd1, Local_Conn, "Update"))
            //                            {
            //                                i++;
            //                            }
            //                        }
            //                    }
            //                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    textBoxvinno.Text = "";
            //                    textBoxmodel.Text = "";
            //                    textBoxbuyer.Text = "";
            //                    textBoxoperationname.Text = "";
            //                    metroGrid1.Columns.Clear();
            //                    metroGrid1.Rows.Clear();
            //                    metroGrid2.Columns.Clear();
            //                    metroGrid2.Rows.Clear();
            //                    status_check_partlist = false;
            //                    status_checl_partlist_scan = false;
            //                }
            //                else
            //                {
            //                    MessageBox.Show("ไม่สามารถบันทึกข้อมูลได้, โปรดติดต่อผู้ดูแลระบบ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            partlist_scan = textBox1.Text;
            ScanPartlists();
        }

        private void FormStore_FormClosing(object sender, FormClosingEventArgs e)
        {
            myserialport.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            partlist_scan = textBox1.Text;
            ScanPartlists();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            //textBoxvinno.Focus();
            //if (textBoxvinno.TextLength > 1)
            //{
            //    labeltest.Show();
            //    string text_scan = textBoxvinno.Text;
            //    labeltest.Text = text_scan;
            //    switch_C = 1;
            //    timer2.Enabled = true;
            //}
            //timer1.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //timer2.Enabled = false;
            ////textBox1.Text = "";
            //textBox1.Focus();
            //if (textBox1.TextLength > 1)
            //{
                
            //    if (count_scan + 1 <= count_switch)
            //    {
            //        textBox1.Text = "";
            //        count_scan++;
            //        if (count_scan >= count_switch)
            //        {
            //            timer1.Enabled = false;
            //            timer2.Enabled = false;
            //            return;
            //        }
            //    }
            //}
            //timer2.Enabled = true;
        }
    }
}
