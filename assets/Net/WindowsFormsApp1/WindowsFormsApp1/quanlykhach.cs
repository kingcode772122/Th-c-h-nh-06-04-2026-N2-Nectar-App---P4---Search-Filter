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
    public partial class quanlykhach : Form
    {
        string connectstring = @"Data Source=DESKTOP-FT8IM0J;Initial Catalog=quanan;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection conn;// link kết nối 
        SqlCommand cmd;//tạo người gủi lệnh gửi tới sql (câu lệnh,conn link tới sql) cmd.ExecuteNonQuery(); đây là cái để gửi lệnh đến sql;
        SqlDataAdapter adt;//thằng này dùng để tạo datablse điền vào datable
        DataTable dt;//tạo dữ liệu bảng điền vào guna.....
        Form ve;
        public quanlykhach(Form lay)
        {

            InitializeComponent();
            ve = lay;

        }
        private void hienthi()
        {
            try
            {
                conn.Open();

                string sql = @"
            SELECT 
                khach_hang.id_khach, 
                tai_khoan.tai_khoan,
                tai_khoan.mat_khau,
                khach_hang.ho_ten,
                khach_hang.so_dien_thoai,
                khach_hang.xep_hang_khach
            FROM khach_hang
            INNER JOIN tai_khoan 
                ON khach_hang.id_khach = tai_khoan.id_khach";

                cmd = new SqlCommand(sql, conn);
                adt = new SqlDataAdapter(cmd);
                dt = new DataTable();
                adt.Fill(dt);

                guna2DataGridView1.DataSource = dt;

                // Ẩn cột id_khach nhưng vẫn giữ để thao tác
                if (guna2DataGridView1.Columns.Contains("id_khach"))
                {
                    guna2DataGridView1.Columns["id_khach"].Visible = false;
                }

                // Nếu muốn cột nào chỉ đọc
                if (guna2DataGridView1.Columns.Contains("tai_khoan"))
                    guna2DataGridView1.Columns["tai_khoan"].ReadOnly = true;

                // Đổi tên cột TIẾNG VIỆT (dùng HeaderText)
              
                    guna2DataGridView1.Columns["tai_khoan"].HeaderText = "Tài khoản";
               
                    guna2DataGridView1.Columns["mat_khau"].HeaderText = "Mật khẩu";
               
                    guna2DataGridView1.Columns["ho_ten"].HeaderText = "Họ tên";
              
                    guna2DataGridView1.Columns["so_dien_thoai"].HeaderText = "Số điện thoại";
             
                    guna2DataGridView1.Columns["xep_hang_khach"].HeaderText = "Xếp hạng";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string tk = guna2TextBox4.Text.Trim();
            string mk = guna2TextBox3.Text.Trim();
            string ht = guna2TextBox1.Text.Trim();
            string sdt = guna2TextBox2.Text.Trim();

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
                    hienthi();
                    guna2TextBox4.Text = ""; // tài khoản
                    guna2TextBox3.Text = ""; // mật khẩu
                    guna2TextBox1.Text = ""; // họ tên
                    guna2TextBox2.Text = ""; // số điện thoại

                    // Xóa hết lỗi nếu có
                    errorProvider1.Clear();
                }
                catch (Exception ex)
                {
                    trans.Rollback();// xóa rác nếu đoạn sau lỗi 
                    MessageBox.Show("Lỗi SQL: " + ex.Message);
                }
            }
        }

        private void quanlykhach_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectstring);
            hienthi();
        }
        // xóa--------------------------------------------------------------
        private void guna2Button3_Click(object sender, EventArgs e)
        {
             // Không chọn dòng nào
    if (guna2DataGridView1.SelectedRows.Count == 0)
    {
        MessageBox.Show("Vui lòng chọn ít nhất 1 tài khoản để xoá!");
        return;
    }

    // Xác nhận xoá
    if (MessageBox.Show("Bạn có chắc muốn xoá các tài khoản đã chọn?",
                        "Xác nhận xoá",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) == DialogResult.No)
        return;

    using (SqlConnection conn = new SqlConnection(connectstring))
    {
        conn.Open();
        SqlTransaction trans = conn.BeginTransaction();

        try
        {
            foreach (DataGridViewRow row in guna2DataGridView1.SelectedRows)
            {
                int idKhach = Convert.ToInt32(row.Cells["id_khach"].Value);

                // Xóa ở bảng KHACH_HANG trước
                string sql1 = "DELETE FROM khach_hang WHERE id_khach = @id";
                using (SqlCommand cmd1 = new SqlCommand(sql1, conn, trans))
                {
                    cmd1.Parameters.AddWithValue("@id", idKhach);
                    cmd1.ExecuteNonQuery();
                }

                // Xóa ở bảng TAI_KHOAN sau
                string sql2 = "DELETE FROM tai_khoan WHERE id_khach = @id";
                using (SqlCommand cmd2 = new SqlCommand(sql2, conn, trans))
                {
                    cmd2.Parameters.AddWithValue("@id", idKhach);
                    cmd2.ExecuteNonQuery();
                }
            }

            trans.Commit();
            MessageBox.Show("Đã xoá thành công các tài khoản đã chọn!");

            hienthi(); // load lại bảng
        }
        catch (Exception ex)
        {
            trans.Rollback();
            MessageBox.Show("Lỗi khi xoá nhiều: " + ex.Message);
        }
    }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string ten = guna2TextBox1.Text.Trim();
            string sdt = guna2TextBox2.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                try
                {
                    conn.Open();

                    string sql = @"
                SELECT 
                    khach_hang.id_khach, 
                    tai_khoan.tai_khoan,
                    tai_khoan.mat_khau,
                    khach_hang.ho_ten,
                    khach_hang.so_dien_thoai,
                    khach_hang.xep_hang_khach
                FROM khach_hang
                INNER JOIN tai_khoan 
                    ON khach_hang.id_khach = tai_khoan.id_khach
                WHERE 1 = 1";

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    // Tìm theo tên
                    if (!string.IsNullOrEmpty(ten))
                    {
                        sql += " AND khach_hang.ho_ten LIKE @ten";
                        cmd.Parameters.AddWithValue("@ten", "%" + ten + "%");//LIKE '%chuỗi%' cho phép đứng trc hoặc sau bắt kể chuxoi ký tự nào 
                    }

                    // Tìm theo số điện thoại
                    if (!string.IsNullOrEmpty(sdt))
                    {
                        sql += " AND khach_hang.so_dien_thoai LIKE @sdt";
                        cmd.Parameters.AddWithValue("@sdt", "%" + sdt + "%");
                    }

                    cmd.CommandText = sql;

                    SqlDataAdapter adt = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adt.Fill(dt);

                    guna2DataGridView1.DataSource = dt;

                    if (dt.Rows.Count == 0)
                        MessageBox.Show("Không tìm thấy khách hàng phù hợp!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
                }
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn hàng muốn cập nhật!");
                return;
            }

            try
            {
                DataGridViewRow row = guna2DataGridView1.SelectedRows[0];

                // Lấy dữ liệu mới từ hàng đã sửa
                int idKhach = Convert.ToInt32(row.Cells["id_khach"].Value);
                string hoTen = row.Cells["ho_ten"].Value.ToString();
                string sdt = row.Cells["so_dien_thoai"].Value.ToString();
                int xepHang = Convert.ToInt32(row.Cells["xep_hang_khach"].Value);

                // Validate số điện thoại
                string pattern = @"^0(2[0-9]|3[2-9]|5[6|8|9]|7[0|6-9]|8[1-9]|9[0-9])[0-9]{7}$";

                if (!Regex.IsMatch(sdt, pattern))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ!");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectstring))
                {
                    conn.Open();

                    string sql = @"UPDATE khach_hang
                           SET ho_ten = @ht,
                               so_dien_thoai = @sdt,
                               xep_hang_khach = @xh
                           WHERE id_khach = @id";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ht", hoTen);
                        cmd.Parameters.AddWithValue("@sdt", sdt);
                        cmd.Parameters.AddWithValue("@xh", xepHang);
                        cmd.Parameters.AddWithValue("@id", idKhach);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Cập nhật thành công!");
                hienthi(); // reload bảng
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            quanlymonan mon = new quanlymonan(ve);
            mon.Show();
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            quanlynhanvien nv = new quanlynhanvien(ve);
            nv.Show();
            this.Close();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            quanlybanan ban = new quanlybanan(ve);
            ban.Show();
            this.Close();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            thongkethunhap tk = new thongkethunhap(ve);
            tk.Show();
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            ve.Show();
            this.Close();
        }
    }
}
