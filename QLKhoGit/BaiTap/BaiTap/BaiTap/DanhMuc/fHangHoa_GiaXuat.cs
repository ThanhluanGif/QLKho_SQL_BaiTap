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
    public partial class fHangHoa_GiaXuat : Form
    {
        public event Action OnDataChanged;

        public fHangHoa_GiaXuat()
        {
            InitializeComponent();
            // Điều chỉnh kích thước cột
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;

            // Đặt chiều cao của hàng đầu tiên
            guna2DataGridView1.RowTemplate.Height = 30;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void fHangHoa_GiaXuat_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadCmbHangHoa();
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            txtMaGiaXuat.Text = TaoMaGiaXuatTuDong(); // Tạo mã giá xuất tự động

        }
        private string TaoMaGiaXuatTuDong()
        {
            try
            {
                // Retrieve the list of existing records
                var bll = new HangHoa_GiaXuatBLL();
                var danhSachGiaXuat = bll.GetAll();

                int soCuoi = 1;

                if (danhSachGiaXuat.Any())
                {
                    // Find the latest MaGiaXuat
                    var maCuoi = danhSachGiaXuat.Max(p => p.MaGiaXuat);

                    // Extract the numeric part of the code (assuming format GX###)
                    if (maCuoi.Length > 2 && int.TryParse(maCuoi.Substring(2), out int so))
                    {
                        soCuoi = so + 1;
                    }
                }

                // Generate the new code
                return $"GX{soCuoi:D3}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo mã giá xuất tự động: {ex.Message}");
                return null;
            }
        }


        private void LoadCmbHangHoa()
        {
            try
            {
                // Create an instance of HangHoaBLL to retrieve the list of products
                var hangHoaBLL = new HangHoaBLL();
                var danhSachHangHoa = hangHoaBLL.LayDanhSachHangHoa();

                // Bind the data to the ComboBox
                cmbMaHang.DataSource = danhSachHangHoa;
                cmbMaHang.DisplayMember = "TenHang"; // Assuming "TenHang" is the property for the product name
                cmbMaHang.ValueMember = "MaHang";   // Assuming "MaHang" is the property for the product ID
                cmbMaHang.SelectedIndex = -1;       // Clear the selection

                // Attach the SelectedIndexChanged event
                cmbMaHang.SelectedIndexChanged += cmbMaHang_SelectedIndexChanged;

                cmbMaHang.MaxDropDownItems = 5;
                cmbMaHang.IntegralHeight = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách hàng hóa: {ex.Message}");
            }
        }


        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                var keyword = txtTimKiem.Text.Trim().ToLower();

                var bllGiaXuat = new HangHoa_GiaXuatBLL();
                var danhSachGiaXuat = bllGiaXuat.GetAll();

                var bllHangHoa = new HangHoaBLL();
                var danhSachHangHoa = bllHangHoa.LayDanhSachHangHoa();

                var result = from giaXuat in danhSachGiaXuat
                             join hangHoa in danhSachHangHoa on giaXuat.MaHang equals hangHoa.MaHang
                             where giaXuat.MaGiaXuat.ToLower().Contains(keyword)
                                || giaXuat.MaHang.ToLower().Contains(keyword)
                                || hangHoa.TenHang.ToLower().Contains(keyword)
                             select new
                             {
                                 giaXuat.MaGiaXuat,
                                 giaXuat.MaHang,
                                 TenHang = hangHoa.TenHang,
                                 giaXuat.DonGiaNhap,
                                 giaXuat.DonGiaXuat
                             };

                var listResult = result.ToList();

                if (listResult.Any())
                {
                    guna2DataGridView1.DataSource = listResult;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy kết quả phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Load lại toàn bộ nếu không tìm thấy
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm giá xuất: {ex.Message}");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMaHang.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn mặt hàng.");
                    return;
                }

                decimal giaNhap = decimal.TryParse(txtGiaNhap.Text, out var gn) ? gn : 0;
                decimal giaXuat = decimal.TryParse(txtGiaXuat.Text, out var gx) ? gx : 0;

                var giaXuatDTO = new HangHoa_GiaXuatDTO
                {
                    MaGiaXuat = txtMaGiaXuat.Text.Trim(),
                    MaHang = cmbMaHang.SelectedValue.ToString(),
                    DonGiaNhap = giaNhap,
                    DonGiaXuat = giaXuat  // ✅ Ghi luôn giá xuất tính đúng
                };

                var bll = new HangHoa_GiaXuatBLL();
                bll.ThemGiaXuat(giaXuatDTO);

                MessageBox.Show("Thêm giá xuất thành công!");
                OnDataChanged?.Invoke();
                ClearForm();
                txtMaGiaXuat.Text = TaoMaGiaXuatTuDong();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm giá xuất: {ex.Message}");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var giaXuat = new HangHoa_GiaXuatDTO
                {
                    MaGiaXuat = txtMaGiaXuat.Text.Trim(),
                    MaHang = cmbMaHang.SelectedValue?.ToString(),
                    DonGiaNhap = decimal.Parse(txtGiaNhap.Text)
                };

                var bll = new HangHoa_GiaXuatBLL();
                bll.SuaGiaXuat(giaXuat);

                MessageBox.Show("Cập nhật giá xuất thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật giá xuất: {ex.Message}");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var maGiaXuat = txtMaGiaXuat.Text.Trim();

                if (string.IsNullOrEmpty(maGiaXuat))
                {
                    MessageBox.Show("Vui lòng chọn giá xuất để xóa.");
                    return;
                }

                var bll = new HangHoa_GiaXuatBLL();
                bll.XoaGiaXuat(maGiaXuat);

                MessageBox.Show("Xóa giá xuất thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa giá xuất: {ex.Message}");
            }
        }
        private void LoadData()
        {
            try
            {
                var bll = new HangHoa_GiaXuatBLL();
                var danhSachGiaXuat = bll.GetAll();

                var hangHoaBLL = new HangHoaBLL();
                var danhSachHangHoa = hangHoaBLL.LayDanhSachHangHoa();

                var data = from giaXuat in danhSachGiaXuat
                           join hangHoa in danhSachHangHoa on giaXuat.MaHang equals hangHoa.MaHang
                           select new
                           {
                               giaXuat.MaGiaXuat,
                               giaXuat.MaHang,
                               TenHang = hangHoa.TenHang,
                               DonGiaNhap = giaXuat.DonGiaNhap,
                               DonGiaXuat = giaXuat.DonGiaXuat // ✅ Phải lấy giá xuất chuẩn theo database
                           };

                guna2DataGridView1.DataSource = data.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }


        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadData();
        }

        private void ClearForm()
        {
            txtMaGiaXuat.Clear();
            cmbMaHang.SelectedIndex = -1;
            txtGiaNhap.Clear();
            txtGiaXuat.Clear();
            txtTimKiem.Clear();
            nudGiaXuat.Value = 0; // Reset the NumericUpDown value
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is selected
            {
                var row = guna2DataGridView1.Rows[e.RowIndex];

                txtMaGiaXuat.Text = row.Cells["MaGiaXuat"].Value?.ToString();
                cmbMaHang.SelectedValue = row.Cells["MaHang"].Value?.ToString();
                txtGiaNhap.Text = row.Cells["DonGiaNhap"].Value?.ToString();
                txtGiaXuat.Text = row.Cells["DonGiaXuat"].Value?.ToString();
                // ✅ Tính lại tỷ lệ phần trăm cho nudGiaXuat
                if (decimal.TryParse(row.Cells["DonGiaNhap"].Value?.ToString(), out var giaNhap) &&
                    decimal.TryParse(row.Cells["DonGiaXuat"].Value?.ToString(), out var giaXuat) &&
                    giaNhap > 0)
                {
                    decimal tyLe = ((giaXuat - giaNhap) / giaNhap) * 100;
                    nudGiaXuat.Value = Math.Min(Math.Max((decimal)Math.Round(tyLe, 2), nudGiaXuat.Minimum), nudGiaXuat.Maximum); // Giới hạn trong min/max
                }
                else
                {
                    nudGiaXuat.Value = 0;
                }
            }
        }

        private void cmbMaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMaHang.SelectedValue != null)
                {
                    var selectedMaHang = cmbMaHang.SelectedValue.ToString();
                    var hangHoaNCCBLL = new HangHoa_NCCBLL();
                    var danhSachHangHoaNCC = hangHoaNCCBLL.LayTatCa();
                    var selectedHangHoaNCC = danhSachHangHoaNCC.FirstOrDefault(hh => hh.MaHang == selectedMaHang);

                    if (selectedHangHoaNCC != null)
                    {
                        decimal giaNhap = selectedHangHoaNCC.GiaCungCap;
                        txtGiaNhap.Text = giaNhap.ToString("N0");

                        // Gọi tính giá xuất ngay sau khi load giá nhập
                        TinhGiaXuatTuGiaNhapVaTiLe();
                    }
                    else
                    {
                        txtGiaNhap.Clear();
                        txtGiaXuat.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi truy xuất đơn giá nhập và tính giá xuất: {ex.Message}");
            }
        }

        private void nudGiaXuat_ValueChanged(object sender, EventArgs e)
        {
            TinhGiaXuatTuGiaNhapVaTiLe();
        }

        private void TinhGiaXuatTuGiaNhapVaTiLe()
        {
            try
            {
                decimal giaNhap = decimal.TryParse(txtGiaNhap.Text, out var gn) ? gn : 0;
                decimal tiLePhanTram = nudGiaXuat.Value;

                if (giaNhap > 0)
                {
                    decimal giaXuat = giaNhap + (giaNhap * tiLePhanTram / 100);
                    txtGiaXuat.Text = giaXuat.ToString("N0"); // Format đẹp có dấu phẩy ngăn cách
                }
                else
                {
                    txtGiaXuat.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tính giá xuất: {ex.Message}");
            }
        }
    }
}
