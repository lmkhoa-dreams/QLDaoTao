using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class ThongKeDiemDanhController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public ThongKeDiemDanhController(QLDaoTaoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Lấy tài khoản đang đăng nhập từ Session
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var sinhVien = _context.SinhViens.FirstOrDefault(s => s.TenDangNhap == username || s.Mssv == username);

            // Xem thống kê điểm danh
            string mssvChuan = sinhVien.Mssv;
            var dsDiemDanh = _context.KetQuaHocTaps
                .Include(k => k.MaHpNavigation)
                    .ThenInclude(h => h.MaMonNavigation)
                .Where(k => k.Mssv == mssvChuan)
                .OrderByDescending(k => k.MaHpNavigation.HocKy)
                .ToList();

            return View(dsDiemDanh);
        }
    }
}