using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace QuanLySSCenter
{
    public partial class SuaKH : Form
    {
        string sCon = "Data Source=LAPTOP-0B1SRHV7;Initial Catalog = BTLNhom7; Integrated Security = True; TrustServerCertificate=True";
        private string MaKH;
        public SuaKH(string MaKH)
        {
            InitializeComponent();
            this.MaKH = MaKH;
        }
        private bool ValidateInput()
        {
            // Kiểm tra TenNCC
            if (string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                MessageBox.Show("Tên khách hàng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKH.Focus();
                return false;
            }

            

            // Kiểm tra SDT (chỉ chứa chữ số, độ dài 10-11)
            if (!string.IsNullOrWhiteSpace(txtSDTKH.Text))
            {
                if (!Regex.IsMatch(txtSDTKH.Text, @"^\d{10,11}$"))
                {
                    MessageBox.Show("Số điện thoại phải chứa 10-11 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDTKH.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Số điện thoại không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDTKH.Focus();
                return false;
            }

            // Kiểm tra DiaChi
            if (string.IsNullOrWhiteSpace(txtDiaChiKH.Text))
            {
                MessageBox.Show("Địa chỉ không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChiKH.Focus();
                return false;
            }

            return true;
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
                    string query = @"UPDATE KhachHang
                                 SET TenKH = @TenKH, 
                                     SDT = @SDT, 
                                     DiaChi = @DiaChi 
                                 WHERE MaKH = @MaKH";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MaKH", txtMaKH.Text);
                    cmd.Parameters.AddWithValue("@TenKH", txtTenKH.Text);
                    cmd.Parameters.AddWithValue("@SDT", txtSDTKH.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChiKH.Text);

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

        private void btnHuy_Click(object sender, EventArgs e)
        {

        }

        private void SuaKH_Load(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM KhachHang WHERE MaKH = @MaKH";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MaKH", MaKH);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtMaKH.Text = reader["MaKH"].ToString();
                        txtTenKH.Text = reader["TenKH"].ToString();
                        txtSDTKH.Text = reader["SDT"].ToString();
                        txtDiaChiKH.Text = reader["DiaChi"].ToString();

                        txtMaKH.Enabled = false; // Không cho sửa mã
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}
