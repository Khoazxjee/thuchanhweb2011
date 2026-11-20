# Tách Layout, Header, Footer và MainContent từ template gốc

File gốc của đề là **Template/index.html**. Đây là một trang HTML hoàn chỉnh, chứa cả phần đầu trang, thân trang và chân trang. Khi chuyển sang MVC, ta chia nội dung này vào nhiều file `.cshtml` để dùng chung và đưa dữ liệu từ SQL Server vào giao diện.

Trong bài của bạn, các file giao diện nằm dưới `HoangNgocKhoa241230762THW/Views`.

## 1. Bảng đối chiếu: copy gì, viết thêm gì?

| File đích | Copy từ index.html | Phần sửa hoặc thêm |
|---|---|---|
| `Shared/HoangNgocKhoa_Layout.cshtml` | Khung `<!DOCTYPE html>`, `html`, `head`, `body`; khung `main`; các link CSS, script và nút lên đầu trang | Đổi đường dẫn thành `~/...`; thêm hai thẻ `partial`, `@RenderBody()` và `RenderSectionAsync` |
| `Shared/HoangNgocKhoa_Header.cshtml` | Toàn bộ `<header class="header">` đến `</header>` | Menu tĩnh đổi thành vòng lặp đọc thể loại; đổi Trang chủ thành họ tên; dùng đường dẫn MVC cho logo và menu |
| `Shared/HoangNgocKhoa_Footer.cshtml` | Toàn bộ `<footer class="footer">` đến `</footer>` | Giữ nội dung, chỉ trình bày lại cho dễ đọc; không cần truy vấn database |
| `Shared/HoangNgocKhoa_MainContent.cshtml` | Khung `section-head`, `grid` và mẫu một `book-card` | Thêm `@model`, `@foreach`; tiêu đề, ảnh, tên, giá lấy từ dữ liệu; thêm liên kết đến form Create |
| `Home/Index.cshtml` | Khối banner trong phần main | Thêm vùng `noiDungSach`, gọi partial MainContent và JavaScript AJAX |

Bạn có thể nhận ra các vùng trong file gốc nhờ các dòng `Begin Header`, `End Header`, `Begin Main Content`, `Begin Footer` và `End Footer`.

## 2. Layout là khung dùng chung

Mở file gốc. Phần từ `<!DOCTYPE html>` đến `<body>` cho biết cấu trúc đầu trang và các file CSS cần nạp. Phần cuối có các script, `</body>`, `</html>`. Những phần này trở thành khung Layout.

Trong Layout hoàn chỉnh, ba vị trí quan trọng là:

```cshtml
<body>
    <partial name="HoangNgocKhoa_Header" />

    <main class="main">
        <div class="container">
            @RenderBody()
        </div>
    </main>

    <partial name="HoangNgocKhoa_Footer" />

    <div class="to-top">&uarr;</div>
    <script src="~/js/jquery.min.js"></script>
    <script src="~/js/main.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
```

Đây là phần thân Layout để giải thích; file Layout thực tế còn có `<!DOCTYPE html>`, `<html>`, `<head>` và các thẻ đóng tương ứng.

- `<partial name="HoangNgocKhoa_Header" />`: đưa nội dung file Header vào vị trí này.
- `@RenderBody()`: đưa nội dung View đang được trả về vào đây. Khi vào trang chủ, đó là `Views/Home/Index.cshtml`; khi vào form thêm, đó là `Views/Home/Create.cshtml`.
- `<partial name="HoangNgocKhoa_Footer" />`: đưa nội dung Footer vào cuối trang.
- `RenderSectionAsync("Scripts", required: false)`: cho từng trang bổ sung script riêng. Index bổ sung AJAX; Create bổ sung kiểm tra dữ liệu form.

Các thẻ `partial`, `@RenderBody()` và `RenderSectionAsync` là phần **viết thêm cho Razor**, không có trong HTML gốc.

Đường dẫn cũng được sửa:

```html
<link href="css/style.css" rel="stylesheet" />
```

thành:

```cshtml
<link href="~/css/style.css" rel="stylesheet" />
```

Trong ứng dụng, đường dẫn này trỏ đến `wwwroot/css/style.css`, và vẫn đúng khi bạn chuyển sang `/Home/Create`.

## 3. Header: copy khung, thay menu bằng dữ liệu

Trong file gốc, copy từ `<header class="header">` đến `</header>` sang file Header. Các phần hotline, logo, giỏ hàng và thanh menu lấy từ mẫu.

Menu gốc là những dòng gõ cố định như:

```html
<li class="active"><a href="index.html">Trang chủ</a></li>
<li><a href="#">Giới thiệu</a></li>
```

Đề yêu cầu menu lấy từ bảng TheLoai, vì vậy không dùng các mục “Giới thiệu”, “Khuyến mãi”, “Tin tức”, “Liên hệ” làm menu chính. Trong bài hoàn chỉnh:

