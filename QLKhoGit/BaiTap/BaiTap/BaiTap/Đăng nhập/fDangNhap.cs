using BLL;
using DAL.Entities.DangkyDangnhap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTap.Đăng_nhập
{
    public partial class fDangNhap : Form
    {
        private readonly NguoiDungBLL _nguoiDungBLL = new NguoiDungBLL();
        public fDangNhap()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;    
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                var bll = new NguoiDungBLL();
                var nguoiDung = bll.DangNhap(txtTenDangNhap.Text.Trim(), txtMatKhau.Text.Trim());

                if (nguoiDung != null)
                {
                    var mainForm = new Form1(nguoiDung);
                    this.Hide();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fDangNhap_Load(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = true;
            this.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
