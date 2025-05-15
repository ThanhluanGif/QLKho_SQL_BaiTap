using DAL;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BLL.BLL_QuanLyKho
{
    public class ChiTietNhapKhoBLL
    {
        private readonly ChiTietNhapKhoDAL _dal;

        public ChiTietNhapKhoBLL()
        {
            _dal = new ChiTietNhapKhoDAL();
        }

        public List<ChiTietNhapKhoDTO> GetAll()
        {
            try
            {
                var dsChiTiet = _dal.GetAll();
                foreach (var chiTiet in dsChiTiet)
                {
                    chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                }
                return dsChiTiet;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chi tiết nhập kho: " + ex.Message);
            }
        }

        public ChiTietNhapKhoDTO GetById(int maCTNhap)
        {
            try
            {
                var result = _dal.GetById(maCTNhap);
                if (result == null)
                {
                    throw new Exception("Không tìm thấy chi tiết nhập kho với mã: " + maCTNhap);
                }
                result.ThanhTien = result.SoLuong * result.DonGia;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết nhập kho: " + ex.Message);
            }
        }

        public IEnumerable<ChiTietNhapKhoDTO> GetByMaPhieuNhap(string maPhieuNhap)
        {
            try
            {
                var dsChiTiet = _dal.GetByMaPhieuNhap(maPhieuNhap);
                foreach (var chiTiet in dsChiTiet)
                {
                    chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                }
                return dsChiTiet;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chi tiết nhập kho theo mã phiếu: " + ex.Message);
            }
        }

        public bool ThemChiTietNhapKho(ChiTietNhapKhoDTO chiTiet)
        {
            try
            {
                if (string.IsNullOrEmpty(chiTiet.MaPhieuNhap) || string.IsNullOrEmpty(chiTiet.MaHang))
                {
                    throw new Exception("Mã phiếu nhập hoặc mã hàng không được để trống.");
                }
                if (chiTiet.SoLuong <= 0 || chiTiet.DonGia <= 0)
                {
                    throw new Exception("Số lượng và đơn giá phải lớn hơn 0.");
                }

                chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                return _dal.ThemChiTietNhapKho(chiTiet);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm chi tiết nhập kho: " + ex.Message);
            }
        }

        public bool SuaChiTietNhapKho(ChiTietNhapKhoDTO chiTiet)
        {
            try
            {
                var existing = _dal.GetById(chiTiet.MaCTNhap);
                if (existing == null)
                {
                    throw new Exception("Không tìm thấy chi tiết nhập kho để cập nhật.");
                }
                if (chiTiet.SoLuong <= 0 || chiTiet.DonGia <= 0)
                {
                    throw new Exception("Số lượng và đơn giá phải lớn hơn 0.");
                }

                chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                return _dal.SuaChiTietNhapKho(chiTiet);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật chi tiết nhập kho: " + ex.Message);
            }
        }

        public bool XoaChiTietNhapKho(int maCTNhap)
        {
            try
            {
                var existing = _dal.GetById(maCTNhap);
                if (existing == null)
                {
                    throw new Exception("Không tìm thấy chi tiết nhập kho để xóa.");
                }

                return _dal.XoaChiTietNhapKho(maCTNhap);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết nhập kho: " + ex.Message);
            }
        }
    }
}
