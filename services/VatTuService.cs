using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
using app.Utils;
// using System.Data.Entity;
namespace app.Services;
public class VatTuService : IVatTuService
{
    private readonly ApplicationDbContext dbContext;
    private readonly ApplicationDbContextQLVT dbContextQLVT;
    private readonly IUtil util;

    public VatTuService(ApplicationDbContext dbContext, ApplicationDbContextQLVT dbContextQLVT, IUtil util){
        this.dbContext = dbContext;
        this.dbContextQLVT = dbContextQLVT;
        this.util = util;
    }

    public async Task<List<VatTu>> GetAll(int index)
    {
        var listVatTu = await dbContextQLVT.VatTu
                                      .OrderBy(x=>x.Id)
                                      .Select(
                                        x=>new VatTu{
                                            Id = x.Id,
                                            TenVatTu = x.TenVatTu,
                                            MaVatTu = x.MaVatTu
                                        }).Skip(index).Take(10).ToListAsync();
        
        return listVatTu;
    }

    public async Task<List<NhaCungUngVatTu>> GetNhaCungUngByVatTu(long idVatTu)
    {
        var listVatTu = await dbContext.VatTuNhaCungUngRelation
                                       .OrderBy(x=>x.NhaCungUng)
                                       .Where(x=>x.MaVatTu==idVatTu)
                                       .Include(x=>x.NhaCungUng)
                                       .Select(x=>new NhaCungUngVatTu{
                                            Id = (long)x.NhaCungUngId,
                                            TenNhaCungUng = x.NhaCungUng.TenNhaCungUng
                                        }).ToListAsync();
        return listVatTu;
    }

    public async Task<VatTuSearch> Search(string hint, int index)
    {
         hint = util.RemoveDauTiengViet(hint);
         var listVatTu = await dbContextQLVT.VatTu
                                      .OrderBy(x=>x.TenVatTu)
                                      .Where(x=>x.TenVatTu.ToLower().Contains(hint.ToLower()) || x.MaVatTu.Contains(hint.ToLower()))
                                      .Select(
                                        x=>new VatTu{
                                            Id = x.Id,
                                            TenVatTu = x.TenVatTu,
                                            MaVatTu = x.MaVatTu
                                        })
                                        .Skip(index).Take(10).ToListAsync();
        // var listVatTu = (from vatTu in dbContextQLVT.VatTu
        //              where util.RemoveDauTiengViet(vatTu.TenVatTu.ToLower()).Contains(hint.ToLower()) || util.RemoveDauTiengViet(vatTu.MaVatTu.ToLower()).Contains(hint.ToLower())
        //              select new VatTu{
        //                 Id = vatTu.Id,
        //                 TenVatTu = vatTu.TenVatTu,
        //                 MaVatTu = vatTu.MaVatTu
        //              }).ToList();
        var countSearch = await dbContextQLVT.VatTu
                                         .Where(x=>x.TenVatTu.ToLower().Contains(hint.ToLower()) || x.MaVatTu.ToLower().Contains(hint.ToLower()))
                                         .CountAsync();
        VatTuSearch vatTuSearch = new()
        {
            ListVatTu = listVatTu,
            Count = (long)countSearch
        };
        return vatTuSearch;
    }
}