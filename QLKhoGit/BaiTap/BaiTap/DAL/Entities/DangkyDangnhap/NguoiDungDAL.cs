using DAL.Helpers;
using DTO.DTO_DangkyDangnhap;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.DangkyDangnhap
{
    public class NguoiDungDAL
    {
        public bool KiemTraTonTai(string tenDangNhap)
        {
            string query = "SELECT COUNT(*) FROM NguoiDung WHERE TenDangNhap = @TenDangNhap";
            SqlParameter[] parameters =
            {
                new SqlParameter("@TenDangNhap", tenDangNhap)
            };

            using (var reader = DatabaseHelper.ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    return reader.GetInt32(0) > 0;
                }
            }

            return false;
        }

        public bool KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            string query = @"SELECT COUNT(*) FROM NguoiDung 
                             WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau AND TrangThai = 1";
            SqlParameter[] parameters =
            {
                new SqlParameter("@TenDangNhap", tenDangNhap),
                new SqlParameter("@MatKhau", matKhau)
            };

            using (var reader = DatabaseHelper.ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    return reader.GetInt32(0) > 0;
                }
            }

            return false;
        }

        public bool ThemNguoiDung(NguoiDung user)
        {
            string query = @"INSERT INTO NguoiDung 
(TenDangNhap, MatKhau, HoTen, Email, SoDienThoai, AnhDaiDien, TrangThai, NgayTao, MaVaiTro)
VALUES (@TenDangNhap, @MatKhau, @HoTen, @Email, @SoDienThoai, @AnhDaiDien, @TrangThai, @NgayTao, @MaVaiTro)";


            SqlParameter[] parameters =
            {
                new SqlParameter("@TenDangNhap", user.TenDangNhap),
                new SqlParameter("@MatKhau", user.MatKhau),
                new SqlParameter("@HoTen", (object)user.HoTen ?? DBNull.Value),
                new SqlParameter("@Email", (object)user.Email ?? DBNull.Value),
                new SqlParameter("@SoDienThoai", (object)user.SoDienThoai ?? DBNull.Value),
                new SqlParameter("@AnhDaiDien", (object)user.AnhDaiDien ?? DBNull.Value),
                new SqlParameter("@TrangThai", user.TrangThai),
                new SqlParameter("@NgayTao", user.NgayTao),
                new SqlParameter("@MaVaiTro", user.MaVaiTro)
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public NguoiDung LayThongTinNguoiDung(string tenDangNhap)
        {
            string query = "SELECT * FROM NguoiDung WHERE TenDangNhap = @TenDangNhap";
            SqlParameter[] parameters = { new SqlParameter("@TenDangNhap", tenDangNhap) };

            using (var reader = DatabaseHelper.ExecuteReader(query, parameters))
            {
                if (reader.Read())
                {
                    return new NguoiDung
                    {
                        MaNguoiDung = reader.GetInt32(reader.GetOrdinal("MaNguoiDung")),
                        TenDangNhap = reader.GetString(reader.GetOrdinal("TenDangNhap")),
                        MatKhau = reader.GetString(reader.GetOrdinal("MatKhau")),
                        HoTen = reader["HoTen"]?.ToString(),
                        Email = reader["Email"]?.ToString(),
                        SoDienThoai = reader["SoDienThoai"]?.ToString(),
                        AnhDaiDien = reader["AnhDaiDien"]?.ToString(),
                        TrangThai = (bool)reader["TrangThai"],
                        NgayTao = (DateTime)reader["NgayTao"]
                    };
                }
            }

            return null;
        }

        public List<NguoiDung> LayTatCaNguoiDung()
        {
            string query = "SELECT * FROM NguoiDung";
            var danhSach = new List<NguoiDung>();

            using (var reader = DatabaseHelper.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    danhSach.Add(new NguoiDung
                    {
                        MaNguoiDung = Convert.ToInt32(reader["MaNguoiDung"]),
                        TenDangNhap = reader["TenDangNhap"].ToString(),
                        MatKhau = reader["MatKhau"].ToString(),
                        HoTen = reader["HoTen"]?.ToString(),
                        Email = reader["Email"]?.ToString(),
                        SoDienThoai = reader["SoDienThoai"]?.ToString(),
                        AnhDaiDien = reader["AnhDaiDien"]?.ToString(),
                        TrangThai = Convert.ToBoolean(reader["TrangThai"]),
                        NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                        MaVaiTro = reader["MaVaiTro"] != DBNull.Value ? Convert.ToInt32(reader["MaVaiTro"]) : (int?)null  // ✅ THÊM DÒNG NÀY
                    });
                }
            }

            return danhSach;
        }


        public bool CapNhatNguoiDung(NguoiDung user)
        {
            string query = @"UPDATE NguoiDung
             SET MatKhau = @MatKhau,
                 HoTen = @HoTen,
                 Email = @Email,
                 SoDienThoai = @SoDienThoai,
                 TrangThai = @TrangThai,
                 MaVaiTro = @MaVaiTro,
                 AnhDaiDien = @AnhDaiDien
             WHERE TenDangNhap = @TenDangNhap";

            using (var conn = DatabaseHelper.GetConnection())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TenDangNhap", user.TenDangNhap);
                cmd.Parameters.AddWithValue("@MatKhau", user.MatKhau);
                cmd.Parameters.AddWithValue("@HoTen", user.HoTen);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@SoDienThoai", user.SoDienThoai);
                cmd.Parameters.AddWithValue("@TrangThai", user.TrangThai);
                cmd.Parameters.AddWithValue("@MaVaiTro", user.MaVaiTro); // chỉ giữ một dòng này
                cmd.Parameters.AddWithValue("@AnhDaiDien", user.AnhDaiDien ?? "");

                return cmd.ExecuteNonQuery() > 0;
            }
        }



        public bool XoaNguoiDung(string tenDangNhap)
        {
            string query = "DELETE FROM NguoiDung WHERE TenDangNhap = @TenDangNhap";
            SqlParameter[] parameters = { new SqlParameter("@TenDangNhap", tenDangNhap) };

            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool ResetMatKhau(string tenDangNhap)
        {
            string query = "UPDATE NguoiDung SET MatKhau = '123456' WHERE TenDangNhap = @TenDangNhap";
            SqlParameter[] parameters = { new SqlParameter("@TenDangNhap", tenDangNhap) };

            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public NguoiDung LayNguoiDungTheoTenDangNhap(string tenDangNhap)
        {
            string query = "SELECT * FROM NguoiDung WHERE TenDangNhap = @TenDangNhap";
            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new NguoiDung
                        {
                            MaNguoiDung = Convert.ToInt32(reader["MaNguoiDung"]),
                            TenDangNhap = reader["TenDangNhap"].ToString(),
                            MatKhau = reader["MatKhau"].ToString(),
                            HoTen = reader["HoTen"]?.ToString(),
                            Email = reader["Email"]?.ToString(),
                            SoDienThoai = reader["SoDienThoai"]?.ToString(),
                            AnhDaiDien = reader["AnhDaiDien"]?.ToString(),
                            TrangThai = Convert.ToBoolean(reader["TrangThai"]),
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            MaVaiTro = reader["MaVaiTro"] != DBNull.Value ? Convert.ToInt32(reader["MaVaiTro"]) : (int?)null
                        };
                    }
                }
            }
            return null;
        }


    }
}