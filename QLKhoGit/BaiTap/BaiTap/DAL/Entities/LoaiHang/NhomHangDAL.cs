using DAL.Helpers;
using DTO.DTO_Basic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class NhomHangDAL
    {
        // Map dữ liệu từ IDataReader sang NhomHangDTO
        private NhomHangDTO MapFromReader(IDataReader reader)
        {
            return new NhomHangDTO
            {
                MaNhom = reader["MaNhom"].ToString(),
                TenNhom = reader["TenNhom"].ToString(),
                NgayTao = Convert.ToDateTime(reader["NgayTao"])
            };
        }

        // Lấy tất cả nhóm hàng
        public IEnumerable<NhomHangDTO> GetAll()
        {
            var list = new List<NhomHangDTO>();
            string query = "SELECT * FROM NhomHang";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapFromReader(reader));
                        }
                    }
                }
            }

            return list;
        }

        // Lấy nhóm hàng theo mã
        public NhomHangDTO GetByMaNhom(string maNhom)
        {
            string query = "SELECT * FROM NhomHang WHERE MaNhom = @MaNhom";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhom", maNhom);

                    using (var reader = cmd.ExecuteReader())
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

        // Thêm nhóm hàng mới
        public void Insert(NhomHangDTO nhomHang)
        {
            string query = "INSERT INTO NhomHang (MaNhom, TenNhom, NgayTao) VALUES (@MaNhom, @TenNhom, @NgayTao)";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhom", nhomHang.MaNhom);
                    cmd.Parameters.AddWithValue("@TenNhom", nhomHang.TenNhom);
                    cmd.Parameters.AddWithValue("@NgayTao", nhomHang.NgayTao);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Sửa thông tin nhóm hàng
        public void Update(NhomHangDTO nhomHang)
        {
            string query = "UPDATE NhomHang SET TenNhom = @TenNhom WHERE MaNhom = @MaNhom";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhom", nhomHang.MaNhom);
                    cmd.Parameters.AddWithValue("@TenNhom", nhomHang.TenNhom);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Xóa nhóm hàng theo mã
        public void Delete(string maNhom)
        {
            string query = "DELETE FROM NhomHang WHERE MaNhom = @MaNhom";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNhom", maNhom);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Tìm kiếm nhóm hàng theo tên
        public IEnumerable<NhomHangDTO> SearchByName(string keyword)
        {
            string query = "SELECT * FROM NhomHang WHERE TenNhom LIKE @Keyword";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    using (var reader = cmd.ExecuteReader())
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
