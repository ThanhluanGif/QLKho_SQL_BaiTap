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
using BLL.BLL_Basic;
using System.Windows.Forms;
using System.Drawing.Printing;
using BLL.BLL_TrungGian;
using DAL.Entities.XuatKho;
using DTO.DTO_DangkyDangnhap;
using System.IO;

namespace BaiTap.XuatKho
{
    public partial class fTaoPhieuXuat : Form
    {
        private readonly XuatKhoBLL _xuatKhoBLL;
        private readonly List<ChiTietXuatKhoDTO> _chiTietXuatKhoList;
        private NguoiDung _nguoiDung;
        public event Action OnDataChanged;

        public fTaoPhieuXuat(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _xuatKhoBLL = new XuatKhoBLL();
            _chiTietXuatKhoList = new List<ChiTietXuatKhoDTO>();
            LoadComboBoxData();
            UpdateDataGridView();
            UpdateTongTien();
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
        private void LoadComboBoxData()
        {
            // Load danh sách hàng hóa
            var hangHoaBLL = new HangHoaBLL();
            var danhSachHangHoa = hangHoaBLL.LayDanhSachHangHoa();
            cmbHangHoa.DataSource = danhSachHangHoa;
            cmbHangHoa.DisplayMember = "TenHang"; // Assuming "TenHang" is the property for the name of the goods
            cmbHangHoa.ValueMember = "MaHang";   // Assuming "MaHang" is the property for the ID of the goods
            cmbHangHoa.SelectedIndex = -1;       // Clear selection

            // Attach the SelectedIndexChanged event
            cmbHangHoa.SelectedIndexChanged += cmbHangHoa_SelectedIndexChanged;

            // Load danh sách kho
            var khoBLL = new KhoBLL();
            var danhSachKho = khoBLL.LayDanhSachKho();
            cmbMaKho.DataSource = danhSachKho;
            cmbMaKho.DisplayMember = "TenKho";
            cmbMaKho.ValueMember = "MaKho";
            cmbMaKho.SelectedIndex = -1;
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

        private void fTaoPhieuXuat_Load(object sender, EventArgs e)
        {
            txtMaPhieuXuat.Text = TaoMaPhieuXuatTuDong();
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            if (_nguoiDung != null)
            {
                guna2HtmlLabel2.Text = $"Xin chào, {_nguoiDung.HoTen}";
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var maHang = cmbHangHoa.SelectedValue?.ToString();
                var soLuong = int.Parse(txtSoLuong.Text);
                var donGia = decimal.Parse(txtDonGia.Text);

                if (string.IsNullOrEmpty(maHang))
                {
                    MessageBox.Show("Vui lòng chọn hàng hóa.");
                    return;
                }

                var chiTiet = new ChiTietXuatKhoDTO
                {
                    MaHang = maHang,
                    SoLuong = soLuong,
                    DonGia = donGia,
                    ThanhTien = soLuong * donGia
                };

                _chiTietXuatKhoList.Add(chiTiet);
                UpdateDataGridView();
                UpdateTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ClearForm();
            try
            {
                if (guna2DataGridView1.SelectedRows.Count > 0)
                {
                    var selectedRow = guna2DataGridView1.SelectedRows[0];
                    var maHang = selectedRow.Cells["MaHang"].Value.ToString();

                    var itemToRemove = _chiTietXuatKhoList.FirstOrDefault(x => x.MaHang == maHang);
                    if (itemToRemove != null)
                    {
                        _chiTietXuatKhoList.Remove(itemToRemove);
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
                var maKho = cmbMaKho.SelectedValue?.ToString();
                var maKhachHang = txtKhachHang.Text.Trim();

                if (string.IsNullOrEmpty(txtMaPhieuXuat.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phiếu xuất.");
                    return;
                }

                if (string.IsNullOrEmpty(maKho) || string.IsNullOrEmpty(maKhachHang))
                {
                    MessageBox.Show("Vui lòng chọn kho xuất và khách hàng.");
                    return;
                }

                if (_chiTietXuatKhoList == null || !_chiTietXuatKhoList.Any())
                {
                    MessageBox.Show("Danh sách chi tiết xuất kho không được để trống.");
                    return;
                }

                var xuatKho = new XuatKhoDTO
                {
                    MaPhieuXuat = txtMaPhieuXuat.Text,
                    MaKho = maKho,
                    KhachHang = maKhachHang,
                    NgayXuat = dtpNgayXuat.Value,
                    NguoiXuat = txtNguoiXuat.Text,
                    TongTien = _chiTietXuatKhoList.Sum(x => x.ThanhTien),
                    GhiChu = txtGhiChu.Text
                };

                var result = _xuatKhoBLL.ThemXuatKho(xuatKho);
                if (string.IsNullOrEmpty(xuatKho.MaPhieuXuat))
                {
                    xuatKho.MaPhieuXuat = TaoMaPhieuXuatTuDong();
                }
                var chiTietXuatKhoBLL = new ChiTietXuatKhoBLL();
                foreach (var chiTiet in _chiTietXuatKhoList)
                {
                    chiTiet.MaCTXuat = LayMaChiTietXuatTuDong(); // ⚡ Gán mã tự động vào
                    chiTiet.MaPhieuXuat = xuatKho.MaPhieuXuat;    // ⚡ Gán mã phiếu xuất
                    chiTietXuatKhoBLL.ThemChiTietXuatKho(chiTiet);
                }


                if (result)
                {
                    // 🔥 BỔ SUNG: Cập nhật tồn kho sau khi lưu phiếu xuất
                    var tonKhoBLL = new TonKhoBLL();
                    foreach (var chiTiet in _chiTietXuatKhoList)
                    {
                        tonKhoBLL.CapNhatTonKhoKhiXuat(chiTiet.MaHang, maKho, chiTiet.SoLuong);
                    }

                    MessageBox.Show("Lưu phiếu xuất kho và cập nhật tồn kho thành công!");
                    OnDataChanged?.Invoke();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Lưu phiếu xuất kho thất bại. Vui lòng thử lại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private int LayMaChiTietXuatTuDong()
        {
            try
            {
                var chiTietBLL = new ChiTietXuatKhoBLL();
                var danhSach = chiTietBLL.GetAll(); // Lấy tất cả chi tiết hiện tại

                if (danhSach == null || !danhSach.Any())
                    return 1; // Nếu chưa có dòng nào thì bắt đầu từ 1

                int soLonNhat = danhSach.Max(x => x.MaCTXuat);
                return soLonNhat + 1; // Lấy số lớn nhất + 1
            }
            catch
            {
                return 1; // Nếu lỗi thì mặc định là 1
            }
        }





        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocument_PrintPage;

                PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog
                {
                    Document = printDocument
                };

                // Show print preview dialog
                printPreviewDialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in phiếu xuất: {ex.Message}");
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                // Get export receipt details
                string maPhieuXuat = txtMaPhieuXuat.Text;
                string ngayXuat = dtpNgayXuat.Value.ToString("dd/MM/yyyy");
                string nguoiXuat = txtNguoiXuat.Text;
                string tongTien = lblTongTien.Text;

                // Draw export receipt content
                Graphics g = e.Graphics;
                Font font = new Font("Arial", 12);
                float y = 20;

                g.DrawString("PHIẾU XUẤT KHO", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, 100, y);
                y += 40;

                g.DrawString($"Mã Phiếu Xuất: {maPhieuXuat}", font, Brushes.Black, 20, y);
                y += 30;
                g.DrawString($"Ngày Xuất: {ngayXuat}", font, Brushes.Black, 20, y);
                y += 30;
                g.DrawString($"Người Xuất: {nguoiXuat}", font, Brushes.Black, 20, y);
                y += 30;
                g.DrawString($"Tổng Tiền: {tongTien} VND", font, Brushes.Black, 20, y);
                y += 40;

                g.DrawString("Chi Tiết Xuất Kho:", font, Brushes.Black, 20, y);
                y += 30;

                // Draw export receipt details
                foreach (var chiTiet in _chiTietXuatKhoList)
                {
                    g.DrawString($"- Mã Hàng: {chiTiet.MaHang}, Số Lượng: {chiTiet.SoLuong}, Đơn Giá: {chiTiet.DonGia}, Thành Tiền: {chiTiet.ThanhTien}", font, Brushes.Black, 20, y);
                    y += 20;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi vẽ nội dung phiếu xuất: {ex.Message}");
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UpdateDataGridView()
        {
            var hangHoaBLL = new HangHoaBLL();
            var danhSachHangHoa = hangHoaBLL.LayDanhSachHangHoa();

            var data = _chiTietXuatKhoList.Select(ct => new
            {
                MaHang = ct.MaHang,
                TenHang = danhSachHangHoa.FirstOrDefault(hh => hh.MaHang == ct.MaHang)?.TenHang ?? "Không xác định",
                SoLuong = ct.SoLuong,
                DonGia = ct.DonGia,
                ThanhTien = ct.ThanhTien
            }).ToList();

            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.DataSource = data;
        }

        private void UpdateTongTien()
        {
            lblTongTien.Text = _chiTietXuatKhoList.Sum(x => x.ThanhTien).ToString("N0");
        }

        private void ClearForm()
        {
            txtMaPhieuXuat.Clear();
            cmbMaKho.SelectedIndex = -1;
            txtKhachHang.Clear();
            cmbHangHoa.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtNguoiXuat.Clear();
            txtGhiChu.Clear();
            lblTongTien.Text = "0";
            _chiTietXuatKhoList.Clear();
            UpdateDataGridView();
        }

        private void cmbHangHoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbHangHoa.SelectedValue != null)
                {
                    // Get the selected product's MaHang
                    var selectedMaHang = cmbHangHoa.SelectedValue.ToString();

                    // Retrieve the product details from HangHoa_GiaXuatBLL
                    var hangHoaGiaXuatBLL = new HangHoa_GiaXuatBLL();
                    var danhSachGiaXuat = hangHoaGiaXuatBLL.GetAll();

                    // Find the selected product's selling price
                    var selectedGiaXuat = danhSachGiaXuat.FirstOrDefault(gx => gx.MaHang == selectedMaHang);

                    if (selectedGiaXuat != null)
                    {
                        // Display the selling price (Giá Xuất) in the txtDonGia TextBox
                        txtDonGia.Text = selectedGiaXuat.DonGiaXuat.ToString("N0"); // Format as a number with thousand separators
                    }
                    else
                    {
                        txtDonGia.Clear(); // Clear the field if no selling price is found
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi truy xuất giá xuất: {ex.Message}");
            }
        }

        private string TaoMaPhieuXuatTuDong()
        {
            try
            {
                var danhSach = _xuatKhoBLL.GetAll();  // 👉 LẤY DỮ LIỆU CHUẨN XuatKhoDTO

                int soCuoi = 1;

                if (danhSach.Any())
                {
                    var danhSachSo = new List<int>();

                    foreach (var item in danhSach)
                    {
                        if (!string.IsNullOrEmpty(item.MaPhieuXuat) && item.MaPhieuXuat.Length > 2)
                        {
                            var phanSo = item.MaPhieuXuat.Substring(2);
                            if (int.TryParse(phanSo, out int soParsed))
                            {
                                danhSachSo.Add(soParsed);
                            }
                        }
                    }

                    if (danhSachSo.Any())
                    {
                        soCuoi = danhSachSo.Max() + 1;
                    }
                }

                return $"PX{soCuoi:D3}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã phiếu xuất tự động: {ex.Message}");
                return null;
            }
        }

    }
}