1. Controller đọc bảng `TheLoai`, đưa danh sách vào `ViewBag.DanhSachLoai`.
2. Đầu file Header nhận danh sách đó bằng Razor.
3. Mục đầu tiên hiển thị **Hoàng Ngọc Khoa**.
4. `@foreach` sinh các mục còn lại từ dữ liệu.

Ví dụ phần vòng lặp:

```cshtml
@foreach (var loai in danhSachLoai)
{
    <li class="@(maTLDangChon == loai.MaTL ? "active" : "")">
        <a class="loc-the-loai"
           asp-controller="Home"
           asp-action="Index"
           asp-route-maTL="@loai.MaTL"
           data-url="@Url.Action("LocTheoLoai", "Home", new { maTL = loai.MaTL })">
            @loai.TenTL
        </a>
    </li>
}
```

Phần `<li>` và `<a>` vẫn là cấu trúc HTML của menu. `@foreach`, `@loai.TenTL`, `asp-route-maTL` và `data-url` là phần thêm để nối với dữ liệu và AJAX.

`asp-action="Index"` tạo đường dẫn về trang chủ. `data-url` chứa đường dẫn lấy riêng danh sách sách, dùng khi bấm menu từ trang Index. Nhờ vậy bấm menu khi đang ở form Create vẫn chuyển về trang chủ được.

## 4. Footer: copy nguyên vùng chân trang

Copy toàn bộ từ `<footer class="footer">` đến `</footer>` sang `HoangNgocKhoa_Footer.cshtml`.

Nội dung “Về BookHub”, “Hỗ trợ khách hàng”, “Tài khoản”, “Liên hệ” và bản quyền có sẵn trong template. Đoạn này không cần C#, không có vòng lặp và không có controller riêng.

Không đưa `</body>`, `</html>` hoặc các thẻ script ở sau Footer vào file này: chúng thuộc Layout. Layout gọi Footer bằng thẻ `partial`.

## 5. MainContent: biến một ô sách mẫu thành danh sách

Template có nhiều `book-card` chứa tên sách, giá và ảnh viết cố định. Ta lấy cấu trúc **một ô sách**, rồi lặp theo danh sách lấy từ database.

Phần HTML gốc có dạng:

```html
<h2>Lập trình C# căn bản</h2>
<span class="price">120</span>
```

Phần tương ứng trong Razor là:

```cshtml
<h2>@sach.TenSach</h2>
<span class="price">@(sach.Gia?.ToString("0"))</span>
```

Các phần mới ở MainContent gồm:

- `@model IEnumerable<Sach>`: khai báo View nhận một danh sách sách.
- `@foreach (var sach in Model)`: mỗi sách tạo một `book-card`.
- `@Url.Content("~/images/" + sach.Anh)`: lấy tên ảnh từ database.
- `@ViewBag.TieuDe`: hiện họ tên hoặc tên thể loại.
- Liên kết `asp-action="Create"`: mở form thêm sách.

MainContent chứa cả **tiêu đề và danh sách**. Vì vậy một lần thay partial bằng AJAX sẽ đổi được cả hai, đúng yêu cầu đề.

## 6. Index và Create dùng chung Layout thế nào?

`Views/_ViewStart.cshtml` có:

```cshtml
@{
    Layout = "HoangNgocKhoa_Layout";
}
```

Đây là phần phải sửa từ `_Layout` mặc định sang tên Layout của bài. Tên trong chuỗi không có phần mở rộng `.cshtml`.

Khi mở trang chủ:

```text
HomeController.Index
  -> Views/Home/Index.cshtml
  -> Layout ghép Header + Index + Footer
  -> Index chứa banner + MainContent
```

Khi mở form thêm:

```text
HomeController.Create
  -> Views/Home/Create.cshtml
  -> Layout ghép Header + form thêm + Footer
```

Khi bấm một thể loại trên trang chủ:

```text
JavaScript ở Index chặn tải lại trang
  -> gọi Home/LocTheoLoai?maTL=...
  -> Controller trả riêng partial MainContent
  -> JavaScript thay nội dung div noiDungSach
```

Banner lấy từ template và đặt ở Index. Form Create và đoạn AJAX là phần **viết mới**, vì HTML gốc chưa có chức năng thêm sách hoặc lọc dữ liệu.

## 7. Phần không copy được từ HTML

`HomeController.cs`, `SachCreateModel.cs`, cấu hình kết nối trong `appsettings.json` và đăng ký DbContext trong `Program.cs` phải được viết thêm cho MVC/EF Core. Những file này xử lý database, điều kiện lọc, kiểm tra form và lưu dữ liệu.

Ba file `Sach.cs`, `TheLoai.cs`, `QLNhaSachContext.cs` được scaffold từ database theo Database First.

Các thư mục CSS, JS và images được copy từ template vào `wwwroot` tương ứng. Riêng JavaScript lọc AJAX nằm trong section Scripts của Index; `main.js` của template chỉ đảm nhiệm hành vi giao diện có sẵn như nút lên đầu trang.
