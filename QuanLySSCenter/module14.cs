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
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLySSCenter
{
    public partial class module14 : Form
    {
        string connectionString = "Data Source=MINHNGOCHOANG;Initial Catalog=BTLNhom7;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public module14()
        {
            InitializeComponent();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThongke_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTungay.Value;
            DateTime denNgay = dtpDenngay.Value;

            float ptMoi = 0;
            float ptQuayLai = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("spPhanTramKhachHang", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120; // thời gian tối đa là 120 giây

                    cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@denNgay", denNgay);

                    SqlParameter pMoi = new SqlParameter("@PTKHMoi", SqlDbType.Float) { Direction = ParameterDirection.Output };
                    SqlParameter pQuayLai = new SqlParameter("@PTKHQuayLai", SqlDbType.Float) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pMoi);
                    cmd.Parameters.Add(pQuayLai);

                    cmd.ExecuteNonQuery();

                    // Lấy giá trị output
                    ptQuayLai = pQuayLai.Value == DBNull.Value ? 0 : Convert.ToSingle(pQuayLai.Value);
                    ptMoi = pMoi.Value == DBNull.Value ? 0 : Convert.ToSingle(pMoi.Value);
                    

                }
            }

            // Kiểm tra nếu không có dữ liệu
            if (ptMoi == 0 && ptQuayLai == 0)
            {
                MessageBox.Show("Trong khoảng thời gian này không có khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chartKhachHang.Series["Khách hàng"].Points.Clear(); // Xóa biểu đồ nếu có cũ
                return;
            }

            // Vẽ biểu đồ tròn
            chartKhachHang.Series.Clear();
            chartKhachHang.Titles.Clear();

            chartKhachHang.Series.Add("Khách hàng");
            chartKhachHang.Series["Khách hàng"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            chartKhachHang.Series["Khách hàng"].Points.AddXY("Khách hàng mới", ptMoi);
            chartKhachHang.Series["Khách hàng"].Points.AddXY("Khách hàng quay lại", ptQuayLai);

            // Hiển thị phần trăm trên biểu đồ
            chartKhachHang.Series["Khách hàng"]["PieLabelStyle"] = "Outside";
            chartKhachHang.Series["Khách hàng"].IsValueShownAsLabel = true;
            chartKhachHang.Series["Khách hàng"].LabelFormat = "0.00'%'";

            // **4. Cấu hình ChartArea để biểu đồ tròn to hơn bên trong khung (Tùy chọn)**
            // Điều này giúp biểu đồ tròn chiếm nhiều diện tích hơn trong vùng vẽ của nó
            if (chartKhachHang.ChartAreas.Any())
            {
                ChartArea mainChartArea = chartKhachHang.ChartAreas[0];
                mainChartArea.InnerPlotPosition.Auto = false; // Tắt tự động điều chỉnh
                mainChartArea.InnerPlotPosition.Width = 85; // % của chiều rộng có sẵn
                mainChartArea.InnerPlotPosition.Height = 85; // % của chiều cao có sẵn
                mainChartArea.InnerPlotPosition.X = 7; // Khoảng cách từ lề trái
                mainChartArea.InnerPlotPosition.Y = 7; // Khoảng cách từ lề trên
            }
        }
    }
}
