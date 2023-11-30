using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.EntityFrameworkCore;
using app.DTOs;

namespace app.Controllers;
[Route("/login")]
public class LoginController : Controller
{
    public async Task<IActionResult> Index()
    {
        
        return View();
    }
    
}
