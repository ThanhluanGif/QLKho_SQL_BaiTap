using DAL.Entities.DangkyDangnhap;
using DTO.DTO_DangkyDangnhap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.BLL_DangNhapDangKy
{
    public class VaiTroBLL
    {
        private readonly VaiTroDAL _dal = new VaiTroDAL();

        public List<VaiTro> LayTatCaVaiTro()
        {
            return _dal.LayTatCaVaiTro();
        }
    }
}