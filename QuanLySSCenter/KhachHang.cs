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
using TP_BVSK;

namespace QuanLySSCenter
{
    public partial class KhachHang : Form
    {
        string sCon = "Data Source=LAPTOP-0B1SRHV7;Initial Catalog = BTLNhom7; Integrated Security = True; TrustServerCertificate=True";
        public KhachHang()
        {
            InitializeComponent();
        }


        private void KhachHang_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(sCon);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message);
            }

            //Lấy dữ liệu về
            string sQuery = "SELECT * FROM KHACHHANG";
            SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "KHACHHANG");
            guna2DataGridView1.DataSource = ds.Tables["KHACHHANG"];

            con.Close();
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    string sQuery = "SELECT * FROM KHACHHANG";
                    SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "KhachHang");
                    guna2DataGridView1.DataSource = ds.Tables["KhachHang"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
                }
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maKH = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(maKH))
            {
                MessageBox.Show("Vui lòng nhập mã khách hàng cần tìm.");
                return;
            }

            using (SqlConnection con = new SqlConnection(sCon))
            {
                string query = "SELECT * FROM KHACHHANG WHERE MaKH = @MaKH";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@MaKH", maKH);

                DataTable dt = new DataTable();
                da.Fill(dt);
                guna2DataGridView1.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy khách hàng với mã đã chọn.");
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ThemKhachHang formThem = new ThemKhachHang();
            formThem.FormClosed += (s, args) => LoadData();
            formThem.ShowDialog();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                string MaKH = guna2DataGridView1.SelectedRows[0].Cells["MaKH"].Value.ToString();
                SuaKH suaForm = new SuaKH(MaKH);
                suaForm.ShowDialog();

                // Tải lại dữ liệu sau khi sửa
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                // Lấy MaNCC từ hàng được chọn trong dataGridView1
                string maKH = guna2DataGridView1.SelectedRows[0].Cells["MaKH"].Value.ToString();

                // Xác nhận trước khi xóa
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                    return;

                using (SqlConnection con = new SqlConnection(sCon))
                {
                    try
                    {
                        con.Open();
                        string query = "DELETE FROM KHACHHANG WHERE MaKH = @MaKH";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@MaKH", maKH);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData(); // Tải lại dữ liệu sau khi xóa
                        }
                        else
                        {
                            MessageBox.Show("Không xóa được!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
            //test
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
