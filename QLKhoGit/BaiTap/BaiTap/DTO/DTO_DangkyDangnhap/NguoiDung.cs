using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_DangkyDangnhap
{
    public class NguoiDung
    {
        public int MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string AnhDaiDien { get; set; }
        public bool TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public int? MaVaiTro { get; set; }

    }

}
