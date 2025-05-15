using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DAL.Helpers; // nhớ using cái namespace DatabaseHelper bạn đã tạo

namespace BaiTap.Báo_cáo
{
    public partial class fBaoCaoTonKho : Form
    {
        public fBaoCaoTonKho()
        {
            InitializeComponent();
        }

        private void fBaoCaoTonKho_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Tạo DataSet
                var ds = new Dataset.DataSetTonKho(); // bạn đang dùng DataSetTonKho

                // 2. Lấy dữ liệu từ Database
                string query = @"
    SELECT 
        tk.MaHang,
        hh.TenHang,
        tk.MaKho,
        k.TenKho,
        tk.SoLuong,
        tk.DonGia,
        (tk.SoLuong * tk.DonGia) AS ThanhTien
    FROM 
        TonKho tk
    INNER JOIN 
        HangHoa hh ON tk.MaHang = hh.MaHang
    INNER JOIN 
        Kho k ON tk.MaKho = k.MaKho
";

                using (var conn = DatabaseHelper.GetConnection())
                {
                    using (var cmd = DatabaseHelper.CreateCommand(query, conn))
                    {
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds.BaoCaoTonKho);
                        }
                    }
                }

                // 3. Load báo cáo
                ReportDocument rpt = new ReportDocument();
                rpt.Load(Application.StartupPath + @"\rptBaoCaoTonKho.rpt");

                // 4. Gán dữ liệu cho báo cáo
                rpt.SetDataSource(ds);

                // 5. Hiển thị
                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message);
            }
        }
    }
}
