using DTO.DTO_DangkyDangnhap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL; // hoặc using BLL.BLL_DangkyDangnhap; tùy đúng namespace bạn đang đặt lớp
using BLL.BLL_DangNhapDangKy;
using Guna.UI2.WinForms;


namespace BaiTap.Hệ_thống
{
    public partial class fQuanLyNguoiDung : Form
    {
        private readonly NguoiDungBLL _bll = new NguoiDungBLL();
        private readonly VaiTroBLL _vaiTroBLL = new VaiTroBLL();
        public event Action OnDataChanged;


        public fQuanLyNguoiDung()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            cmbTrangThai.Items.Clear();
            cmbTrangThai.Items.Add("1 - Hoạt động");
            cmbTrangThai.Items.Add("0 - Khóa");
            cmbTrangThai.SelectedIndex = 0;

            LoadVaiTro();

            // Cấu hình DataGridView
            dgvNguoiDung.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNguoiDung.ColumnHeadersHeight = 50;
            dgvNguoiDung.RowTemplate.Height = 30;

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void LoadVaiTro()
        {
            var danhSachVaiTro = _vaiTroBLL.LayTatCaVaiTro();

            cmbVaiTro.DataSource = null; // Reset trước khi gán lại
            cmbVaiTro.DisplayMember = "TenVaiTro";
            cmbVaiTro.ValueMember = "MaVaiTro";
            cmbVaiTro.DataSource = danhSachVaiTro;
        }

        private void LoadData()
        {
            dgvNguoiDung.DataSource = _bll.LayTatCaNguoiDung();
        }

        private void fQuanLyNguoiDung_Load(object sender, EventArgs e)
        {
            LoadVaiTro();
            LoadData();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var nguoiDung = GetNguoiDungFromInput();
            if (_bll.DangKy(nguoiDung))
            {
                MessageBox.Show("Thêm thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            else MessageBox.Show("Tên đăng nhập đã tồn tại.");
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            var nguoiDung = GetNguoiDungFromInput();
            if (_bll.CapNhatNguoiDung(nguoiDung))
            {
                MessageBox.Show("Cập nhật thành công!");
                OnDataChanged?.Invoke();
                LoadData();
            }
            else MessageBox.Show("Không tìm thấy người dùng.");
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            var tenDangNhap = txtTenDangNhap.Text.Trim();
            if (MessageBox.Show("Xóa người dùng này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (_bll.XoaNguoiDung(tenDangNhap))
                {
                    MessageBox.Show("Đã xóa.");
                    OnDataChanged?.Invoke();
                    LoadData();
                }
                else MessageBox.Show("Không tìm thấy người dùng.");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            txtTenDangNhap.Clear();
            txtMatKhau.Clear();
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSoDienThoai.Clear();
            cmbTrangThai.SelectedIndex = 0;
            cmbVaiTro.SelectedIndex = -1;
            pictureBox1.Image = null;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            var tenDangNhap = txtTenDangNhap.Text.Trim();
            if (_bll.ResetMatKhau(tenDangNhap))
            {
                MessageBox.Show("Mật khẩu đã đặt lại.");
            }
            else MessageBox.Show("Không tìm thấy người dùng.");
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string sourcePath = ofd.FileName;
                string destFolder = Path.Combine(Application.StartupPath, "Images", "Users");
                Directory.CreateDirectory(destFolder);
                string fileName = Path.GetFileName(sourcePath);
                string destPath = Path.Combine(destFolder, fileName);

                File.Copy(sourcePath, destPath, true); // chỉ copy một lần ở đây

                using (var stream = new MemoryStream(File.ReadAllBytes(destPath)))
                {
                    pictureBox1.Image = Image.FromStream(stream);
                }

                pictureBox1.Tag = Path.Combine("Images", "Users", fileName); // đường dẫn tương đối
            }
        }

        private NguoiDung GetNguoiDungFromInput()
        {
            string anhPath = pictureBox1.Tag?.ToString() ?? "";

            return new NguoiDung
            {
                TenDangNhap = txtTenDangNhap.Text.Trim(),
                MatKhau = txtMatKhau.Text.Trim(),
                HoTen = txtHoTen.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                AnhDaiDien = anhPath,
                TrangThai = cmbTrangThai.SelectedItem.ToString().StartsWith("1"),
                NgayTao = DateTime.Now,
                MaVaiTro = cmbVaiTro.SelectedValue != null ? (int?)Convert.ToInt32(cmbVaiTro.SelectedValue) : null
            };
        }


        private void dgvNguoiDung_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvNguoiDung.Rows[e.RowIndex].Cells["TenDangNhap"].Value != null)
            {
                DataGridViewRow row = dgvNguoiDung.Rows[e.RowIndex];

                txtTenDangNhap.Text = row.Cells["TenDangNhap"].Value?.ToString();
                txtMatKhau.Text = row.Cells["MatKhau"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();

                bool trangThai = Convert.ToBoolean(row.Cells["TrangThai"].Value);
                cmbTrangThai.SelectedItem = trangThai ? "1 - Hoạt động" : "0 - Khóa";

                if (int.TryParse(row.Cells["MaVaiTro"].Value?.ToString(), out int maVaiTro))
                {
                    cmbVaiTro.SelectedValue = maVaiTro;
                }
                else
                {
                    cmbVaiTro.SelectedIndex = -1;
                }

                string anhPath = row.Cells["AnhDaiDien"].Value?.ToString();
                if (!string.IsNullOrEmpty(anhPath) && File.Exists(Path.Combine(Application.StartupPath, anhPath)))
                {
                    string fullPath = Path.Combine(Application.StartupPath, anhPath);
                    if (!string.IsNullOrEmpty(anhPath) && File.Exists(fullPath))
                    {
                        using (var stream = new MemoryStream(File.ReadAllBytes(fullPath)))
                        {
                            pictureBox1.Image = Image.FromStream(stream);
                        }
                        pictureBox1.Tag = fullPath;
                    }
                    else
                    {
                        pictureBox1.Image = null;
                        pictureBox1.Tag = null;
                    }
                }
                else
                {
                    pictureBox1.Image = null;
                    pictureBox1.Tag = null;
                }
            }
        }
    }
}
