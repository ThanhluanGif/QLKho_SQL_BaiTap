using DAL.Entities.HangHoa_GiaXuat;
using DTO.DTO_TrungGian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BLL_TrungGian
{
    public class HangHoa_GiaXuatBLL
    {
        private readonly HangHoa_GiaXuatDAL _dal;

        public HangHoa_GiaXuatBLL()
        {
            _dal = new HangHoa_GiaXuatDAL();
        }

        public List<HangHoa_GiaXuatDTO> GetAll()
        {
            try
            {
                return _dal.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách giá xuất: " + ex.Message);
            }
        }

        public HangHoa_GiaXuatDTO GetByMaGiaXuat(string maGiaXuat)
        {
            try
            {
                return _dal.GetByMaGiaXuat(maGiaXuat);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy giá xuất theo mã: " + ex.Message);
            }
        }

        public bool ThemGiaXuat(HangHoa_GiaXuatDTO giaXuat)
        {
            try
            {
                if (string.IsNullOrEmpty(giaXuat.MaGiaXuat) || string.IsNullOrEmpty(giaXuat.MaHang))
                {
                    throw new Exception("Mã giá xuất và mã hàng không được để trống.");
                }

                if (giaXuat.DonGiaNhap <= 0)
                {
                    throw new Exception("Đơn giá nhập phải lớn hơn 0.");
                }

                return _dal.ThemGiaXuat(giaXuat);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm giá xuất: " + ex.Message);
            }
        }

        public bool SuaGiaXuat(HangHoa_GiaXuatDTO giaXuat)
        {
            try
            {
                if (string.IsNullOrEmpty(giaXuat.MaGiaXuat) || string.IsNullOrEmpty(giaXuat.MaHang))
                {
                    throw new Exception("Mã giá xuất và mã hàng không được để trống.");
                }

                if (giaXuat.DonGiaNhap <= 0)
                {
                    throw new Exception("Đơn giá nhập phải lớn hơn 0.");
                }

                return _dal.SuaGiaXuat(giaXuat);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sửa giá xuất: " + ex.Message);
            }
        }

        public bool XoaGiaXuat(string maGiaXuat)
        {
            try
            {
                if (string.IsNullOrEmpty(maGiaXuat))
                {
                    throw new Exception("Mã giá xuất không được để trống.");
                }

                return _dal.XoaGiaXuat(maGiaXuat);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa giá xuất: " + ex.Message);
            }
        }


    }
}