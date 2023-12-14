using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.EntityFrameworkCore;
using app.DTOs;
using app.Services;
using Microsoft.AspNetCore.Authorization;

namespace app.Controllers;
[Route("bao-gia")]
[Authorize]
public class BaoGiaController : Controller
{
    private ApplicationDbContext dbContext;
    private IBaogiaService baogiaService;
    public BaoGiaController(ApplicationDbContext dbContext, IBaogiaService baogiaService){
        this.dbContext = dbContext;
        this.baogiaService = baogiaService;
    }
    public ActionResult Index(){
        return View();
    }
    [Route("chi-tiet")]
    public async Task<ActionResult> ChiTiet(){
        var listNhaCungUng = await dbContext.NhaCungUng.ToListAsync();
        ViewBag.listNhaCungUng = listNhaCungUng;
        return View();
    }
    [Route("cap-nhat")]
    public async Task<ActionResult> Update(Guid id){
        var listBaogiaChiTiet = await baogiaService.GetById(id);
        var listNhaCungUng = await dbContext.NhaCungUng
                                      .OrderBy(x=>x.TenNhaCungUng)
                                      .Where(x=>x.IsDeleted==false)
                                      .Select(
                                        x=>new NhaCungUngVatTu{
                                            Id = x.Id,
                                            TenNhaCungUng = x.TenNhaCungUng
                                        }).ToListAsync();
        ViewBag.listNhaCungUng = listNhaCungUng;
        ViewBag.listBaogiaChiTiet = listBaogiaChiTiet;
        return View();
    }
    
}
