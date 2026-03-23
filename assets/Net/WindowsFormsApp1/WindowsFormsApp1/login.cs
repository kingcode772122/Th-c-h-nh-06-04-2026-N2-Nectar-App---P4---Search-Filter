using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class login : Form
    {

        string connectstring = @"Data Source=DESKTOP-FT8IM0J;Initial Catalog=quanan;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection conn;// link kết nối 
        SqlCommand cmd;//tạo người gủi lệnh gửi tới sql (câu lệnh,conn link tới sql) cmd.ExecuteNonQuery(); đây là cái để gửi lệnh đến sql;
        SqlDataAdapter adt;//thằng này dùng để tạo datablse điền vào datable
        DataTable dt;//tạo dữ liệu bảng điền vào guna.....
        public login()
        {
            InitializeComponent();
        }
        //
        private int GetIdnhanvien(string taiKhoan)
        {
            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                conn.Open();

                string sql = "SELECT id_khach FROM tai_khoan WHERE tai_khoan = @tk AND quyen=2";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tk", taiKhoan);

                    object result = cmd.ExecuteScalar();

                    // Nếu có dữ liệu → trả ID
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);

                    // Không tìm thấy → trả -1
                    return -1;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guna2Panel1.Left = (this.ClientSize.Width - guna2Panel1.Width) / 2;
            guna2Panel1.Top = (this.ClientSize.Height - guna2Panel1.Height) / 2;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            dangky dangky = new dangky();
            dangky.Show();
        }

        // kiểm tra xem tkmk có không 
        private bool KiemTraTaiKhoanVaMatKhau(string taiKhoan, string matKhau)
        {
            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM tai_khoan WHERE tai_khoan = @tk AND mat_khau = @mk AND quyen=2";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tk", taiKhoan);
                    cmd.Parameters.AddWithValue("@mk", matKhau);

                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;  // true nếu tìm thấy đúng tài khoản + mật khẩu
                }
            }
        }
        // đăng nhập 
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int idnhanvien;
            string tk = guna2TextBox1.Text.Trim();
            string mk = guna2TextBox2.Text.Trim();
            if (tk == "" || mk == "" )
            {
                MessageBox.Show("Vui lòng nhập đầy đủ dữ liệu!");
                return;
            }
            if (tk=="admin"&& mk=="admin")
            {
                quanlymonan f2 = new quanlymonan(this);
                f2.Show();
                this.Hide();
            }
            else {
                if (!KiemTraTaiKhoanVaMatKhau(tk, mk))
                {
                    MessageBox.Show("tài khoản or mật khẩu không đúng vui lòng nhập lại!");
                    return;
                }
                idnhanvien = GetIdnhanvien(tk);
                trangnhanvien f4 = new trangnhanvien(idnhanvien,this);
                f4.Show();
                this.Hide();
            }


      
        }
        bool showPass = false;
        //















        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            showPass = !showPass;

            if (showPass)
            {
                guna2TextBox2.UseSystemPasswordChar = false; // Hiển mật khẩu
                guna2PictureBox1.Image = Properties.Resources.view; // icon mắt mở
                guna2PictureBox1.FillColor = Color.FromArgb(200, 200, 200); // làm sáng icon nếu cần
            }
            else
            {
                guna2TextBox2.UseSystemPasswordChar = true; // Ẩn mật khẩu
                guna2PictureBox1.Image = Properties.Resources.hide; // icon mắt gạch chéo
                guna2PictureBox1.FillColor = Color.FromArgb(120, 120, 120); // icon mờ đi
            }
        }
    }
}
