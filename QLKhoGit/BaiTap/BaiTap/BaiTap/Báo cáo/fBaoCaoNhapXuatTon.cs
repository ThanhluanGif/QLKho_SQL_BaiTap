using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTap.Báo_cáo
{
    public partial class fBaoCaoNhapXuatTon : Form
    {
        public fBaoCaoNhapXuatTon()
        {
            InitializeComponent();
        }

        private void fBaoCaoNhapXuatTon_Load(object sender, EventArgs e)
        {
            try
            {
                string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLyKho;Integrated Security=True";
                string query = "SELECT * FROM vBaoCaoNhapXuatTon";

                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.Fill(ds, "vBaoCaoNhapXuatTon");
                }

                ReportDocument rpt = new ReportDocument();
                rpt.Load(Application.StartupPath + @"\rptBaoCaoNhapXuatTon.rpt");

                rpt.SetDataSource(ds.Tables["vBaoCaoNhapXuatTon"]);
                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
