using System;
using System.Collections.Generic;

namespace QLSinhVien.Models;

public partial class LopSinhHoat
{
    public string MaLopSh { get; set; } = null!;

    public string TenLop { get; set; } = null!;

    public string NienKhoa { get; set; } = null!;

    public string TenNganh { get; set; } = null!;

    public string TenKhoa { get; set; } = null!;

    public virtual ICollection<SinhVien> SinhViens { get; set; } = new List<SinhVien>();
}
