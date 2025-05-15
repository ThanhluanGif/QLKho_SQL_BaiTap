using DAL.Helpers;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Entities.XuatKho
{
    public class ChiTietXuatKhoDAL
    {
        private ChiTietXuatKhoDTO MapFromReader(IDataReader reader)
        {
            return new ChiTietXuatKhoDTO
            {
                MaCTXuat = Convert.ToInt32(reader["MaCTXuat"]),
                MaPhieuXuat = reader["MaPhieuXuat"].ToString(),
                MaHang = reader["MaHang"].ToString(),
                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                DonGia = Convert.ToDecimal(reader["DonGia"])
            };
        }

        public List<ChiTietXuatKhoDTO> GetAll()
        {
            var dsChiTiet = new List<ChiTietXuatKhoDTO>();
            string query = "SELECT * FROM ChiTietXuatKho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dsChiTiet.Add(MapFromReader(reader));
                        }
                    }
                }
            }

            return dsChiTiet;
        }

        public ChiTietXuatKhoDTO GetById(int maCTXuat)
        {
            string query = "SELECT * FROM ChiTietXuatKho WHERE MaCTXuat = @MaCTXuat";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaCTXuat", maCTXuat);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapFromReader(reader);
                        }
                    }
                }
            }

            return null;
        }

        public bool ThemChiTietXuatKho(ChiTietXuatKhoDTO chiTiet)
        {
            string query = "INSERT INTO ChiTietXuatKho (MaPhieuXuat, MaHang, SoLuong, DonGia, ThanhTien) " +
                           "VALUES (@MaPhieuXuat, @MaHang, @SoLuong, @DonGia, @ThanhTien)";
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhieuXuat", chiTiet.MaPhieuXuat);
                command.Parameters.AddWithValue("@MaHang", chiTiet.MaHang);
                command.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                command.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                command.Parameters.AddWithValue("@ThanhTien", chiTiet.ThanhTien);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool SuaChiTietXuatKho(ChiTietXuatKhoDTO chiTiet)
        {
            string query = "UPDATE ChiTietXuatKho SET MaPhieuXuat = @MaPhieuXuat, MaHang = @MaHang, " +
                           "SoLuong = @SoLuong, DonGia = @DonGia WHERE MaCTXuat = @MaCTXuat";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaCTXuat", chiTiet.MaCTXuat);
                    command.Parameters.AddWithValue("@MaPhieuXuat", chiTiet.MaPhieuXuat);
                    command.Parameters.AddWithValue("@MaHang", chiTiet.MaHang);
                    command.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                    command.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool XoaChiTietXuatKho(int maCTXuat)
        {
            string query = "DELETE FROM ChiTietXuatKho WHERE MaCTXuat = @MaCTXuat";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaCTXuat", maCTXuat);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool XoaTatCaChiTietTheoMaPhieu(string maPhieuXuat)
        {
            string query = "DELETE FROM ChiTietXuatKho WHERE MaPhieuXuat = @MaPhieuXuat";
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhieuXuat", maPhieuXuat);
                return command.ExecuteNonQuery() >= 0; // không cần > 0 vì có thể không có dòng
            }
        }
    }
}
