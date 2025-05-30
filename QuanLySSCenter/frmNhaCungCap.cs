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
using QuanLySSCenter;
using System.Text.RegularExpressions;

namespace TP_BVSK
{
    public partial class frmNhaCungCap : Form
    {
        string sCon = "Data Source=LAPTOP-0B1SRHV7;Initial Catalog = BTLNhom7; Integrated Security = True; TrustServerCertificate=True";
        public frmNhaCungCap()
        {
            InitializeComponent();
        }

        private void frmNhaCungCap_Load(object sender, EventArgs e)
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
            string sQuery = "SELECT * FROM NhaCungCap";
            SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "NhaCungCap");
            dataGridView1.DataSource = ds.Tables["NhaCungCap"];

            LoadData();


            con.Close();
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    string sQuery = "SELECT * FROM NhaCungCap";
                    SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "NhaCungCap");
                    dataGridView1.DataSource = ds.Tables["NhaCungCap"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp trong bảng.");
                return;
            }

            string maNCC = dataGridView1.CurrentRow.Cells["MaNCC"].Value.ToString();

            using (SqlConnection con = new SqlConnection(sCon))
            {
                string query = "SELECT * FROM NhaCungCap WHERE MaNCC = @MaNCC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@MaNCC", maNCC);

                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy nhà cung cấp với mã đã chọn.");
                }
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmThemNCCmoi formThem = new frmThemNCCmoi();

            // Gắn sự kiện FormThemNCC đóng thì reload lại dữ liệu
            formThem.FormClosed += (s, args) => LoadData();

            formThem.ShowDialog();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string MaNCC = dataGridView1.SelectedRows[0].Cells["MaNCC"].Value.ToString();
                SuaNCC suaForm = new SuaNCC(MaNCC);
                suaForm.ShowDialog();

                // Tải lại dữ liệu sau khi sửa
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy MaNCC từ hàng được chọn trong dataGridView1
                string maNCC = dataGridView1.SelectedRows[0].Cells["MaNCC"].Value.ToString();

                // Xác nhận trước khi xóa
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                    return;

                using (SqlConnection con = new SqlConnection(sCon))
                {
                    try
                    {
                        con.Open();
                        string query = "DELETE FROM NhaCungCap WHERE MaNCC = @MaNCC";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@MaNCC", maNCC);

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
                MessageBox.Show("Vui lòng chọn một nhà cung cấp để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
