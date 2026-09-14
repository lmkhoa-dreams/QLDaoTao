using System;
using System.Collections.Generic;

namespace QLSinhVien.Models;

public partial class MonHoc
{
    public string MaMon { get; set; } = null!;

    public string TenMon { get; set; } = null!;

    public int SoTinChi { get; set; }

    public string? TenKhoa { get; set; }

    public virtual ICollection<HocPhanMo> HocPhanMos { get; set; } = new List<HocPhanMo>();

    public virtual ICollection<MonTienQuyet> MonTienQuyetMaMonNavigations { get; set; } = new List<MonTienQuyet>();

    public virtual ICollection<MonTienQuyet> MonTienQuyetMaMonTruocNavigations { get; set; } = new List<MonTienQuyet>();
}
