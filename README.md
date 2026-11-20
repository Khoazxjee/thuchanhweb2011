# HoangNgocKhoa241230762THW

Bài thực hành theo đề mẫu Lập trình Web 2026: ASP.NET Core MVC, .NET 8, Entity Framework Core Database First, SQL Server Express.

## Chạy bài

1. Mở solution `HoangNgocKhoa241230762THW.slnx` bằng Visual Studio 2026, hoặc mở file `.sln` cùng tên nếu dùng Visual Studio hỗ trợ .NET 8.
2. Trong SSMS, kết nối `localhost\SQLEXPRESS` bằng **Windows Authentication**.
3. Database dùng cho bài là `QLNhaSach`. Trên máy hiện tại database đã có 5 thể loại và 14 sách mẫu.
4. Kiểm tra `HoangNgocKhoa241230762THW/appsettings.json`: mục `ConnectionStrings:QLNhaSach` trỏ đến SQL Server trên.
5. Chọn project `HoangNgocKhoa241230762THW` làm Startup Project, chọn profile **http**, nhấn **Ctrl + F5**.
6. Mở `http://localhost:5253` nếu trình duyệt không tự mở.

Project đã có đủ các gói `Microsoft.EntityFrameworkCore.SqlServer`, `Design`, `Tools` phiên bản **8.0.33**. Model `Sach`, `TheLoai`, `QLNhaSachContext` đã được sinh từ database.

Nếu chuyển sang máy chưa có database, dùng `Database/Database_script.sql` từ bộ đề. **Script gốc xóa database QLNhaSach nếu đã tồn tại rồi tạo lại dữ liệu mẫu. Chỉ chạy khi khởi tạo hoặc khi chủ động muốn đặt lại dữ liệu.** Khi chuyển máy, sửa tên server trong `appsettings.json` theo kết nối thực tế ở SSMS.

## Nội dung đã làm

| Yêu cầu | Vị trí thực hiện |
|---|---|
| Layout, Header, Footer, MainContent theo họ tên | `Views/Shared/HoangNgocKhoa_*.cshtml` |
| CSS, JS, ảnh từ template | `wwwroot/css`, `wwwroot/js`, `wwwroot/images` |
| Database First | `Models/Sach.cs`, `TheLoai.cs`, `QLNhaSachContext.cs` |
| Menu thể loại đọc từ database | `HomeController.NapTheLoai()` và Header |
| Trang chủ: giá từ 100, loại giá NULL | `HomeController.LaySach()` |
| Lọc thể loại bằng AJAX, đổi cả tiêu đề | `Views/Home/Index.cshtml`, `LocTheoLoai()` và MainContent |
| Thêm sách và lưu SQL Server | Hai action `Create()` và `Views/Home/Create.cshtml` |
| Data Annotations và thông báo trên form | `Models/SachCreateModel.cs` |

Giá dùng đơn vị **nghìn đồng**: nhập `120` cho 120 nghìn đồng. Form chấp nhận giá từ **50 đến 1000**, tên sách không trống, tên ảnh kết thúc bằng `.jpg`, `.jpeg`, `.png`, `.webp`. Trường ảnh nhận **tên file**, ví dụ `Img_1.jpg`.

Trang chủ ban đầu hiện 7 sách. Khi chọn Công nghệ thông tin, hiện 4 sách có giá, bao gồm “Nhập môn lập trình” giá 70 theo hình minh họa câu 4. Sau khi thêm, ứng dụng trở về trang chủ với thể loại của sách vừa thêm để thấy được sách giá 50–99.

Các mục giỏ hàng, tìm kiếm, đăng nhập trong template chỉ là thành phần giao diện, không có chức năng ngoài đề.

## Hiểu phần tách giao diện

Đọc [HUONG_DAN_TACH_LAYOUT.md](HUONG_DAN_TACH_LAYOUT.md) để biết chính xác phần nào copy từ `Template/index.html`, phần nào phải sửa thành Razor và phần nào viết mới.

## Kiểm tra trước khi nộp

- Build project thành công.
- Trang chủ có tên Hoàng Ngọc Khoa, menu đọc đủ năm thể loại, ảnh và CSS hiển thị.
- Chọn thể loại đổi danh sách và tiêu đề mà không tải lại toàn trang.
- Form hiển thị tên thể loại; lưu đúng mã thể loại.
- Nhập sai tên, giá hoặc đuôi ảnh thì có thông báo và không lưu.
- Thêm hợp lệ lưu vào bảng Sach và trở về danh sách của thể loại tương ứng.
- Tên project hiện dùng đúng tên bạn đã đặt: `HoangNgocKhoa241230762THW`. Đối chiếu lại quy định tên bài nộp của giảng viên.
- Video ghi quá trình thực hiện và quyền truy cập link video được nộp riêng theo yêu cầu của đề.

Bộ mã nguồn kèm SQL là bài làm; các bản sao lưu và chương trình kiểm tra được giữ ngoài thư mục bài nộp.

## Kết quả kiểm tra

- Build solution: 0 lỗi, 0 cảnh báo.
- 33 kiểm tra trên Chrome: giao diện, ảnh, năm thể loại, tiêu đề, AJAX không tải lại trang, lỗi form và điều hướng.
- 102 kiểm tra HTTP: dữ liệu hiển thị, partial, tài nguyên, 11 trường hợp form sai và thêm thật sách ở hai giá biên 50/1000.
- Kiểm tra ghi dữ liệu thực hiện trên database tạm riêng. Database `QLNhaSach` của bài vẫn giữ 5 thể loại và 14 sách mẫu.
- Chạy ứng dụng với cấu hình thật của bài: trang chủ trả HTTP 200 và hiển thị 7 sách.
