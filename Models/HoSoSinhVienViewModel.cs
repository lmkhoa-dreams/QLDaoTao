namespace QLSinhVien.Models
{
    public class HoSoSinhVienViewModel
    {
        public SinhVien SinhVien { get; set; } = null!;

        public List<GiayToSinhVien> GiayTo { get; set; }
            = new List<GiayToSinhVien>();

        public string TrangThai
        {
            get
            {
                if (GiayTo.Count == 0)
                    return "Chưa có giấy tờ";

                if (GiayTo.Any(x => x.TrangThai == "Chờ duyệt"))
                    return "Chờ duyệt";

                if (GiayTo.Any(x => x.TrangThai == "Từ chối"))
                    return "Đã từ chối";

                return "Đã duyệt";
            }
        }
    }
}