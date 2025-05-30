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

namespace TP_BVSK
{
    public partial class frmModul4 : Form
    {
        string connectionString = "Data Source=MINHNGOCHOANG;Initial Catalog=BTLNhom7;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"; // sửa theo cấu hình thật
        public frmModul4()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThongke_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTungay.Value;
            DateTime denNgay = dtpDenngay.Value;
            int tongHoaDon;
            decimal tongDoanhThu;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Lấy dữ liệu từng nhân viên
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        nv.MANV, 
                        nv.TENNV,
                        COUNT(hdb.SOHDB) AS [Tổng số lượng hóa đơn],
                        SUM(hdb.THANHTOAN) AS [Tổng doanh thu]
                    FROM NHANVIEN nv
                    JOIN HOADON_BAN hdb ON nv.MANV = hdb.MANV
                    WHERE hdb.NGAYBAN BETWEEN @TuNgay AND @DenNgay
                    GROUP BY nv.MANV, nv.TENNV
                    ORDER BY COUNT(hdb.SOHDB) DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                // 2. Gọi stored procedure để lấy tổng doanh thu + tổng hóa đơn toàn nhân viên
                using (SqlCommand cmd = new SqlCommand("spDoanhThuTheoNhanVien", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                    SqlParameter pTongHD = new SqlParameter("@TongSoLuongHoaDon", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter pTongDT = new SqlParameter("@TongDoanhThu", SqlDbType.Money)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(pTongHD);
                    cmd.Parameters.Add(pTongDT);

                    cmd.ExecuteNonQuery();

                    tongHoaDon = (int)pTongHD.Value;
                    tongDoanhThu = (decimal)pTongDT.Value;
                }

                // 3. Gán vào DataGridView
                guna2DataGridView1.DataSource = dt;

                // Tùy chỉnh tên cột hiển thị rõ ràng
                guna2DataGridView1.Columns["MANV"].HeaderText = "Mã nhân viên";
                guna2DataGridView1.Columns["TENNV"].HeaderText = "Tên nhân viên";
                guna2DataGridView1.Columns["Tổng số lượng hóa đơn"].HeaderText = "Tổng số lượng hóa đơn";
                guna2DataGridView1.Columns["Tổng doanh thu"].HeaderText = "Tổng doanh thu";
                // Cho phép sắp xếp tất cả các cột
                foreach (DataGridViewColumn col in guna2DataGridView1.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
                }

                // Vô hiệu hóa sắp xếp cột "Tên nhân viên"
                guna2DataGridView1.Columns["TENNV"].SortMode = DataGridViewColumnSortMode.NotSortable;

                // 4. Thêm dòng tổng vào cuối DataTable
                DataRow row = dt.NewRow();
                row["Tổng số lượng hóa đơn"] = tongHoaDon;
                row["Tổng doanh thu"] = tongDoanhThu;
                dt.Rows.Add(row);
            }
        }

    }
}
