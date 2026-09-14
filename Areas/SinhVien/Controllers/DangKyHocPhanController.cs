using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.SinhVien.Controllers
{
    [Area("SinhVien")]
    public class DangKyHocPhanController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public DangKyHocPhanController(QLDaoTaoContext context)
        {
            _context = context;
        }
        public IActionResult Index(string hocKy)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var sv = _context.SinhViens
                             .Include(s => s.MaLopShNavigation)
                             .FirstOrDefault(x => x.TenDangNhap == username);

            if (sv == null || sv.MaLopShNavigation == null || string.IsNullOrEmpty(sv.MaLopShNavigation.TenKhoa))
            {
                return Content("Lỗi: Sinh viên chưa được phân lớp hoặc lớp chưa có thông tin Khoa!");
            }

            // Nếu vừa vào trang, mặc định gán là HK1
            if (string.IsNullOrEmpty(hocKy))
            {
                hocKy = "HK1_2026_2027";
            }
            ViewBag.HocKyHienTai = hocKy;

            // Check điều kiện
            var dsLichSuToanBo = _context.KetQuaHocTaps
                                         .Include(k => k.MaHpNavigation)
                                         .ThenInclude(hp => hp.MaMonNavigation)
                                         .Where(k => k.Mssv == sv.Mssv)
                                         .ToList();

            // Môn đã đăng ký sẽ ẩn đi
            var dsMaMonDaTungDangKy = dsLichSuToanBo
                .Where(k => k.MaHpNavigation != null && k.MaHpNavigation.MaMon != null)
                .Select(k => k.MaHpNavigation.MaMon.Trim())
                .Distinct()
                .ToList();

            // Môn đã học xong sẽ mở môn tiếp theo 
            var dsMaMonDaHocXong = dsLichSuToanBo
                .Where(k => k.MaHpNavigation != null
                         && k.MaHpNavigation.MaMon != null
                         && k.DiemTongKet.HasValue)
                .Select(k => k.MaHpNavigation.MaMon.Trim())
                .Distinct()
                .ToList();

            ViewBag.DsMaMonDaHoc = dsMaMonDaHocXong;

            // Chỉ lấy dữ liệu đã chọn
            var dsDaDangKyHocKyNay = dsLichSuToanBo
                .Where(k => k.MaHpNavigation != null && k.MaHpNavigation.HocKy == hocKy)
                .ToList();
            ViewBag.DsDaDangKy = dsDaDangKyHocKyNay;
            var dsLopMo = _context.HocPhanMos
                                  .Include(h => h.MaMonNavigation)
                                  .Where(h => h.MaMonNavigation.TenKhoa == sv.MaLopShNavigation.TenKhoa
                                           && h.HocKy == hocKy
                                           && h.MaMon != null
                                           && !dsMaMonDaTungDangKy.Contains(h.MaMon.Trim()))
                                  .ToList()
                                  .GroupBy(h => h.MaMon.Trim())
                                  .Select(g => g.First())
                                  .ToList();

            var dictTienQuyet = _context.MonTienQuyets
                .Where(t => t.MaMon != null && t.MaMonTruoc != null)
                .ToList()
                .GroupBy(t => t.MaMon.Trim())
                .ToDictionary(g => g.Key, g => g.Select(x => x.MaMonTruoc.Trim()).ToList());

            ViewBag.DictTienQuyet = dictTienQuyet;

            return View(dsLopMo);
        }

        [HttpGet]
        public IActionResult LayDanhSachLop(string maMon, string hocKy)
        {
            var dsLop = _context.HocPhanMos
                .Include(h => h.MaMonNavigation)
                .Include(h => h.MaGvNavigation)
                .Where(h => h.MaMon == maMon && h.HocKy == hocKy)
                .Select(h => new
                {
                    maHp = h.MaHp,
                    tenMon = h.MaMonNavigation.TenMon,
                    siSoToiDa = h.SiSoToiDa,
                    siSoHienTai = h.SiSoHienTai,
                    thu = h.Thu,
                    tietBatDau = h.TietBatDau,
                    tietKetThuc = h.TietKetThuc,
                    phongHoc = h.PhongHoc,
                    tenGv = h.MaGvNavigation != null ? h.MaGvNavigation.HoTen : h.MaGv
                })
                .ToList();

            return Json(dsLop);
        }

        // Lưu vào database
        [HttpPost]
        public IActionResult LuuDangKy(string MaHP, string hocKy)
        {
            if (string.IsNullOrEmpty(MaHP))
            {
                TempData["Error"] = "Vui lòng chọn 1 lớp học phần trước khi đăng ký!";
                return RedirectToAction("Index", new { hocKy = hocKy });
            }

            var username = HttpContext.Session.GetString("Username");
            var sv = _context.SinhViens.FirstOrDefault(x => x.TenDangNhap == username);

            var lopHP = _context.HocPhanMos.FirstOrDefault(x => x.MaHp == MaHP);
            if (lopHP == null) return NotFound();

            // Ktra xme đã đăng ký lớp nào hay chưa
            bool daHocMonNay = _context.KetQuaHocTaps
                .Include(k => k.MaHpNavigation)
                .Any(k => k.Mssv == sv.Mssv && k.MaHpNavigation.MaMon == lopHP.MaMon);

            if (daHocMonNay)
            {
                TempData["Error"] = "Bạn đã đăng ký môn học này rồi, không thể đăng ký thêm!";
                return RedirectToAction("Index", new { hocKy = hocKy });
            }

            if (lopHP.SiSoHienTai >= lopHP.SiSoToiDa)
            {
                TempData["Error"] = "Lớp đã đầy sĩ số!";
                return RedirectToAction("Index", new { hocKy = hocKy });
            }

            var dangKyMoi = new KetQuaHocTap
            {
                Mssv = sv.Mssv,
                MaHp = MaHP,
                TrangThai = "Đăng ký thành công"
            };
            _context.KetQuaHocTaps.Add(dangKyMoi);

            lopHP.SiSoHienTai += 1;
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công!";
            return RedirectToAction("Index", new { hocKy = hocKy });
        }
    }
}