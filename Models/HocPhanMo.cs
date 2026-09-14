using System;
using System.Collections.Generic;

namespace QLSinhVien.Models;

public partial class HocPhanMo
{
    public string MaHp { get; set; } = null!;

    public string MaMon { get; set; } = null!;

    public string HocKy { get; set; } = null!;

    public string MaGv { get; set; } = null!;

    public int SiSoToiDa { get; set; }

    public int SiSoHienTai { get; set; }

    public string? Thu { get; set; }

    public int? TietBatDau { get; set; }

    public int? TietKetThuc { get; set; }

    public string? PhongHoc { get; set; }

    public virtual ICollection<KetQuaHocTap> KetQuaHocTaps { get; set; } = new List<KetQuaHocTap>();

    public virtual GiangVien MaGvNavigation { get; set; } = null!;

    public virtual MonHoc MaMonNavigation { get; set; } = null!;
}
