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

namespace TP_BVSK
{
    public partial class frmQuanlyNhaphang : Form
    {
        string sCon = "Data Source=LAPTOP-0B1SRHV7;Initial Catalog = BTLNhom7; Integrated Security = True; TrustServerCertificate=True";

        public frmQuanlyNhaphang()
        {
            InitializeComponent();
            txtSLNhap.TextChanged += (sender, eventArgs) => TinhTien();
            txtDGNhap.TextChanged += (sender, eventArgs) => TinhTien();
            txtThueSuat.TextChanged += (sender, eventArgs) => TinhTien();
            txtVoucher.TextChanged += (sender, eventArgs) => TinhTien();
        }

        private void frmQuanlyNhaphang_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    con.Open();
                    string sQuery = @"
                SELECT HOADON_NHAP.SOHDN, NGAYNHAP, MANV, MANCC, 
                       MAHANG, SL_NHAP, DONGIA_NHAP, TONGTIEN, 
                       NSX, HSD, THUESUAT, VOUCHER, THANHTIEN, THANHTOAN, SOTIEN_TT
                FROM HOADON_NHAP 
                JOIN CHITIET_NHAP ON HOADON_NHAP.SOHDN = CHITIET_NHAP.SOHDN";

                    SqlDataAdapter adapter = new SqlDataAdapter(sQuery, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message);
                }
            }
        }

        private void TinhTien()
        {
            if (decimal.TryParse(txtSLNhap.Text, out decimal soLuong) &&
        decimal.TryParse(txtDGNhap.Text, out decimal donGia))
            {
                // Tính thành tiền
                decimal thanhTien = soLuong * donGia;
                txtThanhTien.Text = thanhTien.ToString("N0");

                // Tổng tiền tạm thời (nếu có nhiều dòng thì cần tính tổng lại sau)
                decimal tongTien = thanhTien;
                txtTongTien.Text = tongTien.ToString("N0");

                // Đọc thuế suất (%) và voucher (số tiền) do người dùng nhập
                decimal thueSuat = 0;
                decimal voucher = 0;

                decimal.TryParse(txtThueSuat.Text, out thueSuat); // phần trăm thuế suât
                decimal.TryParse(txtVoucher.Text, out voucher);     // số tiền voucher

                // Tính tiền thanh toán
                decimal thanhToan = tongTien + (thueSuat / 100) * tongTien - voucher;

                txtThanhToan.Text = thanhToan.ToString("N0");
            }
            else
            {
                txtThanhTien.Text = "";
                txtTongTien.Text = "";
                txtThanhToan.Text = "";
            }
        }

        

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    con.Open();

                    string sSoHDN = txtSoHDN.Text.Trim();
                    string sNgayNhap = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    string sTongTien = txtTongTien.Text.Trim();
                    string sThueSuat = txtThueSuat.Text.Trim();
                    string sVoucher = txtVoucher.Text.Trim();
                    string sThanhToan = txtThanhToan.Text.Trim();
                    string sSoTienTT = txtSoTienTT.Text.Trim();
                    string sNV = txtNV.Text.Trim();
                    string sNCC = txtNCC.Text.Trim();

                    // Kiểm tra hóa đơn đã tồn tại chưa
                    string checkQuery = "SELECT COUNT(*) FROM HOADON_NHAP WHERE SoHDN = @SoHDN";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@SoHDN", sSoHDN);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        // Nếu chưa có, thêm hóa đơn
                        string insertHoaDon = @"INSERT INTO HOADON_NHAP 
                    (SoHDN, NgayNhap, TongTien, ThueSuat, Voucher, ThanhToan, SoTien_TT, MaNV, MaNCC) 
                    VALUES (@SoHDN, @NgayNhap, @TongTien, @ThueSuat, @Voucher, @ThanhToan, @SoTienTT, @NV, @NCC)";

                        SqlCommand cmd1 = new SqlCommand(insertHoaDon, con);
                        cmd1.Parameters.AddWithValue("@SoHDN", sSoHDN);
                        cmd1.Parameters.AddWithValue("@NgayNhap", sNgayNhap);
                        cmd1.Parameters.AddWithValue("@TongTien", sTongTien);
                        cmd1.Parameters.AddWithValue("@ThueSuat", sThueSuat);
                        cmd1.Parameters.AddWithValue("@Voucher", sVoucher);
                        cmd1.Parameters.AddWithValue("@ThanhToan", sThanhToan);
                        cmd1.Parameters.AddWithValue("@SoTienTT", sSoTienTT);
                        cmd1.Parameters.AddWithValue("@NV", sNV);
                        cmd1.Parameters.AddWithValue("@NCC", sNCC);

                        cmd1.ExecuteNonQuery();
                    }

                    // Thêm chi tiết hàng hóa
                    string sMaHang = txtMaHH.Text.Trim();
                    string sSLNhap = txtSLNhap.Text.Trim();
                    string sDonGia_Nhap = txtDGNhap.Text.Trim();
                    string sNSX = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                    string sHSD = dateTimePicker3.Value.ToString("yyyy-MM-dd");
                    string sThanhTien = txtThanhTien.Text.Trim();

                    string insertChiTiet = @"INSERT INTO CHITIET_NHAP 
                (SoHDN, MaHang, SL_Nhap, DONGIA_NHAP, NSX, HSD, ThanhTien) 
                VALUES (@SoHDN, @MaHang, @SL, @DonGia_Nhap, @NSX, @HSD, @ThanhTien)";

                    SqlCommand cmd2 = new SqlCommand(insertChiTiet, con);
                    cmd2.Parameters.AddWithValue("@SoHDN", sSoHDN);
                    cmd2.Parameters.AddWithValue("@MaHang", sMaHang);
                    cmd2.Parameters.AddWithValue("@SL", sSLNhap);
                    cmd2.Parameters.AddWithValue("@DonGia_Nhap", sDonGia_Nhap);
                    cmd2.Parameters.AddWithValue("@NSX", sNSX);
                    cmd2.Parameters.AddWithValue("@HSD", sHSD);
                    cmd2.Parameters.AddWithValue("@ThanhTien", sThanhTien);

                    cmd2.ExecuteNonQuery();

                    MessageBox.Show("Thêm chi tiết hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                    LoadData(); // Cập nhật lại datagridview
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(sCon))
            {
                if (txtSoHDN.Text == "")
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn sửa hóa đơn này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Chuyển đổi an toàn dữ liệu đầu vào
                        decimal tongTien = 0, thueSuat = 0, voucher = 0, thanhToan = 0, soTien_TT = 0, thanhTien = 0, donGiaNhap = 0;
                        int slNhap = 0;

                        decimal.TryParse(txtTongTien.Text, out tongTien);
                        decimal.TryParse(txtThueSuat.Text, out thueSuat);
                        decimal.TryParse(txtVoucher.Text, out voucher);
                        decimal.TryParse(txtThanhToan.Text, out thanhToan);
                        decimal.TryParse(txtSoTienTT.Text, out soTien_TT);
                        decimal.TryParse(txtThanhTien.Text, out thanhTien);
                        decimal.TryParse(txtDGNhap.Text, out donGiaNhap);
                        int.TryParse(txtSLNhap.Text, out slNhap);

                        con.Open();

                        // Cập nhật bảng HOADON_NHAP
                        string query1 = @"UPDATE HOADON_NHAP SET 
                                        MaNV = @MaNV,
                                        MaNCC = @MaNCC,
                                        NgayNhap = @NgayNhap,
                                        TongTien = @TongTien,
                                        ThueSuat = @ThueSuat,
                                        Voucher = @Voucher,
                                        ThanhToan = @ThanhToan,
                                        SoTien_TT = @SoTien_TT
                                    WHERE SoHDN = @SoHDN";

                        SqlCommand cmd1 = new SqlCommand(query1, con);
                        cmd1.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmd1.Parameters.AddWithValue("@MaNV", txtNV.Text);
                        cmd1.Parameters.AddWithValue("@MaNCC", txtNCC.Text);
                        cmd1.Parameters.AddWithValue("@NgayNhap", dateTimePicker1.Value);
                        cmd1.Parameters.AddWithValue("@TongTien", tongTien);
                        cmd1.Parameters.AddWithValue("@ThueSuat", thueSuat);
                        cmd1.Parameters.AddWithValue("@Voucher", voucher);
                        cmd1.Parameters.AddWithValue("@ThanhToan", thanhToan);
                        cmd1.Parameters.AddWithValue("@SoTien_TT", soTien_TT);
                        cmd1.ExecuteNonQuery();

                        // Cập nhật bảng CHITIET_NHAP
                        string query2 = @"UPDATE CHITIET_NHAP SET 
                                        SL_NHAP = @SL_Nhap,
                                        DONGIA_NHAP = @DonGia_Nhap,
                                        ThanhTien = @ThanhTien
                                    WHERE SoHDN = @SoHDN AND MaHang = @MaHang";

                        SqlCommand cmd2 = new SqlCommand(query2, con);
                        cmd2.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmd2.Parameters.AddWithValue("@MaHang", txtMaHH.Text);
                        cmd2.Parameters.AddWithValue("@SL_Nhap", slNhap);
                        cmd2.Parameters.AddWithValue("@DonGia_Nhap", donGiaNhap);
                        cmd2.Parameters.AddWithValue("@ThanhTien", thanhTien);
                        cmd2.ExecuteNonQuery();

                        MessageBox.Show("Sửa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiDSHoaDon(); // Cập nhật lại danh sách hiển thị
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close(); // sửa đúng tên kết nối
                    }
                }
            }
        }

        private void HienThiDSHoaDon()
        {
            using (SqlConnection con = new SqlConnection(sCon))
            {
                string sQuery = @"
                SELECT HOADON_NHAP.SOHDN, NGAYNHAP, MANV, MANCC, 
                       MAHANG, SL_NHAP, DONGIA_NHAP, TONGTIEN, 
                       NSX, HSD, THUESUAT, VOUCHER, THANHTIEN, THANHTOAN, SOTIEN_TT
                FROM HOADON_NHAP 
                JOIN CHITIET_NHAP ON HOADON_NHAP.SOHDN = CHITIET_NHAP.SOHDN";
                SqlCommand cmd = new SqlCommand(sQuery, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

            // === Thông tin hóa đơn nhập ===
            txtSoHDN.Text = dataGridView1.Rows[e.RowIndex].Cells["SOHDN"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["NgayNhap"].Value);
            txtTongTien.Text = dataGridView1.Rows[e.RowIndex].Cells["TONGTIEN"].Value.ToString();
            txtThueSuat.Text = dataGridView1.Rows[e.RowIndex].Cells["THUESUAT"].Value.ToString();
            txtVoucher.Text = dataGridView1.Rows[e.RowIndex].Cells["VOUCHER"].Value.ToString();
            txtThanhToan.Text = dataGridView1.Rows[e.RowIndex].Cells["THANHTOAN"].Value.ToString();
            txtSoTienTT.Text = dataGridView1.Rows[e.RowIndex].Cells["SOTIEN_TT"].Value.ToString();
            txtNV.Text = dataGridView1.Rows[e.RowIndex].Cells["MANV"].Value.ToString();
            txtNCC.Text = dataGridView1.Rows[e.RowIndex].Cells["MANCC"].Value.ToString();

            // === Thông tin hóa đơn nhập ===
            txtMaHH.Text = dataGridView1.Rows[e.RowIndex].Cells["MAHANG"].Value.ToString();
            txtSLNhap.Text = dataGridView1.Rows[e.RowIndex].Cells["SL_NHAP"].Value.ToString();
            txtDGNhap.Text = dataGridView1.Rows[e.RowIndex].Cells["DONGIA_NHAP"].Value.ToString();
            dateTimePicker2.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["NSX"].Value);
            dateTimePicker3.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["HSD"].Value);
            txtThanhTien.Text = dataGridView1.Rows[e.RowIndex].Cells["THANHTIEN"].Value.ToString();




        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(sCon))
            {
                if (txtSoHDN.Text == "")
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này không? Dữ liệu liên quan sẽ bị xóa!", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();

                        // Xóa CONGNO trước
                        string query0 = "DELETE FROM CONGNO WHERE SOHDN = @SoHDN";
                        SqlCommand cmd0 = new SqlCommand(query0, con);
                        cmd0.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmd0.ExecuteNonQuery();

                        // Xóa chi tiết hóa đơn
                        string query1 = "DELETE FROM CHITIET_NHAP WHERE SoHDN = @SoHDN";
                        SqlCommand cmd1 = new SqlCommand(query1, con);
                        cmd1.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmd1.ExecuteNonQuery();

                        // Xóa hóa đơn chính
                        string query2 = "DELETE FROM HOADON_NHAP WHERE SoHDN = @SoHDN";
                        SqlCommand cmd2 = new SqlCommand(query2, con);
                        cmd2.Parameters.AddWithValue("@SoHDN", txtSoHDN.Text);
                        cmd2.ExecuteNonQuery();

                        MessageBox.Show("Xóa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiDSHoaDon();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
        private void label21_Click(object sender, EventArgs e)
        {

        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToUpper();
            string soHDN = null, maHang = null;

            // Cắt chuỗi theo dấu phẩy
            string[] parts = tuKhoa.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string raw in parts)
            {
                string part = raw.Trim();
                if (part.StartsWith("HDN"))
                    soHDN = part;
                else if (part.StartsWith("HH"))
                    maHang = part;
            }

            if (soHDN == null && maHang == null)
            {
                MessageBox.Show("Vui lòng nhập Số HDN hoặc Mã hàng (HH).", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection con = new SqlConnection(sCon))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_TimKiem_HoaDonNhap", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SoHDN", soHDN ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaHang", maHang ?? (object)DBNull.Value);

                        DataTable dt = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }

                        dataGridView1.DataSource = dt;

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy kết quả phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        DataRow row = dt.Rows[0];

                        // Thông tin hóa đơn
                        txtSoHDN.Text = row["SOHDN"].ToString();
                        txtNV.Text = row["MANV"].ToString();
                        txtNCC.Text = row["MANCC"].ToString();
                        dateTimePicker1.Value = Convert.ToDateTime(row["NGAYNHAP"]);

                        txtTongTien.Text = row["TONGTIEN"].ToString();
                        txtThanhToan.Text = row["THANHTOAN"].ToString();
                        txtSoTienTT.Text = row["SOTIEN_TT"].ToString();

                        // Chi tiết
                        txtMaHH.Text = row["MAHANG"].ToString();
                        txtSLNhap.Text = row["SL_NHAP"].ToString();
                        txtDGNhap.Text = row["DONGIA_NHAP"].ToString();
                        txtThueSuat.Text = row["THUESUAT"].ToString();
                        txtVoucher.Text = row["VOUCHER"].ToString();
                        txtThanhTien.Text = row["THANHTIEN"].ToString(); // <- QUAN TRỌNG

                        // Ngày sản xuất và hạn sử dụng
                        dateTimePicker2.Value = Convert.ToDateTime(row["NSX"]);
                        dateTimePicker3.Value = Convert.ToDateTime(row["HSD"]);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear(); // Xóa nội dung tìm kiếm
            ClearInputFields(); // Xóa trắng các trường nhập liệu

            // Tải lại tất cả dữ liệu từ cơ sở dữ liệu (không lọc gì cả)
            LoadData();

            MessageBox.Show("Đã làm mới toàn bộ dữ liệu và các trường nhập!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }

        private void ClearInputFields()
        {
            // Kiểm tra control có null không trước khi thao tác
            if (txtSoHDN != null) txtSoHDN.Clear(); else Console.WriteLine("txtSoHDN is null");
            if (dateTimePicker1 != null) dateTimePicker1.Value = DateTime.Now; else Console.WriteLine("dateTimePicker1 is null");
            if (txtTongTien != null) txtTongTien.Clear(); else Console.WriteLine("txtTongTien is null");
            if (txtThueSuat != null) txtThueSuat.Clear(); else Console.WriteLine("txtThueSuat is null");
            if (txtVoucher != null) txtVoucher.Clear(); else Console.WriteLine("txtVoucher is null");
            if (txtThanhToan != null) txtThanhToan.Clear(); else Console.WriteLine("txtThanhToan is null");
            if (txtSoTienTT != null) txtSoTienTT.Clear(); else Console.WriteLine("txtSoTienTT is null");
            if (txtNV != null) txtNV.Clear(); else Console.WriteLine("txtNV is null");
            if (txtNCC != null) txtNCC.Clear(); else Console.WriteLine("txtNCC is null");

            if (txtMaHH != null) txtMaHH.Clear(); else Console.WriteLine("txtMaHH is null");
            if (txtSLNhap != null) txtSLNhap.Clear(); else Console.WriteLine("txtSLNhap is null");
            if (txtDGNhap != null) txtDGNhap.Clear(); else Console.WriteLine("txtDGNhap is null");
            if (dateTimePicker2 != null) dateTimePicker2.Value = DateTime.Now; else Console.WriteLine("dateTimePicker2 is null");
            if (dateTimePicker3 != null) dateTimePicker3.Value = DateTime.Now; else Console.WriteLine("dateTimePicker3 is null");
            if (txtThanhTien != null) txtThanhTien.Clear(); else Console.WriteLine("txtThanhTien is null");

            if (txtTimKiem != null) txtTimKiem.Clear(); else Console.WriteLine("txtTimKiem is null");

            if (txtSoHDN != null) txtSoHDN.Focus();
        }

        private void txtMaHH_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form hiện tại
        }
    }
}
