using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class thongkethunhap : Form
    {
        string connectstring = @"Data Source=DESKTOP-FT8IM0J;Initial Catalog=quanan;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection conn;// link kết nối 
        SqlCommand cmd;//tạo người gủi lệnh gửi tới sql (câu lệnh,conn link tới sql) cmd.ExecuteNonQuery(); đây là cái để gửi lệnh đến sql;
        SqlDataAdapter adt;//thằng này dùng để tạo datablse điền vào datable
        DataTable dt;
        Form ve;
        private string backupFolder = "";
        public thongkethunhap(Form lay)
        {
            InitializeComponent();
            ve = lay;
        }

        private void thongkethunhap_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(connectstring);
            int thang = DateTime.Now.Month;
            int nam = DateTime.Now.Year;
            label11.Text = "Doanh thu hiện tại là:" + TongThuNhapThang(thang, nam);
            label12.Text = "Doanh thu trước là:" + thangtruocdo(thang - 1);
            label13.Text = "Tổng lương nhân viên là:" + Tongluongnv();
            NapDuLieuVaoChart5ThangTruoc();
        }
        private decimal thangtruocdo(int thang)
        {
            string sql = @"
        SELECT tong_thu_nhap
        FROM thong_ke_thu_nhap
        WHERE thang=@thang";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@thang", thang);
            conn.Open();

            object kq = cmd.ExecuteScalar();
            conn.Close();

            if (kq == DBNull.Value) return 0;

            return Convert.ToDecimal(kq);
        }
        private decimal TongThuNhapThang(int thang, int nam)
        {
            string sql = @"
        SELECT SUM(tien_thu)
        FROM hoa_don
        WHERE MONTH(thoi_gian) = @thang AND YEAR(thoi_gian) = @nam";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@thang", thang);
            cmd.Parameters.AddWithValue("@nam", nam);

            conn.Open();
            object kq = cmd.ExecuteScalar();
            conn.Close();

            if (kq == DBNull.Value) return 0;

            return Convert.ToDecimal(kq);
        }
        private decimal Tongluongnv()
        {
            string sql = @"
        SELECT SUM(luong)
        FROM nhan_vien
        WHERE 1=1";

            SqlCommand cmd = new SqlCommand(sql, conn);

            conn.Open();
            object kq = cmd.ExecuteScalar();
            conn.Close();

            if (kq == DBNull.Value) return 0;

            return Convert.ToDecimal(kq);
        }
        private void CapNhatThuNhapThangTruoc()
        {
            try
            {
                conn.Open();

                // Lấy tháng và năm hiện tại
                int thangHienTai = DateTime.Now.Month;
                int namHienTai = DateTime.Now.Year;

                // Lấy tháng và năm tháng trước
                int thangTruoc = thangHienTai - 1;
                int namTruoc = namHienTai;

                if (thangTruoc == 0)
                {
                    thangTruoc = 12;
                    namTruoc -= 1;
                }

                // Kiểm tra xem tháng trước đã thống kê chưa
                string checkSql = @"
            SELECT COUNT(*) 
            FROM thong_ke_thu_nhap 
            WHERE thang = @thang AND nam = @nam";

                SqlCommand cmdCheck = new SqlCommand(checkSql, conn);
                cmdCheck.Parameters.AddWithValue("@thang", thangTruoc);
                cmdCheck.Parameters.AddWithValue("@nam", namTruoc);

                int count = (int)cmdCheck.ExecuteScalar();

                if (count > 0)
                {
                    // Đã thống kê rồi, không làm gì
                    return;
                }

                // Tính tổng thu nhập tháng trước
                string sql = @"
            SELECT SUM(tien_thu) 
            FROM hoa_don 
            WHERE MONTH(thoi_gian) = @thang AND YEAR(thoi_gian) = @nam";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@thang", thangTruoc);
                cmd.Parameters.AddWithValue("@nam", namTruoc);

                object kq = cmd.ExecuteScalar();
                decimal tongThuNhap = (kq == DBNull.Value) ? 0 : Convert.ToDecimal(kq);

                // Lưu vào bảng thong_ke_thu_nhap
                string insertSql = @"
            INSERT INTO thong_ke_thu_nhap (thang, nam, tong_thu_nhap)
            VALUES (@thang, @nam, @tong)";

                SqlCommand cmdInsert = new SqlCommand(insertSql, conn);
                cmdInsert.Parameters.AddWithValue("@thang", thangTruoc);
                cmdInsert.Parameters.AddWithValue("@nam", namTruoc);
                cmdInsert.Parameters.AddWithValue("@tong", tongThuNhap);

                cmdInsert.ExecuteNonQuery();

                MessageBox.Show($"Đã thống kê thu nhập tháng {thangTruoc}/{namTruoc}: {tongThuNhap:N0} VNĐ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thống kê: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void NapDuLieuVaoChart5ThangTruoc()
        {
            try
            {
                conn.Open();

                // Lấy tháng và năm hiện tại
                int thangHienTai = DateTime.Now.Month;
                int namHienTai = DateTime.Now.Year;

                // Tính mốc thời gian 5 tháng trước (bỏ tháng hiện tại)
                // Ví dụ: nếu là tháng 12/2025 thì 5 tháng trước là 7/2025 đến 11/2025
                DateTime batDau = new DateTime(namHienTai, thangHienTai, 1).AddMonths(-5);
                DateTime ketThuc = new DateTime(namHienTai, thangHienTai, 1).AddMonths(-1);

                string sql = @"
            SELECT thang, nam, tong_thu_nhap 
            FROM thong_ke_thu_nhap
            WHERE 
                (nam > @namBatDau OR (nam = @namBatDau AND thang >= @thangBatDau))
                AND
                (nam < @namKetThuc OR (nam = @namKetThuc AND thang <= @thangKetThuc))
            ORDER BY nam, thang";

                SqlCommand cmd = new SqlCommand(sql, conn);//oder by dùng để xắp xếp theo năm tháng 
                cmd.Parameters.AddWithValue("@namBatDau", batDau.Year);
                cmd.Parameters.AddWithValue("@thangBatDau", batDau.Month);
                cmd.Parameters.AddWithValue("@namKetThuc", ketThuc.Year);
                cmd.Parameters.AddWithValue("@thangKetThuc", ketThuc.Month);

                SqlDataReader rd = cmd.ExecuteReader();// nhận dữ liệu dnagj bảng 

                // Xóa dữ liệu cũ
                chart1.Series.Clear();

                // Tạo 1 Series cho biểu đồ dạng Cột (Column)
                var series = chart1.Series.Add("Thu nhập");
                series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;// tạo biểu đồ cột 

                while (rd.Read())
                {
                    int thang = (int)rd["thang"];// nó trả về dạng object nên phải ép kiểu 
                    int nam = (int)rd["nam"];
                    decimal thuNhap = (decimal)rd["tong_thu_nhap"];

                    string label = $"{thang}/{nam}";

                    // Thêm điểm dữ liệu (label, value)
                    series.Points.AddXY(label, thuNhap);// dòng này để tạo cột nè =))) label là cái chữ dưới cột 
                }

                rd.Close();// đóng lại sau khi đọc ở trên
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp dữ liệu vào Chart: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            quanlymonan ma = new quanlymonan(ve);
            ma.Show();
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            quanlykhach kh = new quanlykhach(ve);
            kh.Show();
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

        private void label5_Click(object sender, EventArgs e)
        {
            ve.Show();
            this.Close();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())// hộp thoại cho phép chọn thư mục 
            {
                fbd.Description = "Chọn thư mục để lưu file backup (*.bak)";

                if (fbd.ShowDialog() == DialogResult.OK)// mwor hộp thoại chọn thưu mục và hủy nếu ko có gì 
                {
                    backupFolder = fbd.SelectedPath;// lấy path thư mục 
                    guna2TextBox1.Text = backupFolder;  // Hiển thị ra textbox
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (backupFolder == "")
            {
                MessageBox.Show("Bạn chưa chọn thư mục lưu backup!");
                return;
            }

            // Tạo file .bak tự động
            string fileName = "Backup_Quanan_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
            string fullPath = System.IO.Path.Combine(backupFolder, fileName);// ghép đường file thay vì \ :))

            try
            {
                using (SqlConnection conn = new SqlConnection(connectstring))
                {
                    conn.Open();

                    string sql = $@"
            BACKUP DATABASE quanan
            TO DISK = '{fullPath}'
            WITH INIT, FORMAT;
            ";
                    // init là dùng để ghi đề  backup cũ 
                    // format định dang lại file trc tức là cho nó trống hoàn toàn 
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Backup thành công!\nFile đã lưu tại:\n" + fullPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi backup: " + ex.Message);
            }
        }
    }
}
