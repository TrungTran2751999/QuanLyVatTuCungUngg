using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;
using app.DTOs;

namespace app.Services;
public class VatTuNhaCungUngRelationService : IVatTuNhaCungUngRelationService
{
    private readonly ApplicationDbContext dbContext;

    public VatTuNhaCungUngRelationService(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }
    public async Task<List<VatTuNhaCungUngRelation>?> GetAll()
    {
        var listData = await dbContext.VatTuNhaCungUngRelation
                                      .Include(x=>x.NhaCungUng)
                                      .Include(x=>x.VatTu)
                                      .Where(x=>x.IsDeleted==false)
                                      .ToListAsync();
        return listData;
    }
    public async Task<List<VatTuNhaCungUngRelation>> GetByNhaCungUng(long idNhaCungUng, bool isDeleted)
    {
        var listData = await dbContext.VatTuNhaCungUngRelation
                            .Where(x=>x.NhaCungUngId==idNhaCungUng && x.IsDeleted == isDeleted)
                            .ToListAsync();
        return listData;
    }
    public async Task<VatTuNhaCungUngResultAdding> AddVatTuToNhaCungUng(VatTuNhaCungUngAddingDTO vatTuAdding)
    {
        long idMax = 0;
        try{
            idMax = await dbContext.VatTuNhaCungUngRelation.MaxAsync(x=>x.Id);
        }catch{
            idMax = 0;
        };
        var result = new VatTuNhaCungUngResultAdding{
            idMax = 0L,
            result = ""
        };

        var checkData = await dbContext.VatTuNhaCungUngRelation
                                .Where(x=>x.NhaCungUngId==vatTuAdding.NhaCungUngId && x.MaVatTu==vatTuAdding.MaVatTu)
                                .FirstOrDefaultAsync();
        if(checkData!=null){
            if(checkData.IsDeleted==true){
                checkData.IsDeleted = false;
                result.idMax = checkData.Id;
                dbContext.SaveChanges();
                result.result = "OK";
            }else{
                result.result = "VatTu:Nhà cung ứng "+checkData.NhaCungUng+ " đang cung ứng vật tư "+checkData.TenVatTu;
            }
            return result;
        }
        

        var resultAdding = await dbContext.VatTuNhaCungUngRelation.AddAsync(vatTuAdding.ToModel(idMax));
        
        dbContext.SaveChanges();
        long idMax1 = await dbContext.VatTuNhaCungUngRelation.MaxAsync(x=>x.Id);

        result.idMax = idMax1;
        result.result = "OK";

        return result;
    }

    public async Task<string> RemoveVatTuFromNhaCungUng(long id)
    {
        var vatTu = await dbContext.VatTuNhaCungUngRelation
                                .Where(x=>x.Id==id)
                                .FirstOrDefaultAsync();
        if(vatTu==null) return "VatTu:BadRequest";

        vatTu.IsDeleted = true;
        
        await dbContext.SaveChangesAsync();

        return "OK";
    }

    public async Task<VatTuNhaCungUngResultAdding> UpdateVatTuNhaCungUng(VatTuNhaCungUngAddingDTO vatTuUpdated)
    {
        var result = new VatTuNhaCungUngResultAdding{
            idMax = 0L,
            result = ""
        };

        var nhaVatTu = await dbContext.NhaCungUng
                            .Where(x=>x.Id==vatTuUpdated.NhaCungUngId)
                            .FirstOrDefaultAsync();
        if(nhaVatTu==null){
            result.result = "NhaVatTu:Nhà cung ứng không tồn tại";
            return result;
        };
        //check xem co ton tai vat tu va nha cung ung khong
        var vatTu = await dbContext.VatTuNhaCungUngRelation
                                .Where(x=>x.Id==vatTuUpdated.Id)
                                .FirstOrDefaultAsync();
        if(vatTu==null){
            result.result = "VatTu:Nhà cung ứng "+vatTuUpdated.NhaCungUng+" cùng với loại vật tư "+vatTuUpdated.TenVatTu+" không tồn tại hoặc đã bị xóa";
            return result;
        };
        //check xem co trung ten nha cung ung khong
        var checkNhaVatTu = await dbContext.VatTuNhaCungUngRelation
                                .Where(x=>x.NhaCungUngId==vatTuUpdated.NhaCungUngId && x.MaVatTu==vatTuUpdated.MaVatTu && x.Id!=vatTuUpdated.Id)
                                .FirstOrDefaultAsync();
        
        if(checkNhaVatTu!=null){
            if(checkNhaVatTu.IsDeleted==true){
                checkNhaVatTu.IsDeleted = false;
                vatTu.IsDeleted = true;
                result.result = "OK";
                result.idMax = checkNhaVatTu.Id;
                dbContext.SaveChanges();
                return result;
            }else{
                result.result = "VatTu:Nhà cung ứng "+checkNhaVatTu.NhaCungUng?.TenNhaCungUng+ " đang cung ứng vật tư "+checkNhaVatTu.TenVatTu;
            }
            return result;
        }

        vatTu.NhaCungUngId = vatTuUpdated.NhaCungUngId;
        vatTu.TenNhaCungUng = vatTuUpdated.NhaCungUng;
        vatTu.MaVatTu = vatTuUpdated.MaVatTu;
        vatTu.UpdatedTime = DateTime.Now;
        vatTu.UpdatedBy = vatTuUpdated.UpdateBy;
        await dbContext.SaveChangesAsync();

        result.idMax = vatTu.Id;
        result.result = "OK";
        return result;
    }

    public async Task<long> GetMaxId()
    {
       long idMax = 1;
        try{
            idMax = await dbContext.VatTuNhaCungUngRelation.MaxAsync(x=>x.Id);
        }catch{
            idMax = 1;
        }
        return idMax;
    }
}