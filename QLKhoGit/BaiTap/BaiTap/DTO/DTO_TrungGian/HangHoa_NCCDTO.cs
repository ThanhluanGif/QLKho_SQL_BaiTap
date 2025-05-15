using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_TrungGian
{
    public class HangHoa_NCCDTO
    {
        public string MaHang { get; set; } // Mã hàng hóa
        public string MaNCC { get; set; } // Mã nhà cung cấp
        public DateTime NgayCungCap { get; set; } // Ngày cung cấp
        public decimal GiaCungCap { get; set; } // Giá cung cấp
        public string GhiChu { get; set; } // Ghi chú (nếu có)

        public string TenHang { get; set; } // Add this property
    }
}
