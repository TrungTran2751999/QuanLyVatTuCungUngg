using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
// using System.Data.Entity;
namespace app.Services;
public class VatTuBaoGiaChiTietService : IVatTuBaoGiaChiTietService
{
    private readonly ApplicationDbContext dbContext;

    public VatTuBaoGiaChiTietService(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }

    public Task<VatTuBaoGiaChiTiet> GetByBaoGiaChiTietId(Guid BaoGiaChiTietId)
    {
        throw new NotImplementedException();
    }
}