using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_QuanLyKho
{
    public class NhapKhoDTO
    {
        public string MaPhieuNhap { get; set; }
        public string MaNCC { get; set; }
        public string MaKho { get; set; }
        public DateTime NgayNhap { get; set; }
        public string NguoiNhap { get; set; }
        public decimal TongTien { get; set; }
        public string GhiChu { get; set; }

        // Add this property
        public List<ChiTietNhapKhoDTO> ChiTietNhapKho { get; set; }
    }
}
