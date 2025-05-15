using DAL;
using DAL.Helpers;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class NhapKhoDAL
    {
        private NhapKhoDTO MapFromReader(IDataReader reader)
        {
            return new NhapKhoDTO
            {
                MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                MaNCC = reader["MaNCC"].ToString(),
                MaKho = reader["MaKho"].ToString(),
                NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                NguoiNhap = reader["NguoiNhap"].ToString(),
                TongTien = Convert.ToDecimal(reader["TongTien"]),
                GhiChu = reader["GhiChu"] != DBNull.Value ? reader["GhiChu"].ToString() : null
            };
        }

        public List<NhapKhoDTO> GetAll()
        {
            var dsNhapKho = new List<NhapKhoDTO>();
            string query = "SELECT * FROM NhapKho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dsNhapKho.Add(MapFromReader(reader));
                        }
                    }
                }
            }

            return dsNhapKho;
        }

        public List<dynamic> GetAllWithDetails()
        {
            var danhSach = new List<dynamic>();
            string query = @"
        SELECT 
            nk.MaPhieuNhap,
            ncc.TenNCC,
            kho.TenKho,
            nk.NgayNhap,
            nk.NguoiNhap,
            ct.MaHang,
            ct.SoLuong,
            ct.DonGia,
            ct.ThanhTien,
            nk.GhiChu
        FROM NhapKho nk
        LEFT JOIN ChiTietNhapKho ct ON nk.MaPhieuNhap = ct.MaPhieuNhap
        LEFT JOIN NhaCungCap ncc ON nk.MaNCC = ncc.MaNCC
        LEFT JOIN Kho kho ON nk.MaKho = kho.MaKho
        ORDER BY nk.MaPhieuNhap DESC";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new
                            {
                                MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                                TenNCC = reader["TenNCC"]?.ToString(),
                                TenKho = reader["TenKho"]?.ToString(),
                                NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                                NguoiNhap = reader["NguoiNhap"]?.ToString(),
                                MaHang = reader["MaHang"]?.ToString(),
                                SoLuong = reader["SoLuong"] != DBNull.Value ? Convert.ToInt32(reader["SoLuong"]) : 0,
                                DonGia = reader["DonGia"] != DBNull.Value ? Convert.ToDecimal(reader["DonGia"]) : 0,
                                ThanhTien = reader["ThanhTien"] != DBNull.Value ? Convert.ToDecimal(reader["ThanhTien"]) : 0,
                                GhiChu = reader["GhiChu"]?.ToString()
                            });
                        }
                    }
                }
            }
            return danhSach;
        }

        public NhapKhoDTO GetByMaPhieuNhap(string maPhieuNhap)
        {
            string query = "SELECT * FROM NhapKho WHERE MaPhieuNhap = @MaPhieuNhap";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);

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

        public bool ThemNhapKho(NhapKhoDTO nhapKho)
        {
            string query = "INSERT INTO NhapKho (MaPhieuNhap, MaNCC, MaKho, NgayNhap, NguoiNhap, TongTien, GhiChu) " +
                           "VALUES (@MaPhieuNhap, @MaNCC, @MaKho, @NgayNhap, @NguoiNhap, @TongTien, @GhiChu)";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaPhieuNhap", nhapKho.MaPhieuNhap);
                    command.Parameters.AddWithValue("@MaNCC", nhapKho.MaNCC);
                    command.Parameters.AddWithValue("@MaKho", nhapKho.MaKho);
                    command.Parameters.AddWithValue("@NgayNhap", nhapKho.NgayNhap);
                    command.Parameters.AddWithValue("@NguoiNhap", nhapKho.NguoiNhap);
                    command.Parameters.AddWithValue("@TongTien", nhapKho.TongTien);
                    command.Parameters.AddWithValue("@GhiChu", (object)nhapKho.GhiChu ?? DBNull.Value);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool SuaNhapKho(NhapKhoDTO nhapKho)
        {
            string query = "UPDATE NhapKho SET MaNCC = @MaNCC, MaKho = @MaKho, NgayNhap = @NgayNhap, " +
                           "NguoiNhap = @NguoiNhap, TongTien = @TongTien, GhiChu = @GhiChu " +
                           "WHERE MaPhieuNhap = @MaPhieuNhap";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaPhieuNhap", nhapKho.MaPhieuNhap);
                    command.Parameters.AddWithValue("@MaNCC", nhapKho.MaNCC);
                    command.Parameters.AddWithValue("@MaKho", nhapKho.MaKho);
                    command.Parameters.AddWithValue("@NgayNhap", nhapKho.NgayNhap);
                    command.Parameters.AddWithValue("@NguoiNhap", nhapKho.NguoiNhap);
                    command.Parameters.AddWithValue("@TongTien", nhapKho.TongTien);
                    command.Parameters.AddWithValue("@GhiChu", (object)nhapKho.GhiChu ?? DBNull.Value);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool XoaNhapKho(string maPhieuNhap)
        {
            string query = "DELETE FROM NhapKho WHERE MaPhieuNhap = @MaPhieuNhap";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaPhieuNhap", maPhieuNhap);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<dynamic> TimKiemNhapKho(string maPhieu, DateTime? tuNgay, DateTime? denNgay)
        {
            var query = @"
SELECT 
    nk.MaPhieuNhap,
    ncc.TenNCC,
    k.TenKho,
    nk.NgayNhap,
    nk.NguoiNhap,
    nk.GhiChu,
    ct.MaHang,
    ct.SoLuong,
    ct.DonGia,
    ct.ThanhTien
FROM NhapKho nk
LEFT JOIN ChiTietNhapKho ct ON nk.MaPhieuNhap = ct.MaPhieuNhap
LEFT JOIN NhaCungCap ncc ON nk.MaNCC = ncc.MaNCC
LEFT JOIN Kho k ON nk.MaKho = k.MaKho
WHERE 1=1";

            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(maPhieu))
            {
                query += " AND nk.MaPhieuNhap LIKE @MaPhieuNhap";
                parameters.Add(new SqlParameter("@MaPhieuNhap", "%" + maPhieu + "%"));
            }

            if (tuNgay.HasValue)
            {
                query += " AND nk.NgayNhap >= @TuNgay";
                parameters.Add(new SqlParameter("@TuNgay", tuNgay.Value));
            }

            if (denNgay.HasValue)
            {
                query += " AND nk.NgayNhap <= @DenNgay";
                parameters.Add(new SqlParameter("@DenNgay", denNgay.Value));
            }

            var danhSach = new List<dynamic>();

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters.Any())
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            danhSach.Add(new
                            {
                                MaPhieuNhap = reader["MaPhieuNhap"].ToString(),
                                TenNCC = reader["TenNCC"].ToString(),
                                TenKho = reader["TenKho"].ToString(),
                                NgayNhap = Convert.ToDateTime(reader["NgayNhap"]),
                                NguoiNhap = reader["NguoiNhap"].ToString(),
                                GhiChu = reader["GhiChu"] != DBNull.Value ? reader["GhiChu"].ToString() : "",
                                MaHang = reader["MaHang"] != DBNull.Value ? reader["MaHang"].ToString() : "",
                                SoLuong = reader["SoLuong"] != DBNull.Value ? Convert.ToInt32(reader["SoLuong"]) : 0,
                                DonGia = reader["DonGia"] != DBNull.Value ? Convert.ToDecimal(reader["DonGia"]) : 0,
                                ThanhTien = reader["ThanhTien"] != DBNull.Value ? Convert.ToDecimal(reader["ThanhTien"]) : 0,
                            });
                        }
                    }
                }
            }

            return danhSach;
        }
    }
}
