using System;
using System.Collections.Generic;
using DTO.DTO_DangkyDangnhap; 
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BaiTap.Đăng_nhập;
using BLL;

namespace BaiTap.Đăng_ký
{
    public partial class fDangKy : Form
    {
        private string selectedImagePath = string.Empty;
        private readonly NguoiDungBLL _nguoiDungBLL = new NguoiDungBLL();
        public fDangKy()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Chọn ảnh đại diện";
            ofd.Filter = "Image Files|*.jpg;*.png;*.jpeg;*.gif";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = ofd.FileName;
                pictureBox1.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void btnDangnhap_Click(object sender, EventArgs e)
        {
            this.Hide();
            new fDangNhap().Show(); // Mở form đăng nhập nếu bạn có
        }

        private void btnDangky_Click(object sender, EventArgs e)
        {
            // Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text) || string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên đăng nhập và Mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Copy ảnh vào thư mục đích nếu có chọn
            string anhDaiDien = string.Empty;
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                string destFolder = Application.StartupPath + @"\Images\Users";
                Directory.CreateDirectory(destFolder);

                string destFile = Path.Combine(destFolder, Path.GetFileName(selectedImagePath));
                File.Copy(selectedImagePath, destFile, true);

                anhDaiDien = @"Images\Users\" + Path.GetFileName(selectedImagePath);
            }

            // Tạo DTO người dùng
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = txtTenDangNhap.Text.Trim(),
                MatKhau = txtMatKhau.Text.Trim(), // có thể mã hóa nếu cần
                HoTen = txtHoTen.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                SoDienThoai = txtSoDienThoai.Text.Trim(),
                AnhDaiDien = anhDaiDien,
                TrangThai = true,
                NgayTao = DateTime.Now,
                MaVaiTro = 2 // ✅ Mặc định là nhân viên
            };

            // Gọi BLL để lưu
            bool ketQua = _nguoiDungBLL.DangKy(nguoiDung);
            if (ketQua)
            {
                MessageBox.Show("Đăng ký thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                new fDangNhap().Show();
            }
            else
            {
                MessageBox.Show("Đăng ký thất bại. Tên đăng nhập có thể đã tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fDangKy_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Hiển thị * thay vì chữ khi nhập mật khẩu
            txtMatKhau.UseSystemPasswordChar = true;
            txtXacNhanMatKhau.UseSystemPasswordChar = true;
        }
    }
}
