using BLL.BLL_Basic;
using BLL.BLL_TrungGian;
using DTO.DTO_TrungGian;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTap.DanhMuc
{
    public partial class fHangHoa_NCC : Form
    {
        public event Action OnDataChanged;

        public fHangHoa_NCC()
        {
            InitializeComponent();
            // Điều chỉnh kích thước cột
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;

            // Đặt chiều cao của hàng đầu tiên
            guna2DataGridView1.RowTemplate.Height = 30;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void fHangHoa_NCC_Load(object sender, EventArgs e)
        {
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            LoadComboBoxData();
            LoadDataGridView();
        }
        private void LoadComboBoxData()
        {
            // Load danh sách hàng hóa
            var hangHoaBLL = new HangHoaBLL();
            var danhSachHangHoa = hangHoaBLL.LayDanhSachHangHoa();
            cmbMaHang.DataSource = danhSachHangHoa;
            cmbMaHang.DisplayMember = "TenHang"; // Hiển thị tên hàng
            cmbMaHang.ValueMember = "MaHang";   // Giá trị là mã hàng
            cmbMaHang.SelectedIndex = -1;

            // Load danh sách nhà cung cấp
            var nhaCungCapBLL = new NhaCungCapBLL();
            var danhSachNhaCungCap = nhaCungCapBLL.LayDanhSachNhaCungCap();
            cmbMaNCC.DataSource = danhSachNhaCungCap;
            cmbMaNCC.DisplayMember = "TenNCC"; // Hiển thị tên nhà cung cấp
            cmbMaNCC.ValueMember = "MaNCC";   // Giá trị là mã nhà cung cấp
            cmbMaNCC.SelectedIndex = -1;

            cmbMaHang.MaxDropDownItems = 5;
            cmbMaHang.IntegralHeight = false;

            cmbMaNCC.MaxDropDownItems = 5;
            cmbMaNCC.IntegralHeight = false;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var maHang = cmbMaHang.SelectedValue?.ToString();
                var maNCC = cmbMaNCC.SelectedValue?.ToString();
                var ngayCungCap = dtpNgayCungCap.Value;
                var giaCungCap = decimal.Parse(txtGiaCungCap.Text);
                var ghiChu = txtGhiChu.Text;

                if (string.IsNullOrEmpty(maHang) || string.IsNullOrEmpty(maNCC))
                {
                    MessageBox.Show("Vui lòng chọn hàng hóa và nhà cung cấp.");
                    return;
                }

                var hangHoaNCC = new HangHoa_NCCDTO
                {
                    MaHang = maHang,
                    MaNCC = maNCC,
                    NgayCungCap = ngayCungCap,
                    GiaCungCap = giaCungCap,
                    GhiChu = ghiChu
                };

                var hangHoaNCCBLL = new HangHoa_NCCBLL();
                hangHoaNCCBLL.Them(hangHoaNCC);

                MessageBox.Show("Thêm thành công!");
                OnDataChanged?.Invoke();
                LoadDataGridView(); // Cập nhật lại DataGridView
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
                var maHang = cmbMaHang.SelectedValue?.ToString();
                var maNCC = cmbMaNCC.SelectedValue?.ToString();
                var ngayCungCap = dtpNgayCungCap.Value;
                var giaCungCap = decimal.Parse(txtGiaCungCap.Text);
                var ghiChu = txtGhiChu.Text;

                if (string.IsNullOrEmpty(maHang) || string.IsNullOrEmpty(maNCC))
                {
                    MessageBox.Show("Vui lòng chọn hàng hóa và nhà cung cấp.");
                    return;
                }

                var hangHoaNCC = new HangHoa_NCCDTO
                {
                    MaHang = maHang,
                    MaNCC = maNCC,
                    NgayCungCap = ngayCungCap,
                    GiaCungCap = giaCungCap,
                    GhiChu = ghiChu
                };

                var hangHoaNCCBLL = new HangHoa_NCCBLL();
                hangHoaNCCBLL.Sua(hangHoaNCC);

                MessageBox.Show("Cập nhật thành công!");
                OnDataChanged?.Invoke();
                LoadDataGridView(); // Cập nhật lại DataGridView
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
                var maHang = cmbMaHang.SelectedValue?.ToString();
                var maNCC = cmbMaNCC.SelectedValue?.ToString();

                if (string.IsNullOrEmpty(maHang) || string.IsNullOrEmpty(maNCC))
                {
                    MessageBox.Show("Vui lòng chọn hàng hóa và nhà cung cấp.");
                    return;
                }

                var hangHoaNCCBLL = new HangHoa_NCCBLL();
                hangHoaNCCBLL.Xoa(maHang, maNCC);

                MessageBox.Show("Xóa thành công!");
                OnDataChanged?.Invoke();
                LoadDataGridView(); // Cập nhật lại DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                // Xóa dữ liệu trong các TextBox
                txtGiaCungCap.Clear();
                txtGhiChu.Clear();

                // Đặt lại giá trị mặc định cho ComboBox
                cmbMaHang.SelectedIndex = -1;
                cmbMaNCC.SelectedIndex = -1;

                // Đặt lại giá trị mặc định cho DateTimePicker
                dtpNgayCungCap.Value = DateTime.Now;

                // Làm mới DataGridView
                LoadDataGridView();

                MessageBox.Show("Làm mới thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void LoadDataGridView()
        {
            var hangHoaNCCBLL = new HangHoa_NCCBLL();
            var danhSach = hangHoaNCCBLL.LayTatCa();

            // Kết hợp dữ liệu để hiển thị tên hàng và tên nhà cung cấp
            var hangHoaBLL = new HangHoaBLL();
            var nhaCungCapBLL = new NhaCungCapBLL();
            var danhSachHangHoa = hangHoaBLL.LayDanhSachHangHoa();
            var danhSachNhaCungCap = nhaCungCapBLL.LayDanhSachNhaCungCap();

            var data = danhSach.Select(hhNCC => new
            {
                MaHang = hhNCC.MaHang,
                TenHang = danhSachHangHoa.FirstOrDefault(hh => hh.MaHang == hhNCC.MaHang)?.TenHang ?? "Không xác định",
                MaNCC = hhNCC.MaNCC,
                TenNCC = danhSachNhaCungCap.FirstOrDefault(ncc => ncc.MaNCC == hhNCC.MaNCC)?.TenNCC ?? "Không xác định",
                NgayCungCap = hhNCC.NgayCungCap,
                GiaCungCap = hhNCC.GiaCungCap,
                GhiChu = hhNCC.GhiChu
            }).ToList();

            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.DataSource = data;
        }
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = guna2DataGridView1.Rows[e.RowIndex];

                // Lấy giá trị từ DataGridView
                cmbMaHang.SelectedValue = row.Cells["MaHang"].Value?.ToString();
                cmbMaNCC.SelectedValue = row.Cells["MaNCC"].Value?.ToString();
                dtpNgayCungCap.Value = Convert.ToDateTime(row.Cells["NgayCungCap"].Value);
                txtGiaCungCap.Text = row.Cells["GiaCungCap"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
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

                var hangHoaNCCBLL = new HangHoa_NCCBLL();
                var danhSach = hangHoaNCCBLL.LayTatCa();

                // Kết hợp dữ liệu để hiển thị tên hàng và tên nhà cung cấp
                var hangHoaBLL = new HangHoaBLL();
                var nhaCungCapBLL = new NhaCungCapBLL();
                var danhSachHangHoa = hangHoaBLL.LayDanhSachHangHoa();
                var danhSachNhaCungCap = nhaCungCapBLL.LayDanhSachNhaCungCap();

                var keywordLower = keyword.ToLower(); // Chuyển keyword về lowercase 1 lần

                var data = danhSach
                    .Where(hhNCC =>
                        (danhSachHangHoa.FirstOrDefault(hh => hh.MaHang == hhNCC.MaHang)?.TenHang?.ToLower().Contains(keywordLower) ?? false) ||
                        (danhSachNhaCungCap.FirstOrDefault(ncc => ncc.MaNCC == hhNCC.MaNCC)?.TenNCC?.ToLower().Contains(keywordLower) ?? false)
                    )
                    .Select(hhNCC => new
                    {
                        MaHang = hhNCC.MaHang,
                        TenHang = danhSachHangHoa.FirstOrDefault(hh => hh.MaHang == hhNCC.MaHang)?.TenHang ?? "Không xác định",
                        MaNCC = hhNCC.MaNCC,
                        TenNCC = danhSachNhaCungCap.FirstOrDefault(ncc => ncc.MaNCC == hhNCC.MaNCC)?.TenNCC ?? "Không xác định",
                        NgayCungCap = hhNCC.NgayCungCap,
                        GiaCungCap = hhNCC.GiaCungCap,
                        GhiChu = hhNCC.GhiChu
                    })
                    .ToList();

                if (data.Any())
                {
                    guna2DataGridView1.DataSource = null;
                    guna2DataGridView1.DataSource = data;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kết quả phù hợp.");
                    guna2DataGridView1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
