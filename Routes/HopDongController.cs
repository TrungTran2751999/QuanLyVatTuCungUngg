using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.EntityFrameworkCore;
using app.DTOs;

namespace app.Controllers;
[Route("/hop-dong")]
public class HopDongController : Controller
{
    public async Task<IActionResult> Index()
    {
        
        return View();
    }
    
}
