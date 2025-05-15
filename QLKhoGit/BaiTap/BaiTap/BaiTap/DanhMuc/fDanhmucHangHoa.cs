using BLL.BLL_Basic;
using DTO;
using DTO.DTO_DangkyDangnhap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTap.DanhMuc
{
    public partial class fDanhmucHangHoa : Form
    {
        private readonly HangHoaBLL _hangHoaBLL;
        private NguoiDung _nguoiDung;
        public event Action OnDataChanged;

        public fDanhmucHangHoa(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _hangHoaBLL = new HangHoaBLL();
            // Điều chỉnh kích thước cột
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;

            // Đặt chiều cao của hàng đầu tiên
            guna2DataGridView1.RowTemplate.Height = 30;
            this.StartPosition = FormStartPosition.CenterScreen;
            _nguoiDung = nguoiDung;
        }

        private void fDanhmucHangHoa_Load(object sender, EventArgs e)
        {
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            LoadData();
            txtFilePic.Visible = false;

            if (_nguoiDung != null)
            {
                string path = _nguoiDung.AnhDaiDien;

                if (!string.IsNullOrEmpty(path))
                {
                    if (!Path.IsPathRooted(path)) // nếu là đường dẫn tương đối
                    {
                        path = Path.Combine(Application.StartupPath, path);
                    }

                    if (File.Exists(path))
                    {
                        using (var stream = new MemoryStream(File.ReadAllBytes(path)))
                        {
                            guna2PictureBox1.Image = Image.FromStream(stream);
                        }

                        guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

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

                guna2HtmlLabel2.Text = $"Xin chào, {_nguoiDung.HoTen}";
            }
        }


        private void LoadData()
        {
            guna2DataGridView1.DataSource = _hangHoaBLL.LayDanhSachHangHoa();
            // Load danh sách hàng hóa vào DataGridView
            guna2DataGridView1.DataSource = _hangHoaBLL.LayDanhSachHangHoa();

            // Load danh sách mã loại vào ComboBox
            var danhSachLoaiHang = new LoaiHangBLL().LayDanhSachLoaiHang();
            cmbMaLoai.DataSource = danhSachLoaiHang;
            cmbMaLoai.DisplayMember = "TenLoai"; // Hiển thị tên loại
            cmbMaLoai.ValueMember = "MaLoai";   // Giá trị là mã loại
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                var hangHoa = new HangHoaDTO
                {
                    MaHang = txtMaHang.Text,
                    TenHang = txtTenHang.Text,
                    MaLoai = cmbMaLoai.SelectedValue?.ToString(),
                    DonViTinh = txtDonviTinh.Text,
                    XuatXu = txtXuatxu.Text,
                    BaoHanh = string.IsNullOrEmpty(txtBaohanh.Text) ? (int?)null : int.Parse(txtBaohanh.Text),
                    MoTa = txtMota.Text,
                    HinhAnh = picHangHoa.Tag?.ToString() // Lưu đường dẫn ảnh
                };

                _hangHoaBLL.ThemHangHoa(hangHoa);
                MessageBox.Show("Thêm hàng hóa thành công!");
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
                var hangHoa = new HangHoaDTO
                {
                    MaHang = txtMaHang.Text,
                    TenHang = txtTenHang.Text,
                    MaLoai = cmbMaLoai.SelectedValue?.ToString(),
                    DonViTinh = txtDonviTinh.Text,
                    XuatXu = txtXuatxu.Text,
                    BaoHanh = string.IsNullOrEmpty(txtBaohanh.Text) ? (int?)null : int.Parse(txtBaohanh.Text),
                    MoTa = txtMota.Text,
                    HinhAnh = picHangHoa.Tag?.ToString() // Lưu đường dẫn ảnh
                };

                _hangHoaBLL.SuaHangHoa(hangHoa);
                MessageBox.Show("Cập nhật hàng hóa thành công!");
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
                var maHang = txtMaHang.Text;
                if (string.IsNullOrEmpty(maHang))
                {
                    MessageBox.Show("Vui lòng nhập mã hàng để xóa.");
                    return;
                }

                _hangHoaBLL.XoaHangHoa(maHang);
                MessageBox.Show("Xóa hàng hóa thành công!");
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
            // Xóa sạch dữ liệu trong các TextBox
            txtMaHang.Clear();
            txtTenHang.Clear();
            txtDonviTinh.Clear();
            txtXuatxu.Clear();
            txtBaohanh.Clear();
            txtMota.Clear();

            // Đặt ComboBox về trạng thái mặc định
            cmbMaLoai.SelectedIndex = -1;

            // Xóa ảnh trong PictureBox
            picHangHoa.Image = null;
            picHangHoa.Tag = null;

            // Tải lại dữ liệu từ cơ sở dữ liệu
            LoadData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = guna2DataGridView1.Rows[e.RowIndex];
                txtMaHang.Text = row.Cells["MaHang"].Value?.ToString();
                txtTenHang.Text = row.Cells["TenHang"].Value?.ToString();
                cmbMaLoai.SelectedValue = row.Cells["MaLoai"].Value?.ToString();
                txtDonviTinh.Text = row.Cells["DonViTinh"].Value?.ToString();
                txtXuatxu.Text = row.Cells["XuatXu"].Value?.ToString();
                txtBaohanh.Text = row.Cells["BaoHanh"].Value?.ToString();
               // txtFilePic.Text = row.Cells["HinhAnh"].Value?.ToString();
                txtMota.Text = row.Cells["MoTa"].Value?.ToString();

                var hinhAnhPath = row.Cells["HinhAnh"].Value?.ToString();
                if (!string.IsNullOrEmpty(hinhAnhPath) && File.Exists(hinhAnhPath))
                {
                    picHangHoa.Image = Image.FromFile(hinhAnhPath);
                    picHangHoa.Tag = hinhAnhPath; // Lưu đường dẫn ảnh
                }
                else
                {
                    picHangHoa.Image = null;
                    picHangHoa.Tag = null;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    picHangHoa.Image = Image.FromFile(openFileDialog.FileName);
                    picHangHoa.Tag = openFileDialog.FileName; // Lưu đường dẫn ảnh vào Tag
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy từ khóa từ TextBox
                var keyword = txtTimKiem.Text.Trim();

                // Kiểm tra nếu từ khóa rỗng
                if (string.IsNullOrEmpty(keyword))
                {
                    MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
                    return;
                }

                // Gọi phương thức tìm kiếm từ HangHoaBLL
                var result = _hangHoaBLL.SearchByName(keyword).ToList();

                // Kiểm tra nếu có kết quả
                if (result.Any())
                {
                    guna2DataGridView1.DataSource = result;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hàng hóa nào phù hợp.");
                    guna2DataGridView1.DataSource = null; // Xóa dữ liệu trong DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
