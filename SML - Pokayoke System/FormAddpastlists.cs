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
    public partial class FormAddpastlists : Form
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

        public FormAddpastlists()
        {
            InitializeComponent();
        }

        private void FormAddpastlists_Load(object sender, EventArgs e)
        {
            Location_File_Tmp = "C:/SSS";
            Read_Systemfile(Location_File_Tmp + "\\System file local.txt");
            Local_Conn = $"Data Source={Ip_Addr_Local};Initial Catalog={Catalog_Local};User ID={Sql_usr_Local};password={Sql_pw_Local}";
            //combocox_partlist();
            combobox_item();
            Create_table();
            ShowGrid_All();
            Autocomplete();
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
            metroGridmodel.Columns.Clear();
            metroGridmodel.Rows.Clear();
            metroGridmodel.ColumnCount = 5;
            metroGridmodel.Columns[0].HeaderText = "Model Base";
            metroGridmodel.Columns[1].HeaderText = "Model Code";
            metroGridmodel.Columns[2].HeaderText = "Buyer Code";
            metroGridmodel.Columns[3].HeaderText = "Partlist";
            metroGridmodel.Columns[4].HeaderText = "Operation Name";
        }
        #endregion

        private void Autocomplete()
        {
            try
            {
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();
                SqlConnection con = new SqlConnection(Local_Conn);
                con.Open();
                string sql = "select * from data_part_local where working = '1'";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sdr = null;
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    col.Add(sdr["part_no"].ToString());
                }
                sdr.Close();
                textBoxpartlist.AutoCompleteCustomSource = col;
                con.Close();
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }

        #region "Combobox partlist"
        private void combocox_partlist()
        {
            //try
            //{
            //    var dt = new DataTable();
            //    using (var conn = new SqlConnection(Local_Conn))
            //    {
            //        var check = conn.CreateCommand();
            //        check.CommandText = "Select * from data_part_local where working = '1'";
            //        var sda = new SqlDataAdapter(check);
            //        sda.Fill(dt);
            //    }
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        comboBoxpartlist.Items.Add(dr["part_no"]);
            //    }
            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            //}
        }
        #endregion

        #region "Combobox item"
        private void combobox_item()
        {
            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check = conn.CreateCommand();
                    check.CommandText = "select distinct buyer_code from data_master_list_local where working = 1 order by buyer_code";
                    var sda = new SqlDataAdapter(check);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    comboBoxbuyercode.Items.Add(dr["buyer_code"]);
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "ShowGrid All"
        private void ShowGrid_All()
        {
            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $"Select * from data_master_list_local where working = '1'";
                    var sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    metroGridmodel.Rows.Add(dr["model_base"], dr["model_code"], dr["buyer_code"], dr["part_no_sync"], dr["operation_name"]);
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Show Grid for Partlist"
        private void ShowGrid_Partlist()
        {
            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = $"Select * from data_master_list_local where model_code = '{textBoxmodelcode.Text}' and buyer_code = '{comboBoxbuyercode.Text}' and working = '1'";
                    var sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    metroGridmodel.Rows.Add(dr["model_base"], dr["model_code"], dr["buyer_code"], dr["part_no_sync"], dr["operation_name"]);
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Text Change for Model Code"
        private void textBoxsearchmodelcode_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxsearchmodelcode.Text = "";
        }

        private void textBoxsearchmodelcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxsearchmodelcode.TextLength > 1)
                {
                    Create_table();
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = "Select * from data_master_list_local where model_code Like '%" + textBoxsearchmodelcode.Text + "%' and working = '1'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        metroGridmodel.Rows.Add(dr["model_base"], dr["model_code"], dr["buyer_code"], dr["part_no_sync"], dr["operation_name"]);
                    }
                }
                //else
                //{
                //    metroGridmodel.Columns.Clear();
                //    metroGridmodel.Rows.Clear();
                //}
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Text Change for Model Base"

        private void textBoxsearchmodelbase_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxsearchmodelbase.Text = "";
        }

        private void textBoxsearchmodelbase_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxsearchmodelbase.TextLength > 1)
                {
                    Create_table();
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = "Select * from data_master_list_local where model_base Like '%" + textBoxsearchmodelbase.Text + "%' and working = '1'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        metroGridmodel.Rows.Add(dr["model_base"], dr["model_code"], dr["buyer_code"], dr["part_no_sync"], dr["operation_name"]);
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Button Add Partlist"
        private void Buttonadd_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = " คุณแน่ใจหรือไม่ ว่าต้องการเพิ่มข้อมูลนี้ในฐานข้อมูล? ";
                const string caption = "Add Data Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    if (textBoxmodelcode.Text == "" || textBoxmodelcode.Text == null || textBoxmodelbase.Text == "" || textBoxmodelbase.Text == null || textBoxoperationname.Text == "" || textBoxoperationname.Text == null || textBoxpartlist.Text == "" || textBoxpartlist.Text == null)
                    {
                        MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * from data_part_local where part_no = '{textBoxpartlist.Text}' and working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int count_rows_check = dt.Rows.Count;
                        if (count_rows_check >= 1)
                        {
                            string add_model_code = textBoxmodelcode.Text;
                            string add_model_base = textBoxmodelbase.Text;
                            // string add_buyer_code = textBoxbuyercode.Text;
                            string add_buyer_code = comboBoxbuyercode.Text;
                            string add_operation_name = textBoxoperationname.Text;
                            string add_partlist = textBoxpartlist.Text;
                            var cmd = $"Insert into data_master_list_local (model_base, model_code, buyer_code, part_no_sync, part_qty, operation_name, image, working) " +
                                $"Values ('{add_model_base}', '{add_model_code}', '{add_buyer_code}', '{add_partlist}', '1' ,'{add_operation_name}', '{null}', '1')";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "ADD"))
                            {
                                var dt_checkpartlist = new DataTable();
                                using (var conn = new SqlConnection(Local_Conn))
                                {
                                    var check = conn.CreateCommand();
                                    check.CommandText = $"Select * from data_part_local where part_no = '{textBoxpartlist.Text}' and working = '1'";
                                    var sda = new SqlDataAdapter(check);
                                    sda.Fill(dt_checkpartlist);
                                }
                                int checkpartlist_count = dt_checkpartlist.Rows.Count;
                                if (checkpartlist_count == 0)
                                {
                                    var cmd1 = $"Insert into data_part_local (part_no, part_qty, working) " +
                                        $"Values ('{add_partlist}', 'null', '1')";
                                    if (ExecuteSqlTransaction(cmd1, Local_Conn, "ADD"))
                                    {
                                        MessageBox.Show(" เพิ่มข้อมูลสำเร็จ ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //textBoxbuyercode.Text = "";
                                        comboBoxbuyercode.Text = "";
                                        textBoxmodelcode.Text = "";
                                        textBoxpartlist.Text = "";
                                        textBoxoperationname.Text = "";
                                        textBoxmodelbase.Text = "";
                                        Create_table();
                                        ShowGrid_All();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show(" ไม่สามารถเพิ่มข้อมูลได้, กรุณาตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
            #region "Hiden Code"
            //try
            //{
            //    const string message = " คุณแน่ใจหรือไม่ ว่าต้องการเพิ่มข้อมูลนี้ในฐานข้อมูล? ";
            //    const string caption = "Add Data Model to Database";
            //    var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //    if (result == DialogResult.Yes)
            //    {
            //        if (textBoxbuyercode.Text == "" || textBoxbuyercode.Text == null || textBoxmodelcode.Text == "" || textBoxmodelcode.Text == null || textBoxpartlist.Text == "" || textBoxpartlist.Text == null || textBoxoperationname.Text == "" || textBoxoperationname.Text == null || textBoxmodelbase.Text == "" || textBoxmodelbase.Text == null)
            //        {
            //            MessageBox.Show(" กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        }
            //        else
            //        {
            //            string add_model_code = textBoxmodelcode.Text;
            //            string add_model_base = textBoxmodelbase.Text;
            //            string add_buyer_code = textBoxbuyercode.Text;
            //            string add_operation_name = textBoxoperationname.Text;
            //            string add_partlist = textBoxpartlist.Text;
            //            var cmd = $"Insert into data_master_list_local (model_base, model_code, buyer_code, part_no_sync, part_qty, operation_name, image, working) " +
            //                $"Values ('{add_model_base}', '{add_model_code}', '{add_buyer_code}', '{add_partlist}', '1' ,'{add_operation_name}', '{null}', '1')";
            //            if (ExecuteSqlTransaction(cmd, Local_Conn, "ADD"))
            //            {
            //                MessageBox.Show(" เพิ่มข้อมูลสำเร็จ ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                textBoxbuyercode.Text = "";
            //                textBoxmodelcode.Text = "";
            //                textBoxpartlist.Text = "";
            //                textBoxoperationname.Text = "";
            //                textBoxmodelbase.Text = "";
            //                Create_table();
            //                ShowGrid_All();
            //            }
            //            else
            //            {
            //                MessageBox.Show(" ไม่สามารถเพิ่มข้อมูลได้, กรุณาตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            }
            //        }
            //    }

            //}
            //catch (Exception error)
            //{
            //    _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            //}


            //try
            //{
            //    const string message = " คุณแน่ใจหรือไม่ ว่าต้องการเพิ่มข้อมูลนี้ในฐานข้อมูล? ";
            //    const string caption = "Add Data Model to Database";
            //    var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //    if (result == DialogResult.Yes)
            //    {
            //        if (textBoxbuyercode.Text == "" || textBoxbuyercode.Text == null || textBoxmodelcode.Text == "" || textBoxmodelcode.Text == null || textBoxpartlist.Text == "" || textBoxpartlist.Text == null || textBoxoperationname.Text == "" || textBoxoperationname.Text == null || textBoxmodelbase.Text == "" || textBoxmodelbase.Text == null)
            //        {
            //            MessageBox.Show(" กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        }
            //        else
            //        {
            //            var dt = new DataTable();
            //            using (var conn = new SqlConnection(Local_Conn))
            //            {
            //                var check = conn.CreateCommand();
            //                check.CommandText = $"Select * From data_master_list_local where model_code = '{textBoxmodelcode.Text}' and working = '1'";
            //                var sda = new SqlDataAdapter(check);
            //                sda.Fill(dt);
            //            }
            //            int count_check = dt.Rows.Count;
            //            if (count_check >= 1)
            //            {
            //                MessageBox.Show(" มีข้อมูลซ้ำในระบบ, กรูณาตรวจสอบอีกครั้ง ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                textBoxbuyercode.Text = "";
            //                textBoxmodelcode.Text = "";
            //                textBoxpartlist.Text = "";
            //                textBoxoperationname.Text = "";
            //                textBoxmodelbase.Text = "";
            //                Create_table();
            //                ShowGrid_All();
            //            }
            //            else
            //            {
            //                string add_model_code = textBoxmodelcode.Text;
            //                string add_model_base = textBoxmodelbase.Text;
            //                string add_buyer_code = textBoxbuyercode.Text;
            //                string add_operation_name = textBoxoperationname.Text;
            //                string add_partlist = textBoxpartlist.Text;
            //                var cmd = $"Insert into data_master_list_local (model_base, model_code, buyer_code, part_no_sync, part_qty, operation_name, image, working) " +
            //                    $"Values ('{add_model_base}', '{add_model_code}', '{add_buyer_code}', '{add_partlist}', '1' ,'{add_operation_name}', '{null}', '1')";
            //                if (ExecuteSqlTransaction(cmd, Local_Conn, "ADD"))
            //                {
            //                    MessageBox.Show(" เพิ่มข้อมูลสำเร็จ ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    textBoxbuyercode.Text = "";
            //                    textBoxmodelcode.Text = "";
            //                    textBoxpartlist.Text = "";
            //                    textBoxoperationname.Text = "";
            //                    textBoxmodelbase.Text = "";
            //                    Create_table();
            //                    ShowGrid_All();
            //                }
            //                else
            //                {
            //                    MessageBox.Show(" ไม่สามารถเพิ่มข้อมูลได้, กรุณาตรวจสอบอีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        #region "Button Update Partlist"
        private void Buttonupdate_Click(object sender, EventArgs e)
        {
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล?";
                const string caption = "Update Data Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    var dt = new DataTable();
                    using (var conn = new SqlConnection(Local_Conn))
                    {
                        var check = conn.CreateCommand();
                        check.CommandText = $"Select * From data_master_list_local where model_code Like '%" + textBoxmodelcode.Text + "%' and working = '1'";
                        var sda = new SqlDataAdapter(check);
                        sda.Fill(dt);
                    }
                    int count_check = dt.Rows.Count;
                    if (count_check == 0)
                    {
                        MessageBox.Show("ไม่มีข้อมูลในระบบ ไม่สามารถแก้ไขข้อมูลได้ กรุณาตรวจสอบอีกครั้ง", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //textBoxbuyercode.Text = "";
                        comboBoxbuyercode.Text = "";
                        textBoxmodelcode.Text = "";
                        textBoxpartlist.Text = "";
                        textBoxoperationname.Text = "";
                        textBoxmodelbase.Text = "";
                        Create_table();
                        ShowGrid_All();
                    }
                    else
                    {
                        var cmd = $"Update data_master_list_local " +
                            $"Set part_no_sync = '{textBoxpartlist.Text}' " +
                            $"Where model_code Like '%" + textBoxmodelcode.Text + "%'";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                        {
                            MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //textBoxbuyercode.Text = "";
                            comboBoxbuyercode.Text = "";
                            textBoxmodelcode.Text = "";
                            textBoxpartlist.Text = "";
                            textBoxoperationname.Text = "";
                            textBoxmodelbase.Text = "";
                            Create_table();
                            ShowGrid_All();
                        }
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }

            #region "Hiden Code"
            //try
            //{
            //    const string message = "คุณแน่ใจหรือไม่ ว่าต้องการอัปเดตข้อมูลนี้ไปยังฐานข้อมูล?";
            //    const string caption = "Update Data Model to Database";
            //    var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //    if (result == DialogResult.Yes)
            //    {
            //        if (textBoxbuyercode.Text == "" || textBoxbuyercode.Text == null || textBoxmodelcode.Text == "" || textBoxmodelcode.Text == null || textBoxpartlist.Text == "" || textBoxpartlist.Text == null || textBoxoperationname.Text == "" || textBoxoperationname.Text == null || textBoxmodelbase.Text == "" || textBoxmodelbase.Text == null)
            //        {
            //            MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        }
            //        else
            //        {
            //            var dt = new DataTable();
            //            using (var conn = new SqlConnection(Local_Conn))
            //            {
            //                var check = conn.CreateCommand();
            //                check.CommandText = $"Select * From data_master_list_local where model_code Like '%" + textBoxmodelcode.Text + "%' and working = '1'";
            //                var sda = new SqlDataAdapter(check);
            //                sda.Fill(dt);
            //            }
            //            int count_check = dt.Rows.Count;
            //            if (count_check == 0)
            //            {
            //                MessageBox.Show("ไม่มีข้อมูลในระบบ ไม่สามารถแก้ไขข้อมูลได้ กรุณาตรวจสอบอีกครั้ง", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                textBoxbuyercode.Text = "";
            //                textBoxmodelcode.Text = "";
            //                textBoxpartlist.Text = "";
            //                textBoxoperationname.Text = "";
            //                textBoxmodelbase.Text = "";
            //                Create_table();
            //                ShowGrid_All();
            //            }
            //            else
            //            {
            //                var cmd = $"Update data_master_list_local " +
            //                    $"Set part_no_sync = '{textBoxpartlist.Text}' " +
            //                    $"Where model_code Like '%" + textBoxmodelcode.Text + "%'";
            //                if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
            //                {
            //                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    textBoxbuyercode.Text = "";
            //                    textBoxmodelcode.Text = "";
            //                    textBoxpartlist.Text = "";
            //                    textBoxoperationname.Text = "";
            //                    textBoxmodelbase.Text = "";
            //                    Create_table();
            //                    ShowGrid_All();
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
            try
            {
                //textBoxbuyercode.Text = "";
                textBoxmodelcode.Text = "";
                textBoxpartlist.Text = "";
                textBoxoperationname.Text = "";
                textBoxmodelbase.Text = "";
                textBoxsearchmodelbase.Text = "Search";
                textBoxsearchmodelcode.Text = "Search";
                comboBoxbuyercode.Text = "";
                combobox_item();
                Create_table();
                ShowGrid_All();
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
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการลบข้อมูลนี้ในฐานข้อมูล?";
                const string caption = "Delete model from database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    string update_model = textBoxmodelcode.Text;
                    //string update_buyer = textBoxbuyercode.Text;
                    string update_buyer = comboBoxbuyercode.Text;
                    string update_part = textBoxpartlist.Text;
                    string update_operationname = textBoxoperationname.Text;
                    var cmd = $"Update data_master_list_local " +
                            $"Set working = '0' " +
                            $"Where model_code = '{textBoxmodelcode.Text}' and buyer_code = '{comboBoxbuyercode.Text}' and part_no_sync = '{textBoxpartlist.Text}' and operation_name = '{textBoxoperationname.Text}'";
                    if (ExecuteSqlTransaction(cmd, Local_Conn, "Delete"))
                    {
                        MessageBox.Show("ลบข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //textBoxbuyercode.Text = "";
                        textBoxmodelcode.Text = "";
                        textBoxpartlist.Text = "";
                        textBoxoperationname.Text = "";
                        textBoxmodelbase.Text = "";
                        comboBoxbuyercode.Text = "";
                        combobox_item();
                        Create_table();
                        ShowGrid_All();
                    }
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }



            //try
            //{
            //    const string message = "คุณแน่ใจหรือไม่ ว่าต้องการลบข้อมูลนี้ในฐานข้อมูล?";
            //    const string caption = "Delete model from database";
            //    var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //    if (result == DialogResult.Yes)
            //    {
            //        if (textBoxmodelcode.Text == "" || textBoxmodelcode.Text == null || textBoxmodelbase.Text == "" || textBoxmodelbase.Text == null || textBoxbuyercode.Text == "" || textBoxbuyercode.Text == null || textBoxoperationname.Text == "" || textBoxoperationname.Text == null || textBoxpartlist.Text == "" || textBoxpartlist.Text == null)
            //        {
            //            MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        }
            //        else
            //        {
            //            var cmd = $"Update data_master_list_local " +
            //                $"Set working = '0' " +
            //                $"Where model_code = '{textBoxmodelcode.Text}' and buyer_code = '{textBoxbuyercode}'";
            //            if (ExecuteSqlTransaction(cmd, Local_Conn, "Delete"))
            //            {
            //                MessageBox.Show("ลบข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                textBoxbuyercode.Text = "";
            //                textBoxmodelcode.Text = "";
            //                textBoxpartlist.Text = "";
            //                textBoxoperationname.Text = "";
            //                textBoxmodelbase.Text = "";
            //                Create_table();
            //                ShowGrid_All();
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

        #region "Grid Cell mouse up"
        private void metroGridmodel_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                textBoxmodelbase.Text = metroGridmodel.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBoxmodelcode.Text = metroGridmodel.Rows[e.RowIndex].Cells[1].Value.ToString();
                //textBoxbuyercode.Text = metroGridmodel.Rows[e.RowIndex].Cells[2].Value.ToString();
                comboBoxbuyercode.Text = metroGridmodel.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBoxpartlist.Text = metroGridmodel.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBoxoperationname.Text = metroGridmodel.Rows[e.RowIndex].Cells[4].Value.ToString();
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }


        #endregion
    }
}
