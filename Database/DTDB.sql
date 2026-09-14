CREATE DATABASE QLDaoTaoDB;
GO
USE QLDaoTaoDB;
GO

-- 1. TẠO CẤU TRÚC BẢNG
-- TÀI KHOẢN HỆ THỐNG
CREATE TABLE TaiKhoan (
    TenDangNhap VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(255) NOT NULL,
    VaiTro VARCHAR(20) NOT NULL CHECK (VaiTro IN ('Admin', 'GiangVien', 'SinhVien'))
);
GO

-- LỚP SINH HOẠT
CREATE TABLE LopSinhHoat (
    MaLopSH VARCHAR(20) PRIMARY KEY,
    TenLop NVARCHAR(100) NOT NULL,
    NienKhoa VARCHAR(20) NOT NULL,
    TenNganh NVARCHAR(100) NOT NULL, 
    TenKhoa NVARCHAR(100) NOT NULL   
);
GO

-- HỒ SƠ SINH VIÊN
CREATE TABLE SinhVien (
    MSSV VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NOT NULL,
    GioiTinh BIT NOT NULL,
    SoDienThoai VARCHAR(15) NULL,
    EmailCaNhan VARCHAR(100) NULL,
    EmailTruong VARCHAR(100) NULL,
    CCCD VARCHAR(20) NULL,
    NgayCap DATE NULL,
    NoiCap NVARCHAR(100) NULL,
    DanToc NVARCHAR(50) DEFAULT N'Kinh',
    TonGiao NVARCHAR(50) DEFAULT N'Không',
    QuocTich NVARCHAR(50) DEFAULT N'Việt Nam',
    KhuVuc NVARCHAR(20) NULL,
    DiaChiLienHe NVARCHAR(255) NULL,
    HoKhauThuongTru NVARCHAR(255) NULL,
    NoiSinh NVARCHAR(100) NULL,
    MaHoSo VARCHAR(50) NULL,
    NgayVaoTruong DATE NULL,
    TrangThaiHocTap NVARCHAR(50) DEFAULT N'Đang học',
    MaLopSH VARCHAR(20) NOT NULL,
    TenDangNhap VARCHAR(50) UNIQUE NOT NULL,
    FOREIGN KEY (MaLopSH) REFERENCES LopSinhHoat(MaLopSH),
    FOREIGN KEY (TenDangNhap) REFERENCES TaiKhoan(TenDangNhap)
);
GO

-- HỒ SƠ GIẢNG VIÊN (Đã tích hợp cột TenKhoa)
CREATE TABLE GiangVien (
    MaGV VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) NULL,
    SoDienThoai VARCHAR(15) NULL,
    TenDangNhap VARCHAR(50) UNIQUE NOT NULL,
    TenKhoa NVARCHAR(100) NULL,
    FOREIGN KEY (TenDangNhap) REFERENCES TaiKhoan(TenDangNhap)
);
GO

-- DANH MỤC MÔN HỌC (Đã tích hợp cột TenKhoa)
CREATE TABLE MonHoc (
    MaMon VARCHAR(20) PRIMARY KEY,
    TenMon NVARCHAR(100) NOT NULL,
    SoTinChi INT NOT NULL,
    TenKhoa NVARCHAR(100) NULL
);
GO

-- DANH MỤC MÔN TIÊN QUYẾT
CREATE TABLE MonTienQuyet (
    GhiChu NVARCHAR(50),
    MaMon VARCHAR(20),
    MaMonTruoc VARCHAR(20),
    PRIMARY KEY (MaMon, MaMonTruoc),
    FOREIGN KEY (MaMon) REFERENCES MonHoc(MaMon),
    FOREIGN KEY (MaMonTruoc) REFERENCES MonHoc(MaMon)
);
GO

-- MỞ LỚP ĐĂNG KÝ HỌC PHẦN (ĐÃ TÍCH HỢP CÁC CỘT LỊCH HỌC)
CREATE TABLE HocPhanMo (
    MaHP VARCHAR(20) PRIMARY KEY,
    MaMon VARCHAR(20) NOT NULL,
    HocKy NVARCHAR(50) NOT NULL,   
    MaGV VARCHAR(20) NOT NULL,     
    SiSoToiDa INT NOT NULL DEFAULT 40,
    SiSoHienTai INT NOT NULL DEFAULT 0,
    Thu NVARCHAR(20) NULL,          
    TietBatDau INT NULL,            
    TietKetThuc INT NULL,           
    PhongHoc NVARCHAR(50) NULL,    
    FOREIGN KEY (MaMon) REFERENCES MonHoc(MaMon),
    FOREIGN KEY (MaGV) REFERENCES GiangVien(MaGV)
);
GO

