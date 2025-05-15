using DAL.Helpers;
using DTO.DTO_TrungGian;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.HangHoa_GiaXuat
{
    public class HangHoa_GiaXuatDAL
    {
        public List<HangHoa_GiaXuatDTO> GetAll()
        {
            var danhSachGiaXuat = new List<HangHoa_GiaXuatDTO>();
            string query = "SELECT MaGiaXuat, MaHang, DonGiaNhap, DonGiaXuat FROM GiaXuatHangHoa";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSachGiaXuat.Add(new HangHoa_GiaXuatDTO
                            {
                                MaGiaXuat = reader["MaGiaXuat"].ToString(),
                                MaHang = reader["MaHang"].ToString(),
                                DonGiaNhap = Convert.ToDecimal(reader["DonGiaNhap"]),
                                DonGiaXuat = Convert.ToDecimal(reader["DonGiaXuat"])
                            });
                        }
                    }
                }
            }

            return danhSachGiaXuat;
        }

        public HangHoa_GiaXuatDTO GetByMaGiaXuat(string maGiaXuat)
        {
            string query = "SELECT MaGiaXuat, MaHang, DonGiaNhap, DonGiaXuat FROM GiaXuatHangHoa WHERE MaGiaXuat = @MaGiaXuat";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaGiaXuat", maGiaXuat);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new HangHoa_GiaXuatDTO
                            {
                                MaGiaXuat = reader["MaGiaXuat"].ToString(),
                                MaHang = reader["MaHang"].ToString(),
                                DonGiaNhap = Convert.ToDecimal(reader["DonGiaNhap"]),
                                DonGiaXuat = Convert.ToDecimal(reader["DonGiaXuat"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        public bool ThemGiaXuat(HangHoa_GiaXuatDTO giaXuat)
        {
            string query = "INSERT INTO GiaXuatHangHoa (MaGiaXuat, MaHang, DonGiaNhap) VALUES (@MaGiaXuat, @MaHang, @DonGiaNhap)";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaGiaXuat", giaXuat.MaGiaXuat);
                    command.Parameters.AddWithValue("@MaHang", giaXuat.MaHang);
                    command.Parameters.AddWithValue("@DonGiaNhap", giaXuat.DonGiaNhap);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool SuaGiaXuat(HangHoa_GiaXuatDTO giaXuat)
        {
            string query = "UPDATE GiaXuatHangHoa SET MaHang = @MaHang, DonGiaNhap = @DonGiaNhap WHERE MaGiaXuat = @MaGiaXuat";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaGiaXuat", giaXuat.MaGiaXuat);
                    command.Parameters.AddWithValue("@MaHang", giaXuat.MaHang);
                    command.Parameters.AddWithValue("@DonGiaNhap", giaXuat.DonGiaNhap);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool XoaGiaXuat(string maGiaXuat)
        {
            string query = "DELETE FROM GiaXuatHangHoa WHERE MaGiaXuat = @MaGiaXuat";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaGiaXuat", maGiaXuat);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool CapNhatGiaXuat(string maGiaXuat, decimal donGiaXuat)
        {
            string query = "UPDATE GiaXuatHangHoa SET DonGiaXuat = @DonGiaXuat WHERE MaGiaXuat = @MaGiaXuat";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaGiaXuat", maGiaXuat);
                    command.Parameters.AddWithValue("@DonGiaXuat", donGiaXuat);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

    }
}
