using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;


namespace app.Routes;
// [ApiController]
[Route("nhom-vat-tu")]
[Authorize(Roles ="ADMIN, EMPLOYEE")]
public class NhomVatTuController : Controller
{
    private ApplicationDbContext dbContext;
    public NhomVatTuController(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }
    // [Authorize(Roles = "ADMIN,USER")]
    public ActionResult Index(){
        var listNhomVatTu = dbContext.NhomVatTu.ToList();
        ViewBag.result = listNhomVatTu;
        return View();
    }
}
