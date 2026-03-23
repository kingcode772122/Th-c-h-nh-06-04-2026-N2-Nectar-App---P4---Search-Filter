using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class quanlybanan : Form
    {
        string connectstring = @"Data Source=DESKTOP-FT8IM0J;Initial Catalog=quanan;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection conn;// link kết nối 
        SqlCommand cmd;//tạo người gủi lệnh gửi tới sql (câu lệnh,conn link tới sql) cmd.ExecuteNonQuery(); đây là cái để gửi lệnh đến sql;
        SqlDataAdapter adt;//thằng này dùng để tạo datablse điền vào datable
        DataTable dt;//tạo dữ liệu bảng điền vào guna.....
        Form ve;

        public quanlybanan(Form lay)
        {
            InitializeComponent();
            ve = lay;
        }
        void listmonan()
        {
            try
            {
                conn.Open();

                string query = "SELECT id_mon, ten_mon FROM mon_an";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                guna2ComboBox1.DataSource = dt;
                guna2ComboBox1.DisplayMember = "ten_mon";// cho vào tên hiển thị 
                guna2ComboBox1.ValueMember = "id_mon"; // value thực sự là id cho dữ liệu vào combobox
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load món ăn: " + ex.Message);
            }
            finally
            {
                conn.Close(); // ✅ BẮT BUỘC PHẢI CÓ
            }
        }

        private void quanlybanan_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectstring);
            listmonan();
        }
        // đếm sô bàn
        int GetTotalBan()
        {
            int count = 0;

            try
            {
                conn.Open();
                string sql = @"SELECT COUNT(*) FROM BAN";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    count = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đếm bàn: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return count;
        }
        // oder món ăn ---------------------------------
        private void guna2Button2_Click(object sender, EventArgs e)
        {
           // === 1.LẤY DỮ LIỆU ===
    int idMon = Convert.ToInt32(guna2ComboBox1.SelectedValue);

            if (!int.TryParse(guna2TextBox3.Text.Trim(), out int soLuong))
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            if (!int.TryParse(guna2TextBox5.Text.Trim(), out int idBan))
            {
                MessageBox.Show("Số bàn không hợp lệ!");
                return;
            }
            if (idBan>GetTotalBan())
            {
                MessageBox.Show("Số bàn không hợp lệ!");
                return;
            }

            int idNhanVien = 14; // nếu bạn chưa đăng nhập thì gán tạm

            // === 2. SQL INSERT ===
            string sql = @"
        INSERT INTO TABLE_ORDER (IDBan, id_mon, SoLuong, id_khach)
        VALUES (@idban, @idmon, @soluong, @idnv)
    ";

            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@idban", idBan);
                    cmd.Parameters.AddWithValue("@idmon", idMon);
                    cmd.Parameters.AddWithValue("@soluong", soLuong);
                    cmd.Parameters.AddWithValue("@idnv", idNhanVien);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm order thành công!");
                LoadOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm order: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        void LoadOrder()
        {
            string sql = @"
        SELECT 
            TABLE_ORDER.idoder,
            TABLE_ORDER.IDBan       AS so_ban,
            mon_an.ten_mon          AS ten_mon,
            TABLE_ORDER.SoLuong    AS so_luong,
            CASE 
                WHEN TABLE_ORDER.id_khach = 100 THEN N'Admin'
                ELSE nhan_vien.ho_ten
            END AS ten_nhan_vien,
            TABLE_ORDER.ThoiGian
        FROM TABLE_ORDER
        JOIN mon_an 
            ON TABLE_ORDER.id_mon = mon_an.id_mon
        LEFT JOIN nhan_vien 
            ON TABLE_ORDER.id_khach = nhan_vien.id_khach
        ORDER BY TABLE_ORDER.ThoiGian DESC
    ";

            try
            {
                
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                guna2DataGridView1.DataSource = dt;
                guna2DataGridView1.Columns["idoder"].Visible = false;

                guna2DataGridView1.Columns["so_ban"].HeaderText = "Số bàn";
                guna2DataGridView1.Columns["ten_mon"].HeaderText = "Tên món";
                guna2DataGridView1.Columns["so_luong"].HeaderText = "Số lượng";
                guna2DataGridView1.Columns["ten_nhan_vien"].HeaderText = "Nhân viên order";
                guna2DataGridView1.Columns["ThoiGian"].HeaderText = "Thời gian";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load order: " + ex.Message);
            }
            finally
            {
                
            }
        }
        // xóa món -----------------------------------
        void XoaOrderDangChon()
{
    if (guna2DataGridView1.CurrentRow == null)
    {
        MessageBox.Show("Vui lòng chọn dòng cần xóa!");
        return;
    }

    // ✅ Lấy dữ liệu từ dòng đang chọn
    int soBan = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["so_ban"].Value);
    string tenMon = guna2DataGridView1.CurrentRow.Cells["ten_mon"].Value.ToString();
    int idoder = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["idoder"].Value);

            // ✅ Hỏi xác nhận trước khi xóa
            DialogResult rs = MessageBox.Show(
        $"Bạn có chắc muốn xóa món [{tenMon}] của bàn [{soBan}] không?",
        "Xác nhận xóa",
        MessageBoxButtons.YesNo);

    if (rs == DialogResult.No) return;

    using (SqlConnection conn2 = new SqlConnection(connectstring))
    {
        conn2.Open();

        // ✅ XÓA ĐÚNG DÒNG ĐÃ CHỌN
        string sql = @"
            DELETE FROM TABLE_ORDER
            WHERE idoder=@idoder
        ";

        using (SqlCommand cmd = new SqlCommand(sql, conn2))
        {
            cmd.Parameters.AddWithValue("@idoder", idoder);
            cmd.ExecuteNonQuery();
        }
    }

    MessageBox.Show("Đã xóa order!");
    LoadOrder(); // ✅ Load lại bảng
}
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            XoaOrderDangChon();
        }
        // hiển thị theo số  bàn
        void LoadOrderTheoSoBan(int soBan)
        {
            string sql = @"
        SELECT 
            TABLE_ORDER.idoder       AS idoder,
            TABLE_ORDER.IDBan        AS so_ban,
            mon_an.ten_mon           AS ten_mon,
            TABLE_ORDER.SoLuong      AS so_luong,
            CASE 
                WHEN TABLE_ORDER.id_khach = 100 THEN N'Admin'
                ELSE nhan_vien.ho_ten
            END AS ten_nhan_vien,
            TABLE_ORDER.ThoiGian
        FROM TABLE_ORDER
        JOIN mon_an 
            ON TABLE_ORDER.id_mon = mon_an.id_mon
        LEFT JOIN nhan_vien 
            ON TABLE_ORDER.id_khach = nhan_vien.id_khach
        WHERE TABLE_ORDER.IDBan = @soban
        ORDER BY TABLE_ORDER.ThoiGian DESC
    ";

            try
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@soban", soBan);

                DataTable dt = new DataTable();
                da.Fill(dt);

                guna2DataGridView1.DataSource = dt;

                // ✅ Ẩn cột idoder nhưng vẫn dùng để xóa
                guna2DataGridView1.Columns["idoder"].Visible = false;

                guna2DataGridView1.Columns["so_ban"].HeaderText = "Số bàn";
                guna2DataGridView1.Columns["ten_mon"].HeaderText = "Tên món";
                guna2DataGridView1.Columns["so_luong"].HeaderText = "Số lượng";
                guna2DataGridView1.Columns["ten_nhan_vien"].HeaderText = "Nhân viên order";
                guna2DataGridView1.Columns["ThoiGian"].HeaderText = "Thời gian";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load theo số bàn: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(guna2TextBox5.Text.Trim(), out int idBan))
            {
                MessageBox.Show("Số bàn không hợp lệ!");
                return;
            }
            if (idBan > GetTotalBan())
            {
                MessageBox.Show("Số bàn không hợp lệ!");
                return;
            }
            LoadOrderTheoSoBan(idBan);
        }
        // xuất hóa đơn
        void ThanhToanHoaDon()
        {
            if (!int.TryParse(guna2TextBox4.Text.Trim(), out int soBan))
            {
                MessageBox.Show("Số bàn không hợp lệ!");
                return;
            }

            string sdt = guna2TextBox2.Text.Trim();

            

            decimal tongTien = 0;
            decimal tongChiTieuCu = 0;
            int xepHangCu = 1;
            int idKhach = 0;

            try
            {
                conn.Open();

                // ✅ 1. TÍNH TỔNG TIỀN BÀN
                string sqlTong = @"
            SELECT SUM(o.SoLuong * m.gia_mon)
            FROM TABLE_ORDER o
            JOIN mon_an m ON o.id_mon = m.id_mon
            WHERE o.IDBan = @soban";
                // chỗ kia đặt bí dnah
                SqlCommand cmdTong = new SqlCommand(sqlTong, conn);
                cmdTong.Parameters.AddWithValue("@soban", soBan);

                object kq = cmdTong.ExecuteScalar();// thằng này trả vè kiểu object
                if (kq == DBNull.Value)
                {
                    MessageBox.Show("Bàn này chưa có order!");
                    return;
                }

                tongTien = Convert.ToDecimal(kq);

                // ✅ 2. KIỂM TRA KHÁCH HÀNG
                string sqlKH = "SELECT id_khach, tong_tien_da_chi, xep_hang_khach FROM khach_hang WHERE so_dien_thoai=@sdt";

                SqlCommand cmdKH = new SqlCommand(sqlKH, conn);
                cmdKH.Parameters.AddWithValue("@sdt", sdt);

                SqlDataReader rd = cmdKH.ExecuteReader();

                bool laThanhVien = false;

                if (rd.Read())// đọc xem có dữ liệu trả về ko
                {
                    laThanhVien = true;
                    idKhach = rd.GetInt32(0);
                    tongChiTieuCu = rd.GetDecimal(1);
                    xepHangCu = rd.GetInt32(2);
                }
                rd.Close();

                // ✅ 3. ÁP DỤNG GIẢM GIÁ
                decimal giamGia = 0;
                if (laThanhVien)
                {
                    giamGia = tongTien * 0.1m * xepHangCu;
                }

                decimal thanhToan = tongTien - giamGia;

                // ✅ 4. THÊM / CẬP NHẬT KHÁCH HÀNG & TỔNG TIỀN
                decimal tongMoi = tongChiTieuCu + thanhToan;

                int xepHangMoi = 1;
                if (tongMoi >= 5000) xepHangMoi = 5;
                else if (tongMoi >= 3000) xepHangMoi = 4;
                else if (tongMoi >= 2000) xepHangMoi = 3;
                else if (tongMoi >= 1000) xepHangMoi = 2;

                if (laThanhVien)
                {
                    string updateKH = @"
                UPDATE khach_hang
                SET tong_tien_da_chi=@tong, xep_hang_khach=@hang
                WHERE id_khach=@id";

                    SqlCommand cmdUpdate = new SqlCommand(updateKH, conn);
                    cmdUpdate.Parameters.AddWithValue("@tong", tongMoi);
                    cmdUpdate.Parameters.AddWithValue("@hang", xepHangMoi);
                    cmdUpdate.Parameters.AddWithValue("@id", idKhach);
                    cmdUpdate.ExecuteNonQuery();

                    string sqlInsertHD = @"
                        INSERT INTO hoa_don (id_khach, tien_thu)
                        VALUES (@idkhach, @tienthu)";

                    SqlCommand cmd = new SqlCommand(sqlInsertHD, conn);
                    cmd.Parameters.AddWithValue("@idkhach", idKhach);
                    cmd.Parameters.AddWithValue("@tienthu", thanhToan);
                    cmd.ExecuteNonQuery();






                }
                else
                {
                    // nếu không phải là khashc hàng phải trả 100%
                    string sqlInsertHD = @"
                        INSERT INTO hoa_don ( tien_thu)
                        VALUES ( @tienthu)";

                    SqlCommand cmd = new SqlCommand(sqlInsertHD, conn);
                    cmd.Parameters.AddWithValue("@tienthu", thanhToan);
                    cmd.ExecuteNonQuery();

                }
                // đoạn này oke
                // ✅ 5. XÓA TOÀN BỘ ORDER CỦA BÀN
                string xoaOrder = "DELETE FROM TABLE_ORDER WHERE IDBan=@soban";
                SqlCommand cmdXoa = new SqlCommand(xoaOrder, conn);
                cmdXoa.Parameters.AddWithValue("@soban", soBan);
                cmdXoa.ExecuteNonQuery();

                MessageBox.Show(
                    $"✅ Thanh toán thành công!\n\n" +
                    $"Tổng tiền: {tongTien:N0}\n" +
                    $"Giảm giá: {giamGia:N0}\n" +
                    $"Khách trả: {thanhToan:N0}\n" +
                    $"Xếp hạng mới: {xepHangMoi}"
                );

                LoadOrder(); // Load lại bảng

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            ThanhToanHoaDon();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            quanlymonan monan = new quanlymonan(ve);
            monan.Show();
            this.Close();
        }

        private void guna2Panel6_Paint(object sender, PaintEventArgs e)
        {
            ;
        }

        private void guna2Panel7_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label10_Click(object sender, EventArgs e)
        {
            thongkethunhap thognke = new thongkethunhap(ve);
            thognke.Show();
            this.Close();
        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            ve.Show();
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            quanlykhach khach = new quanlykhach(ve);
            khach.Show();
            this.Close();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            quanlynhanvien nv = new quanlynhanvien(ve);
            nv.Show();
            this.Close();
        }
    }
}
