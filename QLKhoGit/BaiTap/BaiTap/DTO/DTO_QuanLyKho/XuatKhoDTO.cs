using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_QuanLyKho
{
    public class XuatKhoDTO
    {
        public string MaPhieuXuat { get; set; }
        public string MaKho { get; set; }
        public DateTime NgayXuat { get; set; }
        public string NguoiXuat { get; set; }
        public string KhachHang { get; set; }
        public decimal TongTien { get; set; }
        public string GhiChu { get; set; }
    }
}
