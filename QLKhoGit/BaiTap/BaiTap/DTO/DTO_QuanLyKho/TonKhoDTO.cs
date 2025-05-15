using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_QuanLyKho
{
    public class TonKhoDTO
    {
        public string MaTonKho { get; set; }
        public string MaHang { get; set; }
        public string MaKho { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public string TrangThai { get; set; } // Thêm thuộc tính TrangThai

        public decimal DonGia { get; set; } // Thêm thuộc tính DonGia
    }
}
