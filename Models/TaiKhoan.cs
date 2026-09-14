using System;
using System.Collections.Generic;

namespace QLSinhVien.Models;

public partial class TaiKhoan
{
    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string VaiTro { get; set; } = null!;

    public virtual GiangVien? GiangVien { get; set; }

    public virtual ICollection<GiayToSinhVien> GiayToSinhViens { get; set; } = new List<GiayToSinhVien>();

    public virtual SinhVien? SinhVien { get; set; }
}
