using Microsoft.AspNetCore.Mvc;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.GiangVien.Controllers
{
    [Area("GiangVien")]
    public class DiemController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public DiemController(QLDaoTaoContext context)
        {
            _context = context;
        }

        public IActionResult Index(string hocKy)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account", new { area = "" });

            var giangVien = _context.GiangViens.FirstOrDefault(x => x.TenDangNhap == username);
            if (giangVien == null) return NotFound();

            if (string.IsNullOrEmpty(hocKy))
            {
                hocKy = "HK1_2026_2027";
            }
            ViewBag.HocKyHienTai = hocKy;

            // Lấy danh sách các Học kỳ mà giảng viên này có dạy
            ViewBag.DsHocKy = _context.HocPhanMos
                                      .Where(x => x.MaGv == giangVien.MaGv)
                                      .Select(x => x.HocKy)
                                      .Distinct()
                                      .ToList();

            // Lấy lớp học kỳ đang chọn
            var danhSachLop = _context.HocPhanMos
                                      .Where(x => x.MaGv == giangVien.MaGv && x.HocKy == hocKy)
                                      .ToList();

            ViewBag.TuDienMonHoc = _context.MonHocs.ToDictionary(m => m.MaMon, m => m.TenMon);

            return View(danhSachLop);
        }

        [HttpGet]
        public IActionResult NhapDiem(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var lopHocPhan = _context.HocPhanMos.FirstOrDefault(x => x.MaHp == id);
            if (lopHocPhan == null) return NotFound("Không tìm thấy Lớp học phần này!");

            ViewBag.LopHocPhan = lopHocPhan;
            ViewBag.TenMon = _context.MonHocs.FirstOrDefault(m => m.MaMon == lopHocPhan.MaMon)?.TenMon;

            var danhSachDiem = (from kq in _context.KetQuaHocTaps
                                join sv in _context.SinhViens on kq.Mssv equals sv.Mssv
                                where kq.MaHp == id
                                select new ChiTietDiemViewModel
                                {
                                    Id = kq.Id,
                                    Mssv = sv.Mssv,
                                    HoTen = sv.HoTen,
                                    DiemQuaTrinh = kq.DiemQuaTrinh,
                                    DiemGiuaKy = kq.DiemGiuaKy,
                                    DiemThi = kq.DiemThi,
                                    DiemTongKet = kq.DiemTongKet
                                }).ToList();

            return View(danhSachDiem);
        }

        [HttpPost]
        public IActionResult LuuDiem(List<ChiTietDiemViewModel> danhSachDiem, string maHp)
        {
            if (danhSachDiem != null && danhSachDiem.Any())
            {
                foreach (var item in danhSachDiem)
                {
                    var kq = _context.KetQuaHocTaps.Find(item.Id);
                    if (kq != null)
                    {
                        kq.DiemQuaTrinh = item.DiemQuaTrinh;
                        kq.DiemGiuaKy = item.DiemGiuaKy;
                        kq.DiemThi = item.DiemThi;

                        // Tính điểm tổng kết
                        if (item.DiemQuaTrinh.HasValue && item.DiemGiuaKy.HasValue && item.DiemThi.HasValue)
                        {
                            kq.DiemTongKet = Math.Round((item.DiemQuaTrinh.Value * 0.2) + (item.DiemGiuaKy.Value * 0.3) + (item.DiemThi.Value * 0.5), 1);
                        }
                    }
                }
                _context.SaveChanges();
                TempData["SuccessMsg"] = "Đã lưu bảng điểm thành công!";
            }
            return RedirectToAction("NhapDiem", new { id = maHp });
        }
    }

    public class ChiTietDiemViewModel
    {
        public int Id { get; set; }
        public string? Mssv { get; set; }
        public string? HoTen { get; set; }
        public double? DiemQuaTrinh { get; set; }
        public double? DiemGiuaKy { get; set; }
        public double? DiemThi { get; set; }
        public double? DiemTongKet { get; set; }
    }
}