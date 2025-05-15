using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class NhaCungCapDTO
    {
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string MaSoThue { get; set; }
        public string NguoiDaiDien { get; set; }
        public DateTime NgayTao { get; set; } // Thêm dòng này
        public bool TrangThai { get; set; }
    }
}
