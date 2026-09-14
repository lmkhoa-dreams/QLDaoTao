using System;
using System.Collections.Generic;

namespace QLSinhVien.Models;

public partial class MonTienQuyet
{
    public string? GhiChu { get; set; }

    public string MaMon { get; set; } = null!;

    public string MaMonTruoc { get; set; } = null!;

    public virtual MonHoc MaMonNavigation { get; set; } = null!;

    public virtual MonHoc MaMonTruocNavigation { get; set; } = null!;
}
