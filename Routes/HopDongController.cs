using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.EntityFrameworkCore;
using app.DTOs;
using Microsoft.AspNetCore.Authorization;

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
    public async Task<IActionResult> Create()
    {
        
        return View();
    }
    [Route("update")]
    public async Task<IActionResult> Update()
    {
        
        return View();
    }
    
}
