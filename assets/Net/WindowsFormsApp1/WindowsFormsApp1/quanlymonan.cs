using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class quanlymonan : Form
    {
        string connectstring = @"Data Source=DESKTOP-FT8IM0J;Initial Catalog=quanan;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection conn;// link kết nối 
        SqlCommand cmd;//tạo người gủi lệnh gửi tới sql (câu lệnh,conn link tới sql) cmd.ExecuteNonQuery(); đây là cái để gửi lệnh đến sql;
        SqlDataAdapter adt;//thằng này dùng để tạo datablse điền vào datable
        DataTable dt;//tạo dữ liệu bảng điền vào guna.....
                     //   cmd.ExecuteNonQuery();   // UPDATE, INSERT, DELETE
                     //  cmd.ExecuteReader();     // SELECT
                     //  cmd.ExecuteScalar();     // SELECT trả về 1 giá trị
        String path;
        Form ve;
        public quanlymonan(Form lay)
        {
            InitializeComponent();
            ve = lay;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();//CHO CHỌN LINK  =))
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";// LỌC điều kiện chi cho chọn file ảnh 

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Hiển thị ảnh đã chọn
                    guna2PictureBox6.Image = Image.FromFile(ofd.FileName);
                    path = ofd.FileName; //lấy path
                    
                }
                catch
                {
                    MessageBox.Show("Tải ảnh thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectstring);
            hienthi();
        }
        private void hienthi()
        {
            try
            {
                conn.Open();
                 cmd = new SqlCommand("SELECT * FROM mon_an", conn);
                adt = new SqlDataAdapter(cmd);
                dt = new DataTable();
                adt.Fill(dt);

                guna2DataGridView1.DataSource = dt;
                guna2DataGridView1.Columns["id_mon"].ReadOnly = true;
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
        //nút sửa cập nhật 
       private void guna2Button5_Click(object sender, EventArgs e)
{
    using (SqlConnection conn = new SqlConnection(connectstring))
    {
        conn.Open();

        // ======= CẬP NHẬT ẢNH =======
        if (!string.IsNullOrWhiteSpace(path) && guna2DataGridView1.SelectedRows.Count > 0)
        {
            DataGridViewRow row = guna2DataGridView1.SelectedRows[0];
            int idchon = Convert.ToInt32(row.Cells["id_mon"].Value);

            // ❗ SQL 1 dòng => không bao giờ bị ký tự ẩn
            string lenh = "UPDATE mon_an SET DuongDanAnh = @dd WHERE id_mon = @id";

            using (SqlCommand minh = new SqlCommand(lenh, conn))
            {
                minh.Parameters.AddWithValue("@dd", path);
                minh.Parameters.AddWithValue("@id", idchon);
                minh.ExecuteNonQuery();

                // Reset UI
                guna2PictureBox6.Image = Properties.Resources.ảnh_lỗi_removebg_preview;
                path = "";
                guna2TextBox1.Focus();
                hienthi();
            }
        }

        // ======= CẬP NHẬT TÊN + GIÁ =======
        foreach (DataGridViewRow row in guna2DataGridView1.Rows)
        {
            if (row.IsNewRow) continue;

            int id = Convert.ToInt32(row.Cells["id_mon"].Value);
            string tenMon = row.Cells["ten_mon"].Value.ToString();
            decimal gia = Convert.ToDecimal(row.Cells["gia_mon"].Value);

            // ❗ SQL 1 dòng => sạch tuyệt đối
            string query = "UPDATE mon_an SET ten_mon = @ten, gia_mon = @gia WHERE id_mon = @id";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ten", tenMon);
                cmd.Parameters.AddWithValue("@gia", gia);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        MessageBox.Show("Cập nhật thành công!", "SQL Server");
    }
}

        // nút thêm 
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            {
                string ten = guna2TextBox1.Text.Trim();
                decimal gia;
                // Kiểm tra giá có phải số không
                if (!decimal.TryParse(guna2TextBox2.Text, out gia))
                {
                    MessageBox.Show("Giá món phải là số!", "Lỗi");
                    return;
                }

                // Kiểm tra tên món
                if (ten == "")
                {
                    MessageBox.Show("Tên món không được để trống!", "Lỗi");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectstring))
                {
                    conn.Open();

                    string query = @"INSERT INTO mon_an (ten_mon, gia_mon, DuongDanAnh)
                         VALUES (@ten, @gia, @dananh)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", ten);
                        cmd.Parameters.AddWithValue("@gia", gia);
                        cmd.Parameters.AddWithValue("@dananh", path);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm món thành công!");

                // Load lại bảng
                hienthi();
                guna2TextBox1.Text = "";          // Xóa tên món
                guna2TextBox2.Text = "";          // Xóa giá món
                guna2PictureBox6.Image = Properties.Resources.ảnh_lỗi_removebg_preview;    // Xóa ảnh
                path = "";                        // Reset đường dẫn ảnh
                guna2TextBox1.Focus();

            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa!");
                return;
            }

            // Lấy dòng đang chọn
            DataGridViewRow row = guna2DataGridView1.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["id_mon"].Value);

            DialogResult result = MessageBox.Show(
                "Bạn chắc chắn muốn xóa món này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No) return;

            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                conn.Open();
                string query = "DELETE FROM mon_an WHERE id_mon = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            // Xóa khỏi DataGridView để giao diện cập nhật
            guna2DataGridView1.Rows.Remove(row);

            MessageBox.Show("Xóa thành công!");
            hienthi();
        }
        // hàm tìm kiếm 
        private void TimKiem()
        {
            string ten = guna2TextBox1.Text.Trim();
            string giaText = guna2TextBox2.Text.Trim();

            bool coTen = !string.IsNullOrEmpty(ten);
            bool coGia = decimal.TryParse(giaText, out decimal giaMax);

            string query = "SELECT * FROM mon_an WHERE 1=1";

            if (coTen)
                query += " AND ten_mon LIKE @ten";

            if (coGia)
                query += " AND gia_mon <= @giaMax";

            using (SqlConnection conn = new SqlConnection(connectstring))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (coTen)
                        cmd.Parameters.AddWithValue("@ten", "%" + ten + "%");

                    if (coGia)
                        cmd.Parameters.AddWithValue("@giaMax", giaMax);

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    guna2DataGridView1.DataSource = dt;
                }
            }
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            ve.Show();
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            quanlykhach khach=new quanlykhach(ve);
            khach.Show();
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            quanlynhanvien nhanvien = new quanlynhanvien(ve);
            nhanvien.Show();
            this.Close();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            quanlybanan banan = new quanlybanan(ve);
            banan.Show();
            this.Close();
        }

        private void guna2Panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            thongkethunhap thongke = new thongkethunhap(ve);
            thongke.Show();
            this.Close();
        }
    }
    
}
