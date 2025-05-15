using DAL.Helpers;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class LoaiHangDAL
    {
        public List<LoaiHangDTO> LayDanhSachLoaiHang()
        {
            var dsLoaiHang = new List<LoaiHangDTO>();
            string query = "SELECT * FROM LoaiHang";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dsLoaiHang.Add(new LoaiHangDTO
                            {
                                MaLoai = reader["MaLoai"].ToString(),
                                TenLoai = reader["TenLoai"].ToString(),
                                MoTa = reader["MoTa"].ToString(),
                                NhomHang = reader["NhomHang"].ToString()
                            });
                        }
                    }
                }
            }

            return dsLoaiHang;
        }

        public LoaiHangDTO LayLoaiHangTheoMa(string maLoai)
        {
            string query = "SELECT * FROM LoaiHang WHERE MaLoai = @MaLoai";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaLoai", maLoai);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new LoaiHangDTO
                            {
                                MaLoai = reader["MaLoai"].ToString(),
                                TenLoai = reader["TenLoai"].ToString(),
                                MoTa = reader["MoTa"].ToString(),
                                NhomHang = reader["NhomHang"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void ThemLoaiHang(LoaiHangDTO loaiHang)
        {
            string query = "INSERT INTO LoaiHang (MaLoai, TenLoai, MoTa, NhomHang) VALUES (@MaLoai, @TenLoai, @MoTa, @NhomHang)";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaLoai", loaiHang.MaLoai);
                    command.Parameters.AddWithValue("@TenLoai", loaiHang.TenLoai);
                    command.Parameters.AddWithValue("@MoTa", (object)loaiHang.MoTa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NhomHang", (object)loaiHang.NhomHang ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void SuaLoaiHang(LoaiHangDTO loaiHang)
        {
            string query = "UPDATE LoaiHang SET TenLoai = @TenLoai, MoTa = @MoTa, NhomHang = @NhomHang WHERE MaLoai = @MaLoai";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaLoai", loaiHang.MaLoai);
                    command.Parameters.AddWithValue("@TenLoai", loaiHang.TenLoai);
                    command.Parameters.AddWithValue("@MoTa", (object)loaiHang.MoTa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NhomHang", (object)loaiHang.NhomHang ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void XoaLoaiHang(string maLoai)
        {
            string query = "DELETE FROM LoaiHang WHERE MaLoai = @MaLoai";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaLoai", maLoai);
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<LoaiHangDTO> TimKiemLoaiHangTheoTen(string keyword)
        {
            string query = "SELECT * FROM LoaiHang WHERE TenLoai LIKE @Keyword";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new LoaiHangDTO
                            {
                                MaLoai = reader["MaLoai"].ToString(),
                                TenLoai = reader["TenLoai"].ToString(),
                                MoTa = reader["MoTa"].ToString(),
                                NhomHang = reader["NhomHang"].ToString()
                            };
                        }
                    }
                }
            }
        }
    }
}
