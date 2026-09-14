using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLSinhVien.Models;

namespace QLSinhVien.Controllers
{
    public class GiayToController : Controller
    {
        private readonly QLDaoTaoContext _context;
        private readonly ILogger<GiayToController> _logger;
        private readonly string _thuMucLuu;

        private const long MaxFileSize = 5 * 1024 * 1024;

        public GiayToController(
            QLDaoTaoContext context,
            IWebHostEnvironment environment,
            ILogger<GiayToController> logger)
        {
            _context = context;
            _logger = logger;

            _thuMucLuu = Path.Combine(
                environment.ContentRootPath,
                "App_Data",
                "GiayTo");
        }

        // Lấy tài khoản từ Session do chức năng đăng nhập thiết lập.
        private async Task<TaiKhoan?> LayTaiKhoan()
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrWhiteSpace(username))
                return null;

            return await _context.Set<TaiKhoan>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TenDangNhap == username);
        }

        private IActionResult VeDangNhap()
        {
            return RedirectToAction(
                "Login", "Account", new { area = "" });
        }

        // Trang giấy tờ
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var taiKhoan = await LayTaiKhoan();

            if (taiKhoan == null)
                return VeDangNhap();

            var query = _context.Set<GiayToSinhVien>()
                .AsNoTracking()
                .AsQueryable();

            if (taiKhoan.VaiTro == "SinhVien")
            {
                var sinhVien = await _context.Set<SinhVien>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x => x.TenDangNhap == taiKhoan.TenDangNhap);

                if (sinhVien == null)
                    return NotFound("Không tìm thấy hồ sơ sinh viên.");

                query = query.Where(x => x.Mssv == sinhVien.Mssv);
            }
            else if (taiKhoan.VaiTro != "Admin")
            {
                return StatusCode(403);
            }

            ViewBag.LaAdmin = taiKhoan.VaiTro == "Admin";

            var danhSach = await query
                .OrderByDescending(x => x.NgayTaiLen)
                .ToListAsync();

            return View(danhSach);
        }

        // Quản lý
        [HttpGet]
        public async Task<IActionResult> QuanLyHoSo()
        {
            var taiKhoan = await LayTaiKhoan();

            if (taiKhoan == null)
                return VeDangNhap();

            if (taiKhoan.VaiTro != "Admin")
                return StatusCode(403);

            // Chỉ lấy sinh viên đã có giấy tờ upload.
            var sinhViens = await _context.Set<SinhVien>()
                .AsNoTracking()
                .Where(s => _context.Set<GiayToSinhVien>()
                    .Any(g => g.Mssv == s.Mssv))
                .OrderBy(s => s.HoTen)
                .ToListAsync();

            var giayTos = await _context.Set<GiayToSinhVien>()
                .AsNoTracking()
                .OrderByDescending(g => g.NgayTaiLen)
                .ToListAsync();

            var giayToTheoSinhVien = giayTos.ToLookup(g => g.Mssv);

            var model = sinhViens
                .Select(s => new HoSoSinhVienViewModel
                {
                    SinhVien = s,
                    GiayTo = giayToTheoSinhVien[s.Mssv].ToList()
                })
                .OrderByDescending(h => h.TrangThai == "Chờ duyệt")
                .ThenBy(h => h.SinhVien.HoTen)
                .ToList();

            return View(model);
        }

        // Hồ sơ sinh viên
        [HttpGet]
        public async Task<IActionResult> HoSoSinhVien()
        {
            var taiKhoan = await LayTaiKhoan();

            if (taiKhoan == null)
                return VeDangNhap();

            if (taiKhoan.VaiTro != "SinhVien")
                return StatusCode(403);

            var sinhVien = await _context.Set<SinhVien>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.TenDangNhap == taiKhoan.TenDangNhap);

            if (sinhVien == null)
                return NotFound("Không tìm thấy thông tin sinh viên.");

            var giayTos = await _context.Set<GiayToSinhVien>()
                .AsNoTracking()
                .Where(x => x.Mssv == sinhVien.Mssv)
                .OrderByDescending(x => x.NgayTaiLen)
                .ThenByDescending(x => x.MaGiayTo)
                .ToListAsync();

            return View(giayTos);
        }

        // Đăng ảnh
        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            var taiKhoan = await LayTaiKhoan();

            if (taiKhoan == null)
                return VeDangNhap();

            if (taiKhoan.VaiTro != "SinhVien")
                return StatusCode(403);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(6 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 6 * 1024 * 1024)]
        public async Task<IActionResult> Upload(
            string? loaiGiayTo,
            IFormFile? file)
        {
            var taiKhoan = await LayTaiKhoan();

            if (taiKhoan == null)
                return VeDangNhap();

            if (taiKhoan.VaiTro != "SinhVien")
                return StatusCode(403);

            var sinhVien = await _context.Set<SinhVien>()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.TenDangNhap == taiKhoan.TenDangNhap);

            if (sinhVien == null)
                return NotFound("Không tìm thấy hồ sơ sinh viên.");

            loaiGiayTo = loaiGiayTo?.Trim();

            if (string.IsNullOrWhiteSpace(loaiGiayTo)
                || loaiGiayTo.Length > 100)
            {
                ModelState.AddModelError(
                    "loaiGiayTo",
                    "Nhập loại giấy tờ, tối đa 100 ký tự.");
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Vui lòng chọn file.");
            }
            else if (file.Length > MaxFileSize)
            {
                ModelState.AddModelError(
                    "file", "File không được vượt quá 5 MB.");
            }

            if (!ModelState.IsValid)
                return View();

            var tenGoc = Path.GetFileName(
                file!.FileName.Replace('\\', '/'));

            if (string.IsNullOrWhiteSpace(tenGoc) || tenGoc.Length > 255)
            {
                ModelState.AddModelError(
                    "file", "Tên file không hợp lệ hoặc quá dài.");
                return View();
            }

            var phanMoRong = Path.GetExtension(tenGoc).ToLowerInvariant();

            if (!await DungDinhDang(file, phanMoRong))
            {
                ModelState.AddModelError(
                    "file", "Chỉ nhận file PDF, JPG, JPEG hoặc PNG hợp lệ.");
                return View();
            }

            // Dùng tên ngẫu nhiên để tránh trùng hoặc ghi đè file.
            var tenLuu = $"{Guid.NewGuid():N}{phanMoRong}";
            var duongDanDayDu = Path.Combine(_thuMucLuu, tenLuu);

            try
            {
                Directory.CreateDirectory(_thuMucLuu);

                await using (var stream = new FileStream(
                    duongDanDayDu, FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }

                var giayTo = new GiayToSinhVien
                {
                    Mssv = sinhVien.Mssv,
                    LoaiGiayTo = loaiGiayTo!,
                    TenFile = tenGoc,

                    // Đường dẫn tương đối so với thư mục lưu riêng.
                    DuongDan = tenLuu,

                    NgayTaiLen = DateTime.Now,
                    TrangThai = "Chờ duyệt",
                    GhiChu = null,
                    NguoiDuyet = null,
                    NgayDuyet = null
                };

                _context.Set<GiayToSinhVien>().Add(giayTo);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Nếu lưu database thất bại, dọn file vừa tạo.
                try
                {
                    if (System.IO.File.Exists(duongDanDayDu))
                        System.IO.File.Delete(duongDanDayDu);
                }
                catch (Exception cleanupEx)
                {
                    _logger.LogError(
                        cleanupEx, "Không thể dọn file {TenLuu}", tenLuu);
                }

                _logger.LogError(ex, "Upload giấy tờ thất bại.");

                ModelState.AddModelError(
                    "", "Không thể lưu giấy tờ. Vui lòng thử lại.");
                return View();
            }

            TempData["ThongBao"] = "Upload thành công chờ duyệt.";
            return RedirectToAction(nameof(Upload));
        }

        // Tải file
        [HttpGet]
        public async Task<IActionResult> TaiFile(int id)
        {
            var taiKhoan = await LayTaiKhoan();

            if (taiKhoan == null)
                return VeDangNhap();

            var giayTo = await _context.Set<GiayToSinhVien>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaGiayTo == id);

            if (giayTo == null)
                return NotFound();

            if (taiKhoan.VaiTro != "Admin")
            {
                if (taiKhoan.VaiTro != "SinhVien")
                    return StatusCode(403);

                var laChuSoHuu = await _context.Set<SinhVien>()
                    .AnyAsync(x =>
                        x.TenDangNhap == taiKhoan.TenDangNhap
                        && x.Mssv == giayTo.Mssv);

                if (!laChuSoHuu)
                    return StatusCode(403);
            }

            // Chỉ nhận tên file do controller này lưu.
            var tenLuu = giayTo.DuongDan;

            if (string.IsNullOrWhiteSpace(tenLuu)
                || tenLuu.Contains('/')
                || tenLuu.Contains('\\')
                || tenLuu.Contains(':')
                || tenLuu == "."
                || tenLuu == "..")
            {
                return NotFound("Đường dẫn file không hợp lệ.");
            }

            var duongDan = Path.Combine(_thuMucLuu, tenLuu);

            if (!System.IO.File.Exists(duongDan))
                return NotFound("File không tồn tại trên máy chủ.");

            return PhysicalFile(
                duongDan,
                "application/octet-stream",
                giayTo.TenFile);
        }

        // Xem file
        [HttpGet]
        public async Task<IActionResult> XemFile(int id)
        {
            // Giữ kiểm tra đăng nhập, quyền truy cập và file tồn tại.
            var ketQua = await TaiFile(id);

            if (ketQua is not PhysicalFileResult file)
                return ketQua;

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            var contentType = extension switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".pdf" => "application/pdf",
                _ => null
            };

            if (contentType == null)
                return BadRequest("Định dạng này chưa hỗ trợ xem trực tiếp.");

            Response.Headers["Content-Disposition"] = "inline";
            Response.Headers["Cache-Control"] = "private, no-store";
            Response.Headers["X-Content-Type-Options"] = "nosniff";

            // Truyền ContentType khi tạo kết quả mới.
            return new PhysicalFileResult(file.FileName, contentType)
            {
                EnableRangeProcessing = true
            };
        }

        // Xét duyệt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XetDuyet(
            int id,
            bool? chapNhan,
            string? ghiChu)
        {
            var taiKhoan = await LayTaiKhoan();

            if (taiKhoan == null)
                return VeDangNhap();

            if (taiKhoan.VaiTro != "Admin")
                return StatusCode(403);

            ghiChu = ghiChu?.Trim();

            if (!ModelState.IsValid || chapNhan == null)
                return BadRequest("Vui lòng chọn duyệt hoặc từ chối.");

            if (ghiChu?.Length > 500)
                return BadRequest("Ghi chú tối đa 500 ký tự.");

            if (chapNhan == false && string.IsNullOrWhiteSpace(ghiChu))
                return BadRequest("Vui lòng nhập lý do từ chối.");

            var trangThai = chapNhan.Value ? "Đã duyệt" : "Từ chối";
            var ngayDuyet = DateTime.Now;

            var soDong = await _context.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE dbo.GiayToSinhVien
                SET TrangThai = {trangThai},
                    GhiChu = {ghiChu},
                    NguoiDuyet = {taiKhoan.TenDangNhap},
                    NgayDuyet = {ngayDuyet}
                WHERE MaGiayTo = {id}
                  AND TrangThai = N'Chờ duyệt'");

            if (soDong == 0)
            {
                TempData["Loi"] = "Giấy tờ không tồn tại hoặc đã được xử lý.";
                return RedirectToAction(nameof(QuanLyHoSo));
            }

            var mssv = await _context.Set<GiayToSinhVien>()
                .Where(x => x.MaGiayTo == id)
                .Select(x => x.Mssv)
                .FirstOrDefaultAsync();

            TempData["MoHoSo"] = mssv;
            TempData["ThongBao"] = chapNhan.Value
                ? "Đã duyệt giấy tờ."
                : "Đã từ chối giấy tờ.";

            return RedirectToAction(nameof(QuanLyHoSo));
        }

        // Kiểm tra phần mở rộng và các byte đầu của file.
        private static async Task<bool> DungDinhDang(
            IFormFile file, string extension)
        {
            byte[] signature;

            switch (extension)
            {
                case ".pdf":
                    signature = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D };
                    break;
                case ".jpg":
                case ".jpeg":
                    signature = new byte[] { 0xFF, 0xD8, 0xFF };
                    break;
                case ".png":
                    signature = new byte[]
                    {
                        0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A
                    };
                    break;
                default:
                    return false;
            }

            var header = new byte[signature.Length];
            await using var stream = file.OpenReadStream();

            var total = 0;
            while (total < header.Length)
            {
                var read = await stream.ReadAsync(
                    header, total, header.Length - total);

                if (read == 0)
                    return false;

                total += read;
            }

            return header.SequenceEqual(signature);
        }
    }
}