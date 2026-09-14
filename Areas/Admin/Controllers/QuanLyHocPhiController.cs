using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuanLyHocPhiController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public QuanLyHocPhiController(QLDaoTaoContext context)
        {
            _context = context;
        }

        // 1. GIAO DIỆN QUẢN LÝ THU HỌC PHÍ
        public IActionResult Index(string hocKy)
        {
            var adminUser = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(adminUser)) return RedirectToAction("Login", "Account", new { area = "" });

            if (string.IsNullOrEmpty(hocKy))
            {
                hocKy = "HK1_2026_2027";
            }
            ViewBag.HocKyHienTai = hocKy;
            ViewBag.DsHocKy = _context.HocPhanMos.Select(x => x.HocKy).Distinct().ToList();

            // Lấy toàn bộ dữ liệu đăng ký của Học kỳ hiện tại
            var dsKetQua = _context.KetQuaHocTaps
                .Include(k => k.MssvNavigation)
                .Include(k => k.MaHpNavigation)
                    .ThenInclude(hp => hp.MaMonNavigation)
                .Where(k => k.MaHpNavigation.HocKy == hocKy)
                .ToList();

            // Nhóm theo từng sinh viên
            var baoCaoHocPhi = dsKetQua
                .GroupBy(k => k.MssvNavigation)
                .Select(g => new HocPhiViewModel
                {
                    Mssv = g.Key.Mssv,
                    HoTen = g.Key.HoTen,
                    MaLop = g.Key.MaLopSh,
                    TongTinChi = g.Sum(x => x.MaHpNavigation?.MaMonNavigation?.SoTinChi ?? 0),
                    TrangThai = g.Any(x => x.TrangThai != "Đã thanh toán") ? "Chờ thanh toán" : "Đã thanh toán"
                })
                .OrderBy(x => x.TrangThai)
                .ToList();

            return View(baoCaoHocPhi);
        }

        // 2.  XÁC NHẬN THU TIỀN MẶT
        [HttpPost]
        public IActionResult XacNhanThuTien(string mssv, string hocKy)
        {
            var dsNoHocPhi = _context.KetQuaHocTaps
                .Include(k => k.MaHpNavigation)
                .Where(k => k.Mssv == mssv && k.MaHpNavigation.HocKy == hocKy && k.TrangThai != "Đã thanh toán")
                .ToList();

            foreach (var item in dsNoHocPhi)
            {
                item.TrangThai = "Đã thanh toán";
            }

            _context.SaveChanges();
            TempData["Success"] = $"Đã xác nhận thu tiền thành công cho sinh viên {mssv}!";
            return RedirectToAction("Index", new { hocKy = hocKy });
        }
    }


    public class HocPhiViewModel
    {
        public string Mssv { get; set; }
        public string HoTen { get; set; }
        public string MaLop { get; set; }
        public int TongTinChi { get; set; }
        public long TongTien => TongTinChi * 990000;
        public string TrangThai { get; set; }
    }
}