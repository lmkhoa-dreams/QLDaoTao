using System;
using System.Collections.Generic;

namespace QLSinhVien.Models;

public partial class GiangVien
{
    public string MaGv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string? Email { get; set; }

    public string? SoDienThoai { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string? TenKhoa { get; set; }

    public virtual ICollection<HocPhanMo> HocPhanMos { get; set; } = new List<HocPhanMo>();

    public virtual TaiKhoan TenDangNhapNavigation { get; set; } = null!;
}
