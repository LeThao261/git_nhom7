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
    public partial class frmModul3 : Form
    {
        string connectionString = "Data Source=MINHNGOCHOANG;Initial Catalog=BTLNhom7;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        public frmModul3()
        {
            InitializeComponent();
            SetupDataGridViewColumns();
        }
        private void SetupDataGridViewColumns()
        {
            // RẤT QUAN TRỌNG: Tắt tự động tạo cột để chỉ hiển thị các cột bạn định nghĩa
            guna2DataGridView1.AutoGenerateColumns = false;

            // Xóa tất cả các cột hiện có (đảm bảo không bị trùng lặp khi khởi tạo)
            guna2DataGridView1.Columns.Clear();

            // Thêm các cột thủ công với HeaderText và DataPropertyName chính xác
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colMaHang",
                HeaderText = "Mã Hàng Hóa", // Tên hiển thị rõ nghĩa
                DataPropertyName = "MAHANG", // Tên cột trong DataTable từ SP
                Width = 120,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.Automatic, // ✅ Cho phép click sắp xếp
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0" }
            });

            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colTenHang",
                HeaderText = "Tên Hàng Hóa",
                DataPropertyName = "TENHANG",
                Width = 250,
                ReadOnly = true
            });

            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colTongSLNhap",
                HeaderText = "Tổng Số Lượng Nhập",
                DataPropertyName = "Tongsoluongnhap", // Đảm bảo đúng tên cột từ SP
                Width = 150,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.Automatic, // ✅ Cho phép click sắp xếp
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0" }
            });

            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colTongGiaTriNhap",
                HeaderText = "Tổng Giá Trị Nhập",
                DataPropertyName = "Tonggiatrinhap", // Đảm bảo đúng tên cột từ SP
                Width = 180,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.Automatic, // ✅ Cho phép click sắp xếp
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "#,##0" }
            });

            // Thêm các cột khác nếu có
        }
        private bool CheckMaNccExists(string mancc)
        {
            string query = "SELECT COUNT(*) FROM NHACUNGCAP WHERE MANCC = @MANCC"; // Giả sử bảng nhà cung cấp là NHACUNGCAP và cột mã nhà cung cấp là MANCC

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MANCC", mancc);
                    try
                    {
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar(); // Trả về số lượng bản ghi khớp
                        return count > 0; // Trả về true nếu có ít nhất một bản ghi khớp
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Lỗi kiểm tra Mã NCC: " + ex.Message, "Lỗi Cơ Sở Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Trả về false nếu có lỗi xảy ra
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi kiểm tra Mã NCC: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }
        private void LoadThongKe()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("pTKHN", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout =120;

                    string mancc = txtMaNCC.Text.Trim();
                    
                    //Kiểm tra mã ncc 
                    if (string.IsNullOrWhiteSpace(mancc))
                    {
                        MessageBox.Show("Vui lòng nhập mã nhà cung cấp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //KIỂM TRA TỒN TẠI MÃ NCC 
                    if (!CheckMaNccExists(mancc)) // Gọi hàm kiểm tra
                    {
                        MessageBox.Show("Mã nhà cung cấp này không tồn tại trong hệ thống. Vui lòng kiểm tra lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMaNCC.Focus();
                        return; // Dừng lại nếu Mã NCC không tồn tại
                    }
                    //2. Kiểm tra năm
                    string strNam = txtNam.Text.Trim();
                    int nam;

                    // Kiểm tra năm: phải là số nguyên và có đúng 4 chữ số
                    if (!int.TryParse(strNam, out nam) || strNam.Length != 4)
                    {
                        MessageBox.Show("Vui lòng nhập năm hợp lệ (4 chữ số).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    cmd.Parameters.AddWithValue("@MANCC", mancc);
                    cmd.Parameters.AddWithValue("@nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Kiểm tra nếu không có dòng nào thì thông báo nhẹ
                    if (dt.Rows.Count == 0)
                    {
                        guna2DataGridView1.DataSource = null; // Xóa dữ liệu cũ (nếu có)
                        MessageBox.Show("Không có dữ liệu thống kê cho nhà cung cấp này trong năm đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    // Đổ dữ liệu vào DataGridView
                    guna2DataGridView1.DataSource = dt;

                }
            }
        }
        private void btnThongke_Click(object sender, EventArgs e)
        {
            LoadThongKe();
        }
        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
    }

}
