using BLL.BLL_QuanLyKho;
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
using System.Windows.Forms;
using System.IO;
using DTO.DTO_DangkyDangnhap;

namespace BaiTap.XuatKho
{
    public partial class fDachsachXuatKho : Form
    {
        private readonly XuatKhoBLL _xuatKhoBLL;
        private NguoiDung _nguoiDung;
        public event Action OnDataChanged;


        public fDachsachXuatKho(NguoiDung nguoiDung)
        {
            InitializeComponent();
            _xuatKhoBLL = new XuatKhoBLL();
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

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string maPhieu = txtMaPhieu.Text.Trim();
                DateTime? tuNgay = dtpTuNgay.Value;
                DateTime? denNgay = dtpDenNgay.Value;

                // Call BLL to get data
                var danhSach = _xuatKhoBLL.TimKiemXuatKho(maPhieu, tuNgay, denNgay);

                if (danhSach == null || !danhSach.Any())
                {
                    MessageBox.Show("Không tìm thấy dữ liệu phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    guna2DataGridView1.DataSource = null;
                }
                else
                {
                    guna2DataGridView1.DataSource = danhSach;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm phiếu xuất: {ex.Message}");
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            var taoPhieuForm = new fTaoPhieuXuat(_nguoiDung);
            this.Hide();                   // Hide the current form
            taoPhieuForm.Show();           // Open the new form
            taoPhieuForm.FormClosed += (s, args) => this.Close(); // Close the current form when the new form is closed
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một phiếu xuất kho để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = guna2DataGridView1.SelectedRows[0];
            string maPhieuXuat = selectedRow.Cells["MaPhieuXuat"].Value.ToString();

            var confirmResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu xuất kho {maPhieuXuat}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                if (_xuatKhoBLL.XoaXuatKho(maPhieuXuat))
                {
                    MessageBox.Show("Xóa phiếu xuất kho thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OnDataChanged?.Invoke();
                    LoadData(); // Refresh the data
                }
                else
                {
                    MessageBox.Show("Xóa phiếu xuất kho thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.Landscape = true;

            printDocument.PrintPage += (s, ev) =>
            {
                Graphics g = ev.Graphics;
                Font titleFont = new Font("Arial", 16, FontStyle.Bold);
                Font headerFont = new Font("Arial", 11, FontStyle.Bold);
                Font contentFont = new Font("Arial", 10);
                Brush textBrush = Brushes.Black;

                int startX = 50;
                int startY = 100;
                int cellHeight = 40;
                int cellWidth = 100;

                // ===== VẼ TIÊU ĐỀ =====
                g.DrawString("DANH SÁCH PHIẾU NHẬP KHO", titleFont, textBrush, new PointF(300, 30));

                // ===== VẼ HEADER CÁC CỘT =====
                for (int i = 0; i < guna2DataGridView1.Columns.Count; i++)
                {
                    Rectangle rect = new Rectangle(startX + (i * cellWidth), startY, cellWidth, cellHeight);
                    g.FillRectangle(Brushes.LightGray, rect);
                    g.DrawRectangle(Pens.Black, rect);
                    g.DrawString(guna2DataGridView1.Columns[i].HeaderText, headerFont, textBrush, rect);
                }

                // ===== VẼ NỘI DUNG CÁC HÀNG =====
                startY += cellHeight;
                for (int i = 0; i < guna2DataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < guna2DataGridView1.Columns.Count; j++)
                    {
                        string value = guna2DataGridView1.Rows[i].Cells[j].Value?.ToString() ?? "";
                        Rectangle rect = new Rectangle(startX + (j * cellWidth), startY, cellWidth, cellHeight);
                        g.DrawRectangle(Pens.Black, rect);
                        g.DrawString(value, contentFont, textBrush, rect);
                    }
                    startY += cellHeight;
                }

                // ===== GHI CHÚ TRẠNG THÁI =====
                startY += 40;
                g.DrawString("Ghi chú trạng thái:", headerFont, textBrush, new PointF(startX, startY));
                string[] statuses =
                {
        "0 - Chờ duyệt / đang xử lý",
        "1 - Đã duyệt",
        "2 - Đã nhập kho",
        "3 - Hủy phiếu"
    };

                for (int i = 0; i < statuses.Length; i++)
                {
                    startY += 20;
                    g.DrawString(statuses[i], contentFont, textBrush, new PointF(startX + 20, startY));
                }
            };

            // MỞ FORM XEM TRƯỚC IN
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 1000,
                Height = 700
            };
            previewDialog.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (guna2DataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];

                int currentRow = 1;

                // ======= PHẦN TIÊU ĐỀ TRÊN CÙNG ==========
                worksheet.Cells[currentRow++, 1] = "KHO BẠC NHÀ NƯỚC";
                worksheet.Cells[currentRow++, 1] = "(TT số 77/2017/TT-BTC ngày 28/7/2017 của Bộ Tài Chính)";
                worksheet.Cells[currentRow++, 1] = "Mẫu số C6-12/NS";

                currentRow++; // Dòng trống

                worksheet.Cells[currentRow, 1] = "PHIẾU XUẤT KHO";
                var titleRange = worksheet.get_Range("A" + currentRow, "H" + currentRow);
                titleRange.Merge();
                titleRange.Font.Size = 16;
                titleRange.Font.Bold = true;
                titleRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                currentRow += 2;

                // ======= THÔNG TIN BỔ SUNG ==========
                worksheet.Cells[currentRow++, 1] = "Người nhận: ....................................................";
                worksheet.Cells[currentRow++, 1] = "Đơn vị: ...........................................     Xuất TK: .....................";
                worksheet.Cells[currentRow++, 1] = "Lý do xuất: .................................................................";
                worksheet.Cells[currentRow++, 1] = "Xuất tại kho: ..............................................................";

                currentRow += 1;

                // ======= TIÊU ĐỀ CỘT ==========
                for (int i = 0; i < guna2DataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[currentRow, i + 1] = guna2DataGridView1.Columns[i].HeaderText;
                    worksheet.Cells[currentRow, i + 1].Font.Bold = true;
                    worksheet.Cells[currentRow, i + 1].Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                }

                currentRow++;

                // ======= DỮ LIỆU ==========
                for (int i = 0; i < guna2DataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < guna2DataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[currentRow, j + 1] = guna2DataGridView1.Rows[i].Cells[j].Value?.ToString();
                        worksheet.Cells[currentRow, j + 1].Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    }
                    currentRow++;
                }

                // ======= CHỮ KÝ CUỐI TRANG ==========
                currentRow += 3;
                worksheet.Cells[currentRow++, 1] = $"Lập ngày {DateTime.Now:dd/MM/yyyy}";
                worksheet.Cells[currentRow++, 1] = "Người lập phiếu";
                worksheet.Cells[currentRow - 1, 4] = "Thủ kho";
                worksheet.Cells[currentRow - 1, 7] = "Người nhận hàng";

                // ======= FORMAT ==========
                worksheet.Columns.AutoFit();
                excelApp.Visible = true;

                MessageBox.Show("Xuất báo cáo thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi xuất báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearForm();
            MessageBox.Show("Dữ liệu đã được làm mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ClearForm()
        {
            txtMaPhieu.Clear();
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now;
        }

        private void fDachsachXuatKho_Load(object sender, EventArgs e)
        {
            LoadData();
            guna2Panel1.BackColor = ColorTranslator.FromHtml("#0078D7");
            guna2HtmlLabel1.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            if (_nguoiDung != null)
            {
                guna2HtmlLabel2.Text = $"Xin chào, {_nguoiDung.HoTen}";
            }
        }
        private void LoadData()
        {
            var danhSach = _xuatKhoBLL.GetAllWithDetails();  // 🛠 gọi GetAllWithDetails() mới
            if (danhSach == null || !danhSach.Any())
            {
                MessageBox.Show("Không có dữ liệu để hiển thị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                guna2DataGridView1.DataSource = null;
            }
            else
            {
                guna2DataGridView1.DataSource = danhSach;
            }
        }

        private void dtpTuNgay_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void txtMaPhieu_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSdt_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel7_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel9_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel13_Click(object sender, EventArgs e)
        {

        }

        private void dtpDenNgay_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel15_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0) // đảm bảo không click vào header
                {
                    var selectedRow = guna2DataGridView1.Rows[e.RowIndex];

                    txtMaPhieu.Text = selectedRow.Cells["MaPhieuXuat"].Value?.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn dòng: " + ex.Message);
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
