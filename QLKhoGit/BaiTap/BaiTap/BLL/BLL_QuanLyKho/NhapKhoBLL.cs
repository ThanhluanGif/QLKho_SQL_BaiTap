using DAL;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BLL_QuanLyKho
{
    public class NhapKhoBLL
    {
        private readonly NhapKhoDAL _dal;

        public NhapKhoBLL()
        {
            _dal = new NhapKhoDAL();
        }

        public List<NhapKhoDTO> GetAll()
        {
            try
            {
                return _dal.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phiếu nhập kho: " + ex.Message);
            }
        }

        public List<dynamic> GetAllWithDetails()
        {
            return _dal.GetAllWithDetails();
        }

        public NhapKhoDTO GetByMaPhieuNhap(string maPhieuNhap)
        {
            try
            {
                var result = _dal.GetByMaPhieuNhap(maPhieuNhap);
                if (result == null)
                {
                    throw new Exception($"Không tìm thấy phiếu nhập kho với mã: {maPhieuNhap}");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy phiếu nhập kho: " + ex.Message);
            }
        }

        public bool ThemNhapKho(NhapKhoDTO nhapKho)
        {
            try
            {
                if (string.IsNullOrEmpty(nhapKho.MaPhieuNhap) || string.IsNullOrEmpty(nhapKho.MaNCC) ||
                    string.IsNullOrEmpty(nhapKho.MaKho) || string.IsNullOrEmpty(nhapKho.NguoiNhap))
                {
                    throw new Exception("Các trường bắt buộc (Mã phiếu nhập, mã NCC, mã kho, người nhập) không được để trống.");
                }

                if (nhapKho.NgayNhap > DateTime.Now)
                {
                    throw new Exception("Ngày nhập không được lớn hơn ngày hiện tại.");
                }

                if (nhapKho.TongTien < 0)
                {
                    throw new Exception("Tổng tiền không được nhỏ hơn 0.");
                }

                return _dal.ThemNhapKho(nhapKho);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm phiếu nhập kho: " + ex.Message);
            }
        }

        public bool SuaNhapKho(NhapKhoDTO nhapKho)
        {
            try
            {
                var existing = _dal.GetByMaPhieuNhap(nhapKho.MaPhieuNhap);
                if (existing == null)
                {
                    throw new Exception($"Không tìm thấy phiếu nhập kho với mã: {nhapKho.MaPhieuNhap}");
                }

                if (string.IsNullOrEmpty(nhapKho.MaNCC) || string.IsNullOrEmpty(nhapKho.MaKho) ||
                    string.IsNullOrEmpty(nhapKho.NguoiNhap))
                {
                    throw new Exception("Các trường bắt buộc (Mã NCC, mã kho, người nhập) không được để trống.");
                }

                if (nhapKho.NgayNhap > DateTime.Now)
                {
                    throw new Exception("Ngày nhập không được lớn hơn ngày hiện tại.");
                }

                if (nhapKho.TongTien < 0)
                {
                    throw new Exception("Tổng tiền không được nhỏ hơn 0.");
                }

                return _dal.SuaNhapKho(nhapKho);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật phiếu nhập kho: " + ex.Message);
            }
        }

        public bool XoaNhapKho(string maPhieuNhap)
        {
            try
            {
                var existing = _dal.GetByMaPhieuNhap(maPhieuNhap);
                if (existing == null)
                {
                    throw new Exception($"Không tìm thấy phiếu nhập kho với mã: {maPhieuNhap}");
                }

                return _dal.XoaNhapKho(maPhieuNhap);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa phiếu nhập kho: " + ex.Message);
            }
        }

        public List<dynamic> TimKiemNhapKho(string maPhieu, DateTime? tuNgay, DateTime? denNgay)
        {
            try
            {
                return _dal.TimKiemNhapKho(maPhieu, tuNgay, denNgay);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm phiếu nhập kho: " + ex.Message);
            }
        }
    }
}
