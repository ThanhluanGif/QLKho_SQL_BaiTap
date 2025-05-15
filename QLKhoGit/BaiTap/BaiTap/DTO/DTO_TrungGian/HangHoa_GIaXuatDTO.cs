using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_TrungGian
{
    public class HangHoa_GiaXuatDTO
    {
        public string MaGiaXuat { get; set; }      // Mã dòng giá xuất
        public string MaHang { get; set; }        // Mã hàng hóa
        public decimal DonGiaNhap { get; set; }   // Giá nhập gốc
        public decimal DonGiaXuat { get; set; }   // Giá xuất bán (calculated as DonGiaNhap * 1.1)
    }
}
