using HoangNgocKhoa241230762THW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HoangNgocKhoa241230762THW.Controllers;

public class HomeController : Controller
{
    private readonly QLNhaSachContext _db;

    public HomeController(QLNhaSachContext db)
    {
        _db = db;
    }

    private async Task NapTheLoai()
    {
        var danhSach = await _db.TheLoai
            .AsNoTracking()
            .OrderBy(x => x.MaTL)
            .ToListAsync();

        ViewBag.DanhSachLoai = danhSach;
        ViewBag.MaTL = new SelectList(danhSach, "MaTL", "TenTL");
    }

    private IQueryable<Sach> LaySach(int? maTL)
    {
        var query = _db.Sach
            .AsNoTracking()
            .Where(x => x.Gia.HasValue);

        if (maTL.HasValue)
        {
            query = query.Where(x => x.MaTL == maTL.Value);
        }
        else
        {
            query = query.Where(x => x.Gia >= 100);
        }

        return query.OrderBy(x => x.MaSach);
    }

    private async Task<string?> LayTieuDe(int? maTL)
    {
        if (!maTL.HasValue)
        {
            return "Hoàng Ngọc Khoa";
        }

        return await _db.TheLoai
            .AsNoTracking()
            .Where(x => x.MaTL == maTL.Value)
            .Select(x => x.TenTL)
            .FirstOrDefaultAsync();
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? maTL)
    {
        var tieuDe = await LayTieuDe(maTL);

        if (tieuDe == null)
        {
            return NotFound();
        }

        await NapTheLoai();
        ViewBag.MaTLDangChon = maTL;
        ViewBag.TieuDe = tieuDe;

        var danhSach = await LaySach(maTL).ToListAsync();
        return View(danhSach);
    }

    [HttpGet]
    public async Task<IActionResult> LocTheoLoai(int? maTL)
    {
        var tieuDe = await LayTieuDe(maTL);

        if (tieuDe == null)
        {
            return NotFound();
        }

        ViewBag.TieuDe = tieuDe;

        var danhSach = await LaySach(maTL).ToListAsync();
        return PartialView("HoangNgocKhoa_MainContent", danhSach);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await NapTheLoai();
        return View(new SachCreateModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SachCreateModel model)
    {
        var loaiTonTai = await _db.TheLoai
            .AnyAsync(x => x.MaTL == model.MaTL);

        if (!loaiTonTai)
        {
            ModelState.AddModelError(
                nameof(model.MaTL),
                "Thể loại không tồn tại.");
        }

        if (!ModelState.IsValid)
        {
            await NapTheLoai();
            return View(model);
        }

        var sach = new Sach
        {
            MaTL = model.MaTL,
            TenSach = model.TenSach.Trim(),
            Gia = model.Gia,
            Anh = model.Anh.Trim()
        };

        _db.Sach.Add(sach);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index), new { maTL = model.MaTL });
    }
}
