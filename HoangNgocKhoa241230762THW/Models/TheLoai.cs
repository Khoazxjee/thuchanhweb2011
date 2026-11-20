using System;
using System.Collections.Generic;

namespace HoangNgocKhoa241230762THW.Models;

public partial class TheLoai
{
    public int MaTL { get; set; }

    public string TenTL { get; set; } = null!;

    public virtual ICollection<Sach> Sach { get; set; } = new List<Sach>();
}
