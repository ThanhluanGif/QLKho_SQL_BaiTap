using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.BLL_QuanLyKho;
using System.Windows.Forms;
using BLL.BLL_Basic;
using System.Drawing.Printing;
using BLL.BLL_TrungGian;
using DAL;
using DTO.DTO_DangkyDangnhap;
using System.IO;

namespace BaiTap.Nhập_Kho
{
    public partial class fTaoNhapKho : Form
    {
        private readonly NhapKhoBLL _nhapKhoBLL;
        private readonly List<ChiTietNhapKhoDTO> _chiTietNhapKhoList;
        private NguoiDung _nguoiDung;
        public event Action OnDataChanged;

        public fTaoNhapKho(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _nhapKhoBLL = new NhapKhoBLL();
            _chiTietNhapKhoList = new List<ChiTietNhapKhoDTO>();
            LoadComboBoxData();
            UpdateDataGridView();
            UpdateTongTien();
            // Điều chỉnh kích thước cột
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;

            // Đặt chiều cao của hàng đầu tiên
            guna2DataGridView1.RowTemplate.Height = 30;
            this.StartPosition = FormStartPosition.CenterScreen;
            _nguoiDung = nguoiDung;
            string path = Path.Combine(Application.StartupPath, _nguoiDung.AnhDaiDien);
            if (File.Exists(path))
            {
                guna2PictureBox1.Image = Image.FromFile(path);
            }
        }

