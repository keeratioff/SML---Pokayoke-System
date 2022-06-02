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
    public partial class FormSettingAccessibility : Form
    {
        //Permission
        string Permission;

        //Database
        public static string Local_Conn;
        public static string Catalog_Local;
        public static string Ip_Addr_Local;
        public static string Sql_usr_Local;
        public static string Sql_pw_Local;

        //Location Config file temp
        public static string Location_File_Tmp;
        public static bool status_sql;

        public FormSettingAccessibility()
        {
            InitializeComponent();
            metroGrid1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void FormSettingAccessibility_Load(object sender, EventArgs e)
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

        #region "Function Create table"
        private void Create_table()
        {
            metroGrid1.Columns.Clear();
            metroGrid1.Rows.Clear();
            metroGrid1.ColumnCount = 5;
            metroGrid1.Columns[0].HeaderText = "First Name";
            metroGrid1.Columns[1].HeaderText = "Last Name";
            metroGrid1.Columns[2].HeaderText = "Username";
            metroGrid1.Columns[3].HeaderText = "Password";
            metroGrid1.Columns[4].HeaderText = "Permission";
        }
        #endregion

        #region "Function ShowGrid"
        private void ShowGrid()
        {
            //metroGrid1.Rows.Clear();

            try
            {
                var dt = new DataTable();
                using (var conn = new SqlConnection(Local_Conn))
                {
                    var check = conn.CreateCommand();
                    check.CommandText = "select * from user_local where working = '1'";
                    var sda = new SqlDataAdapter(check);
                    sda.Fill(dt);
                }
                //metroGrid1.DataSource = dt;
                foreach (DataRow dr in dt.Rows)
                {
                    metroGrid1.Rows.Add(dr["name"], dr["surname"], dr["employee_id"], dr["password"], dr["level"]);
                }
            }
            catch (Exception error)
            {
                _ = new LogWriter.LogWriter($" Error Message: {0}, {error.Message}");
            }
        }
        #endregion

        #region "Radio Select"
        private void radioButtonadmin_CheckedChanged(object sender, EventArgs e)
        {
            Permission = "Admin";
        }

        private void radioButtonuser_CheckedChanged(object sender, EventArgs e)
        {
            Permission = "User";
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
                    if (textBoxfirstname.Text == "" || textBoxfirstname.Text == null || textBoxlastname.Text == "" || textBoxlastname.Text == null || textBoxusername.Text == "" || textBoxusername.Text == null || textBoxpassword.Text == "" || textBoxpassword.Text == null || radioButtonadmin.Checked == false && radioButtonuser.Checked == false)
                    {
                        MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string add_firstname = textBoxfirstname.Text;
                        string add_lastname = textBoxlastname.Text;
                        string add_username = textBoxusername.Text;
                        string add_password = textBoxpassword.Text;

                        var dt = new DataTable();
                        using (var conn = new SqlConnection(Local_Conn))
                        {
                            var check = conn.CreateCommand();
                            check.CommandText = $"Select * from user_local where employee_id = '{textBoxusername.Text}' and working = '1'";
                            var sda = new SqlDataAdapter(check);
                            sda.Fill(dt);
                        }
                        int count_rows = dt.Rows.Count;
                        if (count_rows >= 1)
                        {
                            MessageBox.Show("พบ ID พนักงานเหมือนกันกับในฐานข้อมูล โปรดอัปเดตหรือใช้ ID ใหม่เพื่อเพิ่ม", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            textBoxfirstname.Text = "";
                            textBoxlastname.Text = "";
                            textBoxusername.Text = "";
                            textBoxpassword.Text = "";
                            radioButtonadmin.Checked = false;
                            radioButtonuser.Checked = false;
                            Create_table();
                            ShowGrid();
                        }
                        else
                        {
                            var cmd = $"Insert into user_local (employee_id, password, name, surname, level, time_register, time_update, comment, working) " +
                                $"Values ('{add_username}', '{add_password}', '{add_firstname}', '{add_lastname}', '{Permission}', Getdate(), Getdate(), 'System', '1')";
                            if (ExecuteSqlTransaction(cmd, Local_Conn, "Add"))
                            {
                                MessageBox.Show("เพิ่มข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                textBoxfirstname.Text = "";
                                textBoxlastname.Text = "";
                                textBoxusername.Text = "";
                                textBoxpassword.Text = "";
                                radioButtonadmin.Checked = false;
                                radioButtonuser.Checked = false;
                                Create_table();
                                ShowGrid();
                            }
                            else
                            {
                                MessageBox.Show(" ไม่สามารถเพิ่ม ID พนักงานได้, กรุณาลองใหม่อีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            try
            {
                const string message = "คุณแน่ใจหรือไม่ ว่าต้องการอัพเดตโมเดลนี้ในฐานข้อมูล?";
                const string caption = "Add Model to Database";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (textBoxfirstname.Text == "" || textBoxfirstname.Text == null || textBoxlastname.Text == "" || textBoxlastname.Text == null || textBoxusername.Text == "" || textBoxusername.Text == null || textBoxpassword.Text == "" || textBoxpassword.Text == null || radioButtonadmin.Checked == false && radioButtonuser.Checked == false)
                    {
                        MessageBox.Show(" กรุณากรอกข้อมูลให้ครบถ้วนก่อนดำเนินการต่อ ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string add_firstname = textBoxfirstname.Text;
                        string add_lastname = textBoxlastname.Text;
                        string add_username = textBoxusername.Text;
                        string add_password = textBoxpassword.Text;
                        var cmd = $"Update user_local " +
                            $"Set password = '{add_password}', name = '{add_firstname}', surname = '{add_lastname}', level = '{Permission}', time_update = Getdate()" +
                            $"Where employee_id = '{textBoxusername.Text}'";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Update"))
                        {
                            MessageBox.Show("อัพเดทข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBoxfirstname.Text = "";
                            textBoxlastname.Text = "";
                            textBoxusername.Text = "";
                            textBoxpassword.Text = "";
                            radioButtonadmin.Checked = false;
                            radioButtonuser.Checked = false;
                            Create_table();
                            ShowGrid();
                        }
                        else
                        {
                            MessageBox.Show(" ไม่สามารถอัพเดท ID พนักงานได้, กรุณาลองใหม่อีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    if (textBoxfirstname.Text == "" || textBoxfirstname.Text == null || textBoxlastname.Text == "" || textBoxlastname.Text == null || textBoxusername.Text == "" || textBoxusername.Text == null || textBoxpassword.Text == "" || textBoxpassword.Text == null)
                    {
                        MessageBox.Show(" กรุณาเลือกชื่อพนักงานก่อนเริ่มกระบวนการต่อไป", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var cmd = $"Update user_local " +
                            $"Set working = '0' " +
                            $"Where employee_id = '{textBoxusername.Text}'";
                        if (ExecuteSqlTransaction(cmd, Local_Conn, "Delete"))
                        {
                            MessageBox.Show("ลบข้อมูลสำเร็จ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBoxfirstname.Text = "";
                            textBoxlastname.Text = "";
                            textBoxusername.Text = "";
                            textBoxpassword.Text = "";
                            radioButtonadmin.Checked = false;
                            radioButtonuser.Checked = false;
                            Create_table();
                            ShowGrid();
                        }
                        else
                        {
                            MessageBox.Show(" ไม่สามารถลบ ID พนักงานได้, กรุณาลองใหม่อีกครั้ง", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            textBoxfirstname.Text = "";
            textBoxlastname.Text = "";
            textBoxusername.Text = "";
            textBoxpassword.Text = "";
            radioButtonadmin.Checked = false;
            radioButtonuser.Checked = false;
            Create_table();
            ShowGrid();
        }

        #endregion

      
        #region "Gridview Cell Mouseup"
        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxfirstname.Text = this.metroGrid1.CurrentRow.Cells[0].Value.ToString();
            textBoxlastname.Text = this.metroGrid1.CurrentRow.Cells[1].Value.ToString();
            textBoxusername.Text = this.metroGrid1.CurrentRow.Cells[2].Value.ToString();
            textBoxpassword.Text = this.metroGrid1.CurrentRow.Cells[3].Value.ToString();
            string permission_check = this.metroGrid1.CurrentRow.Cells[4].Value.ToString();
            if (permission_check == "Admin")
            {
                radioButtonadmin.Checked = true;
            }
            if (permission_check == "User")
            {
                radioButtonuser.Checked = true;
            }
        }
        #endregion
    }
}
