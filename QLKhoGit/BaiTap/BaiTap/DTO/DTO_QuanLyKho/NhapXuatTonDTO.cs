using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.DTO_QuanLyKho
{
    public class NhapXuatTonDTO
    {
        public string MaHang { get; set; }          // Mã hàng hóa
        public DateTime Ngay { get; set; }          // Ngày thống kê
        public int Nhap { get; set; }               // Số lượng nhập
        public int Xuat { get; set; }               // Số lượng xuất
        public int Ton => Nhap - Xuat;              // Tồn kho (tự động tính)

        // Optional: Constructor
        public NhapXuatTonDTO() { }

        public NhapXuatTonDTO(string maHang, DateTime ngay, int nhap, int xuat)
        {
            MaHang = maHang;
            Ngay = ngay;
            Nhap = nhap;
            Xuat = xuat;
        }
    }
}
