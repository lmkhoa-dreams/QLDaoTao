using Microsoft.AspNetCore.Mvc;
using QLSinhVien.Models;

namespace QLSinhVien.Controllers
{
    [Area("SinhVien")]
    public class SinhVienController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public SinhVienController(QLDaoTaoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. Lấy tên tài khoản đang đăng nhập từ Session
            var username = HttpContext.Session.GetString("Username");

            // Nếu chưa đăng nhập quay về trang Login
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Lấy Database, nối bảng SinhVien và LopSinhHoat để lấy đủ thông tin
            var info = (from s in _context.SinhViens
                        join l in _context.LopSinhHoats on s.MaLopSh equals l.MaLopSh
                        where s.TenDangNhap == username
                        select new
                        {
                            s.Mssv,
                            s.HoTen,
                            s.GioiTinh,
                            s.NgaySinh,
                            l.MaLopSh,
                            l.NienKhoa,
                            l.TenNganh
                        }).FirstOrDefault();

            // 3. Đẩy dữ liệu ra View thông qua ViewBag
            if (info != null)
            {
                ViewBag.MSSV = info.Mssv;
                ViewBag.HoTen = info.HoTen;
                ViewBag.GioiTinh = info.GioiTinh ? "Nam" : "Nữ";
                ViewBag.NgaySinh = info.NgaySinh.ToString("dd/MM/yyyy");
                ViewBag.LopHoc = info.MaLopSh;
                ViewBag.KhoaHoc = "Khóa " + info.NienKhoa.Substring(0, 4);
                ViewBag.Nganh = info.TenNganh;


                var tenParts = info.HoTen.Split(' ');
                ViewBag.AvatarChar = tenParts.Last().Substring(0, 1).ToUpper();
            }

            return View();
        }

        public IActionResult HoSo()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var sinhVien = _context.SinhViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (sinhVien == null) return NotFound("Không tìm thấy thông tin sinh viên!");

            // Móc thêm tên Lớp sinh hoạt và Tên Khoa/Ngành đem ra View nếu cần
            var lopSH = _context.LopSinhHoats.FirstOrDefault(l => l.MaLopSh == sinhVien.MaLopSh);
            ViewBag.LopSinhHoat = lopSH;

            return View(sinhVien);
        }
    }
}