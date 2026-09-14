using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLSinhVien.Models;

public partial class QLDaoTaoContext : DbContext
{
    public QLDaoTaoContext()
    {
    }

    public QLDaoTaoContext(DbContextOptions<QLDaoTaoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GiangVien> GiangViens { get; set; }

    public virtual DbSet<GiayToSinhVien> GiayToSinhViens { get; set; }

    public virtual DbSet<HocPhanMo> HocPhanMos { get; set; }

    public virtual DbSet<KetQuaHocTap> KetQuaHocTaps { get; set; }

    public virtual DbSet<LopSinhHoat> LopSinhHoats { get; set; }

    public virtual DbSet<MonHoc> MonHocs { get; set; }

    public virtual DbSet<MonTienQuyet> MonTienQuyets { get; set; }

    public virtual DbSet<SinhVien> SinhViens { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=QLDaoTaoDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GiangVien>(entity =>
        {
            entity.HasKey(e => e.MaGv).HasName("PK__GiangVie__2725AEF3E04B640D");

            entity.ToTable("GiangVien");

            entity.HasIndex(e => e.TenDangNhap, "UQ__GiangVie__55F68FC006241BA8").IsUnique();

            entity.Property(e => e.MaGv)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaGV");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TenKhoa).HasMaxLength(100);

            entity.HasOne(d => d.TenDangNhapNavigation).WithOne(p => p.GiangVien)
                .HasForeignKey<GiangVien>(d => d.TenDangNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiangVien__TenDa__45F365D3");
        });

        modelBuilder.Entity<GiayToSinhVien>(entity =>
        {
            entity.HasKey(e => e.MaGiayTo).HasName("PK__GiayToSi__D6796CCAF4C4DB9C");

            entity.ToTable("GiayToSinhVien");

            entity.Property(e => e.DuongDan).HasMaxLength(500);
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.LoaiGiayTo).HasMaxLength(100);
            entity.Property(e => e.Mssv)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MSSV");
            entity.Property(e => e.NgayDuyet).HasColumnType("datetime");
            entity.Property(e => e.NgayTaiLen)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NguoiDuyet)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TenFile).HasMaxLength(255);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("Chờ duyệt");

            entity.HasOne(d => d.MssvNavigation).WithMany(p => p.GiayToSinhViens)
                .HasForeignKey(d => d.Mssv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiayToSinh__MSSV__5AEE82B9");

            entity.HasOne(d => d.NguoiDuyetNavigation).WithMany(p => p.GiayToSinhViens)
                .HasForeignKey(d => d.NguoiDuyet)
                .HasConstraintName("FK__GiayToSin__Nguoi__5BE2A6F2");
        });

        modelBuilder.Entity<HocPhanMo>(entity =>
        {
            entity.HasKey(e => e.MaHp).HasName("PK__HocPhanM__2725A6ECC3C2D545");

            entity.ToTable("HocPhanMo");

            entity.Property(e => e.MaHp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaHP");
            entity.Property(e => e.HocKy).HasMaxLength(50);
            entity.Property(e => e.MaGv)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaGV");
            entity.Property(e => e.MaMon)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PhongHoc).HasMaxLength(50);
            entity.Property(e => e.SiSoToiDa).HasDefaultValue(40);
            entity.Property(e => e.Thu).HasMaxLength(20);

            entity.HasOne(d => d.MaGvNavigation).WithMany(p => p.HocPhanMos)
                .HasForeignKey(d => d.MaGv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HocPhanMo__MaGV__5165187F");

            entity.HasOne(d => d.MaMonNavigation).WithMany(p => p.HocPhanMos)
                .HasForeignKey(d => d.MaMon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HocPhanMo__MaMon__5070F446");
        });

        modelBuilder.Entity<KetQuaHocTap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KetQuaHo__3214EC07FC801025");

            entity.ToTable("KetQuaHocTap");

            entity.Property(e => e.DiemChu)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.MaHp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaHP");
            entity.Property(e => e.Mssv)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MSSV");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Đăng ký thành công");

            entity.HasOne(d => d.MaHpNavigation).WithMany(p => p.KetQuaHocTaps)
                .HasForeignKey(d => d.MaHp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KetQuaHocT__MaHP__5629CD9C");

            entity.HasOne(d => d.MssvNavigation).WithMany(p => p.KetQuaHocTaps)
                .HasForeignKey(d => d.Mssv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KetQuaHocT__MSSV__5535A963");
        });

        modelBuilder.Entity<LopSinhHoat>(entity =>
        {
            entity.HasKey(e => e.MaLopSh).HasName("PK__LopSinhH__976A85630C1A375B");

            entity.ToTable("LopSinhHoat");

            entity.Property(e => e.MaLopSh)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaLopSH");
            entity.Property(e => e.NienKhoa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenKhoa).HasMaxLength(100);
            entity.Property(e => e.TenLop).HasMaxLength(100);
            entity.Property(e => e.TenNganh).HasMaxLength(100);
        });

        modelBuilder.Entity<MonHoc>(entity =>
        {
            entity.HasKey(e => e.MaMon).HasName("PK__MonHoc__3A5B29A871D69F08");

            entity.ToTable("MonHoc");

            entity.Property(e => e.MaMon)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenKhoa).HasMaxLength(100);
            entity.Property(e => e.TenMon).HasMaxLength(100);
        });

        modelBuilder.Entity<MonTienQuyet>(entity =>
        {
            entity.HasKey(e => new { e.MaMon, e.MaMonTruoc }).HasName("PK__MonTienQ__92D7B1FF91687972");

            entity.ToTable("MonTienQuyet");

            entity.Property(e => e.MaMon)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MaMonTruoc)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.GhiChu).HasMaxLength(50);

            entity.HasOne(d => d.MaMonNavigation).WithMany(p => p.MonTienQuyetMaMonNavigations)
                .HasForeignKey(d => d.MaMon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MonTienQu__MaMon__4AB81AF0");

            entity.HasOne(d => d.MaMonTruocNavigation).WithMany(p => p.MonTienQuyetMaMonTruocNavigations)
                .HasForeignKey(d => d.MaMonTruoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MonTienQu__MaMon__4BAC3F29");
        });

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(e => e.Mssv).HasName("PK__SinhVien__6CB3B7F967F1FB5D");

            entity.ToTable("SinhVien");

            entity.HasIndex(e => e.TenDangNhap, "UQ__SinhVien__55F68FC08AABA481").IsUnique();

            entity.Property(e => e.Mssv)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MSSV");
            entity.Property(e => e.Cccd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CCCD");
            entity.Property(e => e.DanToc)
                .HasMaxLength(50)
                .HasDefaultValue("Kinh");
            entity.Property(e => e.DiaChiLienHe).HasMaxLength(255);
            entity.Property(e => e.EmailCaNhan)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmailTruong)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoKhauThuongTru).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.KhuVuc).HasMaxLength(20);
            entity.Property(e => e.MaHoSo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MaLopSh)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaLopSH");
            entity.Property(e => e.NoiCap).HasMaxLength(100);
            entity.Property(e => e.NoiSinh).HasMaxLength(100);
            entity.Property(e => e.QuocTich)
                .HasMaxLength(50)
                .HasDefaultValue("Việt Nam");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TonGiao)
                .HasMaxLength(50)
                .HasDefaultValue("Không");
            entity.Property(e => e.TrangThaiHocTap)
                .HasMaxLength(50)
                .HasDefaultValue("Đang học");

            entity.HasOne(d => d.MaLopShNavigation).WithMany(p => p.SinhViens)
                .HasForeignKey(d => d.MaLopSh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SinhVien__MaLopS__412EB0B6");

            entity.HasOne(d => d.TenDangNhapNavigation).WithOne(p => p.SinhVien)
                .HasForeignKey<SinhVien>(d => d.TenDangNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SinhVien__TenDan__4222D4EF");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.TenDangNhap).HasName("PK__TaiKhoan__55F68FC10155A318");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.VaiTro)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
