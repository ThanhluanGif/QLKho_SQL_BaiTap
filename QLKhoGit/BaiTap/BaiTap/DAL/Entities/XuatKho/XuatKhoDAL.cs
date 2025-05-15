using DAL.Helpers;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Entities.XuatKho
{
    public class XuatKhoDAL
    {
        private readonly ChiTietXuatKhoDAL _chiTietDAL = new ChiTietXuatKhoDAL();
        private XuatKhoDTO MapFromReader(IDataReader reader)
        {
            return new XuatKhoDTO
            {
                MaPhieuXuat = reader["MaPhieuXuat"].ToString(),
                MaKho = reader["MaKho"].ToString(),
                NgayXuat = Convert.ToDateTime(reader["NgayXuat"]),
                NguoiXuat = reader["NguoiXuat"].ToString(),
                KhachHang = reader["KhachHang"].ToString(),
                TongTien = Convert.ToDecimal(reader["TongTien"]),
                GhiChu = reader["GhiChu"].ToString()
            };
        }

        public List<XuatKhoDTO> GetAll()
        {
            var danhSach = new List<XuatKhoDTO>();
            string query = "SELECT * FROM XuatKho";
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    danhSach.Add(new XuatKhoDTO
                    {
                        MaPhieuXuat = reader["MaPhieuXuat"].ToString(),
                        MaKho = reader["MaKho"].ToString(),
                        NgayXuat = Convert.ToDateTime(reader["NgayXuat"]),
                        NguoiXuat = reader["NguoiXuat"].ToString(),
                        KhachHang = reader["KhachHang"].ToString(),
                        TongTien = Convert.ToDecimal(reader["TongTien"]),
                        GhiChu = reader["GhiChu"]?.ToString()
                    });
                }
            }
            return danhSach;
        }

        public List<dynamic> GetAllWithDetails()
        {
            var ds = new List<dynamic>();
            string query = @"
SELECT 
    xk.MaPhieuXuat,
    xk.MaKho,
    k.TenKho,
    xk.NgayXuat,
    xk.NguoiXuat,
    xk.KhachHang,
    ct.SoLuong,
    xk.TongTien,
    xk.GhiChu
FROM XuatKho xk
LEFT JOIN Kho k ON xk.MaKho = k.MaKho
LEFT JOIN ChiTietXuatKho ct ON xk.MaPhieuXuat = ct.MaPhieuXuat
ORDER BY xk.MaPhieuXuat DESC";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ds.Add(new
                    {
                        MaPhieuXuat = reader["MaPhieuXuat"].ToString(),
                        MaKho = reader["MaKho"].ToString(),
                        TenKho = reader["TenKho"]?.ToString(),
                        NgayXuat = Convert.ToDateTime(reader["NgayXuat"]),
                        NguoiXuat = reader["NguoiXuat"]?.ToString(),
                        KhachHang = reader["KhachHang"]?.ToString(),
                        SoLuong = reader["SoLuong"] != DBNull.Value ? Convert.ToInt32(reader["SoLuong"]) : 0,
                        TongTien = reader["TongTien"] != DBNull.Value ? Convert.ToDecimal(reader["TongTien"]) : 0,
                        GhiChu = reader["GhiChu"]?.ToString()
                    });
                }
            }

            return ds;
        }

        public XuatKhoDTO GetByMaPhieuXuat(string maPhieuXuat)
        {
            string query = "SELECT * FROM XuatKho WHERE MaPhieuXuat = @MaPhieuXuat";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhieuXuat", maPhieuXuat);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapFromReader(reader);
                    }
                }
            }

            return null;
        }

        public bool ThemXuatKho(XuatKhoDTO xuatKho)
        {
            string query = "INSERT INTO XuatKho (MaPhieuXuat, MaKho, NgayXuat, NguoiXuat, KhachHang, TongTien, GhiChu) " +
                           "VALUES (@MaPhieuXuat, @MaKho, @NgayXuat, @NguoiXuat, @KhachHang, @TongTien, @GhiChu)";
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhieuXuat", xuatKho.MaPhieuXuat);
                command.Parameters.AddWithValue("@MaKho", xuatKho.MaKho);
                command.Parameters.AddWithValue("@NgayXuat", xuatKho.NgayXuat);
                command.Parameters.AddWithValue("@NguoiXuat", xuatKho.NguoiXuat);
                command.Parameters.AddWithValue("@KhachHang", xuatKho.KhachHang);
                command.Parameters.AddWithValue("@TongTien", xuatKho.TongTien);
                command.Parameters.AddWithValue("@GhiChu", (object)xuatKho.GhiChu ?? DBNull.Value);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool SuaXuatKho(XuatKhoDTO xuatKho)
        {
            string query = "UPDATE XuatKho SET MaKho = @MaKho, NgayXuat = @NgayXuat, NguoiXuat = @NguoiXuat, " +
                           "KhachHang = @KhachHang, TongTien = @TongTien, GhiChu = @GhiChu WHERE MaPhieuXuat = @MaPhieuXuat";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhieuXuat", xuatKho.MaPhieuXuat);
                command.Parameters.AddWithValue("@MaKho", xuatKho.MaKho);
                command.Parameters.AddWithValue("@NgayXuat", xuatKho.NgayXuat);
                command.Parameters.AddWithValue("@NguoiXuat", xuatKho.NguoiXuat);
                command.Parameters.AddWithValue("@KhachHang", xuatKho.KhachHang);
                command.Parameters.AddWithValue("@TongTien", xuatKho.TongTien);
                command.Parameters.AddWithValue("@GhiChu", (object)xuatKho.GhiChu ?? DBNull.Value);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaXuatKho(string maPhieuXuat)
        {
            // 1. Xóa chi tiết trước
            _chiTietDAL.XoaTatCaChiTietTheoMaPhieu(maPhieuXuat);

            // 2. Xóa phiếu xuất chính
            string query = "DELETE FROM XuatKho WHERE MaPhieuXuat = @MaPhieuXuat";
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhieuXuat", maPhieuXuat);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public List<XuatKhoDTO> TimKiemXuatKho(string maPhieu, DateTime? tuNgay, DateTime? denNgay)
        {
            var ds = new List<XuatKhoDTO>();
            string query = "SELECT * FROM XuatKho WHERE 1=1";

            if (!string.IsNullOrEmpty(maPhieu))
            {
                query += " AND MaPhieuXuat LIKE @MaPhieuXuat";
            }

            if (tuNgay.HasValue)
            {
                query += " AND NgayXuat >= @TuNgay";
            }

            if (denNgay.HasValue)
            {
                query += " AND NgayXuat <= @DenNgay";
            }

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                if (!string.IsNullOrEmpty(maPhieu))
                    command.Parameters.AddWithValue("@MaPhieuXuat", "%" + maPhieu + "%");
                if (tuNgay.HasValue)
                    command.Parameters.AddWithValue("@TuNgay", tuNgay);
                if (denNgay.HasValue)
                    command.Parameters.AddWithValue("@DenNgay", denNgay);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ds.Add(MapFromReader(reader));
                    }
                }
            }

            return ds;
        }
    }
}
