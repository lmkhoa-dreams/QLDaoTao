using Microsoft.AspNetCore.Mvc;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.GiangVien.Controllers
{
    [Area("GiangVien")]
    public class SinhVienController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public SinhVienController(QLDaoTaoContext context)
        {
            _context = context;
        }

        // 1. Danh sách lớp giảng viên dạy
        public IActionResult Index(string hocKy)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account", new { area = "" });

            var giangVien = _context.GiangViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (giangVien == null) return NotFound("Không tìm thấy thông tin giảng viên");

            // Mặc định là HK1
            if (string.IsNullOrEmpty(hocKy))
            {
                hocKy = "HK1_2026_2027";
            }
            ViewBag.HocKyHienTai = hocKy;

            // Lấy danh sách học kỳ của giảng viên 
            ViewBag.DsHocKy = _context.HocPhanMos
                                      .Where(x => x.MaGv == giangVien.MaGv)
                                      .Select(x => x.HocKy)
                                      .Distinct()
                                      .ToList();

            var danhSachLop = _context.HocPhanMos
                                      .Where(x => x.MaGv == giangVien.MaGv && x.HocKy == hocKy)
                                      .ToList();

            var danhSachMon = _context.MonHocs.ToList();
            ViewBag.TuDienMonHoc = danhSachMon.ToDictionary(m => m.MaMon, m => m.TenMon);

            return View(danhSachLop);
        }

        // 2. Danh Sách sinh viên trong lớp
        [HttpGet]
        public IActionResult DanhSach(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var lopHocPhan = _context.HocPhanMos.FirstOrDefault(x => x.MaHp == id);
            if (lopHocPhan == null) return NotFound();

            ViewBag.LopHocPhan = lopHocPhan;
            var sinhViens = (from kq in _context.KetQuaHocTaps
                             join sv in _context.SinhViens on kq.Mssv equals sv.Mssv
                             where kq.MaHp == id
                             select sv).ToList();

            return View(sinhViens);
        }

        // 3. Xem hồ sơ sinh viên
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var sv = _context.SinhViens.Find(id);
            if (sv == null) return NotFound();

            var lop = _context.LopSinhHoats.FirstOrDefault(x => x.TenLop == sv.MaLopSh);
            ViewBag.ChuyenNganh = lop != null ? lop.TenNganh : "Chưa xác định";

            return View(sv);
        }
    }
}