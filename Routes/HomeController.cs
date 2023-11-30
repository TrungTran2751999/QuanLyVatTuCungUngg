using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.EntityFrameworkCore;
using app.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace app.Controllers;
[Route("")]
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ApplicationDbContext dbContext1;
    private ApplicationDbContextPhieuNVT dbContext;
    public HomeController(ApplicationDbContext dbContext1, ApplicationDbContextPhieuNVT dbContext){
        this.dbContext = dbContext;
        this.dbContext1 = dbContext1;
    }

    public async Task<IActionResult> Index()
    {
        var listPhieuDaDuyet = await dbContext1.PhieuDeNghiNhanVatTuDaDuyet
                                       .Where(x=>x.IsDeleted==false)
                                       .OrderByDescending(x=>x.CreatedTime)
                                       .ToListAsync();

        var listPhieu = dbContext.PhieuNhanVatTuFasts.FromSqlRaw("EXEC FastBusiness$App$Voucher$Loading {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
        "PX0", 
        "c87$000000", 
        "m87$", 
        "ngay_ct", 
        "convert(char(6), {0}, 112)", 
        "000000", 
        200, 
        "stt_rec", 
        "rtrim(stt_rec) as stt_rec,rtrim(ma_dvcs) as ma_dvcs,ngay_ct,rtrim(so_ct) as so_ct,rtrim(dept_id) as dept_id,t_so_luong,rtrim(dien_giai) as dien_giai,rtrim(ma_ct) as ma_ct,rtrim(status) as status,datetime0,datetime2,user_id0,user_id2", 
        "rtrim(stt_rec) as stt_rec,rtrim(ma_dvcs) as ma_dvcs,ngay_ct,rtrim(so_ct) as so_ct,rtrim(a.dept_id) as dept_id,b.ten_bp,t_so_luong,rtrim(dien_giai) as dien_giai,rtrim(a.ma_ct) as ma_ct,rtrim(a.status) as status,rtrim(u0.statusname) as u0,a.datetime0,a.datetime2,a.user_id0,rtrim(u1.u_name) as u1,a.user_id2,rtrim(u2.u_name) as u2,'''' as Hash", 
        "a left join dmbp b on a.dept_id = b.ma_bp left join dmttct u0 on a.ma_ct = u0.ma_ct and a.status = u0.status left join vsysuser u1 on a.user_id0 = u1.u_id left join vsysuser u2 on a.user_id2 = u2.u_id", 
        "ngay_ct, so_ct", 
        1, 
        1, 
        1, 
        0).ToArray().Reverse();

        ViewBag.listPhieuDaDuyet = listPhieuDaDuyet;
        ViewBag.listPhieuFast = listPhieu;

        return View();
    }
    [Route("/404")]
    public ActionResult NotFound(){
        return View();
    }
    [Route("/500")]
    public ActionResult ErrorServer(){
        return View();
    }
    [Route("tai-khoan-ca-nhan")]
    public ActionResult CaiDat(){
        return View();
    }
}
