using System;
using System.Collections.Generic;

namespace QLSinhVien.Models;

public partial class GiayToSinhVien
{
    public int MaGiayTo { get; set; }

    public string Mssv { get; set; } = null!;

    public string LoaiGiayTo { get; set; } = null!;

    public string TenFile { get; set; } = null!;

    public string DuongDan { get; set; } = null!;

    public DateTime NgayTaiLen { get; set; }

    public string TrangThai { get; set; } = null!;

    public string? GhiChu { get; set; }

    public string? NguoiDuyet { get; set; }

    public DateTime? NgayDuyet { get; set; }

    public virtual SinhVien MssvNavigation { get; set; } = null!;

    public virtual TaiKhoan? NguoiDuyetNavigation { get; set; }
}
