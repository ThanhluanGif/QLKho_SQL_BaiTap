using DAL.Helpers;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class HangHoaDAL
    {
        private HangHoaDTO MapFromReader(IDataReader reader)
        {
            return new HangHoaDTO
            {
                MaHang = reader["MaHang"].ToString(),
                TenHang = reader["TenHang"].ToString(),
                MaLoai = reader["MaLoai"].ToString(),
                DonViTinh = reader["DonViTinh"].ToString(),
                XuatXu = reader["XuatXu"].ToString(),
                BaoHanh = reader["BaoHanh"] != DBNull.Value ? Convert.ToInt32(reader["BaoHanh"]) : (int?)null,
                HinhAnh = reader["HinhAnh"] != DBNull.Value ? reader["HinhAnh"].ToString() : null,
                MoTa = reader["MoTa"] != DBNull.Value ? reader["MoTa"].ToString() : null
            };
        }

        // Lấy danh sách hàng hóa
        public List<HangHoaDTO> LayDanhSachHangHoa()
        {
            var dsHangHoa = new List<HangHoaDTO>();
            string query = "SELECT * FROM HangHoa";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dsHangHoa.Add(MapFromReader(reader));
                        }
                    }
                }
            }

            return dsHangHoa;
        }

        // Lấy hàng hóa theo mã
        public HangHoaDTO LayHangHoaTheoMa(string maHang)
        {
            string query = "SELECT * FROM HangHoa WHERE MaHang = @MaHang";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaHang", maHang);

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

        // Thêm hàng hóa mới
        public void ThemHangHoa(HangHoaDTO hangHoa)
        {
            string query = "INSERT INTO HangHoa (MaHang, TenHang, MaLoai, DonViTinh, XuatXu, BaoHanh, HinhAnh, MoTa) " +
                           "VALUES (@MaHang, @TenHang, @MaLoai, @DonViTinh, @XuatXu, @BaoHanh, @HinhAnh, @MoTa)";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaHang", hangHoa.MaHang);
                    command.Parameters.AddWithValue("@TenHang", hangHoa.TenHang);
                    command.Parameters.AddWithValue("@MaLoai", hangHoa.MaLoai);
                    command.Parameters.AddWithValue("@DonViTinh", hangHoa.DonViTinh);
                    command.Parameters.AddWithValue("@XuatXu", hangHoa.XuatXu);
                    command.Parameters.AddWithValue("@BaoHanh", (object)hangHoa.BaoHanh ?? DBNull.Value);
                    command.Parameters.AddWithValue("@HinhAnh", (object)hangHoa.HinhAnh ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MoTa", (object)hangHoa.MoTa ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Sửa thông tin hàng hóa
        public void SuaHangHoa(HangHoaDTO hangHoa)
        {
            string query = "UPDATE HangHoa SET TenHang = @TenHang, MaLoai = @MaLoai, DonViTinh = @DonViTinh, " +
                           "XuatXu = @XuatXu, BaoHanh = @BaoHanh, HinhAnh = @HinhAnh, MoTa = @MoTa " +
                           "WHERE MaHang = @MaHang";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaHang", hangHoa.MaHang);
                    command.Parameters.AddWithValue("@TenHang", hangHoa.TenHang);
                    command.Parameters.AddWithValue("@MaLoai", hangHoa.MaLoai);
                    command.Parameters.AddWithValue("@DonViTinh", hangHoa.DonViTinh);
                    command.Parameters.AddWithValue("@XuatXu", hangHoa.XuatXu);
                    command.Parameters.AddWithValue("@BaoHanh", (object)hangHoa.BaoHanh ?? DBNull.Value);
                    command.Parameters.AddWithValue("@HinhAnh", (object)hangHoa.HinhAnh ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MoTa", (object)hangHoa.MoTa ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Xóa hàng hóa theo mã
        public void XoaHangHoa(string maHang)
        {
            string query = "DELETE FROM HangHoa WHERE MaHang = @MaHang";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaHang", maHang);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Tìm kiếm hàng hóa theo tên
        public IEnumerable<HangHoaDTO> SearchByName(string keyword)
        {
            string query = "SELECT * FROM HangHoa WHERE TenHang LIKE @Keyword";

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
