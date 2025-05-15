using BLL.BLL_Basic;
using BLL.BLL_QuanLyKho;
using BLL.BLL_TrungGian;
using DTO.DTO_DangkyDangnhap;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace BaiTap.DanhMuc
{
    public partial class fDanhmucTonKho : Form
    {
        private readonly TonKhoBLL _tonKhoBLL;
        private NguoiDung _nguoiDung;
        public event Action OnDataChanged;


        public fDanhmucTonKho(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _tonKhoBLL = new TonKhoBLL();
            LoadData();
            // Điều chỉnh kích thước cột
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;

            // Đặt chiều cao của hàng đầu tiên
            guna2DataGridView1.RowTemplate.Height = 30;
            this.StartPosition = FormStartPosition.CenterScreen;
            // Gán sự kiện cho ComboBox
            cmbMaHang.SelectedIndexChanged += cmbMaHang_SelectedIndexChanged;
            _nguoiDung = nguoiDung;
            string path = Path.Combine(Application.StartupPath, _nguoiDung.AnhDaiDien);
            if (File.Exists(path))
            {
                guna2PictureBox1.Image = Image.FromFile(path);
            }
        }

        private void fDanhmucTonKho_Load(object sender, EventArgs e)
        {
            LoadData();
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            //txtMaTonKho.Text = TaoMaTonKhoTuDong(); // Tạo mã tự động khi load form 
            if (_nguoiDung != null)
            {
                guna2HtmlLabel2.Text = $"Xin chào, {_nguoiDung.HoTen}";
            }
        }

        // 1. Load danh sách tồn kho
        private void LoadData()
        {
            try
            {
                // Lấy danh sách tồn kho
                var danhSachTonKho = _tonKhoBLL.LayDanhSachTonKho();
                guna2DataGridView1.DataSource = null;
                guna2DataGridView1.DataSource = danhSachTonKho;

                // Tải danh sách kho
                var khoBLL = new KhoBLL();
                var danhSachKho = khoBLL.LayDanhSachKho();
                cmbMaKho.DataSource = danhSachKho;
                cmbMaKho.DisplayMember = "TenKho"; // Hiển thị tên kho
                cmbMaKho.ValueMember = "MaKho";   // Giá trị là mã kho
                cmbMaKho.SelectedIndex = -1;      // Không chọn mục nào mặc định

                // Tải danh sách hàng hóa
                var hangHoaBLL = new HangHoaBLL();
                var danhSachHangHoa = hangHoaBLL.LayDanhSachHangHoa();
                cmbMaHang.DataSource = danhSachHangHoa;
                cmbMaHang.DisplayMember = "TenHang"; // Hiển thị tên hàng hóa
                cmbMaHang.ValueMember = "MaHang";   // Giá trị là mã hàng hóa
                cmbMaHang.SelectedIndex = -1;       // Không chọn mục nào mặc định

                // Thêm trạng thái tồn kho
                var danhSachTrangThai = new List<string> { "Hoạt động", "Không hoạt động" };
                cmbTrangThai.DataSource = danhSachTrangThai;
                cmbTrangThai.SelectedIndex = 0; // Mặc định là "Hoạt động"

                txtMaTonKho.Text = TaoMaTonKhoTuDong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }

        private string TaoMaTonKhoTuDong()
        {
            var danhSach = _tonKhoBLL.LayDanhSachTonKho();

            int soThuTu = 1;

            if (danhSach != null && danhSach.Any())
            {
                var maxSo = danhSach
                    .Where(t => t.MaTonKho.StartsWith("TK"))
                    .Select(t =>
                    {
                        string so = t.MaTonKho.Substring(2); // Bỏ "TK"
                        return int.TryParse(so, out int parsed) ? parsed : 0;
                    })
                    .Max();

                soThuTu = maxSo + 1;
            }

            return $"TK{soThuTu.ToString("D4")}";
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

                var danhSachTonKho = _tonKhoBLL.LayDanhSachTonKho()
                    .Where(tk => tk.MaTonKho.Contains(keyword) || tk.MaHang.Contains(keyword) || tk.MaKho.Contains(keyword))
                    .ToList();

                if (danhSachTonKho.Any())
                {
                    guna2DataGridView1.DataSource = null;
                    guna2DataGridView1.DataSource = danhSachTonKho;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kết quả phù hợp.");
                    guna2DataGridView1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maTonKho = txtMaTonKho.Text.Trim();

                // Nếu không nhập thủ công thì tạo mã tự động
                if (string.IsNullOrEmpty(maTonKho))
                {
                    maTonKho = TaoMaTonKhoTuDong();
                    txtMaTonKho.Text = maTonKho; // Gán lại vào TextBox
                }

                var tonKho = new TonKhoDTO
                {
                    MaTonKho = maTonKho,
                    MaHang = cmbMaHang.SelectedValue?.ToString(),
                    MaKho = cmbMaKho.SelectedValue?.ToString(),
                    SoLuong = int.Parse(txtSoLuong.Text.Trim()),
                    NgayCapNhat = dtpNgayCapNhat.Value,
                    DonGia = decimal.Parse(txtDonGia.Text.Trim())
                };

                if (_tonKhoBLL.ThemTonKho(tonKho))
                {
                    MessageBox.Show("Thêm tồn kho thành công!");
                    OnDataChanged?.Invoke();

                    LoadData();
                    ClearForm(); // Gọi clear form và sinh lại mã mới tại đây
                }
                else
                {
                    MessageBox.Show("Thêm tồn kho thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm tồn kho: {ex.Message}");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView1.SelectedRows.Count > 0)
                {
                    var selectedRow = guna2DataGridView1.SelectedRows[0];
                    var maTonKho = selectedRow.Cells["MaTonKho"].Value.ToString();

                    var tonKho = new TonKhoDTO
                    {
                        MaTonKho = maTonKho,
                        MaHang = cmbMaHang.SelectedValue?.ToString(),
                        MaKho = cmbMaKho.SelectedValue?.ToString(),
                        SoLuong = int.Parse(txtSoLuong.Text.Trim()),
                        NgayCapNhat = dtpNgayCapNhat.Value,
                        DonGia = decimal.Parse(txtDonGia.Text.Trim())
                    };

                    if (_tonKhoBLL.SuaTonKho(tonKho))
                    {
                        MessageBox.Show("Sửa tồn kho thành công!");
                        OnDataChanged?.Invoke();
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Sửa tồn kho thất bại!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một dòng để sửa!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa tồn kho: {ex.Message}");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView1.SelectedRows.Count > 0)
                {
                    var selectedRow = guna2DataGridView1.SelectedRows[0];
                    var maTonKho = selectedRow.Cells["MaTonKho"].Value.ToString();

                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa tồn kho này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (_tonKhoBLL.XoaTonKho(maTonKho))
                        {
                            MessageBox.Show("Xóa tồn kho thành công!");
                            OnDataChanged?.Invoke();
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Xóa tồn kho thất bại!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một dòng để xóa!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa tồn kho: {ex.Message}");
            }
        }

        // Xóa dữ liệu trên form
        private void ClearForm()
        {
            txtMaTonKho.Text = TaoMaTonKhoTuDong(); // Gán lại mã
            cmbMaHang.SelectedIndex = -1;
            cmbMaKho.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtDonGia.Clear();
            dtpNgayCapNhat.Value = DateTime.Now;
            txtTimKiem.Clear();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var selectedRow = guna2DataGridView1.Rows[e.RowIndex];

                    txtMaTonKho.Text = selectedRow.Cells["MaTonKho"].Value?.ToString();
                    cmbMaHang.SelectedValue = selectedRow.Cells["MaHang"].Value?.ToString();
                    cmbMaKho.SelectedValue = selectedRow.Cells["MaKho"].Value?.ToString();
                    txtSoLuong.Text = selectedRow.Cells["SoLuong"].Value?.ToString();
                    txtDonGia.Text = selectedRow.Cells["DonGia"].Value?.ToString();

                    dtpNgayCapNhat.Value = DateTime.TryParse(
                        selectedRow.Cells["NgayCapNhat"].Value?.ToString(),
                        out DateTime ngayCapNhat) ? ngayCapNhat : DateTime.Now;

                    // Nếu có cột Trạng Thái
                    if (selectedRow.Cells["TrangThai"] != null)
                    {
                        cmbTrangThai.SelectedItem = selectedRow.Cells["TrangThai"].Value?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}");
            }
        }

        private void btnInBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy danh sách tồn kho từ BLL
                var danhSachTonKho = _tonKhoBLL.LayDanhSachTonKho();

                if (danhSachTonKho == null || !danhSachTonKho.Any())
                {
                    MessageBox.Show("Không có dữ liệu tồn kho để in báo cáo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Tạo PrintDocument
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += (s, ev) => PrintPageHandler(ev, danhSachTonKho);

                // Hiển thị PrintPreviewDialog
                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog
                {
                    Document = printDocument,
                    Width = 800,
                    Height = 600
                };

                printPreviewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintPageHandler(PrintPageEventArgs e, List<TonKhoDTO> danhSachTonKho)
        {
            try
            {
                // Thiết lập font và khoảng cách
                Font headerFont = new Font("Arial", 14, FontStyle.Bold);
                Font contentFont = new Font("Arial", 12);
                float y = 20; // Vị trí Y ban đầu
                float lineHeight = contentFont.GetHeight() + 5;

                // Vẽ tiêu đề báo cáo
                e.Graphics.DrawString("BÁO CÁO TỒN KHO", headerFont, Brushes.Black, 300, y);
                y += lineHeight * 2;

                // Vẽ tiêu đề cột
                e.Graphics.DrawString("Mã Tồn Kho", contentFont, Brushes.Black, 50, y);
                e.Graphics.DrawString("Mã Hàng", contentFont, Brushes.Black, 150, y);
                e.Graphics.DrawString("Mã Kho", contentFont, Brushes.Black, 250, y);
                e.Graphics.DrawString("Số Lượng", contentFont, Brushes.Black, 350, y);
                e.Graphics.DrawString("Ngày Cập Nhật", contentFont, Brushes.Black, 450, y);
                e.Graphics.DrawString("Đơn Giá", contentFont, Brushes.Black, 600, y);
                y += lineHeight;

                // Vẽ dữ liệu tồn kho
                foreach (var tonKho in danhSachTonKho)
                {
                    e.Graphics.DrawString(tonKho.MaTonKho.ToString(), contentFont, Brushes.Black, 50, y);
                    e.Graphics.DrawString(tonKho.MaHang, contentFont, Brushes.Black, 150, y);
                    e.Graphics.DrawString(tonKho.MaKho, contentFont, Brushes.Black, 250, y);
                    e.Graphics.DrawString(tonKho.SoLuong.ToString(), contentFont, Brushes.Black, 350, y);
                    e.Graphics.DrawString(tonKho.NgayCapNhat.ToString("dd/MM/yyyy"), contentFont, Brushes.Black, 450, y);
                    e.Graphics.DrawString(tonKho.DonGia.ToString("C"), contentFont, Brushes.Black, 600, y);
                    y += lineHeight;

                    // Kiểm tra nếu vượt quá chiều cao trang giấy
                    if (y + lineHeight > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }
                }

                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMaHang.SelectedValue != null)
                {
                    // Lấy mã hàng được chọn
                    var maHang = cmbMaHang.SelectedValue.ToString();

                    // Lấy danh sách nhập xuất tồn từ BLL
                    var nhapXuatTonBLL = new NhapXuatTonBLL();
                    var danhSachNhapXuatTon = nhapXuatTonBLL.LayDanhSachNhapXuatTon();

                    // Tìm số lượng tồn kho của mã hàng được chọn
                    var tonKho = danhSachNhapXuatTon.FirstOrDefault(t => t.MaHang == maHang)?.Ton ?? 0;

                    // Hiển thị số lượng tồn kho vào TextBox
                    txtSoLuong.Text = tonKho.ToString();

                    // Lấy danh sách giá xuất từ BLL
                    var hangHoaGiaXuatBLL = new HangHoa_GiaXuatBLL();
                    var danhSachGiaXuat = hangHoaGiaXuatBLL.GetAll();

                    // Tìm đơn giá của mã hàng được chọn
                    var donGia = danhSachGiaXuat.FirstOrDefault(gx => gx.MaHang == maHang)?.DonGiaXuat ?? 0;

                    // Hiển thị đơn giá vào TextBox
                    txtDonGia.Text = donGia.ToString("F2"); // Định dạng số với dấu phân cách hàng nghìn
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin hàng hóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearForm();
        }
    }
}
