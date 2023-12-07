using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Data.Entity;


namespace app.Routes;
// [ApiController]
[Route("nha-cung-ung")]
[Authorize(Roles ="ADMIN, EMPLOYEE")]
public class NhaCungUngController : Controller
{
    private ApplicationDbContext dbContext;
    public NhaCungUngController(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }
    // [Authorize(Roles = "ADMIN,USER")]
    public ActionResult Index(int index){
        var countNhaCungUng = dbContext.NhaCungUng.Count(x=>x.IsDeleted==false);
        var countNhaCungUngResore = dbContext.NhaCungUng.Count(x=>x.IsDeleted==true);
        // var listNhaCungUng = dbContext.NhaCungUng
        //                               .OrderByDescending(x=>x.UpdateAt)
        //                               .Where(x=>x.IsDeleted==false)
        //                               .Select(
        //                                 x=>new{
        //                                     Id = x.Id,
        //                                     Name = x.TenNhaCungUng
        //                                 }).Skip(index).Take(10);
        // ViewBag.result = listNhaCungUng;
        ViewBag.count = countNhaCungUng;
        ViewBag.countRestore = countNhaCungUngResore;
        return View();
    }
    [Route("edit")]
    public ActionResult Update(long id){
        var listNhaCungUng = dbContext.NhaCungUng
                             .Where(x=>x.Id==id)
                             .Select(x=>new{
                                Id = x.Id,
                                Name = x.TenNhaCungUng
                             }).FirstOrDefault();
        ViewBag.result = listNhaCungUng;
        return View();
    }
    [Route("create")]
    public ActionResult Create(long id){

        return View();
    }
    [Route("restore")]
    public ActionResult Restore(){
        var countNhaCungUng = dbContext.NhaCungUng.Count(x=>x.IsDeleted==true);
        // var listNhaCungUng = dbContext.NhaCungUng
        //                               .OrderByDescending(x=>x.UpdateAt)
        //                               .Where(x=>x.IsDeleted==true)
        //                               .Select(
        //                                 x=>new{
        //                                     Id = x.Id,
        //                                     Name = x.TenNhaCungUng
        //                                 });
        // ViewBag.result = listNhaCungUng;
        ViewBag.count = countNhaCungUng;
        return View();
    }
    [Route("danh-sach-vat-tu")]
    public ActionResult ListVatTuCungUng(long id){
        var listNhaCungUng = dbContext.VatTuNhaCungUngRelation
                                    .Where(x=>x.NhaCungUngId == id && x.IsDeleted==false)
                                    .Include(x=>x.NhaCungUng)
                                    .Include(x=>x.VatTu)
                                    .Select(x=>new {
                                        x.Id,
                                        x.VatTu,
                                        x.NhaCungUng
                                    }).ToList();
        ViewBag.listNhaCungUng = listNhaCungUng;
        return View();
    }
}
