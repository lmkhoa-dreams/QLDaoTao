using Microsoft.AspNetCore.Mvc;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MonHocController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public MonHocController(QLDaoTaoContext context)
        {
            _context = context;
        }

        // 1. Danh sách môn học
        public IActionResult Index()
        {
            var danhSachMon = _context.MonHocs.ToList();
            return View(danhSachMon);
        }

        // 2. Chi tiết môn học
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var mon = _context.MonHocs.Find(id);
            if (mon == null) return NotFound();
            return View(mon);
        }

        // 3. Thêm môn
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MonHoc mon)
        {
            try
            {
                if (_context.MonHocs.Any(x => x.MaMon == mon.MaMon))
                {
                    ViewBag.Error = "Mã môn học này đã tồn tại!";
                    return View(mon);
                }
                _context.MonHocs.Add(mon);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi: " + ex.Message;
                return View(mon);
            }
        }

        // 4. Sửa
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var mon = _context.MonHocs.Find(id);
            if (mon == null) return NotFound();
            return View(mon);
        }

        [HttpPost]
        public IActionResult Edit(MonHoc mon)
        {
            try
            {
                var existingMon = _context.MonHocs.Find(mon.MaMon);
                if (existingMon == null) return NotFound();

                existingMon.TenMon = mon.TenMon;
                existingMon.SoTinChi = mon.SoTinChi;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi cập nhật: " + ex.Message;
                return View(mon);
            }
        }

        // 5. Xóa
        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                var mon = _context.MonHocs.Find(id);
                if (mon != null)
                {
                    _context.MonHocs.Remove(mon);
                    _context.SaveChanges();
                }
            }
            catch
            {
                TempData["Error"] = "Không thể xóa! Môn học này đang được tham chiếu ở Học phần mở hoặc Môn tiên quyết.";
            }
            return RedirectToAction("Index");
        }
    }
}