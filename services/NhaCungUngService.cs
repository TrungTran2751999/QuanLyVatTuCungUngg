using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
// using System.Data.Entity;
namespace app.Services;
public class NhaCungUngService : INhaCungUngService
{
    private readonly ApplicationDbContext dbContext;

    public NhaCungUngService(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }

    public async Task<List<NhaCungUngVatTu>?> GetAll(bool isDeleted, int index)
    {
        var nhaCungUng = await dbContext.NhaCungUng
                                      .OrderByDescending(x=>x.UpdateAt)
                                      .Where(x=>x.IsDeleted==isDeleted)
                                      .Skip(index).Take(10).ToListAsync();
        
        return nhaCungUng;
    }

    public async Task<NhaCungUngVatTu?> GetById(long id, bool isDeleted)
    {
        var nhaCungUng = await dbContext.NhaCungUng.FirstOrDefaultAsync(param=>param.Id==id && param.IsDeleted==isDeleted);
        return nhaCungUng;
    }

    public async Task<string?> Save(NhaCungUngCreateDTO nhaCungUngCreate)
    {
        var checkNhaCungUng = await dbContext.NhaCungUng.FirstOrDefaultAsync(x=>x.TenNhaCungUng.ToLower()==nhaCungUngCreate.Name.ToLower());
        if(checkNhaCungUng?.TenNhaCungUng!=null) return "TenNhaCungUng:Nhà cung ứng đã tồn tại";
                
        long maxId = 0;
        try{
            maxId = await dbContext.NhaCungUng.MaxAsync(x=>x.Id); 
        }catch(Exception e){
            maxId = 0;
        }           
        
        NhaCungUngVatTu nhaCungUng = nhaCungUngCreate.ToModel();
        nhaCungUng.Id = maxId+1;
        await dbContext.NhaCungUng.AddAsync(nhaCungUng);
        dbContext.SaveChanges();
        return "OK";
        
    }

    public async Task<string?> Update(NhaCungUngUpdateDTO nhaCungUngUpdate)
    {
        var nhaCungUng = await dbContext.NhaCungUng.FirstOrDefaultAsync(p=>p.Id==nhaCungUngUpdate.Id);
        if(nhaCungUng?.TenNhaCungUng==null) return "TenNhaCungUng:Nhà cung ứng không tồn tại";

        var checkNhaCungUng = await dbContext.NhaCungUng.FirstOrDefaultAsync(x=>x.TenNhaCungUng.ToLower()==nhaCungUngUpdate.Name.ToLower() && x.Id!=nhaCungUngUpdate.Id);
        if(checkNhaCungUng?.TenNhaCungUng!=null) return "TenNhaCungUng:Nhà cung ứng đã tồn tại";

        nhaCungUng.TenNhaCungUng = nhaCungUngUpdate.Name;
        nhaCungUng.TenVietTat = nhaCungUngUpdate.TenVietTat;
        nhaCungUng.MaSoThue = nhaCungUngUpdate.MaSoThue;
        nhaCungUng.DiaChi = nhaCungUngUpdate.DiaChi;
        nhaCungUng.DienThoai = nhaCungUngUpdate.DienThoai;
        nhaCungUng.DienThoaiDiDong = nhaCungUngUpdate.DienThoaiDiDong;
        nhaCungUng.SoTaiKhoan = nhaCungUngUpdate.SoTaiKhoan;
        nhaCungUng.NganHang = nhaCungUngUpdate.NganHang;
        nhaCungUng.ChiNhanhNganHang = nhaCungUngUpdate.ChiNhanhNganHang;
        nhaCungUng.ChucVu = nhaCungUngUpdate.ChucVu;
        nhaCungUng.NguoiDaiDien = nhaCungUngUpdate.NguoiDaiDien;
        nhaCungUng.GioiTinhNguoiDaiDien = nhaCungUngUpdate.GioiTinhNguoiDaiDien;
        nhaCungUng.UpdateAt = DateTime.Now;
        nhaCungUng.UpdateBy = nhaCungUngUpdate.UpdateBy;
        dbContext.SaveChanges();
        return "OK";
    }
    public async Task<string?> SoftDelete(long id, long userId)
    {
        var nhaCungUng = await dbContext.NhaCungUng.FirstOrDefaultAsync(x=>x.Id==id);
        if(nhaCungUng?.TenNhaCungUng==null) return "TenNhaCungUng:Nhà cung ứng không tồn tại";

        nhaCungUng.IsDeleted = true;
        nhaCungUng.UpdateAt = DateTime.Now;
        nhaCungUng.UpdateBy = userId;
        dbContext.SaveChanges();
        
        return "OK";
    }

    public async Task<string?> Restore(long id, long userId)
    {
        var nhaCungUng = await dbContext.NhaCungUng.FirstOrDefaultAsync(x=>x.Id==id);
        if(nhaCungUng?.TenNhaCungUng==null) return "TenNhaCungUng:Nhà cung ứng không tồn tại";

        nhaCungUng.IsDeleted = false;
        nhaCungUng.UpdateAt = DateTime.Now;
        nhaCungUng.UpdateBy = userId;
        dbContext.SaveChanges();
        
        return "OK";
    }
}