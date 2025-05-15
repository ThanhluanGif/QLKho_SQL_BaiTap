using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL.BLL_Basic; // For KhoBLL
using DTO;
using DTO.DTO_DangkyDangnhap; // For KhoDTO
using System.IO;
using System.Drawing.Drawing2D;

namespace BaiTap.DanhMuc
{
    public partial class fDanhmucKho : Form
    {
        private readonly KhoBLL _KhoBLL;
        private NguoiDung _nguoiDung;
        public event Action OnDataChanged;

        public fDanhmucKho(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _KhoBLL = new KhoBLL();
            _nguoiDung = nguoiDung;
            LoadData();
            // Điều chỉnh kích thước cột
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;

            // Đặt chiều cao của hàng đầu tiên
            guna2DataGridView1.RowTemplate.Height = 30;
            this.StartPosition = FormStartPosition.CenterScreen;

            string path = _nguoiDung.AnhDaiDien;

            if (!string.IsNullOrEmpty(path))
            {
                // Nếu là đường dẫn tương đối → Combine
                if (!Path.IsPathRooted(path))
                {
                    path = Path.Combine(Application.StartupPath, path);
                }

                // Load ảnh nếu tồn tại
                if (File.Exists(path))
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(path)))
                    {
                        guna2PictureBox1.Image = Image.FromStream(stream);
                    }

                    guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                    // Bo tròn nếu bạn cần:
                    GraphicsPath gp = new GraphicsPath();
                    gp.AddEllipse(0, 0, guna2PictureBox1.Width - 1, guna2PictureBox1.Height - 1);
                    guna2PictureBox1.Region = new Region(gp);
                }
                else
                {
                    guna2PictureBox1.Image = null;
                    guna2PictureBox1.Region = null;
                }
            }

        }

        private void LoadData()
        {
            // Load all warehouse data into the DataGridView
            guna2DataGridView1.DataSource = _KhoBLL.LayDanhSachKho();
        }
        private void fDanhmucKho_Load(object sender, EventArgs e)
        {
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            LoadData();
            if (_nguoiDung != null)
            {
                guna2HtmlLabel2.Text = $"Xin chào, {_nguoiDung.HoTen}";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var kho = new KhoDTO
                {
                    MaKho = txtMaKho.Text,
                    TenKho = txtTenKho.Text,
                    DiaChi = txtDiaChi.Text,
                    DienTich = decimal.Parse(txtDienTich.Text),
                    NguoiQuanLy = txtNguoiQuanLy.Text,
                    SoDienThoai = txtSoDienThoai.Text,
                    TrangThai = chkTrangThaiYes.Checked
                };

                _KhoBLL.ThemKho(kho);
                MessageBox.Show("Thêm kho thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var kho = new KhoDTO
                {
                    MaKho = txtMaKho.Text,
                    TenKho = txtTenKho.Text,
                    DiaChi = txtDiaChi.Text,
                    DienTich = decimal.Parse(txtDienTich.Text),
                    NguoiQuanLy = txtNguoiQuanLy.Text,
                    SoDienThoai = txtSoDienThoai.Text,
                    TrangThai = chkTrangThaiYes.Checked
                };

                _KhoBLL.SuaKho(kho);
                MessageBox.Show("Cập nhật kho thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var maKho = txtMaKho.Text;

                if (string.IsNullOrEmpty(maKho))
                {
                    MessageBox.Show("Vui lòng nhập mã kho để xóa.");
                    return;
                }

                _KhoBLL.XoaKho(maKho);
                MessageBox.Show("Xóa kho thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                var keyword = txtTimKiem.Text.Trim();

                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
                    return;
                }

                var result = _KhoBLL.TimKiemKhoTheoTen(keyword).ToList();

                if (result.Any())
                {
                    guna2DataGridView1.DataSource = result;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kho nào phù hợp.");
                    guna2DataGridView1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaKho.Clear();
            txtTenKho.Clear();
            txtDiaChi.Clear();
            txtDienTich.Clear();
            txtNguoiQuanLy.Clear();
            txtSoDienThoai.Clear();
            chkTrangThaiYes.Checked = false;
            txtTimKiem.Clear();
            LoadData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = guna2DataGridView1.Rows[e.RowIndex];
                txtMaKho.Text = row.Cells["MaKho"].Value?.ToString();
                txtTenKho.Text = row.Cells["TenKho"].Value?.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
                txtDienTich.Text = row.Cells["DienTich"].Value?.ToString();
                txtNguoiQuanLy.Text = row.Cells["NguoiQuanLy"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
                chkTrangThaiYes.Checked = row.Cells["TrangThai"].Value != null && (bool)row.Cells["TrangThai"].Value;
            }
        }
    }
}
