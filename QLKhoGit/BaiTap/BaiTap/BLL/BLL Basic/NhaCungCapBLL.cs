using System;
using System.Collections.Generic;
using System.Linq;
using DAL;  // Tham chiếu đến Data Access Layer
using DTO;  // Tham chiếu đến Data Transfer Objects

namespace BLL.BLL_Basic
{
    public class NhaCungCapBLL
    {
        private readonly NhaCungCapDAL _nhaCungCapDAL;

        // Constructor để khởi tạo đối tượng DAL
        public NhaCungCapBLL()
        {
            _nhaCungCapDAL = new NhaCungCapDAL();
        }

        // Lấy danh sách nhà cung cấp
        public List<NhaCungCapDTO> LayDanhSachNhaCungCap()
        {
            return _nhaCungCapDAL.LayDanhSachNhaCungCap();
        }

        // Lấy tổng số nhà cung cấp
        public int LayTongSoNhaCungCap()
        {
            return _nhaCungCapDAL.LayDanhSachNhaCungCap().Count;
        }

        // Lấy thông tin nhà cung cấp theo mã
        public NhaCungCapDTO LayNhaCungCapTheoMa(string maNCC)
        {
            if (string.IsNullOrEmpty(maNCC))
            {
                throw new ArgumentException("Mã nhà cung cấp không được để trống.");
            }

            var nhaCungCap = _nhaCungCapDAL.LayNhaCungCapTheoMa(maNCC);
            if (nhaCungCap == null)
            {
                throw new Exception($"Không tìm thấy nhà cung cấp với mã {maNCC}.");
            }

            return nhaCungCap;
        }

        // Thêm một nhà cung cấp mới
        public void ThemNhaCungCap(NhaCungCapDTO nhaCungCap)
        {
            if (nhaCungCap == null)
            {
                throw new ArgumentNullException(nameof(nhaCungCap), "Thông tin nhà cung cấp không được để trống.");
            }

            if (string.IsNullOrEmpty(nhaCungCap.MaNCC))
            {
                throw new ArgumentException("Mã nhà cung cấp không được để trống.");
            }

            if (string.IsNullOrEmpty(nhaCungCap.TenNCC))
            {
                throw new ArgumentException("Tên nhà cung cấp không được để trống.");
            }

            // Kiểm tra xem mã nhà cung cấp đã tồn tại chưa
            var existingNhaCungCap = _nhaCungCapDAL.LayNhaCungCapTheoMa(nhaCungCap.MaNCC);
            if (existingNhaCungCap != null)
            {
                throw new Exception($"Mã nhà cung cấp {nhaCungCap.MaNCC} đã tồn tại.");
            }

            // Đặt giá trị mặc định cho NgayTao nếu chưa có
            if (nhaCungCap.NgayTao == default(DateTime))
            {
                nhaCungCap.NgayTao = DateTime.Now;
            }

            _nhaCungCapDAL.ThemNhaCungCap(nhaCungCap);
        }

        // Sửa thông tin nhà cung cấp
        public void SuaNhaCungCap(NhaCungCapDTO nhaCungCap)
        {
            if (nhaCungCap == null)
            {
                throw new ArgumentNullException(nameof(nhaCungCap), "Thông tin nhà cung cấp không được để trống.");
            }

            // Kiểm tra xem nhà cung cấp có tồn tại không
            var existingNhaCungCap = _nhaCungCapDAL.LayNhaCungCapTheoMa(nhaCungCap.MaNCC);
            if (existingNhaCungCap == null)
            {
                throw new Exception($"Không tìm thấy nhà cung cấp với mã {nhaCungCap.MaNCC}.");
            }

            _nhaCungCapDAL.SuaNhaCungCap(nhaCungCap);
        }

        // Xóa nhà cung cấp theo mã
        public void XoaNhaCungCap(string maNCC)
        {
            if (string.IsNullOrEmpty(maNCC))
            {
                throw new ArgumentException("Mã nhà cung cấp không được để trống.");
            }

            // Kiểm tra xem nhà cung cấp có tồn tại không
            var existingNhaCungCap = _nhaCungCapDAL.LayNhaCungCapTheoMa(maNCC);
            if (existingNhaCungCap == null)
            {
                throw new Exception($"Không tìm thấy nhà cung cấp với mã {maNCC}.");
            }

            _nhaCungCapDAL.XoaNhaCungCap(maNCC);
        }

        // Tìm kiếm nhà cung cấp theo tên
        public IEnumerable<NhaCungCapDTO> TimKiemTheoTen(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException("Từ khóa tìm kiếm không được để trống.");
            }

            return _nhaCungCapDAL.TimKiemTheoTen(keyword);
        }
    }
}
