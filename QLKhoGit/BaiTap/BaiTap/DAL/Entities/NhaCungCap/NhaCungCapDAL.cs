using DAL.Helpers;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class NhaCungCapDAL
    {
        // Map dữ liệu từ IDataReader sang NhaCungCapDTO
        private NhaCungCapDTO MapFromReader(IDataReader reader)
        {
            return new NhaCungCapDTO
            {
                MaNCC = reader["MaNCC"].ToString(),
                TenNCC = reader["TenNCC"].ToString(),
                DiaChi = reader["DiaChi"].ToString(),
                SoDienThoai = reader["SoDienThoai"].ToString(),
                Email = reader["Email"].ToString(),
                MaSoThue = reader["MaSoThue"].ToString(),
                NguoiDaiDien = reader["NguoiDaiDien"].ToString(),
                NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                TrangThai = Convert.ToBoolean(reader["TrangThai"])
            };
        }

        // Lấy danh sách nhà cung cấp
        public List<NhaCungCapDTO> LayDanhSachNhaCungCap()
        {
            var dsNhaCungCap = new List<NhaCungCapDTO>();
            string query = "SELECT * FROM NhaCungCap";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dsNhaCungCap.Add(MapFromReader(reader));
                        }
                    }
                }
            }

            return dsNhaCungCap;
        }

        // Lấy thông tin nhà cung cấp theo mã
        public NhaCungCapDTO LayNhaCungCapTheoMa(string maNCC)
        {
            string query = "SELECT * FROM NhaCungCap WHERE MaNCC = @MaNCC";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaNCC", maNCC);

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

        // Thêm một nhà cung cấp mới
        public void ThemNhaCungCap(NhaCungCapDTO nhaCungCap)
        {
            string query = "INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, SoDienThoai, Email, MaSoThue, NguoiDaiDien, NgayTao, TrangThai) " +
                           "VALUES (@MaNCC, @TenNCC, @DiaChi, @SoDienThoai, @Email, @MaSoThue, @NguoiDaiDien, @NgayTao, @TrangThai)";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaNCC", nhaCungCap.MaNCC);
                    command.Parameters.AddWithValue("@TenNCC", nhaCungCap.TenNCC);
                    command.Parameters.AddWithValue("@DiaChi", (object)nhaCungCap.DiaChi ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SoDienThoai", (object)nhaCungCap.SoDienThoai ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object)nhaCungCap.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MaSoThue", (object)nhaCungCap.MaSoThue ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NguoiDaiDien", (object)nhaCungCap.NguoiDaiDien ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NgayTao", nhaCungCap.NgayTao);
                    command.Parameters.AddWithValue("@TrangThai", nhaCungCap.TrangThai);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Sửa thông tin nhà cung cấp
        public void SuaNhaCungCap(NhaCungCapDTO nhaCungCap)
        {
            string query = "UPDATE NhaCungCap SET TenNCC = @TenNCC, DiaChi = @DiaChi, SoDienThoai = @SoDienThoai, " +
                           "Email = @Email, MaSoThue = @MaSoThue, NguoiDaiDien = @NguoiDaiDien, TrangThai = @TrangThai " +
                           "WHERE MaNCC = @MaNCC";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaNCC", nhaCungCap.MaNCC);
                    command.Parameters.AddWithValue("@TenNCC", nhaCungCap.TenNCC);
                    command.Parameters.AddWithValue("@DiaChi", (object)nhaCungCap.DiaChi ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SoDienThoai", (object)nhaCungCap.SoDienThoai ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object)nhaCungCap.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MaSoThue", (object)nhaCungCap.MaSoThue ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NguoiDaiDien", (object)nhaCungCap.NguoiDaiDien ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TrangThai", nhaCungCap.TrangThai);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Xóa nhà cung cấp theo mã
        public void XoaNhaCungCap(string maNCC)
        {
            string query = "DELETE FROM NhaCungCap WHERE MaNCC = @MaNCC";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaNCC", maNCC);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Tìm kiếm nhà cung cấp theo tên
        public IEnumerable<NhaCungCapDTO> TimKiemTheoTen(string keyword)
        {
            string query = "SELECT * FROM NhaCungCap WHERE TenNCC LIKE @Keyword";

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
