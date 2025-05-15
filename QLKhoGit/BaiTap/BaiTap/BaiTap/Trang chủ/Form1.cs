using BLL.BLL_Basic;
using DTO.DTO_QuanLyKho;
using System;
using BLL.BLL_QuanLyKho;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BaiTap.DanhMuc;
using BaiTap.Nhập_Kho;
using BaiTap.XuatKho;
using BaiTap.Báo_cáo;
using BaiTap.Hướng_dẫn;
using BaiTap.Đăng_ký;
using BaiTap.Đăng_nhập;
using DTO.DTO_DangkyDangnhap;
using BaiTap.Hệ_thống;
using System.Drawing.Drawing2D;

namespace BaiTap
{
    public partial class Form1 : Form
    {
        private readonly HangHoaBLL _hangHoaBLL;
        private readonly TonKhoBLL _tonKhoBLL;
        private readonly NhaCungCapBLL _nhaCungCapBLL;

        private NguoiDung _nguoiDung;


        public Form1(NguoiDung nguoiDung)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this._nguoiDung = nguoiDung;

            // Khởi tạo các BLL
            _hangHoaBLL = new HangHoaBLL();
            _tonKhoBLL = new TonKhoBLL();
            _nhaCungCapBLL = new NhaCungCapBLL();

            // Cấu hình DataGridView
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            guna2DataGridView1.ColumnHeadersHeight = 50;
            guna2DataGridView1.RowTemplate.Height = 30;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigureUI();
            LoadDashboardData();

            if (_nguoiDung == null)
            {
                guna2HtmlLabel2.Text = "Xin chào!";
                guna2PictureBox1.Image = null;
                return;
            }

            // Gán tên người dùng (nếu có)
            guna2HtmlLabel2.Text = !string.IsNullOrEmpty(_nguoiDung.HoTen)
                ? $"Xin chào, {_nguoiDung.HoTen}"
                : "Xin chào!";

            // Phân quyền: nếu không phải admin thì ẩn các chức năng quản lý
            if (_nguoiDung != null && _nguoiDung.MaVaiTro != 1)
            {
                quảnLýNgườiDùngToolStripMenuItem.Visible = false;
                báoCáoTồnKhoToolStripMenuItem.Visible = false;
                báoCáoNhậpXuấtTồnToolStripMenuItem.Visible = false;
                đăngKýToolStripMenuItem.Visible = false;
                đăngNhậpToolStripMenuItem.Visible = false;

                //hàngHóaVàGiáXuấtToolStripMenuItem.Visible = false;
            }
            hàngHóaVàGiáXuấtToolStripMenuItem.Visible = false;

            // Xử lý ảnh đại diện (nếu có)
            if (!string.IsNullOrEmpty(_nguoiDung.AnhDaiDien))
            {
                string fullPath = _nguoiDung.AnhDaiDien;

                if (!Path.IsPathRooted(fullPath))
                    fullPath = Path.Combine(Application.StartupPath, fullPath);

                if (File.Exists(fullPath))
                {
                    try
                    {
                        using (var stream = new MemoryStream(File.ReadAllBytes(fullPath)))
                        {
                            guna2PictureBox1.Image = Image.FromStream(stream);
                        }

                        guna2PictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                        GraphicsPath gp = new GraphicsPath();
                        gp.AddEllipse(0, 0, guna2PictureBox1.Width - 1, guna2PictureBox1.Height - 1);
                        guna2PictureBox1.Region = new Region(gp);
                    }
                    catch
                    {
                        guna2PictureBox1.Image = null;
                        guna2PictureBox1.Region = null;
                    }
                }
                else
                {
                    guna2PictureBox1.Image = null;
                    guna2PictureBox1.Region = null;
                }
            }
            else
            {
                guna2PictureBox1.Image = null;
                guna2PictureBox1.Region = null;
            }

        }



        private void ConfigureUI()
        {
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            tổngQuanToolStripMenuItem.ForeColor = ColorTranslator.FromHtml("#0078D7");

            menuStrip1.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                item.ForeColor = ColorTranslator.FromHtml("#000000");
                foreach (ToolStripItem subItem in item.DropDownItems)
                {
                    subItem.ForeColor = ColorTranslator.FromHtml("#000000");
                }
            }

