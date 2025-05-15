using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities.HangHoa_NhaCungCap;
using DTO.DTO_TrungGian;

namespace BLL.BLL_TrungGian
{
    public class HangHoa_NCCBLL
    {
        private readonly HangHoa_NCCDAL _dal;

        public HangHoa_NCCBLL()
        {
            _dal = new HangHoa_NCCDAL();
        }

        // Lấy danh sách tất cả mối quan hệ Hàng hóa - Nhà cung cấp
        public List<HangHoa_NCCDTO> LayTatCa()
        {
            return _dal.LayTatCa();
        }

        // Lấy mối quan hệ Hàng hóa - Nhà cung cấp theo Mã Hàng và Mã NCC
        public HangHoa_NCCDTO LayTheoMa(string maHang, string maNCC)
        {
            if (string.IsNullOrWhiteSpace(maHang) || string.IsNullOrWhiteSpace(maNCC))
            {
                throw new ArgumentException("Mã hàng và mã nhà cung cấp không được để trống.");
            }

            return _dal.LayTheoMa(maHang, maNCC);
        }

        // Thêm mối quan hệ Hàng hóa - Nhà cung cấp
        public void Them(HangHoa_NCCDTO hangHoaNCC)
        {
            if (hangHoaNCC == null)
            {
                throw new ArgumentNullException(nameof(hangHoaNCC), "Thông tin không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(hangHoaNCC.MaHang) || string.IsNullOrWhiteSpace(hangHoaNCC.MaNCC))
            {
                throw new ArgumentException("Mã hàng và mã nhà cung cấp không được để trống.");
            }

            _dal.Them(hangHoaNCC);
        }

        // Sửa mối quan hệ Hàng hóa - Nhà cung cấp
        public void Sua(HangHoa_NCCDTO hangHoaNCC)
        {
            if (hangHoaNCC == null)
            {
                throw new ArgumentNullException(nameof(hangHoaNCC), "Thông tin không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(hangHoaNCC.MaHang) || string.IsNullOrWhiteSpace(hangHoaNCC.MaNCC))
            {
                throw new ArgumentException("Mã hàng và mã nhà cung cấp không được để trống.");
            }

            _dal.Sua(hangHoaNCC);
        }

        // Xóa mối quan hệ Hàng hóa - Nhà cung cấp
        public void Xoa(string maHang, string maNCC)
        {
            if (string.IsNullOrWhiteSpace(maHang) || string.IsNullOrWhiteSpace(maNCC))
            {
                throw new ArgumentException("Mã hàng và mã nhà cung cấp không được để trống.");
            }

            _dal.Xoa(maHang, maNCC);
        }
        public List<HangHoa_NCCDTO> LayTheoNCC(string maNCC)
        {
            var hangHoaNCCDAL = new HangHoa_NCCDAL();
            return hangHoaNCCDAL.LayTheoNCC(maNCC);
        }

    }
}