using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace QuanLySSCenter
{
    public partial class SuaCongNo : Form
    {
        string sCon = "Data Source = MINHNGOCHOANG; Initial Catalog = BTLNhom7; Integrated Security = True; Encrypt=True;TrustServerCertificate=True";

        private string MaCN;
        public SuaCongNo(string MaCN)
        {
            InitializeComponent();
            this.MaCN = MaCN;
        }
        private void SuaCongNo_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM CongNo WHERE MaCN = @MaCN";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MaCN", MaCN);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtMacn.Text = reader["MaCN"].ToString();
                        dtpHantra.Value = Convert.ToDateTime(reader["HanTraNo"]);
                        dtpNgaytra.Value = Convert.ToDateTime(reader["NgayTraNo"]);
                        txtTrangthai.Text = reader["TrangThai"].ToString();

                        txtMacn.Enabled = false; // Không cho sửa mã

                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy công nợ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                    this.Close();
                }
            }
        }
        private bool ValidateInput()
        {
            // Kiểm tra hạn trả nợ không được nhỏ hơn ngày hiện tại (nếu cần)
            if (dtpHantra.Value.Date < DateTime.Now.Date)
            {
                DialogResult result = MessageBox.Show("Hạn trả nợ đã nhỏ hơn ngày hiện tại. Bạn có chắc muốn tiếp tục?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return false;
            }

            if (string.IsNullOrWhiteSpace(txtTrangthai.Text))
            {
                MessageBox.Show("Trạng thái công nợ không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTrangthai.Focus();
                return false;
            }

            if (dtpNgaytra.Checked)
            {
                if (dtpNgaytra.Value.Date < dtpHantra.Value.Date)
                {
                    MessageBox.Show("Ngày trả nợ không được nhỏ hơn hạn trả!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpNgaytra.Focus();
                    return false;
                }
            }

            return true; // Dữ liệu hợp lệ
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ValidateInput())
                return;

            using (SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    con.Open();
                    string query = @"
                                    UPDATE CongNo
                                    SET 
                                        HanTraNo = @HanTraNo,
                                        NgayTraNo = @NgayTraNo,
                                        TrangThai = @TrangThai
                                    WHERE MaCN = @MaCN";

                    SqlCommand cmd = new SqlCommand(query, con);

                    // Nếu người dùng đã chọn ngày trả, thì thêm giá trị
                    if (dtpNgaytra.Checked)
                        cmd.Parameters.AddWithValue("@NgayTraNo", dtpNgaytra.Value);
                    else
                        cmd.Parameters.AddWithValue("@NgayTraNo", DBNull.Value);

                    cmd.Parameters.AddWithValue("@TrangThai", txtTrangthai.Text);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không cập nhật được!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
    }
}