            guna2Panel5.FillColor = ColorTranslator.FromHtml("#0099FF");
            guna2Panel6.FillColor = ColorTranslator.FromHtml("#008000");
            guna2Panel7.FillColor = ColorTranslator.FromHtml("#FF0000");
            guna2Panel8.FillColor = ColorTranslator.FromHtml("#FFA500");
            guna2Panel10.FillColor = ColorTranslator.FromHtml("#D3D3D3");
        }

        private void LoadDashboardData()
        {
            try
            {
                // 🧼 Reset nội dung cũ
                guna2HtmlLabel7.Text = "0";
                guna2HtmlLabel8.Text = "0";
                guna2HtmlLabel9.Text = "0";
                guna2HtmlLabel10.Text = "0";

                guna2DataGridView1.DataSource = null;
                chart1.Series.Clear();
                chart1.ChartAreas.Clear();

                // ✅ Hiển thị dữ liệu mới
                guna2HtmlLabel7.Text = _hangHoaBLL.LayTongSoHangHoa().ToString();
                guna2HtmlLabel8.Text = _tonKhoBLL.LayTongSoKho().ToString();
                guna2HtmlLabel9.Text = _tonKhoBLL.TinhTongGiaTriTonKho().ToString("C");
                guna2HtmlLabel10.Text = _nhaCungCapBLL.LayTongSoNhaCungCap().ToString();

                guna2DataGridView1.DataSource = _tonKhoBLL.LayDanhSachTonKho();
                ConfigureDataGridViewHeaders();

                LoadChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }

        private void ConfigureDataGridViewHeaders()
        {
            guna2DataGridView1.Columns["MaTonKho"].HeaderText = "Mã Tồn Kho";
            guna2DataGridView1.Columns["MaHang"].HeaderText = "Mã Hàng";
            guna2DataGridView1.Columns["MaKho"].HeaderText = "Mã Kho";
            guna2DataGridView1.Columns["SoLuong"].HeaderText = "Số Lượng";
            guna2DataGridView1.Columns["NgayCapNhat"].HeaderText = "Ngày Cập Nhật";
            guna2DataGridView1.Columns["DonGia"].HeaderText = "Đơn Giá";
        }

        private void LoadChart()
        {
            var duLieu = _tonKhoBLL.LaySoLieuNhapXuat30Ngay();

            if (duLieu == null || duLieu.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị biểu đồ.");
                return;
            }

            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            // Chart Area
            var area = new ChartArea("MainArea");
            area.AxisX.Title = "Ngày";
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.Enabled = false;
            area.AxisY.Title = "Số lượng";
            chart1.ChartAreas.Add(area);

            // Series Nhập
            var nhapSeries = new Series("Nhập")
            {
                ChartType = SeriesChartType.Column,
                XValueType = ChartValueType.String,
                IsValueShownAsLabel = true
            };

            // Series Xuất
            var xuatSeries = new Series("Xuất")
            {
                ChartType = SeriesChartType.Column,
                XValueType = ChartValueType.String,
                IsValueShownAsLabel = true
            };

            foreach (var item in duLieu)
            {
                nhapSeries.Points.AddXY(item.Ngay, item.Nhap);
                xuatSeries.Points.AddXY(item.Ngay, item.Xuat);
            }

            chart1.Series.Add(nhapSeries);
            chart1.Series.Add(xuatSeries);
        }


        private void ResetPanelColors()
        {
            tổngQuanToolStripMenuItem.ForeColor = Color.Gray;
        }

        private void tổngQuanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetPanelColors();
            tổngQuanToolStripMenuItem.ForeColor = ColorTranslator.FromHtml("#0078D7");
        }

        private void nhàCungCấpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new fDanhmucNhaCungCap(_nguoiDung);
            form.OnDataChanged += () => LoadDashboardData(); // lắng nghe thay đổi
            OpenForm(form);
        }

        private void hàngHóaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new fDanhmucHangHoa(_nguoiDung);
            form.OnDataChanged += () => LoadDashboardData(); // lắng nghe thay đổi
            OpenForm(form);
        }

        private void loạiHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new fDanhmucLoaiHang(_nguoiDung);
            form.OnDataChanged += () => LoadDashboardData(); // lắng nghe thay đổi
            OpenForm(form);
        }

        private void OpenForm(Form form)
        {
            this.Hide();
            form.FormClosed += (s, args) => this.Show();
            form.Show();
        }

        // Add this method to the Form1 class
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            // You can leave this method empty or add custom painting logic here
        }
        // Add this method to the Form1 class
        private void chart1_Click(object sender, EventArgs e)
        {
        }

        private void khoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new fDanhmucKho(_nguoiDung);
            form.OnDataChanged += () => LoadDashboardData(); // ✅ khi form con báo thay đổi
            OpenForm(form);
        }

        private void tạoPhiếuNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new fTaoNhapKho(_nguoiDung);
            form.OnDataChanged += () => LoadDashboardData(); // ✅ khi form con báo thay đổi
            OpenForm(form);

        }

        private void hàngHóaVàNhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fHangHoa_NCC());
        }

        private void danhSáchPhiếuNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new fDanhsachNhapKho(_nguoiDung);
            form.OnDataChanged += () => LoadDashboardData(); // ✅ khi form con báo thay đổi
            OpenForm(form);
        }

        private void tạoPhiếuXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var form = new fTaoPhieuXuat(_nguoiDung);
            form.OnDataChanged += () => LoadDashboardData(); // ✅ khi form con báo thay đổi
            OpenForm(form);
        }

        private void hàngHóaVàGiáXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fHangHoa_GiaXuat());
        }

        private void danhSáchPhiếuXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var form = new fDachsachXuatKho(_nguoiDung);
            form.OnDataChanged += () => LoadDashboardData(); // ✅ khi form con báo thay đổi
            OpenForm(form);
        }


        private void tồnKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new fDanhmucTonKho(_nguoiDung);
            form.OnDataChanged += () => LoadDashboardData(); // ✅ khi form con báo thay đổi
            OpenForm(form);
        }

        private void hướngDẫnSửDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fHuongDanSuDung());
        }

        private void báoCáoTồnKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fBaoCaoTonKho());
        }

        private void báoCáoNhậpXuấtTồnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fBaoCaoNhapXuatTon());
        }


        private void thôngTinPhầnMềmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fThongTinPhanMem());
        }

        private void liênHệHỗTrợToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fLienHeHoTro());
        }

        private void đăngKýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fDangKy());
        }

        private void đăngNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fDangNhap());
        }

        private void quảnLýNgườiDùngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenForm(new fQuanLyNguoiDung());
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void thoátChươngTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Ẩn form hiện tại
            this.Hide();

            // Mở lại form đăng nhập
            var loginForm = new fDangNhap();  // hoặc new fDangKy();
            loginForm.Show();

            // Khi form đăng nhập đóng thì thoát toàn bộ ứng dụng
            loginForm.FormClosed += (s, args) => this.Close();
        }
    }
}
