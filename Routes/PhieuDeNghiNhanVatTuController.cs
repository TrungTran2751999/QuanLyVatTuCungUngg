using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using app.Models;
using app.DTOs;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyVatTu.Routes
{
    [Route("phieu-de-nghi")]
    [Authorize(Roles ="ADMIN, EMPLOYEE")]
    public class PhieuDeNghiNhanVatTuController : Controller
    {
        private ApplicationDbContextUsers applicationDbContextUsers;
        private ApplicationDbContext applicationDbContext;
        private ApplicationDbContextPhieuNVT applicationDbContextPhieuNVT;

        public PhieuDeNghiNhanVatTuController(ApplicationDbContextUsers applicationDbContextUsers, ApplicationDbContext applicationDbContext, ApplicationDbContextPhieuNVT applicationDbContextPhieuNVT)
        {
            this.applicationDbContextUsers = applicationDbContextUsers;
            this.applicationDbContext = applicationDbContext;
            this.applicationDbContextPhieuNVT = applicationDbContextPhieuNVT;
        }
        public async Task<IActionResult> Index(string status)
        {
            return View();
        }
        [Route("chi-tiet")]
        public async Task<IActionResult> Chitiet()
        {
            return View();
        }
        [Route("tong-hop")]

        public async Task<IActionResult> Tonghop()
        {
            var listNhaCungUng = await applicationDbContext.NhaCungUng
                                      .OrderBy(x=>x.TenNhaCungUng)
                                      .Where(x=>x.IsDeleted==false)
                                      .Select(
                                        x=>new NhaCungUngVatTu{
                                            Id = x.Id,
                                            TenNhaCungUng = x.TenNhaCungUng
                                        }).ToListAsync();
            ViewBag.listNhaCungUng = listNhaCungUng;
            return View();
        }
    }
}