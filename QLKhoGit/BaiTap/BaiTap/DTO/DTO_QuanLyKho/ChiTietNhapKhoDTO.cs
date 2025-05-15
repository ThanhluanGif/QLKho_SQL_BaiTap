using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_QuanLyKho
{
    public class ChiTietNhapKhoDTO
    {
        public int MaCTNhap { get; set; }
        public string MaPhieuNhap { get; set; }
        public string MaHang { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }

        // 🛠 BỔ SUNG THÊM:
        public string MaKho { get; set; }
    }
}
