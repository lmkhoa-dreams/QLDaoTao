using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class KetQuaHocTapController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public KetQuaHocTapController(QLDaoTaoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. Kiểm tra đăng nhập
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // 2. Lấy thông tin Sinh viên 
            var sv = _context.SinhViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (sv == null)
            {
                return NotFound("Không tìm thấy thông tin sinh viên!");
            }

            ViewBag.SinhVien = sv;

            // 3. Lấy Bảng điểm, kết nối với Học phần
            var bangDiem = _context.KetQuaHocTaps
                                   .Include(k => k.MaHpNavigation)
                                       .ThenInclude(hp => hp.MaMonNavigation)
                                   .Where(k => k.Mssv == sv.Mssv)
                                   .ToList()
                                   .GroupBy(k => k.MaHpNavigation.HocKy)
                                   .OrderBy(g => g.Key)
                                   .ToList();

            return View(bangDiem);
        }
    }
}