using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_QuanLyKho
{
    public class ChiTietXuatKhoDTO
    {
        public int MaCTXuat { get; set; }
        public string MaPhieuXuat { get; set; }
        public string MaHang { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
