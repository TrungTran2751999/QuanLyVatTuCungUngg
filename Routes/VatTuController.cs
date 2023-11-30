using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Data.Entity;
using app.DTOs;


namespace app.Routes;
// [ApiController]
[Route("vat-tu")]
[Authorize(Roles ="ADMIN, EMPLOYEE")]
public class VatTuController : Controller
{
    private ApplicationDbContext dbContext;
    public VatTuController(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }
    // [Authorize(Roles = "ADMIN,USER")]
    public ActionResult Index(){
        var countVatTu = dbContext.VatTu.Count();
        ViewBag.count = countVatTu;
        return View();
    }
    [Route("nha-cung-ung")]
    public ActionResult ListNhaCungUng(long idVatTu){
        var listNhaCungUngInclude = dbContext.VatTuNhaCungUngRelation
                                       .OrderBy(x=>x.CreatedTime)
                                       .Where(x=>x.MaVatTu==idVatTu && x.IsDeleted==false)
                                       .Include(x=>x.NhaCungUng)
                                       .Select(x=>new VatTuNhaCungUngDTO{
                                            Id = x.Id,
                                            NhaCungUngId = (long)x.NhaCungUngId,
                                            TenNhaCungUng = x.NhaCungUng.TenNhaCungUng
                                       }).ToList();
        var listNhaCungUng = dbContext.NhaCungUng.OrderBy(x=>x.TenNhaCungUng).ToList();
        
        ViewBag.listNhaCungUng = listNhaCungUng;
        ViewBag.listVatTu = listNhaCungUngInclude;
        return View();
    }
}
