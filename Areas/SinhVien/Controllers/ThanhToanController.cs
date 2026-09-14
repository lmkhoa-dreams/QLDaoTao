using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSinhVien.Models;
using System.Text;

namespace QLSinhVien.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class ThanhToanController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public ThanhToanController(QLDaoTaoContext context)
        {
            _context = context;
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC).Replace("Đ", "D").Replace("đ", "d");
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account", new { area = "" });

            var sv = _context.SinhViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (sv == null) return NotFound();

            ViewBag.SinhVien = sv;

            var dsNoHocPhi = _context.KetQuaHocTaps
                .Include(k => k.MaHpNavigation)
                .ThenInclude(hp => hp.MaMonNavigation)
                .Where(k => k.Mssv == sv.Mssv && k.TrangThai != "Đã thanh toán")
                .ToList();

            // Nội dung ck : tên + mssv + hk
            string hocKy = dsNoHocPhi.FirstOrDefault()?.MaHpNavigation?.HocKy ?? "";
            string tenKhongDau = RemoveDiacritics(sv.HoTen ?? "").ToUpper();
            ViewBag.NoiDungCK = $"{tenKhongDau} {sv.Mssv} {hocKy}".Trim();
            return View(dsNoHocPhi);
        }

        [HttpPost]
        public IActionResult XacNhan()
        {
            var username = HttpContext.Session.GetString("Username");
            var sv = _context.SinhViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (sv == null) return RedirectToAction("Login", "Account", new { area = "" });

            var dsNoHocPhi = _context.KetQuaHocTaps
                .Where(k => k.Mssv == sv.Mssv && k.TrangThai != "Đã thanh toán")
                .ToList();

            foreach (var item in dsNoHocPhi)
            {
                item.TrangThai = "Đã thanh toán";
            }

            _context.SaveChanges();

            TempData["Success"] = "Thanh toán thành công! Hệ thống đã ghi nhận học phí của bạn.";
            return RedirectToAction("Index");
        }
    }
}