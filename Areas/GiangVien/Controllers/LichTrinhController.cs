using Microsoft.AspNetCore.Mvc;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.GiangVien.Controllers
{
    [Area("GiangVien")]
    public class LichTrinhController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public LichTrinhController(QLDaoTaoContext context)
        {
            _context = context;
        }

        public IActionResult Index(string hocKy)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account", new { area = "" });

            var giangVien = _context.GiangViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (giangVien == null) return NotFound("Không tìm thấy giảng viên");

            // Mặc định là HK1
            if (string.IsNullOrEmpty(hocKy))
            {
                hocKy = "HK1_2026_2027";
            }
            ViewBag.HocKyHienTai = hocKy;

            // Lấy danh sách học kỳ ra View
            ViewBag.DsHocKy = _context.HocPhanMos
                                      .Where(x => x.MaGv == giangVien.MaGv)
                                      .Select(x => x.HocKy)
                                      .Distinct()
                                      .ToList();

            // Chỉ lấy lịch dạy đang chọn
            var lichGiangDay = _context.HocPhanMos
                .Where(x => x.MaGv == giangVien.MaGv && x.Thu != null && x.HocKy == hocKy)
                .ToList();

            ViewBag.TuDienMonHoc = _context.MonHocs.ToDictionary(m => m.MaMon, m => m.TenMon);

            return View(lichGiangDay);
        }
    }
}