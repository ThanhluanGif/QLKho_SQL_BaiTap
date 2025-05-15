using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Helpers;
using DTO.DTO_TrungGian;

namespace DAL.Entities.HangHoa_NhaCungCap
{
    public class HangHoa_NCCDAL
    {
        // Map dữ liệu từ IDataReader sang HangHoa_NCCDTO
        private HangHoa_NCCDTO MapFromReader(IDataReader reader)
        {
            return new HangHoa_NCCDTO
            {
                MaHang = reader["MaHang"].ToString(),
                MaNCC = reader["MaNCC"].ToString(),
                NgayCungCap = Convert.ToDateTime(reader["NgayCungCap"]),
                GiaCungCap = Convert.ToDecimal(reader["GiaCungCap"]),
                GhiChu = reader["GhiChu"]?.ToString()
            };
        }

        // Lấy danh sách tất cả mối quan hệ Hàng hóa - Nhà cung cấp
        public List<HangHoa_NCCDTO> LayTatCa()
        {
            var list = new List<HangHoa_NCCDTO>();
            string query = "SELECT * FROM HangHoa_NhaCungCap";

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

        // Lấy mối quan hệ Hàng hóa - Nhà cung cấp theo Mã Hàng và Mã NCC
        public HangHoa_NCCDTO LayTheoMa(string maHang, string maNCC)
        {
            string query = "SELECT * FROM HangHoa_NhaCungCap WHERE MaHang = @MaHang AND MaNCC = @MaNCC";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHang", maHang);
                    cmd.Parameters.AddWithValue("@MaNCC", maNCC);

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

        // Thêm mối quan hệ Hàng hóa - Nhà cung cấp
        public void Them(HangHoa_NCCDTO hangHoaNCC)
        {
            string query = "INSERT INTO HangHoa_NhaCungCap (MaHang, MaNCC, NgayCungCap, GiaCungCap, GhiChu) " +
                           "VALUES (@MaHang, @MaNCC, @NgayCungCap, @GiaCungCap, @GhiChu)";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHang", hangHoaNCC.MaHang);
                    cmd.Parameters.AddWithValue("@MaNCC", hangHoaNCC.MaNCC);
                    cmd.Parameters.AddWithValue("@NgayCungCap", hangHoaNCC.NgayCungCap);
                    cmd.Parameters.AddWithValue("@GiaCungCap", hangHoaNCC.GiaCungCap);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)hangHoaNCC.GhiChu ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Sửa mối quan hệ Hàng hóa - Nhà cung cấp
        public void Sua(HangHoa_NCCDTO hangHoaNCC)
        {
            string query = "UPDATE HangHoa_NCC SET NgayCungCap = @NgayCungCap, GiaCungCap = @GiaCungCap, GhiChu = @GhiChu " +
                           "WHERE MaHang = @MaHang AND MaNCC = @MaNCC";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHang", hangHoaNCC.MaHang);
                    cmd.Parameters.AddWithValue("@MaNCC", hangHoaNCC.MaNCC);
                    cmd.Parameters.AddWithValue("@NgayCungCap", hangHoaNCC.NgayCungCap);
                    cmd.Parameters.AddWithValue("@GiaCungCap", hangHoaNCC.GiaCungCap);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)hangHoaNCC.GhiChu ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Xóa mối quan hệ Hàng hóa - Nhà cung cấp
        public void Xoa(string maHang, string maNCC)
        {
            string query = "DELETE FROM HangHoa_NhaCungCap WHERE MaHang = @MaHang AND MaNCC = @MaNCC";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHang", maHang);
                    cmd.Parameters.AddWithValue("@MaNCC", maNCC);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<HangHoa_NCCDTO> LayTheoNCC(string maNCC)
        {
            var list = new List<HangHoa_NCCDTO>();
            string query = @"
       SELECT hhncc.MaHang, hhncc.GiaCungCap, hh.TenHang
       FROM HangHoa_NhaCungCap hhncc
       INNER JOIN HangHoa hh ON hhncc.MaHang = hh.MaHang
       WHERE hhncc.MaNCC = @MaNCC";

            using (var conn = DatabaseHelper.GetConnection())
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNCC", maNCC);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new HangHoa_NCCDTO
                            {
                                MaHang = reader["MaHang"].ToString(),
                                GiaCungCap = Convert.ToDecimal(reader["GiaCungCap"]),
                                TenHang = reader["TenHang"].ToString() // Map TenHang
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}