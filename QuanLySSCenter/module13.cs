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
    public partial class module13 : Form
    {
        string connectionString = "Data Source=MINHNGOCHOANG;Initial Catalog=BTLNhom7;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public module13()
        {
            InitializeComponent();
        }

        private void btnThongke_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                DateTime tuNgay = dtpTungay.Value;
                DateTime denNgay = dtpDenngay.Value;
                decimal tongDoanhThu = 0;

                // Lấy dữ liệu chi tiết
                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand("spDoanhThuTheoThoiGian", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120; // thời gian tối đa là 120 giây

                    cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                    SqlParameter pTongDT = new SqlParameter("@TongDoanhThu", SqlDbType.Money)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(pTongDT);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);


                    tongDoanhThu = (pTongDT.Value != DBNull.Value) ? Convert.ToDecimal(pTongDT.Value) : 0;
                }

                // Nếu không có dữ liệu, thông báo và thoát
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    guna2DataGridView1.DataSource = null;
                    return;
                }
                // Thêm dòng tổng doanh thu
                DataRow rowTong = dt.NewRow();
                rowTong["Doanh Thu"] = tongDoanhThu;
                dt.Rows.Add(rowTong);

                // Gán vào DataGridView
                guna2DataGridView1.DataSource = dt;

                // Tùy chỉnh tên cột
                guna2DataGridView1.Columns["MAHANG"].HeaderText = "Mã hàng";
                guna2DataGridView1.Columns["TENHANG"].HeaderText = "Tên hàng";
                guna2DataGridView1.Columns["Doanh Thu"].HeaderText = "Doanh thu (VNĐ)";

                // Hiển thị tổng doanh thu
                guna2DataGridView1.Text = "Tổng doanh thu: " + tongDoanhThu.ToString("N0") + " VNĐ";
                foreach (DataGridViewColumn col in guna2DataGridView1.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
                }

                // Vô hiệu hóa sắp xếp cột "Tên nhân viên"
                guna2DataGridView1.Columns["TENHANG"].SortMode = DataGridViewColumnSortMode.NotSortable;


            }

        }
        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
