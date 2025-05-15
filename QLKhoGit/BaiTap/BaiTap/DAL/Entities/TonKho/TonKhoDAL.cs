using DAL.Helpers;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class TonKhoDAL
    {
        private TonKhoDTO MapFromReader(IDataReader reader)
        {
            return new TonKhoDTO
            {
                MaTonKho = reader["MaTonKho"].ToString(),
                MaHang = reader["MaHang"].ToString(),
                MaKho = reader["MaKho"].ToString(),
                SoLuong = Convert.ToInt32(reader["SoLuong"]),
                NgayCapNhat = Convert.ToDateTime(reader["NgayCapNhat"]),
                TrangThai = reader["TrangThai"].ToString(),
                DonGia = Convert.ToDecimal(reader["DonGia"])
            };
        }

        public List<TonKhoDTO> GetAll()
        {
            var result = new List<TonKhoDTO>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = DatabaseHelper.CreateCommand("SELECT * FROM TonKho", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(MapFromReader(reader));
                    }
                }
            }
            return result;
        }

        public TonKhoDTO GetById(string maTonKho)
        {
            string query = "SELECT * FROM TonKho WHERE MaTonKho = @MaTonKho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaTonKho", maTonKho);

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

        public List<int> GetImportData(int days)
        {
            var result = new List<int>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = DatabaseHelper.CreateCommand(
                    "SELECT TOP (@Days) SoLuong FROM NhapKho ORDER BY NgayNhap DESC",
                    connection
                );
                command.AddParameter("@Days", days);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["SoLuong"]));
                    }
                }
            }
            return result;
        }

        public List<int> GetExportData(int days)
        {
            var result = new List<int>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var command = DatabaseHelper.CreateCommand(
                    "SELECT TOP (@Days) SoLuong FROM XuatKho ORDER BY NgayXuat DESC",
                    connection
                );
                command.AddParameter("@Days", days);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["SoLuong"]));
                    }
                }
            }
            return result;
        }

        public int LaySoLuongTon(string maHang, string maKho)
        {
            string query = "SELECT SoLuong FROM TonKho WHERE MaHang = @MaHang AND MaKho = @MaKho";
            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaHang", maHang);
                    command.Parameters.AddWithValue("@MaKho", maKho);

                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        public bool ThemTonKho(TonKhoDTO tonKho)
        {
            string query = @"INSERT INTO TonKho (MaTonKho, MaHang, MaKho, SoLuong, DonGia, NgayCapNhat)
VALUES (@MaTonKho, @MaHang, @MaKho, @SoLuong, @DonGia, @NgayCapNhat
)";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaTonKho", tonKho.MaTonKho);
                    command.Parameters.AddWithValue("@MaHang", tonKho.MaHang);
                    command.Parameters.AddWithValue("@MaKho", tonKho.MaKho);
                    command.Parameters.AddWithValue("@SoLuong", tonKho.SoLuong);
                    command.Parameters.AddWithValue("@DonGia", tonKho.DonGia);
                    command.Parameters.AddWithValue("@NgayCapNhat", tonKho.NgayCapNhat == DateTime.MinValue ? DateTime.Now : tonKho.NgayCapNhat);
                   // command.Parameters.AddWithValue("@TrangThai", tonKho.TrangThai);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }



        public bool SuaTonKho(TonKhoDTO tonKho)
        {
            string query = "UPDATE TonKho SET MaHang = @MaHang, MaKho = @MaKho, SoLuong = @SoLuong, DonGia = @DonGia, NgayCapNhat = @NgayCapNhat, TrangThai = @TrangThai WHERE MaTonKho = @MaTonKho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaTonKho", tonKho.MaTonKho);
                    command.Parameters.AddWithValue("@MaHang", tonKho.MaHang);
                    command.Parameters.AddWithValue("@MaKho", tonKho.MaKho);
                    command.Parameters.AddWithValue("@SoLuong", tonKho.SoLuong);
                    command.Parameters.AddWithValue("@DonGia", tonKho.DonGia);
                    command.Parameters.AddWithValue("@NgayCapNhat", tonKho.NgayCapNhat);
                    command.Parameters.AddWithValue("@TrangThai", tonKho.TrangThai);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool XoaTonKho(string maTonKho)
        {
            string query = "DELETE FROM TonKho WHERE MaTonKho = @MaTonKho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaTonKho", maTonKho);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool CapNhatTonKhoKhiNhap(string maHang, string maKho, int soLuongNhap)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                var checkTonKho = "SELECT COUNT(*) FROM TonKho WHERE MaHang = @MaHang AND MaKho = @MaKho";
                using (var command = new SqlCommand(checkTonKho, connection))
                {
                    command.Parameters.AddWithValue("@MaHang", maHang);
                    command.Parameters.AddWithValue("@MaKho", maKho);

                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {
                        var updateTonKho = "UPDATE TonKho SET SoLuong = SoLuong + @SoLuongNhap WHERE MaHang = @MaHang AND MaKho = @MaKho";
                        using (var updateCommand = new SqlCommand(updateTonKho, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@SoLuongNhap", soLuongNhap);
                            updateCommand.Parameters.AddWithValue("@MaHang", maHang);
                            updateCommand.Parameters.AddWithValue("@MaKho", maKho);
                            return updateCommand.ExecuteNonQuery() > 0;
                        }
                    }
                    else
                    {
                        var insertTonKho = "INSERT INTO TonKho (MaHang, MaKho, SoLuong) VALUES (@MaHang, @MaKho, @SoLuongNhap)";
                        using (var insertCommand = new SqlCommand(insertTonKho, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@MaHang", maHang);
                            insertCommand.Parameters.AddWithValue("@MaKho", maKho);
                            insertCommand.Parameters.AddWithValue("@SoLuongNhap", soLuongNhap);
                            return insertCommand.ExecuteNonQuery() > 0;
                        }
                    }
                }
            }
        }

        public bool CapNhatTonKhoKhiXuat(string maHang, string maKho, int soLuongXuat)
        {
            string query = @"
        UPDATE TonKho
        SET SoLuong = SoLuong - @SoLuongXuat
        WHERE MaHang = @MaHang AND MaKho = @MaKho";

            using (var connection = DatabaseHelper.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaHang", maHang);
                    command.Parameters.AddWithValue("@MaKho", maKho);
                    command.Parameters.AddWithValue("@SoLuongXuat", soLuongXuat);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool CapNhatTonKhoKhiXoaNhap(string maHang, string maKho, int soLuongBiXoa)
        {
            var tonKhoDAL = new TonKhoDAL();
            return tonKhoDAL.CapNhatTonKhoKhiXuat(maHang, maKho, soLuongBiXoa);
        }

        public List<(DateTime Ngay, int TongNhap)> GetImportDataByDay(int days)
        {
            var result = new List<(DateTime, int)>();
            string query = @"
        SELECT 
            CAST(nk.NgayNhap AS DATE) AS Ngay,
            SUM(ct.SoLuong) AS TongNhap
        FROM NhapKho nk
        JOIN ChiTietNhapKho ct ON nk.MaPhieuNhap = ct.MaPhieuNhap
        WHERE nk.NgayNhap >= DATEADD(DAY, -@Days, CAST(GETDATE() AS DATE))
        GROUP BY CAST(nk.NgayNhap AS DATE)
        ORDER BY Ngay";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Days", days);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ngay = Convert.ToDateTime(reader["Ngay"]);
                        var tongNhap = Convert.ToInt32(reader["TongNhap"]);
                        result.Add((ngay, tongNhap));
                    }
                }
            }

            return result;
        }

        public List<(DateTime Ngay, int TongXuat)> GetExportDataByDay(int days)
        {
            var result = new List<(DateTime, int)>();
            string query = @"
        SELECT 
            CAST(xk.NgayXuat AS DATE) AS Ngay,
            SUM(ct.SoLuong) AS TongXuat
        FROM XuatKho xk
        JOIN ChiTietXuatKho ct ON xk.MaPhieuXuat = ct.MaPhieuXuat
        WHERE xk.NgayXuat >= DATEADD(DAY, -@Days, CAST(GETDATE() AS DATE))
        GROUP BY CAST(xk.NgayXuat AS DATE)
        ORDER BY Ngay";

            using (var connection = DatabaseHelper.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Days", days);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ngay = Convert.ToDateTime(reader["Ngay"]);
                        var tongXuat = Convert.ToInt32(reader["TongXuat"]);
                        result.Add((ngay, tongXuat));
                    }
                }
            }

            return result;
        }


    }
}
