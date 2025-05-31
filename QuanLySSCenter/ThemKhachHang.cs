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
using Guna.UI2.WinForms;

namespace QuanLySSCenter
{
    public partial class ThemKhachHang : Form
    {
        string sCon = "Data Source=LAPTOP-0B1SRHV7;Initial Catalog = BTLNhom7; Integrated Security = True; TrustServerCertificate=True";
        public ThemKhachHang()
        {
            InitializeComponent();
        }

        private void ThemKhachHang_Load(object sender, EventArgs e)
        {
            
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void txtMST_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTenNCC_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMaNCC_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using(SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    con.Open();
                    string query = @"INSERT INTO KHACHHANG
                            (MaKH, TenKH,SDT, DiaChi) 
                            VALUES (@MaKH, @TenKH, @SDTKH, @DiaChiKH)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@MaKH", txtMaKH.Text);
                    cmd.Parameters.AddWithValue("@TenKH", txtTenKH.Text);
                    cmd.Parameters.AddWithValue("@SDTKH", txtSDTKH.Text);
                    cmd.Parameters.AddWithValue("@DiaChiKH", txtDiaChiKH.Text);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close(); // Đóng form sau khi thêm
                    }
                    else
                    {
                        MessageBox.Show("Thêm thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
