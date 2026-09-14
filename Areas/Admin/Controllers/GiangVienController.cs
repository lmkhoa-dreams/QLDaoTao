using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GiangVienController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public GiangVienController(QLDaoTaoContext context)
        {
            _context = context;
        }

        // Dách sách giảng viên
        public IActionResult Index()
        {
            // Lấy danh sách giảng viên
            var danhSachGV = _context.GiangViens.ToList();
            return View(danhSachGV);
        }

        // Chi tiết
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var gv = _context.GiangViens.Find(id);
            if (gv == null) return NotFound();

            return View(gv);
        }

        // Thêm gv
        [HttpGet]
        public IActionResult Create()
        {
            var danhSachKhoa = _context.LopSinhHoats.Select(l => l.TenKhoa).Distinct().ToList();
            ViewBag.DanhSachKhoa = new SelectList(danhSachKhoa);
            return View();
        }

        [HttpPost]
        public IActionResult Create(Models.GiangVien gv)
        {
            try
            {
                // Ktra có trùng hay không
                if (_context.GiangViens.Any(x => x.MaGv == gv.MaGv))
                {
                    ViewBag.Error = "Mã giảng viên này đã tồn tại trong hệ thống!";
                    var dsKhoa = _context.LopSinhHoats.Select(l => l.TenKhoa).Distinct().ToList();
                    ViewBag.DanhSachKhoa = new SelectList(dsKhoa);
                    return View(gv);
                }

                // Tạo tài khoản mặc định
                var taiKhoan = new TaiKhoan
                {
                    TenDangNhap = gv.MaGv,
                    MatKhau = "1111",
                    VaiTro = "GiangVien"
                };
                _context.TaiKhoans.Add(taiKhoan);

                // Lưu tài khoản
                gv.TenDangNhap = gv.MaGv;

                _context.GiangViens.Add(gv);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi hệ thống: " + ex.Message;
                var dsKhoa = _context.LopSinhHoats.Select(l => l.TenKhoa).Distinct().ToList();
                ViewBag.DanhSachKhoa = new SelectList(dsKhoa);
                return View(gv);
            }
        }

        // Sửa thông tin gv
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var gv = _context.GiangViens.Find(id);
            if (gv == null) return NotFound();

            return View(gv);
        }

        [HttpPost]
        public IActionResult Edit(Models.GiangVien gv)
        {
            try
            {
                var existingGv = _context.GiangViens.Find(gv.MaGv);
                if (existingGv == null) return NotFound();
                existingGv.HoTen = gv.HoTen;
                existingGv.Email = gv.Email;
                existingGv.SoDienThoai = gv.SoDienThoai;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi cập nhật: " + ex.Message;
                return View(gv);
            }
        }

        // Xóa
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var gv = _context.GiangViens.Find(id);
            if (gv != null)
            {
                _context.GiangViens.Remove(gv);
                var tk = _context.TaiKhoans.Find(gv.TenDangNhap);
                if (tk != null) _context.TaiKhoans.Remove(tk);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}