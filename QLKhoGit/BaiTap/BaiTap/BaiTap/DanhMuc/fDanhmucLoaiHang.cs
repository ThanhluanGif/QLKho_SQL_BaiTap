using BLL.BLL_Basic;
using DTO;
using DTO.DTO_Basic;
using DTO.DTO_DangkyDangnhap;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace BaiTap.DanhMuc
{
    public partial class fDanhmucLoaiHang : Form
    {
        private readonly LoaiHangBLL _loaiHangBLL;
        private readonly NhomHangBLL _nhomHangBLL; // Khai báo BLL cho Nhóm Hàng
        private NguoiDung _nguoiDung;
        public event Action OnDataChanged;
        public fDanhmucLoaiHang(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _loaiHangBLL = new LoaiHangBLL(); // Khởi tạo BLL
            _nhomHangBLL = new NhomHangBLL(); // Khởi tạo BLL cho Nhóm Hàng
            _nguoiDung = nguoiDung;
            // Điều chỉnh kích thước cột
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;

            // Đặt chiều cao của hàng đầu tiên
            guna2DataGridView1.RowTemplate.Height = 30;
            // Điều chỉnh kích thước cột
            guna2DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView2.ColumnHeadersHeight = 50;

            // Đặt chiều cao của hàng đầu tiên
            guna2DataGridView2.RowTemplate.Height = 30;
            this.StartPosition = FormStartPosition.CenterScreen;
            gunaGroupBoxNhomHang.Visible = true; // hiện GroupBox nhóm hàng

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
            // Lấy danh sách loại hàng từ BLL và hiển thị vào DataGridView
            guna2DataGridView1.DataSource = _loaiHangBLL.LayDanhSachLoaiHang();
            guna2DataGridView2.DataSource = _nhomHangBLL.LayTatCaNhomHang();

            // Tải danh sách nhóm hàng vào ComboBox
            var danhSachNhomHang = _nhomHangBLL.LayTatCaNhomHang(); // Lấy từ LoaiHangBLL
            cmbNhomHang.DataSource = danhSachNhomHang;
            cmbNhomHang.ValueMember = "MaNhom"; // Giá trị là mã nhóm
            cmbNhomHang.DisplayMember = "TenNhom"; // Hiển thị tên nhóm
            cmbNhomHang.SelectedIndex = -1;
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

                var result = new LoaiHangBLL().TimKiemLoaiHangTheoTen(keyword).ToList();
                if (result.Any())
                {
                    guna2DataGridView1.DataSource = result;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy loại hàng nào phù hợp.");
                    guna2DataGridView1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                var loaiHang = new LoaiHangDTO
                {
                    MaLoai = txtMaLoai.Text,
                    TenLoai = txtTenLoai.Text,
                    MoTa = txtMoTa.Text,
                    NhomHang = cmbNhomHang.SelectedValue?.ToString() // <-- Thêm dòng này!
                };

                new LoaiHangBLL().ThemLoaiHang(loaiHang);
                MessageBox.Show("Thêm loại hàng thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                var loaiHang = new LoaiHangDTO
                {
                    MaLoai = txtMaLoai.Text,
                    TenLoai = txtTenLoai.Text,
                    MoTa = txtMoTa.Text,
                    NhomHang = cmbNhomHang.SelectedValue?.ToString() // <-- Thêm dòng này!
                };

                new LoaiHangBLL().SuaLoaiHang(loaiHang);
                MessageBox.Show("Cập nhật loại hàng thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                var maLoai = txtMaLoai.Text;
                if (string.IsNullOrEmpty(maLoai))
                {
                    MessageBox.Show("Vui lòng nhập mã loại để xóa.");
                    return;
                }

                new LoaiHangBLL().XoaLoaiHang(maLoai);
                MessageBox.Show("Xóa loại hàng thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            txtMaLoai.Clear();
            txtTenLoai.Clear();
            txtMoTa.Clear();
            LoadData();
        }


        private void fDanhmucLoaiHang_Load(object sender, EventArgs e)
        {
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            LoadData();
            LoadDanhSachNhomHang();
            if (_nguoiDung != null)
            {
                guna2HtmlLabel2.Text = $"Xin chào, {_nguoiDung.HoTen}";
            }
        }

        private void LoadDanhSachNhomHang()
        {
            guna2DataGridView2.DataSource = _nhomHangBLL.LayTatCaNhomHang();
        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                var nhomHang = new NhomHangDTO
                {
                    MaNhom = txtMaNhom.Text,
                    TenNhom = txtTenNhom.Text,
                   // MoTa = txtMoTaNhom.Text, // TextBox cho mô tả nhóm
                    NgayTao = DateTime.Now
                };

                _nhomHangBLL.Them(nhomHang);
                MessageBox.Show("Thêm nhóm hàng thành công!");
                LoadDanhSachNhomHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                var nhomHang = new NhomHangDTO
                {
                    MaNhom = txtMaNhom.Text,
                    TenNhom = txtTenNhom.Text,
                   // MoTa = txtMoTaNhom.Text
                };

                _nhomHangBLL.Sua(nhomHang);
                MessageBox.Show("Cập nhật nhóm hàng thành công!");
                LoadDanhSachNhomHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                var maNhom = txtMaNhom.Text;
                if (string.IsNullOrEmpty(maNhom))
                {
                    MessageBox.Show("Vui lòng nhập mã nhóm để xóa.");
                    return;
                }

                _nhomHangBLL.Xoa(maNhom);
                MessageBox.Show("Xóa nhóm hàng thành công!");
                LoadDanhSachNhomHang();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            txtMaNhom.Clear();
            txtTenNhom.Clear();

            LoadDanhSachNhomHang();
        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = guna2DataGridView2.Rows[e.RowIndex];
                txtMaNhom.Text = row.Cells["MaNhom"].Value?.ToString();
                txtTenNhom.Text = row.Cells["TenNhom"].Value?.ToString();
            }
        }

        private void guna2Button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                var keyword = txtTimKiemNhom.Text.Trim();
                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
                    return;
                }

                var result = _nhomHangBLL.TimKiem(keyword).ToList();
                if (result.Any())
                {
                    guna2DataGridView2.DataSource = result;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy nhóm hàng nào phù hợp.");
                    guna2DataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = guna2DataGridView1.Rows[e.RowIndex];
                txtMaLoai.Text = row.Cells["MaLoai"].Value?.ToString();
                txtTenLoai.Text = row.Cells["TenLoai"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
                var nhomHangValue = row.Cells["NhomHang"].Value?.ToString();
                if (!string.IsNullOrEmpty(nhomHangValue))
                {
                    cmbNhomHang.SelectedValue = nhomHangValue;
                }
            }
        }
    }
}
