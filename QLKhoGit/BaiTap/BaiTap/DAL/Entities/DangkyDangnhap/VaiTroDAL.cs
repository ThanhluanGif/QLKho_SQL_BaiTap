using DAL.Helpers;
using DTO.DTO_DangkyDangnhap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.DangkyDangnhap
{
    public class VaiTroDAL
    {
        public List<VaiTro> LayTatCaVaiTro()
        {
            var list = new List<VaiTro>();
            string query = "SELECT MaVaiTro, TenVaiTro FROM VaiTro";

            using (var reader = DatabaseHelper.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    list.Add(new VaiTro
                    {
                        MaVaiTro = reader.GetInt32(0),
                        TenVaiTro = reader.GetString(1)
                    });
                }
            }
            return list;
        }
    }
}
