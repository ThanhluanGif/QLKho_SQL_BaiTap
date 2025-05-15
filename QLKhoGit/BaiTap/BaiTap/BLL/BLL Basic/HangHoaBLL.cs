using DAL;
using DTO;
using System;
using System.Collections.Generic;

namespace BLL.BLL_Basic
{
    public class HangHoaBLL
    {
        private readonly HangHoaDAL _hangHoaDAL;

        public HangHoaBLL()
        {
            _hangHoaDAL = new HangHoaDAL();
        }

        public List<HangHoaDTO> LayDanhSachHangHoa()
        {
            return _hangHoaDAL.LayDanhSachHangHoa();
        }

        public void ThemHangHoa(HangHoaDTO hangHoa)
        {
            if (hangHoa == null)
            {
                throw new ArgumentNullException(nameof(hangHoa), "Thông tin hàng hóa không được để trống.");
            }

            if (string.IsNullOrEmpty(hangHoa.MaHang))
            {
                throw new ArgumentException("Mã hàng không được để trống.");
            }

            if (string.IsNullOrEmpty(hangHoa.TenHang))
            {
                throw new ArgumentException("Tên hàng không được để trống.");
            }

            var existingHangHoa = _hangHoaDAL.LayHangHoaTheoMa(hangHoa.MaHang);
            if (existingHangHoa != null)
            {
                throw new Exception($"Mã hàng {hangHoa.MaHang} đã tồn tại.");
            }

            _hangHoaDAL.ThemHangHoa(hangHoa);
        }

        public void SuaHangHoa(HangHoaDTO hangHoa)
        {
            if (hangHoa == null)
            {
                throw new ArgumentNullException(nameof(hangHoa), "Thông tin hàng hóa không được để trống.");
            }

            var existingHangHoa = _hangHoaDAL.LayHangHoaTheoMa(hangHoa.MaHang);
            if (existingHangHoa == null)
            {
                throw new Exception($"Không tìm thấy hàng hóa với mã {hangHoa.MaHang}.");
            }

            _hangHoaDAL.SuaHangHoa(hangHoa);
        }

        public void XoaHangHoa(string maHang)
        {
            if (string.IsNullOrEmpty(maHang))
            {
                throw new ArgumentException("Mã hàng không được để trống.");
            }

            var existingHangHoa = _hangHoaDAL.LayHangHoaTheoMa(maHang);
            if (existingHangHoa == null)
            {
                throw new Exception($"Không tìm thấy hàng hóa với mã {maHang}.");
            }

            _hangHoaDAL.XoaHangHoa(maHang);
        }

        public int LayTongSoHangHoa()
        {
            return _hangHoaDAL.LayDanhSachHangHoa().Count;
        }

        public IEnumerable<HangHoaDTO> SearchByName(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException("Từ khóa tìm kiếm không được để trống.", nameof(keyword));
            }

            return _hangHoaDAL.SearchByName(keyword);
        }
    }
}
