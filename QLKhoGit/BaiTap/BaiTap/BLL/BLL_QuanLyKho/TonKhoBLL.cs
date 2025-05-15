using DAL;
using DTO.DTO_QuanLyKho;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BLL_QuanLyKho
{
    public class TonKhoBLL
    {
        private readonly TonKhoDAL _dal;

        public TonKhoBLL()
        {
            _dal = new TonKhoDAL();
        }

        public List<TonKhoDTO> LayDanhSachTonKho()
        {
            return _dal.GetAll();
        }

        public int LayTongSoKho()
        {
            return _dal.GetAll().Select(t => t.MaKho).Distinct().Count();
        }

        public decimal TinhTongGiaTriTonKho()
        {
            return _dal.GetAll().Sum(t => t.SoLuong * t.DonGia);
        }

        public int LaySoLuongTon(string maHang, string maKho)
        {
            var tonKhoDAL = new TonKhoDAL();
            return tonKhoDAL.LaySoLuongTon(maHang, maKho);
        }

        public List<int> LayDuLieuNhap(int days)
        {
            return _dal.GetImportData(days);
        }

        public List<int> LayDuLieuXuat(int days)
        {
            return _dal.GetExportData(days);
        }

        public TonKhoDTO GetById(string maTonKho)
        {
            var result = _dal.GetById(maTonKho);
            if (result == null)
                throw new Exception($"Không tìm thấy tồn kho với mã {maTonKho}.");
            return result;
        }

        public bool ThemTonKho(TonKhoDTO tonKho)
        {
            if (string.IsNullOrEmpty(tonKho.MaHang) || string.IsNullOrEmpty(tonKho.MaKho))
                throw new ArgumentException("Mã hàng và mã kho không được để trống.");

            if (tonKho.SoLuong < 0)
                throw new ArgumentException("Số lượng không được nhỏ hơn 0.");

            if (string.IsNullOrEmpty(tonKho.MaTonKho))
                tonKho.MaTonKho = TaoMaTonKhoTuDong();

            return _dal.ThemTonKho(tonKho);
        }

        private string TaoMaTonKhoTuDong()
        {
            var danhSach = _dal.GetAll();
            int soCuoi = 1;

            if (danhSach.Any())
            {
                var maCuoi = danhSach.Max(t => t.MaTonKho);
                if (maCuoi.Length > 2 && int.TryParse(maCuoi.Substring(2), out int so))
                    soCuoi = so + 1;
            }

            return $"TK{soCuoi:D3}";
        }

        public bool SuaTonKho(TonKhoDTO tonKho)
        {
            var existing = _dal.GetById(tonKho.MaTonKho);
            if (existing == null)
                throw new Exception($"Không tìm thấy tồn kho với mã {tonKho.MaTonKho}.");

            return _dal.SuaTonKho(tonKho);
        }

        public bool XoaTonKho(string maTonKho)
        {
            return _dal.XoaTonKho(maTonKho);
        }

        public bool CapNhatTonKhoKhiNhap(string maHang, string maKho, int soLuongNhap, decimal donGiaNhap)
        {
            var tonKhoDAL = new TonKhoDAL();
            var danhSachTonKho = tonKhoDAL.GetAll();

            var tonKhoHienTai = danhSachTonKho
                .FirstOrDefault(t => t.MaHang == maHang && t.MaKho == maKho);

            if (tonKhoHienTai != null)
            {
                tonKhoHienTai.SoLuong += soLuongNhap;
                tonKhoHienTai.NgayCapNhat = DateTime.Now;
                return tonKhoDAL.SuaTonKho(tonKhoHienTai);
            }
            else
            {
                var tonKhoMoi = new TonKhoDTO
                {
                    MaTonKho = TaoMaTonKhoTuDong(),
                    MaHang = maHang,
                    MaKho = maKho,
                    SoLuong = soLuongNhap,
                    DonGia = donGiaNhap,
                    NgayCapNhat = DateTime.Now
                };
                return tonKhoDAL.ThemTonKho(tonKhoMoi);
            }
        }

        public bool CapNhatTonKhoKhiXuat(string maHang, string maKho, int soLuongXuat)
        {
            var tonKhoDAL = new TonKhoDAL();
            var soLuongHienTai = tonKhoDAL.LaySoLuongTon(maHang, maKho);

            if (soLuongHienTai < soLuongXuat)
                throw new Exception("Số lượng tồn kho không đủ để xuất!");

            return tonKhoDAL.CapNhatTonKhoKhiXuat(maHang, maKho, soLuongXuat);
        }

        public bool CapNhatTonKhoKhiXoaNhap(string maHang, string maKho, int soLuongBiXoa)
        {
            var tonKhoDAL = new TonKhoDAL();
            return tonKhoDAL.CapNhatTonKhoKhiXuat(maHang, maKho, soLuongBiXoa);
        }

        public bool CapNhatTonKhoKhiSuaNhap(string maHang, string maKho, int soLuongCu, int soLuongMoi)
        {
            var tonKhoDAL = new TonKhoDAL();
            int chenhLech = soLuongMoi - soLuongCu;

            if (chenhLech > 0)
                return tonKhoDAL.CapNhatTonKhoKhiNhap(maHang, maKho, chenhLech);
            else if (chenhLech < 0)
                return tonKhoDAL.CapNhatTonKhoKhiXuat(maHang, maKho, -chenhLech);
            else
                return true;
        }

        public List<(string Ngay, int Nhap, int Xuat)> LaySoLieuNhapXuat30Ngay()
        {
            var nhapData = _dal.GetImportDataByDay(30);
            var xuatData = _dal.GetExportDataByDay(30);
            var dict = new Dictionary<DateTime, (int Nhap, int Xuat)>();

            foreach (var item in nhapData)
                dict[item.Ngay] = (item.TongNhap, 0);

            foreach (var item in xuatData)
            {
                if (dict.ContainsKey(item.Ngay))
                    dict[item.Ngay] = (dict[item.Ngay].Nhap, item.TongXuat);
                else
                    dict[item.Ngay] = (0, item.TongXuat);
            }

            return dict
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => (kvp.Key.ToString("dd/MM"), kvp.Value.Nhap, kvp.Value.Xuat))
                .ToList();
        }
    }
}