using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QLSinhVien.Models;

namespace QLSinhVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly QLDaoTaoContext _context;

        public AdminController(QLDaoTaoContext context)
        {
            _context = context;
        }

        // 1. Xem danh sách
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username)) return RedirectToAction("Login", "Account", new { area = "" });

            var account = _context.TaiKhoans.FirstOrDefault(t => t.TenDangNhap == username);
            if (account == null || account.VaiTro != "Admin")
                return RedirectToAction("Login", "Account", new { area = "" });

            var danhSachSV = (from s in _context.SinhViens
                              join l in _context.LopSinhHoats on s.MaLopSh equals l.MaLopSh
                              select new
                              {
                                  s.Mssv,
                                  s.HoTen,
                                  s.GioiTinh,
                                  s.NgaySinh,
                                  l.TenLop,
                                  s.TrangThaiHocTap
                              }).ToList();

            ViewBag.DanhSachSV = danhSachSV;
            return View();
        }

        // 2. Chi tiết
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound("Không tìm thấy mã sinh viên!");

            // Dữ liệu của 2 bảng sv và lsh
            var info = (from s in _context.SinhViens
                        join l in _context.LopSinhHoats on s.MaLopSh equals l.MaLopSh into sl
                        from l in sl.DefaultIfEmpty()
                        where s.Mssv == id
                        select new
                        {
                            s.Mssv,
                            s.HoTen,
                            s.GioiTinh,
                            s.NgaySinh,
                            s.SoDienThoai,
                            s.Cccd,
                            s.NgayCap,
                            s.NoiCap,
                            s.DanToc,
                            s.TonGiao,
                            s.QuocTich,
                            s.KhuVuc,
                            s.DiaChiLienHe,
                            s.HoKhauThuongTru,
                            s.NoiSinh,
                            s.MaHoSo,
                            s.NgayVaoTruong,
                            s.EmailCaNhan,
                            s.EmailTruong,
                            s.TrangThaiHocTap,
                            // Xử lý null nếu chưa có lớp
                            TenLop = l != null ? l.TenLop : "Chưa cập nhật",
                            NienKhoa = l != null ? l.NienKhoa : "",
                            TenNganh = l != null ? l.TenNganh : "Chưa cập nhật",
                            TenKhoa = l != null ? l.TenKhoa : "Chưa cập nhật"
                        }).FirstOrDefault();

            if (info == null) return NotFound("Sinh viên không tồn tại trong hệ thống!");

            // 3. Thêm toàn bộ dữ liệu ra ViewBag
            ViewBag.MSSV = info.Mssv;
            ViewBag.HoTen = info.HoTen;
            ViewBag.GioiTinh = info.GioiTinh ? "Nam" : "Nữ";
            ViewBag.NgaySinh = info.NgaySinh.ToString("dd/MM/yyyy");
            ViewBag.Lop = info.TenLop;
            ViewBag.Khoa = info.TenKhoa;
            ViewBag.Nganh = info.TenNganh;

            // Xử lý khóa học thêm mssv
            if (!string.IsNullOrEmpty(info.Mssv) && info.Mssv.Length >= 2)
            {
                ViewBag.KhoaHoc = "Khóa " + info.Mssv.Substring(0, 2);
            }
            else
            {
                ViewBag.KhoaHoc = "Không xác định";
            }

            ViewBag.TrangThai = info.TrangThaiHocTap;
            ViewBag.MaHoSo = info.MaHoSo;
            ViewBag.NgayVaoTruong = info.NgayVaoTruong?.ToString("dd/MM/yyyy");

            ViewBag.CCCD = info.Cccd;
            ViewBag.NgayCap = info.NgayCap?.ToString("dd/MM/yyyy");
            ViewBag.NoiCap = info.NoiCap;
            ViewBag.DanToc = info.DanToc;
            ViewBag.TonGiao = info.TonGiao;
            ViewBag.QuocTich = info.QuocTich;
            ViewBag.KhuVuc = info.KhuVuc;
            ViewBag.DienThoai = info.SoDienThoai;
            ViewBag.EmailCaNhan = info.EmailCaNhan;
            ViewBag.EmailTruong = info.EmailTruong;
            ViewBag.DiaChi = info.DiaChiLienHe;
            ViewBag.HoKhau = info.HoKhauThuongTru;
            ViewBag.NoiSinh = info.NoiSinh;

            return View();
        }

        // 3. Thêm sinh viên
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MaLopSh = new SelectList(_context.LopSinhHoats, "MaLopSh", "TenLop");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Models.SinhVien sv)
        {
            try
            {
                if (_context.SinhViens.Any(x => x.Mssv == sv.Mssv))
                {
                    ViewBag.Error = "Mã số sinh viên này đã tồn tại!";
                    ViewBag.MaLopSh = new SelectList(_context.LopSinhHoats, "MaLopSh", "TenLop", sv.MaLopSh);
                    return View(sv);
                }

                // Tạo tài khoản mặc định
                var taiKhoan = new TaiKhoan
                {
                    TenDangNhap = sv.Mssv,
                    MatKhau = "1111",
                    VaiTro = "SinhVien"
                };
                _context.TaiKhoans.Add(taiKhoan);

                // Lưu sinh viên
                sv.TenDangNhap = sv.Mssv;
                sv.NgayVaoTruong = DateOnly.FromDateTime(DateTime.Now);
                sv.TrangThaiHocTap = "Đang học";
                sv.EmailTruong = sv.Mssv + "@ntt.edu.vn";

                _context.SinhViens.Add(sv);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi hệ thống: " + ex.Message;
                ViewBag.MaLopSh = new SelectList(_context.LopSinhHoats, "MaLopSh", "TenLop", sv.MaLopSh);
                return View(sv);
            }
        }

        // 4. Chỉnh sửa
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var sv = _context.SinhViens.Find(id);
            if (sv == null) return NotFound();

            ViewBag.MaLopSh = new SelectList(_context.LopSinhHoats, "MaLopSh", "TenLop", sv.MaLopSh);
            return View(sv);
        }

        [HttpPost]
        public IActionResult Edit(Models.SinhVien sv)
        {
            try
            {
                var existingSv = _context.SinhViens.Find(sv.Mssv);
                if (existingSv == null) return NotFound();

                // Cập nhật thông tin
                existingSv.HoTen = sv.HoTen;
                existingSv.TrangThaiHocTap = sv.TrangThaiHocTap;
                existingSv.NgaySinh = sv.NgaySinh;
                existingSv.GioiTinh = sv.GioiTinh;
                existingSv.MaLopSh = sv.MaLopSh;
                existingSv.MaHoSo = sv.MaHoSo;
                existingSv.SoDienThoai = sv.SoDienThoai;
                existingSv.EmailCaNhan = sv.EmailCaNhan;
                existingSv.Cccd = sv.Cccd;
                existingSv.NgayCap = sv.NgayCap;
                existingSv.NoiCap = sv.NoiCap;
                existingSv.DanToc = sv.DanToc;
                existingSv.TonGiao = sv.TonGiao;
                existingSv.NoiSinh = sv.NoiSinh;
                existingSv.KhuVuc = sv.KhuVuc;
                existingSv.DiaChiLienHe = sv.DiaChiLienHe;
                existingSv.HoKhauThuongTru = sv.HoKhauThuongTru;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi cập nhật: " + ex.Message;
                ViewBag.MaLopSh = new SelectList(_context.LopSinhHoats, "MaLopSh", "TenLop", sv.MaLopSh);
                return View(sv);
            }
        }

        // 5. Xóa)
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var sv = _context.SinhViens.Find(id);
            if (sv != null)
            {
                var kq = _context.KetQuaHocTaps.Where(x => x.Mssv == id).ToList();
                _context.KetQuaHocTaps.RemoveRange(kq);

                // Xóa sv và tài khoản
                _context.SinhViens.Remove(sv);
                var tk = _context.TaiKhoans.Find(sv.TenDangNhap);
                if (tk != null) _context.TaiKhoans.Remove(tk);

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}