-- KẾT QUẢ VÀ SỔ ĐIỂM DỮ LIỆU
CREATE TABLE KetQuaHocTap (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MSSV VARCHAR(20) NOT NULL,
    MaHP VARCHAR(20) NOT NULL,
    DiemQuaTrinh FLOAT NULL,
    DiemGiuaKy FLOAT NULL,
    DiemThi FLOAT NULL,
    DiemTongKet FLOAT NULL,
    DiemChu VARCHAR(2) NULL,
    TrangThai NVARCHAR(50) NOT NULL DEFAULT N'Đăng ký thành công',
    SoBuoiVang int NULL,
    SoBuoiCoPhep int NULL,
    FOREIGN KEY (MSSV) REFERENCES SinhVien(MSSV),
    FOREIGN KEY (MaHP) REFERENCES HocPhanMo(MaHP)
);

CREATE TABLE GiayToSinhVien (
    MaGiayTo INT IDENTITY(1,1) PRIMARY KEY,
    MSSV VARCHAR(20) NOT NULL,
    LoaiGiayTo NVARCHAR(100) NOT NULL,
    TenFile NVARCHAR(255) NOT NULL,
    DuongDan NVARCHAR(500) NOT NULL,
    NgayTaiLen DATETIME NOT NULL DEFAULT GETDATE(),
    TrangThai NVARCHAR(20) NOT NULL DEFAULT N'Chờ duyệt',
    GhiChu NVARCHAR(500) NULL,
    NguoiDuyet VARCHAR(50) NULL,
    NgayDuyet DATETIME NULL,
    FOREIGN KEY (MSSV) REFERENCES SinhVien(MSSV),
    FOREIGN KEY (NguoiDuyet) REFERENCES TaiKhoan(TenDangNhap),
);
GO

-- DỮ LIỆU GIẢ LẬP (MOCK DATA)
-- Tài Khoản
INSERT INTO TaiKhoan VALUES 
('admin', 'admin123', 'Admin'),
('gvtest', '123456', 'GiangVien'),
('gvtest1', '123456', 'GiangVien'), 
('svtest', '123456', 'SinhVien'),
('svtest1', '123456', 'SinhVien');

-- Sinh Hoạt
INSERT INTO LopSinhHoat VALUES 
('24DKTPM1A', N'Kỹ Thuật Phần Mềm', '2024-2028', N'Kỹ thuật phần mềm', N'Khoa Công nghệ thông tin'),
('24DTLH1A', N'Tâm lý học ', '2024-2028', N'Tâm lý học', N'Khoa Khoa học Xã hội');

-- Sinh Viên
INSERT INTO SinhVien (MSSV, HoTen, NgaySinh, GioiTinh, SoDienThoai, EmailCaNhan, EmailTruong, CCCD, NgayCap, NoiCap, KhuVuc, DiaChiLienHe, HoKhauThuongTru, NoiSinh, MaHoSo, NgayVaoTruong, MaLopSH, TenDangNhap) 
VALUES 
('SV001', N'Trịnh Anh Khoa', '2006-10-22', 1, '0912345678', 'khoa@gmail.com', 'SV001@ntt.edu.vn', '075206018347', '2021-05-10', N'Cục Cảnh sát QLHC về TTXH', N'Khu vực 2', N'Đồng Nai', N'Đồng Nai', N'Đồng Nai', '24NTT.0001', '2024-08-21', '24DKTPM1A', 'svtest'),
('SV002', N'Lê Thị Ngọc Hân', '2007-10-21', 0, '0912345687', 'han@gmail.com', 'SV002@ntt.edu.vn', '079307011111', '2022-01-15', N'Cục Cảnh sát QLHC về TTXH', N'Khu vực 1', N'TP.HCM', N'TP.HCM', N'TP.HCM', '24NTT.0002', '2024-08-21', '24DTLH1A', 'svtest1');

-- Giảng Viên (Đã thêm thông tin Khoa)
INSERT INTO GiangVien VALUES 
('GV001', N'Ngô Nhật Phi', 'khoatd@truong.edu.vn', '0123456789', 'gvtest', N'Khoa Công nghệ thông tin'),
('GV002', N'Lê Thị Tuyết An', 'coltc@truong.edu.vn', '0123456798', 'gvtest1', N'Khoa Khoa học Xã hội');

