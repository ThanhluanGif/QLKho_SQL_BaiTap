using DAL;
using DTO;
using DTO.DTO_Basic;
using System;
using System.Collections.Generic;

namespace BLL.BLL_Basic
{
    public class LoaiHangBLL
    {
        private readonly LoaiHangDAL _loaiHangDAL;

        public LoaiHangBLL()
        {
            _loaiHangDAL = new LoaiHangDAL();
        }

        public List<LoaiHangDTO> LayDanhSachLoaiHang()
        {
            return _loaiHangDAL.LayDanhSachLoaiHang();
        }

        public LoaiHangDTO LayLoaiHangTheoMa(string maLoai)
        {
            if (string.IsNullOrEmpty(maLoai))
            {
                throw new ArgumentException("Mã loại hàng không được để trống.");
            }

            return _loaiHangDAL.LayLoaiHangTheoMa(maLoai);
        }

        public void ThemLoaiHang(LoaiHangDTO loaiHang)
        {
            if (loaiHang == null)
            {
                throw new ArgumentNullException(nameof(loaiHang), "Thông tin loại hàng không được để trống.");
            }

            if (string.IsNullOrEmpty(loaiHang.MaLoai))
            {
                throw new ArgumentException("Mã loại hàng không được để trống.");
            }

            if (string.IsNullOrEmpty(loaiHang.TenLoai))
            {
                throw new ArgumentException("Tên loại hàng không được để trống.");
            }

            var existingLoaiHang = _loaiHangDAL.LayLoaiHangTheoMa(loaiHang.MaLoai);
            if (existingLoaiHang != null)
            {
                throw new Exception($"Mã loại hàng {loaiHang.MaLoai} đã tồn tại.");
            }

            _loaiHangDAL.ThemLoaiHang(loaiHang);
        }

        public void SuaLoaiHang(LoaiHangDTO loaiHang)
        {
            if (loaiHang == null)
            {
                throw new ArgumentNullException(nameof(loaiHang), "Thông tin loại hàng không được để trống.");
            }

            if (string.IsNullOrEmpty(loaiHang.TenLoai))
            {
                throw new ArgumentException("Tên loại hàng không được để trống.");
            }

            var existingLoaiHang = _loaiHangDAL.LayLoaiHangTheoMa(loaiHang.MaLoai);
            if (existingLoaiHang == null)
            {
                throw new Exception($"Không tìm thấy loại hàng với mã {loaiHang.MaLoai}.");
            }

            _loaiHangDAL.SuaLoaiHang(loaiHang);
        }

        public void XoaLoaiHang(string maLoai)
        {
            if (string.IsNullOrEmpty(maLoai))
            {
                throw new ArgumentException("Mã loại hàng không được để trống.");
            }

            var existingLoaiHang = _loaiHangDAL.LayLoaiHangTheoMa(maLoai);
            if (existingLoaiHang == null)
            {
                throw new Exception($"Không tìm thấy loại hàng với mã {maLoai}.");
            }

            _loaiHangDAL.XoaLoaiHang(maLoai);
        }

        public IEnumerable<LoaiHangDTO> TimKiemLoaiHangTheoTen(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException("Từ khóa tìm kiếm không được để trống.");
            }

            return _loaiHangDAL.TimKiemLoaiHangTheoTen(keyword);
        }
    }
}
