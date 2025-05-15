using DAL.Entities.DangkyDangnhap;
using DAL.Helpers;
using DTO.DTO_DangkyDangnhap;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class NguoiDungBLL
    {
        private readonly NguoiDungDAL _dal = new NguoiDungDAL();

        // Đăng ký người dùng mới
        public bool DangKy(NguoiDung user)
        {
            if (_dal.KiemTraTonTai(user.TenDangNhap))
                return false; // Tên đã tồn tại

            return _dal.ThemNguoiDung(user);
        }

        // Đăng nhập trả về bool (nếu muốn dùng đơn giản)
        public bool KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            return _dal.KiemTraDangNhap(tenDangNhap, matKhau);
        }

        // Đăng nhập trả về đối tượng và kiểm tra khóa tài khoản
        public NguoiDung DangNhap(string tenDangNhap, string matKhau)
        {
            var nd = _dal.LayNguoiDungTheoTenDangNhap(tenDangNhap);
            if (nd == null || nd.MatKhau != matKhau)
                return null;

            if (!nd.TrangThai)
                throw new Exception("Tài khoản đang bị khóa!");

            return nd;
        }

        public bool KiemTraTonTai(string tenDangNhap)
        {
            return _dal.KiemTraTonTai(tenDangNhap);
        }

        public NguoiDung LayThongTinNguoiDung(string tenDangNhap)
        {
            return _dal.LayThongTinNguoiDung(tenDangNhap);
        }

        public List<NguoiDung> LayTatCaNguoiDung()
        {
            return _dal.LayTatCaNguoiDung();
        }

        public bool CapNhatNguoiDung(NguoiDung user)
        {
            return _dal.CapNhatNguoiDung(user);
        }

        public bool XoaNguoiDung(string tenDangNhap)
        {
            return _dal.XoaNguoiDung(tenDangNhap);
        }

        public bool ResetMatKhau(string tenDangNhap)
        {
            return _dal.ResetMatKhau(tenDangNhap);
        }
    }
}