-- Chèn 10 Môn Học (Đã phân bổ về đúng Khoa)
INSERT INTO MonHoc VALUES 
('001', N'Nhập môn lập trình', 3, N'Khoa Công nghệ thông tin'),
('002', N'Lập trình Web', 3, N'Khoa Công nghệ thông tin'),
('003', N'Cơ sở dữ liệu', 3, N'Khoa Công nghệ thông tin'),
('004', N'Cấu trúc dữ liệu và giải thuật', 3, N'Khoa Công nghệ thông tin'),
('005', N'Mạng máy tính', 3, N'Khoa Công nghệ thông tin'),
('101', N'Tâm lý học đại cương', 3, N'Khoa Khoa học Xã hội'),
('102', N'Tâm lý học phát triển', 3, N'Khoa Khoa học Xã hội'),
('103', N'Tâm lý học giao tiếp', 3, N'Khoa Khoa học Xã hội'),
('104', N'Tham vấn tâm lý', 3, N'Khoa Khoa học Xã hội'),
('105', N'Tâm lý học xã hội', 3, N'Khoa Khoa học Xã hội');

-- Môn Tiên Quyết
INSERT INTO MonTienQuyet (MaMon, MaMonTruoc) VALUES
('002', '001'),
('004', '001'),
('102', '101'),
('104', '101');

-- Mở Lớp Học Phần Đã Được Lên Lịch Chi Tiết
INSERT INTO HocPhanMo (MaHP, MaMon, HocKy, MaGV, SiSoToiDa, SiSoHienTai, Thu, TietBatDau, TietKetThuc, PhongHoc) VALUES 
-- Lớp của Thầy Phi (Khoa CNTT)
('HP_001_01', '001', N'HK1_2026_2027', 'GV001', 40, 1, N'Thứ 2', 1, 3, N'L.506'),
('HP_002_01', '002', N'HK1_2026_2027', 'GV001', 40, 1, N'Thứ 4', 4, 6, N'L.404A'),
('HP_003_01', '003', N'HK1_2026_2027', 'GV001', 40, 0, N'Thứ 2', 7, 9, N'L.604'),
('HP_004_01', '004', N'HK1_2026_2027', 'GV001', 40, 0, N'Thứ 5', 1, 3, N'L.704'),
('HP_005_01', '005', N'HK1_2026_2027', 'GV001', 40, 0, N'Thứ 6', 4, 6, N'L.404B'),
('HP_002_02', '002', N'HK2_2026_2027', 'GV001', 40, 0, N'Thứ 3', 1, 3, N'L.601'),
('HP_004_02', '004', N'HK2_2026_2027', 'GV001', 40, 0, N'Thứ 5', 4, 6, N'L.702'),
-- Lớp của Cô An (Khoa KHXH)
('HP_101_01', '101', N'HK1_2026_2027', 'GV002', 40, 1, N'Thứ 3', 7, 9, N'L.902'),
('HP_102_01', '102', N'HK1_2026_2027', 'GV002', 40, 0, N'Thứ 3', 1, 3, N'L.606'),
('HP_103_01', '103', N'HK1_2026_2027', 'GV002', 40, 1, N'Thứ 6', 1, 3, N'L2.132'),
('HP_104_01', '104', N'HK1_2026_2027', 'GV002', 40, 0, N'Thứ 4', 7, 9, N'L.404'),
('HP_105_01', '105', N'HK1_2026_2027', 'GV002', 40, 0, N'Thứ 5', 4, 6, N'L2.135'),
('HP_102_02', '102', N'HK2_2026_2027', 'GV002', 40, 0, N'Thứ 7', 1, 3, N'L.801'),
('HP_104_02', '104', N'HK2_2026_2027', 'GV002', 40, 0, N'Thứ 7', 4, 6, N'L.502');

-- Đăng ký học tập cho sinh viên
INSERT INTO KetQuaHocTap (MSSV, MaHP, DiemQuaTrinh, DiemThi, DiemTongKet, DiemChu, TrangThai)
VALUES 
('SV001', 'HP_001_01', NULL, NULL, NULL, NULL, N'Đăng ký thành công'),
('SV001', 'HP_002_01', NULL, NULL, NULL, NULL, N'Đăng ký thành công'),
('SV002', 'HP_101_01', NULL, NULL, NULL, NULL, N'Đăng ký thành công'),
('SV002', 'HP_103_01', NULL, NULL, NULL, NULL, N'Đăng ký thành công');

INSERT INTO GiayToSinhVien(MSSV, LoaiGiayTo, TenFile, DuongDan)
VALUES
('SV001', N'Bằng tốt nghiệp THPT', N'bangtotnghiep.pdf', N'Uploads/SinhVien/SV001/bangtotnghiep.pdf'),
('SV002', N'Giấy khai sinh', N'giaykhaisinh.pdf', N'Uploads/SinhVien/SV002/giaykhaisinh.pdf');
GO