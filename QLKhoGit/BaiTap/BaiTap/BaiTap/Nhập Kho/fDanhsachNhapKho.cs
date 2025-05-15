using BLL.BLL_QuanLyKho;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DTO.DTO_DangkyDangnhap;

namespace BaiTap.Nhập_Kho
{
    public partial class fDanhsachNhapKho : Form
    {
        private readonly NhapKhoBLL _nhapKhoBLL;
        private NguoiDung _nguoiDung;
        public event Action OnDataChanged;

        public fDanhsachNhapKho(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _nhapKhoBLL = new NhapKhoBLL();

            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;
            guna2DataGridView1.RowTemplate.Height = 30;
            this.StartPosition = FormStartPosition.CenterScreen;
            _nguoiDung = nguoiDung;
            string path = Path.Combine(Application.StartupPath, _nguoiDung.AnhDaiDien);
            if (File.Exists(path))
            {
                guna2PictureBox1.Image = Image.FromFile(path);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text.Trim();
            DateTime? tuNgay = dtpTuNgay.Value;
            DateTime? denNgay = dtpDenNgay.Value;
            var danhSach = _nhapKhoBLL.TimKiemNhapKho(maPhieu, tuNgay, denNgay);

            if (danhSach == null || !danhSach.Any())
            {
                MessageBox.Show("Không tìm thấy dữ liệu phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                guna2DataGridView1.DataSource = null;
            }
            else
            {
                guna2DataGridView1.DataSource = danhSach;
            }
        }

        private object GetPropertyValue(dynamic obj, string propertyName)
        {
            try
            {
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                {
                    Console.WriteLine($"Property '{propertyName}' not found in object.");
                    return null;
                }
                var value = propertyInfo.GetValue(obj, null);
                Console.WriteLine($"Property '{propertyName}' value: {value}");
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accessing property '{propertyName}': {ex.Message}");
                return null;
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < guna2DataGridView1.Rows.Count)
            {
                var selectedRow = guna2DataGridView1.Rows[e.RowIndex];
                string maPhieuNhap = selectedRow.Cells["MaPhieuNhap"].Value.ToString();
                txtMaPhieu.Text = maPhieuNhap;
            }
        }

        private void fDanhsachNhapKho_Load(object sender, EventArgs e)
        {
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            LoadData();
            if (_nguoiDung != null)
            {
                guna2HtmlLabel2.Text = $"Xin chào, {_nguoiDung.HoTen}";
            }
        }

        private void LoadData()
        {
            var danhSachNhapKho = _nhapKhoBLL.GetAllWithDetails();
            if (danhSachNhapKho == null || !danhSachNhapKho.Any())
            {
                MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                guna2DataGridView1.DataSource = danhSachNhapKho;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            var taoPhieuForm = new fTaoNhapKho(_nguoiDung);
            this.Hide();
            taoPhieuForm.Show();
            taoPhieuForm.FormClosed += (s, args) => this.Close();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // Export logic giữ nguyên
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Print logic giữ nguyên
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng sửa trạng thái đã bị loại bỏ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show("Dữ liệu đã được làm mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Không cần xử lý trạng thái
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập kho để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = guna2DataGridView1.SelectedRows[0];
            string maPhieuNhap = selectedRow.Cells["MaPhieuNhap"].Value.ToString();

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu nhập '{maPhieuNhap}' không?",
                                                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    var chiTietNhapKhoBLL = new ChiTietNhapKhoBLL();
                    var chiTiets = chiTietNhapKhoBLL.GetByMaPhieuNhap(maPhieuNhap);
                    var tonKhoBLL = new TonKhoBLL();

                    foreach (var chiTiet in chiTiets)
                    {
                        tonKhoBLL.CapNhatTonKhoKhiXoaNhap(chiTiet.MaHang, chiTiet.MaKho, chiTiet.SoLuong);
                    }

                    foreach (var chiTiet in chiTiets)
                    {
                        chiTietNhapKhoBLL.XoaChiTietNhapKho(chiTiet.MaCTNhap);
                    }

                    if (_nhapKhoBLL.XoaNhapKho(maPhieuNhap))
                    {
                        MessageBox.Show("Xóa phiếu nhập kho thành công và đã cập nhật tồn kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        OnDataChanged?.Invoke();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa phiếu nhập kho thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}