        private void LoadComboBoxData()
        {
            // Load danh sách nhà cung cấp
            var nhaCungCapBLL = new NhaCungCapBLL();
            var danhSachNhaCungCap = nhaCungCapBLL.LayDanhSachNhaCungCap();
            cmbNhaCungCap.DataSource = danhSachNhaCungCap;
            cmbNhaCungCap.DisplayMember = "TenNCC";
            cmbNhaCungCap.ValueMember = "MaNCC";
            cmbNhaCungCap.SelectedIndex = -1;

            // Gắn sự kiện SelectedIndexChanged cho cmbNhaCungCap
            cmbNhaCungCap.SelectedIndexChanged += CmbNhaCungCap_SelectedIndexChanged;

            // Load danh sách kho
            var khoBLL = new KhoBLL();
            var danhSachKho = khoBLL.LayDanhSachKho();
            cmbKhoNhap.DataSource = danhSachKho;
            cmbKhoNhap.DisplayMember = "TenKho";
            cmbKhoNhap.ValueMember = "MaKho";
            cmbKhoNhap.SelectedIndex = -1;
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

                // Giả sử có phương thức tìm kiếm hàng hóa trong BLL
                var hangHoaBLL = new HangHoaBLL();
                var result = hangHoaBLL.SearchByName(keyword).ToList();

                if (result.Any())
                {
                    guna2DataGridView1.DataSource = result;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hàng hóa nào phù hợp.");
                    guna2DataGridView1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Thêm hàng hóa vào danh sách chi tiết nhập kho
            try
            {
                var maHang = cmbHangHoa.SelectedValue?.ToString();
                var soLuong = int.Parse(txtSoLuong.Text);
                var donGia = decimal.Parse(txtDonGia.Text);
                var maKho = cmbKhoNhap.SelectedValue?.ToString(); // ✅ Thêm dòng này

                if (string.IsNullOrEmpty(maHang))
                {
                    MessageBox.Show("Vui lòng chọn hàng hóa.");
                    return;
                }

                if (string.IsNullOrEmpty(maKho))
                {
                    MessageBox.Show("Vui lòng chọn kho trước khi thêm hàng.");
                    return;
                }

                var chiTiet = new ChiTietNhapKhoDTO
                {
                    MaHang = maHang,
                    SoLuong = soLuong,
                    DonGia = donGia,
                    ThanhTien = soLuong * donGia,
                    MaKho = maKho // ✅ bạn đã gán rồi, đúng
                };

                _chiTietNhapKhoList.Add(chiTiet);
                UpdateDataGridView(); // Cập nhật lại DataGridView
                UpdateTongTien();     // Cập nhật tổng tiền
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            //Hủy hàng hóa đã chọn trong danh sách chi tiết nhập kho
            ClearForm();
            try
            {
                if (guna2DataGridView1.SelectedRows.Count > 0)
                {
                    var selectedRow = guna2DataGridView1.SelectedRows[0];
                    var maHang = selectedRow.Cells["MaHang"].Value.ToString();

                    var itemToRemove = _chiTietNhapKhoList.FirstOrDefault(x => x.MaHang == maHang);
                    if (itemToRemove != null)
                    {
                        _chiTietNhapKhoList.Remove(itemToRemove);
                        UpdateDataGridView();
                        UpdateTongTien();
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn hàng hóa để hủy.");
                }
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
                var maKho = cmbKhoNhap.SelectedValue?.ToString();
                var maNCC = cmbNhaCungCap.SelectedValue?.ToString();

                if (string.IsNullOrEmpty(txtMaPhieuNhap.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phiếu nhập.");
                    return;
                }

                if (string.IsNullOrEmpty(maKho) || string.IsNullOrEmpty(maNCC))
                {
                    MessageBox.Show("Vui lòng chọn kho nhập và nhà cung cấp.");
                    return;
                }

                if (_chiTietNhapKhoList == null || !_chiTietNhapKhoList.Any())
                {
                    MessageBox.Show("Danh sách chi tiết nhập kho không được để trống.");
                    return;
                }

                // 1. Tạo đối tượng Nhập Kho
                var nhapKho = new NhapKhoDTO
                {
                    MaPhieuNhap = txtMaPhieuNhap.Text,
                    MaKho = maKho,
                    MaNCC = maNCC,
                    NgayNhap = dtpNgayNhap.Value,
                    NguoiNhap = txtNguoiNhap.Text,
                    TongTien = _chiTietNhapKhoList.Sum(x => x.ThanhTien),
                    GhiChu = txtGhiChu.Text,
                    ChiTietNhapKho = _chiTietNhapKhoList
                };

                // 2. Lưu phiếu nhập kho
                var result = _nhapKhoBLL.ThemNhapKho(nhapKho);

                if (!result)
                {
                    MessageBox.Show("Lưu phiếu nhập kho thất bại. Vui lòng thử lại.");
                    return;
                }

                // 3. Lưu chi tiết nhập kho
                var chiTietNhapKhoBLL = new ChiTietNhapKhoBLL();
                foreach (var chiTiet in _chiTietNhapKhoList)
                {
                    chiTiet.MaPhieuNhap = nhapKho.MaPhieuNhap; // Gán mã phiếu nhập
                    chiTietNhapKhoBLL.ThemChiTietNhapKho(chiTiet);
                }

                // 4. Cập nhật tồn kho (Có kiểm tra: nếu chưa có thì tự tạo mới luôn)
                var tonKhoBLL = new TonKhoBLL();
                foreach (var chiTiet in _chiTietNhapKhoList)
                {
                    // Đảm bảo gán lại MaKho vào từng chi tiết (nếu có bị mất)
                    chiTiet.MaKho = maKho;

                    tonKhoBLL.CapNhatTonKhoKhiNhap(chiTiet.MaHang, chiTiet.MaKho, chiTiet.SoLuong, chiTiet.DonGia);
                }


                MessageBox.Show("Lưu phiếu nhập kho và cập nhật tồn kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnDataChanged?.Invoke();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void guna2Button4_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog
            {
                Document = printDocument
            };

            // Hiển thị bản xem trước in
            printPreviewDialog.ShowDialog();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Lấy thông tin phiếu nhập kho
            string maPhieuNhap = txtMaPhieuNhap.Text;
            string ngayNhap = dtpNgayNhap.Value.ToString("dd/MM/yyyy");
            string nguoiNhap = txtNguoiNhap.Text;
            string tongTien = lblTongTien.Text;

            // Vẽ nội dung phiếu nhập kho
            Graphics g = e.Graphics;
            Font font = new Font("Arial", 12);
            float y = 20;

            g.DrawString("PHIẾU NHẬP KHO", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, 100, y);
            y += 40;

            g.DrawString($"Mã Phiếu Nhập: {maPhieuNhap}", font, Brushes.Black, 20, y);
            y += 30;
            g.DrawString($"Ngày Nhập: {ngayNhap}", font, Brushes.Black, 20, y);
            y += 30;
            g.DrawString($"Người Nhập: {nguoiNhap}", font, Brushes.Black, 20, y);
            y += 30;
            g.DrawString($"Tổng Tiền: {tongTien} VND", font, Brushes.Black, 20, y);
            y += 40;

            g.DrawString("Chi Tiết Nhập Kho:", font, Brushes.Black, 20, y);
            y += 30;

            // Vẽ bảng chi tiết nhập kho
            foreach (var chiTiet in _chiTietNhapKhoList)
            {
                g.DrawString($"- Mã Hàng: {chiTiet.MaHang}, Số Lượng: {chiTiet.SoLuong}, Đơn Giá: {chiTiet.DonGia}, Thành Tiền: {chiTiet.ThanhTien}", font, Brushes.Black, 20, y);
                y += 20;
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Xử lý sự kiện khi người dùng chọn hàng hóa trong DataGridView
            if (e.RowIndex >= 0) // Ensure a valid row is selected
            {
                var row = guna2DataGridView1.Rows[e.RowIndex];

                // Populate text boxes with the selected row's data
                cmbHangHoa.Text = row.Cells["MaHang"].Value?.ToString();
                txtSoLuong.Text = row.Cells["SoLuong"].Value?.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value?.ToString();
            }
        }

        private void UpdateDataGridView()
        {
            var hangHoaBLL = new HangHoaBLL();
            var danhSachHangHoa = hangHoaBLL.LayDanhSachHangHoa();

            // Combine data from _chiTietNhapKhoList and danh sách hàng hóa
            var data = _chiTietNhapKhoList.Select(ct => new
            {
                MaHang = ct.MaHang,
                TenHang = danhSachHangHoa.FirstOrDefault(hh => hh.MaHang == ct.MaHang)?.TenHang ?? "Không xác định",
                SoLuong = ct.SoLuong,
                DonGia = ct.DonGia,
                ThanhTien = ct.ThanhTien
            }).ToList();

            // Display data in DataGridView
            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.DataSource = data;
        }

        private void UpdateTongTien()
        {
            lblTongTien.Text = _chiTietNhapKhoList.Sum(x => x.ThanhTien).ToString("N0");
        }

        private void ClearForm()
        {
            txtMaPhieuNhap.Clear();
            cmbKhoNhap.SelectedIndex = -1;
            cmbNhaCungCap.SelectedIndex = -1;
            cmbHangHoa.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtNguoiNhap.Clear();
            txtGhiChu.Clear();
            lblTongTien.Text = "0";
            _chiTietNhapKhoList.Clear(); // Xóa danh sách chi tiết nhập kho
            UpdateDataGridView(); // Cập nhật lại DataGridView
        }

        private void fTaoNhapKho_Load(object sender, EventArgs e)
        {
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            txtMaPhieuNhap.Text = TaoMaPhieuNhapTuDong();
            if (_nguoiDung != null)
            {
                guna2HtmlLabel2.Text = $"Xin chào, {_nguoiDung.HoTen}";
            }
        }


        private void CmbNhaCungCap_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNhaCungCap.SelectedValue == null) return;

            var maNCC = cmbNhaCungCap.SelectedValue.ToString();
            LoadHangHoaTheoNCC(maNCC);
        }

        private void LoadHangHoaTheoNCC(string maNCC)
        {
            var hangHoaNCCBLL = new HangHoa_NCCBLL();
            var danhSachHangHoaNCC = hangHoaNCCBLL.LayTheoNCC(maNCC);

            // Chuyển đổi dữ liệu để hiển thị tên hàng và đơn giá
            var danhSachHienThi = danhSachHangHoaNCC.Select(hhNCC => new
            {
                MaHang = hhNCC.MaHang,
                TenHang = hhNCC.TenHang, // Replace with actual name if available
                GiaCungCap = hhNCC.GiaCungCap
            }).ToList();

            // Gán dữ liệu vào ComboBox hàng hóa
            cmbHangHoa.DataSource = null; // Clear previous data source
            cmbHangHoa.DataSource = danhSachHienThi;
            cmbHangHoa.DisplayMember = "TenHang";
            cmbHangHoa.ValueMember = "MaHang";
            cmbHangHoa.SelectedIndex = -1;

            // Xóa các sự kiện cũ để tránh bị gọi nhiều lần
            cmbHangHoa.SelectedIndexChanged -= CmbHangHoa_SelectedIndexChanged;
            cmbHangHoa.SelectedIndexChanged += CmbHangHoa_SelectedIndexChanged;

            // Hiển thị đơn giá của hàng hóa khi chọn
            void CmbHangHoa_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (cmbHangHoa.SelectedValue != null)
                {
                    var hangHoa = danhSachHienThi.FirstOrDefault(hh => hh.MaHang == cmbHangHoa.SelectedValue.ToString());
                    if (hangHoa != null)
                    {
                        txtDonGia.Text = hangHoa.GiaCungCap.ToString("N0");
                    }
                }
            }
        }

        private string TaoMaPhieuNhapTuDong()
        {
            try
            {
                // Retrieve the list of existing import receipts
                var danhSach = _nhapKhoBLL.GetAll();
                int soCuoi = 1;

                if (danhSach.Any())
                {
                    // Find the latest MaPhieuNhap
                    var maCuoi = danhSach.Max(p => p.MaPhieuNhap);

                    // Extract the numeric part of the code (assuming format PN###)
                    if (maCuoi.Length > 2 && int.TryParse(maCuoi.Substring(2), out int so))
                    {
                        soCuoi = so + 1;
                    }
                }

                // Generate the new code
                return $"PN{soCuoi:D3}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã phiếu nhập tự động: {ex.Message}");
                return null;
            }
        }


    }
}
