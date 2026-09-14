# 🎓 QLDaoTao - Hệ thống quản lý đào tạo và hồ sơ sinh viên trực tuyến

## 📝 Giới thiệu dự án

QLDaoTao là hệ thống web hỗ trợ quản lý đào tạo và hồ sơ sinh viên trực tuyến.  
Hệ thống được xây dựng bằng ASP.NET Core MVC, Entity Framework Core và SQL Server, phục vụ ba nhóm người dùng chính: Admin, Giảng viên và Sinh viên.

---

## 🚀 Công nghệ sử dụng

- **Backend:** ASP.NET Core MVC (C#)
- **ORM:** Entity Framework Core - Database First
- **Database:** SQL Server
- **Frontend:** Razor View, HTML, Tailwind CSS, JavaScript
- **IDE:** Visual Studio 2022
- **Version Control:** Git / GitHub

---

## 🛠️ Chức năng chính

### 👨‍💼 Admin
- Quản lý giảng viên, lớp sinh hoạt, môn học và học phí.
- Kiểm tra, duyệt hoặc từ chối hồ sơ sinh viên.

### 👨‍🏫 Giảng viên
- Đăng ký giảng dạy và xem lịch trình.
- Điểm danh sinh viên.
- Nhập và cập nhật điểm học tập.

### 🎓 Sinh viên
- Đăng ký học phần, xem lịch học và kết quả học tập.
- Theo dõi điểm danh và học phí.
- Cập nhật thông tin cá nhân.
- Tải lên và theo dõi trạng thái hồ sơ.
---

# 🔐 Phân quyền người dùng

| Vai trò | Quyền chính |
|---|---|
| **Admin** | Quản lý dữ liệu hệ thống, học phí và duyệt hồ sơ |
| **Giảng viên** | Quản lý điểm, điểm danh, lịch giảng dạy |
| **Sinh viên** | Đăng ký học phần, xem kết quả, quản lý thông tin và hồ sơ |

Sau khi đăng nhập thành công, hệ thống kiểm tra `VaiTro` của tài khoản và tự động chuyển người dùng đến khu vực tương ứng.

---

# 🗄️ Cơ sở dữ liệu

Hệ thống sử dụng **Microsoft SQL Server**.

Tên cơ sở dữ liệu:

```text
QLDaoTaoDB
```

Các bảng chính bao gồm:

```text
TaiKhoan
LopSinhHoat
SinhVien
GiangVien
MonHoc
MonTienQuyet
HocPhanMo
KetQuaHocTap
GiayToSinhVien
```

File SQL để tạo cơ sở dữ liệu được lưu trong:

```text
Database/QLDaoTaoDB.sql
```

> Nếu repository của bạn đang sử dụng tên file SQL khác, hãy mở file `.sql` tương ứng nằm trong thư mục `Database`.

---

# 📁 Cấu trúc project

```text
QLSinhVien/
│
├── Areas/
│   ├── Admin/
│   │   ├── Controllers/
│   │   └── Views/
│   │
│   ├── GiangVien/
│   │   ├── Controllers/
│   │   └── Views/
│   │
│   └── SinhVien/
│       ├── Controllers/
│       └── Views/
│
├── Controllers/
│
├── Database/
│   └── QLDaoTaoDB.sql
│
├── Models/
├── Properties/
├── Views/
├── wwwroot/
│
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
├── QLSinhVien.csproj
├── QLSinhVien.sln
├── .gitignore
└── README.md
```

---

# ⚙️ HƯỚNG DẪN CÀI ĐẶT VÀ CHẠY PROJECT

## 1. Các phần mềm cần cài đặt

Trước khi clone project, máy cần cài:

### Visual Studio 2022

Nên cài workload:

```text
ASP.NET and web development
```

---

### .NET SDK

Kiểm tra phiên bản .NET mà project sử dụng bằng cách mở:

```text
QLSinhVien.csproj
```

Tìm:

```xml
<TargetFramework>...</TargetFramework>
```

Ví dụ:

```xml
<TargetFramework>net8.0</TargetFramework>
```

thì máy cần cài **.NET 8 SDK**.

Kiểm tra máy đã có .NET hay chưa:

```bash
dotnet --version
```

---

### SQL Server

Cần cài một trong các phiên bản:

- SQL Server Developer.
- SQL Server Express.
- SQL Server LocalDB.

---

### SQL Server Management Studio

SSMS được sử dụng để:

- Kết nối SQL Server.
- Tạo database.
- Chạy file SQL.
- Kiểm tra dữ liệu.

---

### Git

Nếu muốn clone bằng Git:

```bash
git --version
```

Nếu lệnh trên chạy được thì Git đã được cài.

---

# 📥 2. Clone project từ GitHub

Mở Git Bash, CMD hoặc Terminal.

Chạy:

```bash
git clone https://github.com/Imkhoa-dreams/QLDaoTao.git
```

Sau khi clone:

```bash
cd QLDaoTao
```

---

## Hoặc tải bằng ZIP

Trên GitHub:

```text
Code
→ Download ZIP
```

Sau đó:

1. Giải nén file ZIP.
2. Mở thư mục project.
3. Tiếp tục các bước bên dưới.

---

# 🗄️ 3. Tạo cơ sở dữ liệu

Sau khi clone project về máy:

Mở **SQL Server Management Studio (SSMS)**.

Kết nối vào SQL Server của máy.

Sau đó mở file:

```text
Database/QLDaoTaoDB.sql
```

Có thể mở bằng:

```text
File
→ Open
→ File
```

Chọn:

```text
QLDaoTaoDB.sql
```

Sau đó nhấn:

```text
Execute
```

hoặc:

```text
F5
```

Nếu chạy thành công, trong:

```text
Databases
```

sẽ xuất hiện:

```text
QLDaoTaoDB
```

---

# 🔌 4. Cấu hình Connection String

Project hiện sử dụng Connection String:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=QLDaoTaoDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

File cấu hình nằm tại:

```text
appsettings.json
```

---

## Trường hợp 1: SQL Server sử dụng localhost

Nếu trong SSMS có thể đăng nhập bằng:

```text
localhost
```

thì giữ nguyên:

```json
"DefaultConnection": "Server=localhost;Database=QLDaoTaoDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

---

## Trường hợp 2: SQL Server Express

Nếu Server Name trong SSMS là:

```text
.\SQLEXPRESS
```

thì sửa thành:

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=QLDaoTaoDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

---

## Trường hợp 3: LocalDB

Nếu Server Name là:

```text
(localdb)\MSSQLLocalDB
```

thì sử dụng:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=QLDaoTaoDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

Database phải được tạo trên đúng LocalDB instance này.

---

## Trường hợp 4: Server Name theo tên máy

Ví dụ SSMS sử dụng:

```text
DESKTOP-ABC123\SQLEXPRESS
```

thì connection string:

```json
"DefaultConnection": "Server=DESKTOP-ABC123\\SQLEXPRESS;Database=QLDaoTaoDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

> Trong JSON, ký tự `\` phải viết thành `\\`.

---

## Trường hợp 5: SQL Server Authentication

Nếu SQL Server sử dụng username/password thay vì Windows Authentication:

```json
"DefaultConnection": "Server=localhost;Database=QLDaoTaoDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

Thay:

```text
YOUR_USERNAME
YOUR_PASSWORD
```

bằng tài khoản SQL Server của máy.

Không nên đưa password thật lên GitHub.

---

# 📦 5. Restore các package

Sau khi cấu hình database xong, mở Terminal tại thư mục project.

Chạy:

```bash
dotnet restore
```

Lệnh này tải lại các package NuGet mà project đang sử dụng.

---

# 🔨 6. Build project

Chạy:

```bash
dotnet build
```

Nếu thành công sẽ thấy:

```text
Build succeeded.
```

Nếu lỗi, kiểm tra phần **Các lỗi thường gặp** bên dưới.

---

# ▶️ 7. Chạy project

## Cách 1: Visual Studio 2022

Mở:

```text
QLSinhVien.sln
```

Sau đó chọn:

```text
https
```

trên thanh công cụ và nhấn Run.

Hoặc:

```text
Ctrl + F5
```

---

## Cách 2: Terminal

Tại thư mục chứa file `.csproj` chạy:

```bash
dotnet run
```

Terminal sẽ hiển thị URL tương tự:

```text
https://localhost:xxxx
```

Mở URL đó trên trình duyệt.

---

# 👤 8. Tài khoản thử nghiệm

Nếu sử dụng dữ liệu mẫu đi kèm file SQL:

## Admin

```text
Tên đăng nhập: admin
Mật khẩu: admin123
```

---

## Giảng viên

```text
Tên đăng nhập: gvtest
Mật khẩu: 123456
```

Tài khoản khác:

```text
Tên đăng nhập: gvtest1
Mật khẩu: 123456
```

---

## Sinh viên

```text
Tên đăng nhập: svtest
Mật khẩu: 123456
```

Tài khoản khác:

```text
Tên đăng nhập: svtest1
Mật khẩu: 123456
```

---

# ✅ 9. Quy trình chạy nhanh

Người mới clone project chỉ cần thực hiện:

```text
Clone repository
        ↓
Cài Visual Studio + .NET + SQL Server
        ↓
Mở SSMS
        ↓
Chạy Database/QLDaoTaoDB.sql
        ↓
Kiểm tra QLDaoTaoDB đã tồn tại
        ↓
Mở appsettings.json
        ↓
Sửa Server trong Connection String nếu cần
        ↓
dotnet restore
        ↓
dotnet build
        ↓
Mở QLSinhVien.sln
        ↓
Run
        ↓
Đăng nhập bằng tài khoản mẫu
```

---

# ⚠️ 10. Các lỗi thường gặp

## ❌ Không kết nối được SQL Server

Lỗi thường có dạng:

```text
A network-related or instance-specific error occurred while establishing a connection to SQL Server
```

### Cách xử lý

Kiểm tra Server Name đang sử dụng trong SSMS.

Ví dụ nếu SSMS dùng:

```text
DESKTOP-ABC\SQLEXPRESS
```

thì `appsettings.json` cũng phải dùng:

```json
"Server=DESKTOP-ABC\\SQLEXPRESS;"
```

---

# ❌ Cannot open database QLDaoTaoDB

Lỗi:

```text
Cannot open database "QLDaoTaoDB" requested by the login
```

### Cách xử lý

Kiểm tra trong SSMS:

```text
Databases
```

có:

```text
QLDaoTaoDB
```

hay chưa.

Nếu chưa, chạy lại:

```text
Database/QLDaoTaoDB.sql
```

---

# ❌ Invalid object name

Ví dụ:

```text
Invalid object name 'SinhVien'
```

Nguyên nhân thường là:

- Chưa chạy đầy đủ file SQL.
- Project đang kết nối nhầm database.
- Connection String sai.

Kiểm tra:

```text
Database=QLDaoTaoDB
```

trong `appsettings.json`.

---

# ❌ Login failed for user

Nếu gặp:

```text
Login failed for user
```

kiểm tra phương thức xác thực SQL Server.

Nếu sử dụng Windows Authentication:

```text
Trusted_Connection=True
```

Nếu sử dụng SQL Server Authentication:

```text
User Id=...
Password=...
```

---

# ❌ Lỗi NuGet Package

Chạy:

```bash
dotnet restore
```

Sau đó:

```bash
dotnet build
```

---

# ❌ Sai phiên bản .NET

Nếu gặp:

```text
The current .NET SDK does not support targeting...
```

Mở:

```text
QLSinhVien.csproj
```

xem:

```xml
<TargetFramework>...</TargetFramework>
```

Sau đó cài đúng phiên bản .NET SDK.

---

# ❌ HTTPS Certificate Error

Chạy:

```bash
dotnet dev-certs https --trust
```

Sau đó mở lại Visual Studio.

---

# ❌ Giao diện Tailwind không hiển thị đúng

Nếu project đang sử dụng Tailwind CSS thông qua file CSS hoặc CDN thì không cần cài gì thêm.

Nếu repository có:

```text
package.json
```

thì cần cài Node.js và chạy:

```bash
npm install
```

Sau đó chạy script build Tailwind nếu project có cấu hình.

Ví dụ:

```bash
npm run build
```

Nếu project không có `package.json`, bỏ qua bước này.

---

# 📂 11. Upload hồ sơ và file người dùng

Các file do sinh viên upload không nên đưa lên GitHub.

Ví dụ:

```text
Uploads/
App_Data/Uploads/
wwwroot/uploads/
```

có thể được khai báo trong `.gitignore`.

Nếu khi clone project thư mục upload chưa tồn tại, ứng dụng nên tự tạo thư mục khi người dùng upload file lần đầu.

---

# 🧩 12. Entity Framework Core - Database First

Project sử dụng:

```text
Entity Framework Core - Database First
```

Các Model và `DbContext` đã được tạo sẵn trong source code.

Do đó người clone project về **không cần Scaffold lại database**.

Chỉ cần:

```text
1. Chạy file SQL.
2. Sửa Connection String.
3. Restore package.
4. Build.
5. Run.
```

---

## Không cần chạy Migration

Project không sử dụng Code-First để tạo database.

Do đó thông thường **không cần chạy**:

```bash
dotnet ef database update
```

Database được tạo thông qua:

```text
Database/QLDaoTaoDB.sql
```

---

# 🔄 13. Nếu database có thay đổi trong quá trình phát triển

Chỉ khi cấu trúc database được thay đổi và lập trình viên muốn cập nhật lại các Entity thì mới cần Scaffold lại.

Ví dụ:

```bash
dotnet ef dbcontext scaffold "CONNECTION_STRING" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --force
```

Người chỉ muốn clone và chạy project bình thường **không cần thực hiện bước này**.

---

# 🔒 14. Lưu ý bảo mật

Không đưa lên GitHub các thông tin như:

```text
SQL Password
API Key
Access Token
Secret Key
```

Các file do người dùng upload cũng không nên commit lên repository.

Các thư mục sau thường được bỏ qua bằng `.gitignore`:

```text
.vs/
bin/
obj/
node_modules/
Uploads/
```

---

# 📌 Đề tài

**Xây dựng hệ thống quản lý đào tạo và hồ sơ sinh viên trực tuyến**

---

# 📚 Mục đích

Project được xây dựng phục vụ mục đích học tập và nghiên cứu trong quá trình thực hiện đồ án cơ sở.
