using DAL;
using DTO.DTO_Basic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BLL_Basic
{
    public class NhomHangBLL
    {
        private readonly NhomHangDAL _dal;

        public NhomHangBLL()
        {
            _dal = new NhomHangDAL();
        }

        // Lấy tất cả nhóm hàng
        public List<NhomHangDTO> LayTatCaNhomHang()
        {
            return _dal.GetAll().ToList();
        }

        // Lấy nhóm hàng theo mã
        public NhomHangDTO LayTheoMa(string maNhom)
        {
            if (string.IsNullOrWhiteSpace(maNhom))
            {
                throw new ArgumentException("Mã nhóm không được để trống.");
            }

            var nhomHang = _dal.GetByMaNhom(maNhom);
            if (nhomHang == null)
            {
                throw new Exception($"Không tìm thấy nhóm hàng với mã {maNhom}.");
            }

            return nhomHang;
        }

        // Thêm nhóm hàng mới
        public void Them(NhomHangDTO nhomHang)
        {
            if (nhomHang == null)
            {
                throw new ArgumentNullException(nameof(nhomHang), "Thông tin nhóm hàng không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(nhomHang.MaNhom))
            {
                throw new ArgumentException("Mã nhóm không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(nhomHang.TenNhom))
            {
                throw new ArgumentException("Tên nhóm không được để trống.");
            }

            // Kiểm tra xem mã nhóm đã tồn tại chưa
            var existingNhomHang = _dal.GetByMaNhom(nhomHang.MaNhom);
            if (existingNhomHang != null)
            {
                throw new Exception($"Mã nhóm {nhomHang.MaNhom} đã tồn tại.");
            }

            // Đặt giá trị mặc định cho NgayTao nếu chưa có
            if (nhomHang.NgayTao == default(DateTime))
            {
                nhomHang.NgayTao = DateTime.Now;
            }

            _dal.Insert(nhomHang);
        }

        // Sửa thông tin nhóm hàng
        public void Sua(NhomHangDTO nhomHang)
        {
            if (nhomHang == null)
            {
                throw new ArgumentNullException(nameof(nhomHang), "Thông tin nhóm hàng không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(nhomHang.MaNhom))
            {
                throw new ArgumentException("Mã nhóm không được để trống.");
            }

            // Kiểm tra xem nhóm hàng có tồn tại không
            var existingNhomHang = _dal.GetByMaNhom(nhomHang.MaNhom);
            if (existingNhomHang == null)
            {
                throw new Exception($"Không tìm thấy nhóm hàng với mã {nhomHang.MaNhom}.");
            }

            _dal.Update(nhomHang);
        }

        // Xóa nhóm hàng theo mã
        public void Xoa(string maNhom)
        {
            if (string.IsNullOrWhiteSpace(maNhom))
            {
                throw new ArgumentException("Mã nhóm không được để trống.");
            }

            // Kiểm tra xem nhóm hàng có tồn tại không
            var existingNhomHang = _dal.GetByMaNhom(maNhom);
            if (existingNhomHang == null)
            {
                throw new Exception($"Không tìm thấy nhóm hàng với mã {maNhom}.");
            }

            _dal.Delete(maNhom);
        }

        // Tìm kiếm nhóm hàng theo tên
        public IEnumerable<NhomHangDTO> TimKiem(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                throw new ArgumentException("Từ khóa tìm kiếm không được để trống.");
            }

            return _dal.SearchByName(keyword);
        }
    }
}
