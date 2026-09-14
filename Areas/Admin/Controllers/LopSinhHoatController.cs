using Microsoft.AspNetCore.Mvc;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LopSinhHoatController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public LopSinhHoatController(QLDaoTaoContext context)
        {
            _context = context;
        }

        // 1. Danh sách lớp
        public IActionResult Index()
        {
            var danhSachLop = _context.LopSinhHoats.ToList();
            return View(danhSachLop);
        }

        // 2. Chi tiết lớp
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var lop = _context.LopSinhHoats.Find(id);
            if (lop == null) return NotFound();

            return View(lop);
        }

        // 3. Thêm lớp
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(LopSinhHoat lop)
        {
            try
            {
                if (_context.LopSinhHoats.Any(x => x.MaLopSh == lop.MaLopSh))
                {
                    ViewBag.Error = "Mã lớp sinh hoạt này đã tồn tại!";
                    return View(lop);
                }
                _context.LopSinhHoats.Add(lop);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi hệ thống: " + ex.Message;
                return View(lop);
            }
        }

        // 4. Sửa
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var lop = _context.LopSinhHoats.Find(id);
            if (lop == null) return NotFound();

            return View(lop);
        }

        [HttpPost]
        public IActionResult Edit(LopSinhHoat lop)
        {
            try
            {
                var existingLop = _context.LopSinhHoats.Find(lop.MaLopSh);
                if (existingLop == null) return NotFound();

                // Cập nhật thông tin
                existingLop.TenLop = lop.TenLop;
                existingLop.NienKhoa = lop.NienKhoa;
                existingLop.TenNganh = lop.TenNganh;
                existingLop.TenKhoa = lop.TenKhoa;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi cập nhật: " + ex.Message;
                return View(lop);
            }
        }

        // 5. Xóa
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var lop = _context.LopSinhHoats.Find(id);
            if (lop != null)
            {
                _context.LopSinhHoats.Remove(lop);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}