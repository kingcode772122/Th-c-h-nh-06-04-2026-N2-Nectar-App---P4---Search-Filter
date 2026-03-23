using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class trangnhanvien : Form
    {
        int idnhanvien;
        string connectstring = @"Data Source=DESKTOP-FT8IM0J;Initial Catalog=quanan;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection conn;
        Form g;

        public trangnhanvien(int id,Form login)
        {
            InitializeComponent();
            idnhanvien = id;
             g = login;
        }

        private void trangnhanvien_Load(object sender, EventArgs e)
        {
            try
            {
                // Load thông tin nhân viên
                LoadNhanVienInfo();

                // Load danh sách món ăn
                listmonan();

                // Load danh sách order
                LoadOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load form: " + ex.Message);
            }
        }

        // Load thông tin nhân viên
        void LoadNhanVienInfo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectstring))
                {
                    conn.Open();
                    string sql = "SELECT tai_khoan FROM tai_khoan WHERE id_khach = @id";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idnhanvien);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string name = result.ToString();
                            label1.Text = name;
                            label2.Text = name + "@gmail.com";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load thông tin nhân viên: " + ex.Message);
            }
        }

        // Load danh sách món ăn vào combobox
        void listmonan()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectstring))
                {
                    conn.Open();
                    string query = "SELECT id_mon, ten_mon FROM mon_an";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    guna2ComboBox1.DataSource = dt;
                    guna2ComboBox1.DisplayMember = "ten_mon";
                    guna2ComboBox1.ValueMember = "id_mon";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load món ăn: " + ex.Message);
            }
        }

        // Lấy tổng số bàn
        int GetTotalBan()
        {
            int count = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectstring))
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM BAN";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        count = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đếm bàn: " + ex.Message);
            }
            return count;
        }

        // Hiển thị món ăn đã order
        void LoadOrder()
        {
            string sql = @"
        SELECT 
            TABLE_ORDER.idoder,
            TABLE_ORDER.IDBan AS so_ban,
            mon_an.ten_mon AS ten_mon,
            TABLE_ORDER.SoLuong AS so_luong,
            CASE 
                WHEN TABLE_ORDER.id_khach = 100 THEN N'Admin'
                ELSE nhan_vien.ho_ten
            END AS ten_nhan_vien,
            TABLE_ORDER.ThoiGian
        FROM TABLE_ORDER
        JOIN mon_an ON TABLE_ORDER.id_mon = mon_an.id_mon
        LEFT JOIN nhan_vien ON TABLE_ORDER.id_khach = nhan_vien.id_khach
        ORDER BY TABLE_ORDER.ThoiGian DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectstring))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    guna2DataGridView1.DataSource = dt;

                    // Ẩn cột idoder
                    if (guna2DataGridView1.Columns.Contains("idoder"))
                        guna2DataGridView1.Columns["idoder"].Visible = false;

                    // Đặt tên tiêu đề cho các cột
                    if (guna2DataGridView1.Columns.Contains("so_ban"))
                        guna2DataGridView1.Columns["so_ban"].HeaderText = "Số bàn";

                    if (guna2DataGridView1.Columns.Contains("ten_mon"))
                        guna2DataGridView1.Columns["ten_mon"].HeaderText = "Tên món";

                    if (guna2DataGridView1.Columns.Contains("so_luong"))
                        guna2DataGridView1.Columns["so_luong"].HeaderText = "Số lượng";

                    if (guna2DataGridView1.Columns.Contains("ten_nhan_vien"))
                        guna2DataGridView1.Columns["ten_nhan_vien"].HeaderText = "Nhân viên order";

                    if (guna2DataGridView1.Columns.Contains("ThoiGian"))
                        guna2DataGridView1.Columns["ThoiGian"].HeaderText = "Thời gian";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load order: " + ex.Message);
            }
        }

        // Order món ăn
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Validate input
            if (guna2ComboBox1.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn món ăn!");
                return;
            }

            if (!int.TryParse(guna2ComboBox1.SelectedValue.ToString(), out int idMon))
            {
                MessageBox.Show("Món ăn không hợp lệ!");
                return;
            }

            if (!int.TryParse(guna2TextBox3.Text.Trim(), out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ! Vui lòng nhập số lớn hơn 0.");
                return;
            }

            if (!int.TryParse(guna2TextBox5.Text.Trim(), out int idBan) || idBan <= 0)
            {
                MessageBox.Show("Số bàn không hợp lệ! Vui lòng nhập số lớn hơn 0.");
                return;
            }

            // Kiểm tra số bàn có tồn tại không
            int totalBan = GetTotalBan();
            if (idBan > totalBan)
            {
                MessageBox.Show($"Số bàn không hợp lệ! Chỉ có {totalBan} bàn.");
                return;
            }

            // SQL INSERT
            string sql = @"
        INSERT INTO TABLE_ORDER (IDBan, id_mon, SoLuong, id_khach, ThoiGian)
        VALUES (@idban, @idmon, @soluong, @idnv, GETDATE())";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectstring))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@idban", idBan);
                        cmd.Parameters.AddWithValue("@idmon", idMon);
                        cmd.Parameters.AddWithValue("@soluong", soLuong);
                        cmd.Parameters.AddWithValue("@idnv", idnhanvien);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm order thành công!");

                            // Clear input
                            guna2TextBox3.Text = "";
                            guna2TextBox5.Text = "";

                            // Reload danh sách order
                            LoadOrder();
                        }
                        else
                        {
                            MessageBox.Show("Thêm order thất bại!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm order: " + ex.Message);
            }
        }

        // Xóa món ăn đã chọn
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn order cần xóa!");
                return;
            }

            // Lấy idoder từ dòng được chọn
            int idoder = Convert.ToInt32(guna2DataGridView1.SelectedRows[0].Cells["idoder"].Value);

            // Xác nhận xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa order này?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectstring))
                    {
                        conn.Open();
                        string sql = "DELETE FROM TABLE_ORDER WHERE idoder = @idoder";

                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@idoder", idoder);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa order thành công!");
                                LoadOrder();
                            }
                            else
                            {
                                MessageBox.Show("Xóa order thất bại!");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa order: " + ex.Message);
                }
            }
        }

        // Xem thông tin order của bàn
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(guna2TextBox5.Text.Trim(), out int idBan) || idBan <= 0)
            {
                MessageBox.Show("Vui lòng nhập số bàn hợp lệ!");
                return;
            }

            string sql = @"
        SELECT 
            TABLE_ORDER.idoder,
            mon_an.ten_mon AS ten_mon,
            TABLE_ORDER.SoLuong AS so_luong,
            TABLE_ORDER.ThoiGian,
            (mon_an.gia_mon * TABLE_ORDER.SoLuong) AS thanh_tien
        FROM TABLE_ORDER
        JOIN mon_an ON TABLE_ORDER.id_mon = mon_an.id_mon
        WHERE TABLE_ORDER.IDBan = @idban
        ORDER BY TABLE_ORDER.ThoiGian DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectstring))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@idban", idBan);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show($"Bàn {idBan} chưa có order nào!");
                            return;
                        }

                        // Hiển thị trong một MessageBox hoặc DataGridView
                        string message = $"Danh sách order bàn {idBan}:\n\n";
                        decimal total = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            string tenMon = row["ten_mon"].ToString();
                            int soLuong = Convert.ToInt32(row["so_luong"]);
                            decimal thanhTien = Convert.ToDecimal(row["thanh_tien"]);

                            message += $"{tenMon} x{soLuong}: {thanhTien.ToString("N0")} VNĐ\n";
                            total += thanhTien;
                        }

                        message += $"\nTổng tiền: {total.ToString("N0")} VNĐ";

                        MessageBox.Show(message, $"Thông tin order bàn {idBan}",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load thông tin bàn: " + ex.Message);
            }
        }

        // Refresh danh sách order
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOrder();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            g.Show();
            this.Close();
        }
    }
}