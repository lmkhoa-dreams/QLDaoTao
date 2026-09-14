using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class LichHocController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public LichHocController(QLDaoTaoContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime? date)
        {
            // Kiểm tra đăng nhập
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account", new { area = "" });

            var sv = _context.SinhViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (sv == null) return NotFound("Không tìm thấy thông tin sinh viên!");

            // Chuyển lịch
            DateTime targetDate = date ?? DateTime.Now;
            int diff = (7 + (targetDate.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime thu2 = targetDate.AddDays(-1 * diff).Date;

            // Truyền dữ liệu ngày sang View để gán vào nút bấm
            ViewBag.CurrentDate = targetDate;
            ViewBag.PrevWeek = targetDate.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.NextWeek = targetDate.AddDays(7).ToString("yyyy-MM-dd");
            DateTime[] tuanNay = new DateTime[7];
            for (int i = 0; i < 7; i++)
            {
                tuanNay[i] = thu2.AddDays(i);
            }
            ViewBag.TuanNay = tuanNay;

            // Truy vấn các môn đã đăng ký
            var dsLichHoc = _context.KetQuaHocTaps
                                    .Include(k => k.MaHpNavigation)
                                        .ThenInclude(hp => hp.MaMonNavigation)
                                    .Include(k => k.MaHpNavigation)
                                        .ThenInclude(hp => hp.MaGvNavigation)
                                    .Where(k => k.Mssv == sv.Mssv)
                                    .Select(k => k.MaHpNavigation)
                                    .ToList();

            return View(dsLichHoc);
        }
    }
}