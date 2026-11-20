using System.ComponentModel.DataAnnotations;

namespace HoangNgocKhoa241230762THW.Models;

public class SachCreateModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Hãy chọn thể loại.")]
    public int MaTL { get; set; }

    [Required(ErrorMessage = "Tên sách không được để trống.")]
    [StringLength(100, ErrorMessage = "Tên sách tối đa 100 ký tự.")]
    public string TenSach { get; set; } = "";

    [Required(ErrorMessage = "Hãy nhập giá.")]
    [Range(typeof(decimal), "50", "1000",
        ErrorMessage = "Giá phải nằm trong khoảng 50 đến 1000.")]
    public decimal? Gia { get; set; }

    [Required(ErrorMessage = "Hãy nhập tên file ảnh.")]
    [StringLength(100)]
    [RegularExpression(@"^.+\.(jpg|jpeg|png|webp)$",
        ErrorMessage = "Ảnh phải có đuôi .jpg, .jpeg, .png hoặc .webp.")]
    public string Anh { get; set; } = "";
}
