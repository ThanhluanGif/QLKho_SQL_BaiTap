using DAL;
using DTO;
using System;
using System.Collections.Generic;

namespace BLL.BLL_Basic
{
    public class KhoBLL
    {
        private readonly KhoDAL _khoDAL;

        public KhoBLL()
        {
            _khoDAL = new KhoDAL();
        }

        public List<KhoDTO> LayDanhSachKho()
        {
            return _khoDAL.LayDanhSachKho();
        }

        public List<KhoDTO> LayDanhSachKhoDangHoatDong()
        {
            return _khoDAL.LayDanhSachKhoDangHoatDong();
        }

        public KhoDTO LayKhoTheoMa(string maKho)
        {
            if (string.IsNullOrEmpty(maKho))
            {
                throw new ArgumentException("Mã kho không được để trống.");
            }

            var kho = _khoDAL.LayKhoTheoMa(maKho);
            if (kho == null)
            {
                throw new Exception($"Không tìm thấy kho với mã {maKho}.");
            }

            return kho;
        }

        public void ThemKho(KhoDTO kho)
        {
            if (kho == null)
            {
                throw new ArgumentNullException(nameof(kho), "Thông tin kho không được để trống.");
            }

            if (string.IsNullOrEmpty(kho.MaKho))
            {
                throw new ArgumentException("Mã kho không được để trống.");
            }

            if (string.IsNullOrEmpty(kho.TenKho))
            {
                throw new ArgumentException("Tên kho không được để trống.");
            }

            if (kho.DienTich <= 0)
            {
                throw new ArgumentException("Diện tích kho phải lớn hơn 0.");
            }

            var existingKho = _khoDAL.LayKhoTheoMa(kho.MaKho);
            if (existingKho != null)
            {
                throw new Exception($"Mã kho {kho.MaKho} đã tồn tại.");
            }

            _khoDAL.ThemKho(kho);
        }

        public void SuaKho(KhoDTO kho)
        {
            if (kho == null)
            {
                throw new ArgumentNullException(nameof(kho), "Thông tin kho không được để trống.");
            }

            if (kho.DienTich <= 0)
            {
                throw new ArgumentException("Diện tích kho phải lớn hơn 0.");
            }

            var existingKho = _khoDAL.LayKhoTheoMa(kho.MaKho);
            if (existingKho == null)
            {
                throw new Exception($"Không tìm thấy kho với mã {kho.MaKho}.");
            }

            _khoDAL.SuaKho(kho);
        }

        public void XoaKho(string maKho)
        {
            if (string.IsNullOrEmpty(maKho))
            {
                throw new ArgumentException("Mã kho không được để trống.");
            }

            var existingKho = _khoDAL.LayKhoTheoMa(maKho);
            if (existingKho == null)
            {
                throw new Exception($"Không tìm thấy kho với mã {maKho}.");
            }

            _khoDAL.XoaKho(maKho);
        }

        public IEnumerable<KhoDTO> TimKiemKhoTheoTen(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException("Từ khóa tìm kiếm không được để trống.");
            }

            return _khoDAL.TimKiemKhoTheoTen(keyword);
        }
    }
}
