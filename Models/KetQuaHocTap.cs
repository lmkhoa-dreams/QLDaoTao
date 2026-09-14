using System;
using System.Collections.Generic;

namespace QLSinhVien.Models;

public partial class KetQuaHocTap
{
    public int Id { get; set; }

    public string Mssv { get; set; } = null!;

    public string MaHp { get; set; } = null!;

    public double? DiemQuaTrinh { get; set; }

    public double? DiemGiuaKy { get; set; }

    public double? DiemThi { get; set; }

    public double? DiemTongKet { get; set; }

    public string? DiemChu { get; set; }

    public string TrangThai { get; set; } = null!;

    public int? SoBuoiVang { get; set; }

    public int? SoBuoiCoPhep { get; set; }

    public virtual HocPhanMo MaHpNavigation { get; set; } = null!;

    public virtual SinhVien MssvNavigation { get; set; } = null!;
}
