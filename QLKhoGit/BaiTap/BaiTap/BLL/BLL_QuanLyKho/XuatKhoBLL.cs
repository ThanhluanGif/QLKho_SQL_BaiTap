using DAL.Entities.XuatKho;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;

namespace BLL.BLL_QuanLyKho
{
    public class XuatKhoBLL
    {
        private readonly XuatKhoDAL _dal;

        public XuatKhoBLL()
        {
            _dal = new XuatKhoDAL();
        }

        public List<XuatKhoDTO> GetAll()
        {
            try
            {
                return _dal.GetAll();  // Gọi DAL để lấy danh sách phiếu xuất kho
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phiếu xuất kho: " + ex.Message);
            }
        }

        public List<dynamic> GetAllWithDetails()
        {
            try
            {
                return _dal.GetAllWithDetails();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chi tiết phiếu xuất: " + ex.Message);
            }
        }


        public XuatKhoDTO GetByMaPhieuXuat(string maPhieuXuat)
        {
            try
            {
                var result = _dal.GetByMaPhieuXuat(maPhieuXuat);
                if (result == null)
                {
                    throw new Exception($"Không tìm thấy phiếu xuất với mã: {maPhieuXuat}");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy phiếu xuất: " + ex.Message);
            }
        }

        public bool ThemXuatKho(XuatKhoDTO xuatKho)
        {
            try
            {
                if (string.IsNullOrEmpty(xuatKho.MaPhieuXuat) || string.IsNullOrEmpty(xuatKho.MaKho))
                {
                    throw new Exception("Mã phiếu xuất hoặc mã kho không được để trống.");
                }

                if (xuatKho.TongTien < 0)
                {
                    throw new Exception("Tổng tiền không được nhỏ hơn 0.");
                }

                return _dal.ThemXuatKho(xuatKho);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm phiếu xuất: " + ex.Message);
            }
        }

        public bool SuaXuatKho(XuatKhoDTO xuatKho)
        {
            try
            {
                var existing = _dal.GetByMaPhieuXuat(xuatKho.MaPhieuXuat);
                if (existing == null)
                {
                    throw new Exception($"Không tìm thấy phiếu xuất với mã: {xuatKho.MaPhieuXuat}");
                }

                if (string.IsNullOrEmpty(xuatKho.MaPhieuXuat) || string.IsNullOrEmpty(xuatKho.MaKho))
                {
                    throw new Exception("Mã phiếu xuất hoặc mã kho không được để trống.");
                }

                if (xuatKho.TongTien < 0)
                {
                    throw new Exception("Tổng tiền không được nhỏ hơn 0.");
                }

                return _dal.SuaXuatKho(xuatKho);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật phiếu xuất: " + ex.Message);
            }
        }

        public bool XoaXuatKho(string maPhieuXuat)
        {
            try
            {
                var existing = _dal.GetByMaPhieuXuat(maPhieuXuat);
                if (existing == null)
                {
                    throw new Exception($"Không tìm thấy phiếu xuất với mã: {maPhieuXuat}");
                }

                return _dal.XoaXuatKho(maPhieuXuat);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa phiếu xuất: " + ex.Message);
            }
        }

        public List<XuatKhoDTO> TimKiemXuatKho(string maPhieu, DateTime? tuNgay, DateTime? denNgay)
        {
            try
            {
                return _dal.TimKiemXuatKho(maPhieu, tuNgay, denNgay);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm phiếu xuất kho: " + ex.Message);
            }
        }

    }
}
