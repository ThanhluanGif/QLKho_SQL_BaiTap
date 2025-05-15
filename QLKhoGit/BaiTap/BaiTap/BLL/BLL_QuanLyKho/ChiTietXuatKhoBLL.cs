using DAL.Entities.XuatKho;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;

namespace BLL.BLL_QuanLyKho
{
    public class ChiTietXuatKhoBLL
    {
        private readonly ChiTietXuatKhoDAL _dal;

        public ChiTietXuatKhoBLL()
        {
            _dal = new ChiTietXuatKhoDAL();
        }

        public List<ChiTietXuatKhoDTO> GetAll()
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
                throw new Exception("Lỗi khi lấy danh sách chi tiết xuất kho: " + ex.Message);
            }
        }

        public ChiTietXuatKhoDTO GetById(int maCTXuat)
        {
            try
            {
                var result = _dal.GetById(maCTXuat);
                if (result == null)
                {
                    throw new Exception("Không tìm thấy chi tiết xuất kho với mã: " + maCTXuat);
                }
                result.ThanhTien = result.SoLuong * result.DonGia;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết xuất kho: " + ex.Message);
            }
        }

        public bool ThemChiTietXuatKho(ChiTietXuatKhoDTO chiTiet)
        {
            try
            {
                if (string.IsNullOrEmpty(chiTiet.MaPhieuXuat) || string.IsNullOrEmpty(chiTiet.MaHang))
                {
                    throw new Exception("Mã phiếu xuất hoặc mã hàng không được để trống.");
                }
                if (chiTiet.SoLuong <= 0 || chiTiet.DonGia <= 0)
                {
                    throw new Exception("Số lượng và đơn giá phải lớn hơn 0.");
                }

                chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                return _dal.ThemChiTietXuatKho(chiTiet);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm chi tiết xuất kho: " + ex.Message);
            }
        }

        public bool SuaChiTietXuatKho(ChiTietXuatKhoDTO chiTiet)
        {
            try
            {
                var existing = _dal.GetById(chiTiet.MaCTXuat);
                if (existing == null)
                {
                    throw new Exception("Không tìm thấy chi tiết xuất kho để cập nhật.");
                }
                if (chiTiet.SoLuong <= 0 || chiTiet.DonGia <= 0)
                {
                    throw new Exception("Số lượng và đơn giá phải lớn hơn 0.");
                }

                chiTiet.ThanhTien = chiTiet.SoLuong * chiTiet.DonGia;
                return _dal.SuaChiTietXuatKho(chiTiet);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật chi tiết xuất kho: " + ex.Message);
            }
        }

        public bool XoaChiTietXuatKho(int maCTXuat)
        {
            try
            {
                var existing = _dal.GetById(maCTXuat);
                if (existing == null)
                {
                    throw new Exception("Không tìm thấy chi tiết xuất kho để xóa.");
                }

                return _dal.XoaChiTietXuatKho(maCTXuat);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết xuất kho: " + ex.Message);
            }
        }
    }
}
