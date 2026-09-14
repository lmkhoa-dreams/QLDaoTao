using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.GiangVien.Controllers
{
    [Area("GiangVien")]
    public class DiemDanhController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public DiemDanhController(QLDaoTaoContext context)
        {
            _context = context;
        }

        // 1. HIỂN THỊ DANH SÁCH LỚP GIẢNG VIÊN ĐANG DẠY
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account", new { area = "" });

            var gv = _context.GiangViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (gv == null) return NotFound();

            // Lấy các lớp GV này dạy
            var dsLop = _context.HocPhanMos
                .Include(h => h.MaMonNavigation)
                .Where(h => h.MaGv == gv.MaGv)
                .ToList();

            return View(dsLop);
        }

        // 2. MỞ DANH SÁCH SINH VIÊN CỦA LỚP ĐỂ ĐIỂM DANH
        public IActionResult ChiTiet(string maHp)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account", new { area = "" });

            var lop = _context.HocPhanMos
                .Include(h => h.MaMonNavigation)
                .FirstOrDefault(h => h.MaHp == maHp);

            if (lop == null) return NotFound();
            ViewBag.LopHocPhan = lop;

            // Lấy danh sách sinh viên đã đăng ký lớp này
            var dsSinhVien = _context.KetQuaHocTaps
                .Include(k => k.MssvNavigation)
                .Where(k => k.MaHp == maHp)
                .OrderBy(k => k.MssvNavigation.HoTen)
                .ToList();

            return View(dsSinhVien);
        }

        // 3. XỬ LÝ LƯU ĐIỂM DANH
        [HttpPost]
        public IActionResult Luu(string maHp)
        {
            var dsHocTap = _context.KetQuaHocTaps.Where(k => k.MaHp == maHp).ToList();

            foreach (var item in dsHocTap)
            {
                string trangThai = Request.Form["diemDanh_" + item.Mssv];
                if (trangThai == "vang")
                {
                    item.SoBuoiVang = (item.SoBuoiVang ?? 0) + 1;
                }
                else if (trangThai == "cophep")
                {
                    item.SoBuoiCoPhep = (item.SoBuoiCoPhep ?? 0) + 1;
                }
            }

            _context.SaveChanges();
            TempData["Success"] = "Đã chốt danh sách điểm danh hôm nay!";

            return RedirectToAction("ChiTiet", new { maHp = maHp });
        }
    }
}