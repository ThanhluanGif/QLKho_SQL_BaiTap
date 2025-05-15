using BLL.BLL_Basic;
using DTO;
using DTO.DTO_DangkyDangnhap;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace BaiTap
{
    public partial class fDanhmucNhaCungCap : Form
    {
        private readonly NhaCungCapBLL _nhaCungCapBLL;
        private NguoiDung _nguoiDung;
        public event Action OnDataChanged;


        public fDanhmucNhaCungCap(NguoiDung nguoiDung)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _nhaCungCapBLL = new NhaCungCapBLL();
            _nguoiDung = nguoiDung;

            // Điều chỉnh kích thước cột
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;

            // Đặt chiều cao của hàng đầu tiên
            guna2DataGridView1.RowTemplate.Height = 30;

            string path = Path.Combine(Application.StartupPath, _nguoiDung.AnhDaiDien);
            if (File.Exists(path))
            {
                guna2PictureBox1.Image = Image.FromFile(path);
            }
        }

        private void fDanhmucNhaCungCap_Load(object sender, EventArgs e)
        {
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            guna2HtmlLabel3.ForeColor = ColorTranslator.FromHtml("#0078D7");
            LoadData();
            cmbTrangThai.Items.Add("Hoạt động");
            cmbTrangThai.Items.Add("Không hoạt động");
            if (_nguoiDung != null)
            {
                guna2HtmlLabel2.Text = $"Xin chào, {_nguoiDung.HoTen}";
            }
        }

        private void LoadData()
        {
            guna2DataGridView1.DataSource = _nhaCungCapBLL.LayDanhSachNhaCungCap();
        }

        private void guna2Button2_Click(object sender, EventArgs e) // Thêm
        {
            try
            {
                var nhaCungCap = new NhaCungCapDTO
                {
                    MaNCC = txtMaNCC.Text,
                    TenNCC = txtTenNCC.Text,
                    DiaChi = txtDiaChi.Text,
                    SoDienThoai = txtSDT.Text,
                    Email = txtEmail.Text,
                    MaSoThue = txtMaSoThue.Text,
                    NguoiDaiDien = txtNguoiDaiDien.Text,
                    NgayTao = guna2DateTimePicker1.Value,
                    TrangThai = cmbTrangThai.SelectedIndex == 0 // 0: Hoạt động, 1: Không hoạt động
                };

                _nhaCungCapBLL.ThemNhaCungCap(nhaCungCap);
                MessageBox.Show("Thêm nhà cung cấp thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e) // Sửa
        {
            try
            {
                var nhaCungCap = new NhaCungCapDTO
                {
                    MaNCC = txtMaNCC.Text,
                    TenNCC = txtTenNCC.Text,
                    DiaChi = txtDiaChi.Text,
                    SoDienThoai = txtSDT.Text,
                    Email = txtEmail.Text,
                    MaSoThue = txtMaSoThue.Text,
                    NguoiDaiDien = txtNguoiDaiDien.Text,
                    TrangThai = cmbTrangThai.SelectedIndex == 0
                };

                _nhaCungCapBLL.SuaNhaCungCap(nhaCungCap);
                MessageBox.Show("Cập nhật nhà cung cấp thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e) // Xóa
        {
            try
            {
                var maNCC = txtMaNCC.Text;
                if (string.IsNullOrEmpty(maNCC))
                {
                    MessageBox.Show("Vui lòng nhập mã nhà cung cấp để xóa.");
                    return;
                }

                _nhaCungCapBLL.XoaNhaCungCap(maNCC);
                MessageBox.Show("Xóa nhà cung cấp thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e) // Làm mới
        {
            txtMaNCC.Clear();
            txtTenNCC.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            txtEmail.Clear();
            txtMaSoThue.Clear();
            txtNguoiDaiDien.Clear();
            cmbTrangThai.SelectedIndex = -1;
            guna2DateTimePicker1.Value = DateTime.Now;
            LoadData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo không click vào tiêu đề cột
            {
                var row = guna2DataGridView1.Rows[e.RowIndex];
                txtMaNCC.Text = row.Cells["MaNCC"].Value?.ToString();
                txtTenNCC.Text = row.Cells["TenNCC"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                txtSDT.Text = row.Cells["SoDienThoai"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtMaSoThue.Text = row.Cells["MaSoThue"].Value?.ToString();
                txtNguoiDaiDien.Text = row.Cells["NguoiDaiDien"].Value?.ToString();
                guna2DateTimePicker1.Value = DateTime.TryParse(row.Cells["NgayTao"].Value?.ToString(), out var ngayTao) ? ngayTao : DateTime.Now;
                cmbTrangThai.SelectedIndex = Convert.ToBoolean(row.Cells["TrangThai"].Value) ? 0 : 1; // 0: Hoạt động, 1: Không hoạt động
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                var keyword = txtTimKiem.Text;
                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
                    return;
                }

                var result = _nhaCungCapBLL.TimKiemTheoTen(keyword).ToList();
                if (result.Any())
                {
                    guna2DataGridView1.DataSource = result;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhà cung cấp nào.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
