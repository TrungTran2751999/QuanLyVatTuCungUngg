using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
// using System.Data.Entity;
namespace app.Services;
public class VatTuService : IVatTuService
{
    private readonly ApplicationDbContext dbContext;

    public VatTuService(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }

    public async Task<List<VatTu>> GetAll(int index)
    {
        var listVatTu = await dbContext.VatTu
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
         var listVatTu = await dbContext.VatTu
                                      .OrderBy(x=>x.TenVatTu)
                                      .Where(x=>x.TenVatTu.Contains(hint) || x.MaVatTu.Contains(hint))
                                      .Select(
                                        x=>new VatTu{
                                            Id = x.Id,
                                            TenVatTu = x.TenVatTu,
                                            MaVatTu = x.MaVatTu
                                        })
                                        .Skip(index).Take(10).ToListAsync();
        var countSearch = await dbContext.VatTu
                                         .Where(x=>x.TenVatTu.Contains(hint) || x.MaVatTu.Contains(hint))
                                         .CountAsync();
        VatTuSearch vatTuSearch = new()
        {
            ListVatTu = listVatTu,
            Count = (long)countSearch
        };
        return vatTuSearch;
    }
}