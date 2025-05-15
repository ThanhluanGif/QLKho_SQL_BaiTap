using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HangHoaDTO
    {
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        public string MaLoai { get; set; }
        public string DonViTinh { get; set; }
        public string XuatXu { get; set; }
        public int? BaoHanh { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; } // Thêm dòng này
    }
}
