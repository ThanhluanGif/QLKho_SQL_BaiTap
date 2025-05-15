using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_Basic
{
    public class NhomHangDTO
    {
        public string MaNhom { get; set; } // Mã nhóm hàng
        public string TenNhom { get; set; } // Tên nhóm hàng
        public DateTime NgayTao { get; set; } // Ngày tạo nhóm hàng
    }
}
