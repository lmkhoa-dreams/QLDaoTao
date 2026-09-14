using System;
using System.Collections.Generic;

namespace QLSinhVien.Models;

public partial class SinhVien
{
    public string Mssv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public DateOnly NgaySinh { get; set; }

    public bool GioiTinh { get; set; }

    public string? SoDienThoai { get; set; }

    public string? EmailCaNhan { get; set; }

    public string? EmailTruong { get; set; }

    public string? Cccd { get; set; }

    public DateOnly? NgayCap { get; set; }

    public string? NoiCap { get; set; }

    public string? DanToc { get; set; }

    public string? TonGiao { get; set; }

    public string? QuocTich { get; set; }

    public string? KhuVuc { get; set; }

    public string? DiaChiLienHe { get; set; }

    public string? HoKhauThuongTru { get; set; }

    public string? NoiSinh { get; set; }

    public string? MaHoSo { get; set; }

    public DateOnly? NgayVaoTruong { get; set; }

    public string? TrangThaiHocTap { get; set; }

    public string MaLopSh { get; set; } = null!;

    public string TenDangNhap { get; set; } = null!;

    public virtual ICollection<GiayToSinhVien> GiayToSinhViens { get; set; } = new List<GiayToSinhVien>();

    public virtual ICollection<KetQuaHocTap> KetQuaHocTaps { get; set; } = new List<KetQuaHocTap>();

    public virtual LopSinhHoat MaLopShNavigation { get; set; } = null!;

    public virtual TaiKhoan TenDangNhapNavigation { get; set; } = null!;
}
