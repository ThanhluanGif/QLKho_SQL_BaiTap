using DAL.Entities.NhapXuatTon;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BLL_TrungGian
{
    public class NhapXuatTonBLL
    {
        private readonly NhapXuatTonDAL _dal;

        public NhapXuatTonBLL()
        {
            _dal = new NhapXuatTonDAL();
        }

        // Lấy danh sách tồn kho
        public List<NhapXuatTonDTO> LayDanhSachNhapXuatTon()
        {
            return _dal.LayDanhSachNhapXuatTon();
        }
    }
}
