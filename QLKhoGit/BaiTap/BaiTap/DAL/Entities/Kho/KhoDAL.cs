using DAL.Helpers;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class KhoDAL
    {
        // Map dữ liệu từ IDataReader sang KhoDTO
        private KhoDTO MapFromReader(IDataReader reader)
        {
            return new KhoDTO
            {
                MaKho = reader["MaKho"].ToString(),
                TenKho = reader["TenKho"].ToString(),
                DiaChi = reader["DiaChi"].ToString(),
                DienTich = Convert.ToDecimal(reader["DienTich"]),
                NguoiQuanLy = reader["NguoiQuanLy"].ToString(),
                SoDienThoai = reader["SoDienThoai"].ToString(),
                TrangThai = Convert.ToBoolean(reader["TrangThai"])
            };
        }

        // Lấy danh sách tất cả kho
        public List<KhoDTO> LayDanhSachKho()
        {
            var dsKho = new List<KhoDTO>();
            string query = "SELECT * FROM Kho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dsKho.Add(MapFromReader(reader));
                        }
                    }
                }
            }

            return dsKho;
        }

        // Lấy danh sách kho đang hoạt động
        public List<KhoDTO> LayDanhSachKhoDangHoatDong()
        {
            var dsKho = new List<KhoDTO>();
            string query = "SELECT * FROM Kho WHERE TrangThai = 1";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dsKho.Add(MapFromReader(reader));
                        }
                    }
                }
            }

            return dsKho;
        }

        // Lấy thông tin kho theo mã
        public KhoDTO LayKhoTheoMa(string maKho)
        {
            string query = "SELECT * FROM Kho WHERE MaKho = @MaKho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaKho", maKho);

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

        // Thêm một kho mới
        public void ThemKho(KhoDTO kho)
        {
            string query = "INSERT INTO Kho (MaKho, TenKho, DiaChi, DienTich, NguoiQuanLy, SoDienThoai, TrangThai) " +
                           "VALUES (@MaKho, @TenKho, @DiaChi, @DienTich, @NguoiQuanLy, @SoDienThoai, @TrangThai)";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaKho", kho.MaKho);
                    command.Parameters.AddWithValue("@TenKho", kho.TenKho);
                    command.Parameters.AddWithValue("@DiaChi", (object)kho.DiaChi ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DienTich", kho.DienTich);
                    command.Parameters.AddWithValue("@NguoiQuanLy", (object)kho.NguoiQuanLy ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SoDienThoai", (object)kho.SoDienThoai ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TrangThai", kho.TrangThai);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Sửa thông tin kho
        public void SuaKho(KhoDTO kho)
        {
            string query = "UPDATE Kho SET TenKho = @TenKho, DiaChi = @DiaChi, DienTich = @DienTich, " +
                           "NguoiQuanLy = @NguoiQuanLy, SoDienThoai = @SoDienThoai, TrangThai = @TrangThai " +
                           "WHERE MaKho = @MaKho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaKho", kho.MaKho);
                    command.Parameters.AddWithValue("@TenKho", kho.TenKho);
                    command.Parameters.AddWithValue("@DiaChi", (object)kho.DiaChi ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DienTich", kho.DienTich);
                    command.Parameters.AddWithValue("@NguoiQuanLy", (object)kho.NguoiQuanLy ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SoDienThoai", (object)kho.SoDienThoai ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TrangThai", kho.TrangThai);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Xóa kho theo mã
        public void XoaKho(string maKho)
        {
            string query = "DELETE FROM Kho WHERE MaKho = @MaKho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaKho", maKho);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Tìm kiếm kho theo tên
        public IEnumerable<KhoDTO> TimKiemKhoTheoTen(string keyword)
        {
            string query = "SELECT * FROM Kho WHERE TenKho LIKE @Keyword";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return MapFromReader(reader);
                        }
                    }
                }
            }
        }
    }
}
