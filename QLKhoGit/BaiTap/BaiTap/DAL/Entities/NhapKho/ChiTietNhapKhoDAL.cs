using DAL.Helpers;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ChiTietNhapKhoDAL
    {
        private ChiTietNhapKhoDTO MapFromReader(IDataReader reader)
        {
            return new ChiTietNhapKhoDTO
            {
                MaCTNhap = Convert.ToInt32(reader["MaCTNhap"]),
                MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                MaHang = reader["MaHang"].ToString(),
                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                DonGia = Convert.ToDecimal(reader["DonGia"]),
                ThanhTien = Convert.ToDecimal(reader["ThanhTien"]),
                MaKho = reader["MaKho"].ToString()
            };
        }

        public List<ChiTietNhapKhoDTO> GetAll()
        {
            var dsChiTiet = new List<ChiTietNhapKhoDTO>();
            string query = "SELECT * FROM ChiTietNhapKho";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dsChiTiet.Add(MapFromReader(reader));
                }
            }

            return dsChiTiet;
        }

        public ChiTietNhapKhoDTO GetById(int maCTNhap)
        {
            string query = "SELECT * FROM ChiTietNhapKho WHERE MaCTNhap = @MaCTNhap";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaCTNhap", maCTNhap);

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

        public IEnumerable<ChiTietNhapKhoDTO> GetByMaPhieuNhap(string maPhieuNhap)
        {
            string query = "SELECT * FROM ChiTietNhapKho WHERE MaPhieuNhap = @MaPhieuNhap";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return MapFromReader(reader);
                    }
                }
            }
        }

        public bool ThemChiTietNhapKho(ChiTietNhapKhoDTO chiTiet)
        {
            string query = @"INSERT INTO ChiTietNhapKho (MaPhieuNhap, MaHang, MaKho, SoLuong, DonGia, ThanhTien) 
                 VALUES (@MaPhieuNhap, @MaHang, @MaKho, @SoLuong, @DonGia, @ThanhTien)";


            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaPhieuNhap", chiTiet.MaPhieuNhap);

                command.Parameters.AddWithValue("@MaKho", chiTiet.MaKho); // <- thêm dòng này
                command.Parameters.AddWithValue("@MaHang", chiTiet.MaHang);
                command.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                command.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                command.Parameters.AddWithValue("@ThanhTien", chiTiet.ThanhTien);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool SuaChiTietNhapKho(ChiTietNhapKhoDTO chiTiet)
        {
            string query = @"UPDATE ChiTietNhapKho 
                             SET MaPhieuNhap = @MaPhieuNhap, MaHang = @MaHang, MaKho = @MaKho, 
                                 SoLuong = @SoLuong, DonGia = @DonGia, ThanhTien = @ThanhTien
                             WHERE MaCTNhap = @MaCTNhap";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaCTNhap", chiTiet.MaCTNhap);
                command.Parameters.AddWithValue("@MaPhieuNhap", chiTiet.MaPhieuNhap);
                command.Parameters.AddWithValue("@MaHang", chiTiet.MaHang);
                command.Parameters.AddWithValue("@MaKho", chiTiet.MaKho);
                command.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                command.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                command.Parameters.AddWithValue("@ThanhTien", chiTiet.ThanhTien);

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool XoaChiTietNhapKho(int maCTNhap)
        {
            string query = "DELETE FROM ChiTietNhapKho WHERE MaCTNhap = @MaCTNhap";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@MaCTNhap", maCTNhap);

                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
