using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.EntityFrameworkCore;
using app.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace app.Controllers;
[Route("hop-dong")]
[Authorize]
public class HopDongController : Controller
{
    public async Task<IActionResult> Index()
    {
        
        return View();
    }
    [Route("create")]
    public async Task<IActionResult> Create([FromForm] string data)
    {
        if(data!=null){
            var result = JsonSerializer.Deserialize<HopDongResponse>(data);
            ViewBag.result = result;
        }else{
            ViewBag.result = new HopDongResponse();
        }
        return View();
    }
    [Route("update")]
    public async Task<IActionResult> Update()
    {
        return View();
    }
    
}
