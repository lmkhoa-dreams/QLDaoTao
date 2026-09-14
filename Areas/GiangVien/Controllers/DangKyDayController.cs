using Microsoft.AspNetCore.Mvc;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.GiangVien.Controllers
{
    [Area("GiangVien")]
    public class DangKyDayController : Controller
    {
        private readonly QLDaoTaoContext _context;
        public DangKyDayController(QLDaoTaoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Lấy username đang đăng nhập
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account", new { area = "" });

            // Lấy thông tin để biết thuộc Khoa nào
            var gv = _context.GiangViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (gv == null) return NotFound("Lỗi: Không tìm thấy giảng viên!");

            // Kiểm tra database
            var dsMonHoc = _context.MonHocs
                                   .Where(m => m.TenKhoa == gv.TenKhoa)
                                   .ToList();
            ViewBag.TenKhoa = gv.TenKhoa ?? "Chưa có khoa";

            return View(dsMonHoc);
        }

        [HttpGet]
        public IActionResult MoLop(string maMon)
        {
            var monHoc = _context.MonHocs.FirstOrDefault(m => m.MaMon == maMon);
            if (monHoc == null) return NotFound();

            ViewBag.TenMon = monHoc.TenMon;
            ViewBag.MaMon = monHoc.MaMon;

            return View();
        }

        [HttpPost]
        public IActionResult MoLop(Models.HocPhanMo hp)
        {
            try
            {
                var username = HttpContext.Session.GetString("Username");
                var gv = _context.GiangViens.FirstOrDefault(x => x.TenDangNhap == username);

                // Tự động sinh mã Lớp học phần
                hp.MaHp = "HP_" + hp.MaMon + "_" + DateTime.Now.ToString("HHmmss");
                hp.MaGv = gv.MaGv;
                hp.SiSoHienTai = 0;

                if (hp.SiSoToiDa == 0) hp.SiSoToiDa = 60;
                _context.HocPhanMos.Add(hp);
                _context.SaveChanges();

                return RedirectToAction("Index", "SinhVien");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View(hp);
            }
        }
    }
}