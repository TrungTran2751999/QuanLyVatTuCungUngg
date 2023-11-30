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
            // var listPhieuDaDuyet = new List<PhieuDeNghiNhanVatTuDaDuyet>();
            // var listPhieuFast = new List<PhieuNhanVatTuFast>();
            // if(status=="da-duyet"){
            //     listPhieuDaDuyet = await applicationDbContext.PhieuDeNghiNhanVatTuDaDuyet
            //                            .Where(x=>x.IsDeleted==false)
            //                            .OrderByDescending(x=>x.CreatedTime)
            //                            .Skip(0).Take(10)
            //                            .ToListAsync();
            // }else if(status=="chua-duyet"){
            //     listPhieuDaDuyet = await applicationDbContext.PhieuDeNghiNhanVatTuDaDuyet
            //                            .Where(x=>x.IsDeleted==false)
            //                            .OrderByDescending(x=>x.CreatedTime)
            //                            .ToListAsync();
            //     var listPhieu = applicationDbContextPhieuNVT.PhieuNhanVatTuFasts.FromSqlRaw("EXEC FastBusiness$App$Voucher$Loading {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
            //     "PX0", 
            //     "c87$000000", 
            //     "m87$", 
            //     "ngay_ct", 
            //     "convert(char(6), {0}, 112)", 
            //     "000000", 
            //     200, 
            //     "stt_rec", 
            //     "rtrim(stt_rec) as stt_rec,rtrim(ma_dvcs) as ma_dvcs,ngay_ct,rtrim(so_ct) as so_ct,rtrim(dept_id) as dept_id,t_so_luong,rtrim(dien_giai) as dien_giai,rtrim(ma_ct) as ma_ct,rtrim(status) as status,datetime0,datetime2,user_id0,user_id2", 
            //     "rtrim(stt_rec) as stt_rec,rtrim(ma_dvcs) as ma_dvcs,ngay_ct,rtrim(so_ct) as so_ct,rtrim(a.dept_id) as dept_id,b.ten_bp,t_so_luong,rtrim(dien_giai) as dien_giai,rtrim(a.ma_ct) as ma_ct,rtrim(a.status) as status,rtrim(u0.statusname) as u0,a.datetime0,a.datetime2,a.user_id0,rtrim(u1.u_name) as u1,a.user_id2,rtrim(u2.u_name) as u2,'''' as Hash", 
            //     "a left join dmbp b on a.dept_id = b.ma_bp left join dmttct u0 on a.ma_ct = u0.ma_ct and a.status = u0.status left join vsysuser u1 on a.user_id0 = u1.u_id left join vsysuser u2 on a.user_id2 = u2.u_id", 
            //     "ngay_ct, so_ct", 
            //     1, 
            //     1, 
            //     1, 
            //     0).ToArray().Reverse();
            // }
            // ViewBag.listPhieuDaDuyet = listPhieuDaDuyet;
            // ViewBag.listPhieuFast = listPhieuFast;
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