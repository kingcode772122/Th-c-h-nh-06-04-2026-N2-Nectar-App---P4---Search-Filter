using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class dangky : Form
    {

        string connectstring = @"Data Source=DESKTOP-FT8IM0J;Initial Catalog=quanan;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection conn;// link kết nối 
        SqlCommand cmd;//tạo người gủi lệnh gửi tới sql (câu lệnh,conn link tới sql) cmd.ExecuteNonQuery(); đây là cái để gửi lệnh đến sql;
        SqlDataAdapter adt;//thằng này dùng để tạo datablse điền vào datable
        DataTable dt;//tạo dữ liệu bảng điền vào guna.....
        public dangky()
        {
            InitializeComponent();
        }
        private bool KiemTraTaiKhoanTrung(string taiKhoan)
        {
            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM tai_khoan WHERE tai_khoan = @tk";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tk", taiKhoan);

                    int count = (int)cmd.ExecuteScalar(); // trả về số dòng trùng

                    return count > 0; // true nếu đã tồn tại
                }
            }
        }
        private int GetIdKhach(string taiKhoan)
        {
            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                conn.Open();

                string sql = "SELECT id_khach FROM tai_khoan WHERE tai_khoan = @tk";

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
        private int Getquyen(string taiKhoan)
        {
            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                conn.Open();

                string sql = "SELECT quyen FROM tai_khoan WHERE tai_khoan = @tk";

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
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string tk = guna2TextBox1.Text.Trim();
            string mk = guna2TextBox2.Text.Trim();
            string ht = guna2TextBox3.Text.Trim();
            string sdt = guna2TextBox4.Text.Trim();
            if (KiemTraTaiKhoanTrung(tk))
            {
                MessageBox.Show("Tài khoản tồn tại vui lòng nhập tài khoản khác!");
                return; 
            }
            if (tk == "" || mk == "" || ht == "" || sdt == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ dữ liệu!");
                return;
            }
            string pattern = @"^0(2[0-9]|3[2-9]|5[6|8|9]|7[0|6-9]|8[1-9]|9[0-9])[0-9]{7}$";

            if (!Regex.IsMatch(sdt, pattern))
            {
                errorProvider1.SetError(guna2TextBox2, "Số điện thoại Việt Nam phải đúng 10 chữ số và đúng đầu số nhà mạng!");
                return;
            }
            else
            {
                errorProvider1.SetError(guna2TextBox2, "");
            }

            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                conn.Open();

                SqlTransaction trans = conn.BeginTransaction();// đảm bảo tạo tkmk thì sau sẽ cso thoogn tin khách hàng gắn liền vưới id

                try
                {
                    // 1. INSERT vào KHACH_HANG
                    string sql1 = "INSERT INTO tai_khoan (tai_khoan, mat_khau,quyen) " +
                                  "OUTPUT INSERTED.id_khach " +
                                  "VALUES (@tk, @mk,1)";

                    int idKhach = 0;

                    using (SqlCommand cmd1 = new SqlCommand(sql1, conn, trans))
                    {
                        cmd1.Parameters.AddWithValue("@tk", tk);
                        cmd1.Parameters.AddWithValue("@mk", mk); // nếu muốn mã hóa bcrypt mình viết sau

                        idKhach = (int)cmd1.ExecuteScalar();// trả về id khách mà sql vừa tao ra 
                    }

                    // 2. INSERT vào THONG_TIN_KHACH
                    string sql2 = "INSERT INTO khach_hang (id_khach, ho_ten, so_dien_thoai) " +
                                  "VALUES (@id, @ht, @sdt)";

                    using (SqlCommand cmd2 = new SqlCommand(sql2, conn, trans))
                    {
                        cmd2.Parameters.AddWithValue("@id", idKhach);
                        cmd2.Parameters.AddWithValue("@ht", ht);
                        cmd2.Parameters.AddWithValue("@sdt", sdt);
                        cmd2.ExecuteNonQuery();
                    }

                    // Commit nếu thành công cả 2 bảng
                    trans.Commit();// xác nhận cả 2 vào sql
                    MessageBox.Show("Tạo tài khoản khách hàng thành công!");
                    guna2TextBox4.Text = ""; // tài khoản
                    guna2TextBox3.Text = ""; // mật khẩu
                    guna2TextBox1.Text = ""; // họ tên
                    guna2TextBox2.Text = ""; // số điện thoại

                    // Xóa hết lỗi nếu có
                    errorProvider1.Clear();
                    // truyển id và đổi sang trang thông tin dành cho khách 















                }
                catch (Exception ex)
                {
                    trans.Rollback();// xóa rác nếu đoạn sau lỗi 
                    MessageBox.Show("Lỗi Tạo Tài Khoản " + ex.Message);
                }
            }
        }

       
    }
    
}
