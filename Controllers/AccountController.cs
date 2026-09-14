using Microsoft.AspNetCore.Mvc;
using QLSinhVien.Models;

namespace QLSinhVien.Controllers
{
    public class AccountController : Controller
    {
        // Khai báo biến để xài Database
        private readonly QLDaoTaoContext _context;
        public AccountController(QLDaoTaoContext context)
        {
            _context = context;
        }

        // 1. ĐĂNG NHẬP
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // 1. Database tìm tài khoản
            var account = _context.TaiKhoans.FirstOrDefault(t => t.TenDangNhap == username && t.MatKhau == password);

            // 2. Nếu sai pass hoặc không thấy user
            if (account == null)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
                return View();
            }

            // 3. LƯU SESSION ĐĂNG NHẬP
            HttpContext.Session.SetString("Username", account.TenDangNhap);
            HttpContext.Session.SetString("Role", account.VaiTro);

            if (account.VaiTro == "GiangVien")
            {
                var gv = _context.GiangViens.FirstOrDefault(x => x.TenDangNhap == account.TenDangNhap);
                if (gv != null) HttpContext.Session.SetString("FullName", gv.HoTen);
            }
            else if (account.VaiTro == "SinhVien")
            {
                var sv = _context.SinhViens.FirstOrDefault(x => x.Mssv == account.TenDangNhap);
                if (sv != null) HttpContext.Session.SetString("FullName", sv.HoTen);
            }
            else
            {
                // Nếu là Admin
                HttpContext.Session.SetString("FullName", "Quản trị viên");
            }

            // 4. Phân luồng theo vai trò
            switch (account.VaiTro)
            {
                case "Admin":
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });

                case "GiangVien":
                    return RedirectToAction("Index", "Home", new { area = "GiangVien" });

                case "SinhVien":
                    return RedirectToAction("Index", "SinhVien", new { area = "SinhVien" });

                default:
                    ViewBag.Error = "Tài khoản không được cấp quyền truy cập!";
                    return View();
            }
        }

        // 2. ĐĂNG XUẤT
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Role");
            return RedirectToAction("Login", "Account");
        }

        // 3. ĐỔI MẬT KHẨU
        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword)
        {
            // 1. Kiểm tra xem người dùng đã đăng nhập chưa
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                // Báo lỗi chưa đăng nhập
                return Json(new { success = false, message = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại!" });
            }

            // 2. Lấy thông tin tài khoản từ DB
            var account = _context.TaiKhoans.FirstOrDefault(t => t.TenDangNhap == username);
            if (account == null)
            {
                return Json(new { success = false, message = "Không tìm thấy tài khoản trong hệ thống!" });
            }

            // 3. Kiểm tra mật khẩu cũ xem có khớp không
            if (account.MatKhau != oldPassword)
            {
                return Json(new { success = false, message = "Mật khẩu cũ không chính xác!" });
            }

            // 4. Cập nhật mật khẩu mới và lưu vào DB
            try
            {
                account.MatKhau = newPassword;
                _context.SaveChanges();
                return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}