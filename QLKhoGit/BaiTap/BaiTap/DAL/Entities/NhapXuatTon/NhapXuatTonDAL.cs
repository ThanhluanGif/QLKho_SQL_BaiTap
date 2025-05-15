using DAL.Helpers;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.NhapXuatTon
{
    public class NhapXuatTonDAL
    {
        // Lấy danh sách tồn kho
        public List<NhapXuatTonDTO> LayDanhSachNhapXuatTon()
        {
            var danhSachNhapXuatTon = new List<NhapXuatTonDTO>();
            string query = @"
                SELECT 
                    nhap.MaHang,
                    ISNULL(SUM(nhap.SoLuong), 0) AS TongNhap,
                    ISNULL(SUM(xuat.SoLuong), 0) AS TongXuat,
                    ISNULL(SUM(nhap.SoLuong), 0) - ISNULL(SUM(xuat.SoLuong), 0) AS TonKho
                FROM HangHoa hh
                LEFT JOIN ChiTietNhapKho nhap ON nhap.MaHang = hh.MaHang
                LEFT JOIN ChiTietXuatKho xuat ON xuat.MaHang = hh.MaHang
                GROUP BY nhap.MaHang";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachNhapXuatTon.Add(new NhapXuatTonDTO
                            {
                                MaHang = reader["MaHang"].ToString(),
                                Nhap = Convert.ToInt32(reader["TongNhap"]),
                                Xuat = Convert.ToInt32(reader["TongXuat"]),
                            });
                        }
                    }
                }
            }

            return danhSachNhapXuatTon;
        }
    }
}
