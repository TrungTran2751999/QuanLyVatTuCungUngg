using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
using Microsoft.AspNetCore.Mvc;
namespace app.DTOs;

public class BaoGiaCreateDTO{
    public List<Guid>? ListPhieuPheDuyet{get;set;}
    public long CreatedBy{get;set;}
    public long UpdateBy{get;set;}
    public List<NhaCungUngListVatTu>? ListNhaCungUngListVatTu{get;set;}
    public List<VatTuBaoGiaChiTietDTO>? ListVatTuBaoGiaChiTietDTO{get;set;}
    public BaoGia ToModel(){
        BaoGia baoGia = new()
        {
            Guid = Guid.NewGuid(),
            IsDeleted = false,
            CreatedTime = DateTime.Now,
            UpdatedTime = DateTime.Now,
            MaBaoGia = CreateMaBaoGia(DateTime.Now),
            CreatedAt = CreatedBy,
            UpdatedAt = UpdateBy
        };
        return baoGia;
    }
    private string CreateMaBaoGia(DateTime createdTime){
        var maBaoGia = "BG"+Guid.NewGuid().ToString().Split("-")[0]+createdTime.ToString("ddMMyyyyHHmmss");
        return maBaoGia;
    } 
    public List<BaoGiaChiTiet> ToListBaoGiaChiTiet(Guid BaoGiaId){
        var listResult = ListNhaCungUngListVatTu.Select(x=>new BaoGiaChiTiet{
                            NhaCungUngId = x.IdNhaCungUng,
                            TenNhaCungUng = x.TenNhaCungUng,
                            BaoGiaId = BaoGiaId,
                            CreatedAt = CreatedBy,
                            UpdatedAt = UpdateBy,
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now
                         }).ToList();
        return listResult;
    }
}
public class VatTuBaoGiaChiTietDTO{
    
    public string? TenVatTu{get;set;}
    public long VatTuId{get;set;}
    public string? MaVatTu{get;set;}
    public string? TenNhaCungUng{get;set;}
    public long NhaCungUngId{get;set;}
    public decimal SoLuongBaoGia{get;set;}
    public string? GhiChu{get;set;}
    public string? MaPhieu{get;set;}
    public VatTuBaoGiaChiTiet ToModel(Guid BaoGiaChiTietVatTuId, long CreatedBy, long UpdatedBy){
        VatTuBaoGiaChiTiet vatTuBaoGia = new(){
            BaoGiaChiTietVatTuId = BaoGiaChiTietVatTuId,
            TenVatTu = TenVatTu,
            VatTuId = VatTuId,
            MaVatTu = MaVatTu,
            TenNhaCungUng = TenNhaCungUng,
            NhaCungUngId = NhaCungUngId,
            SoLuongBaogia = SoLuongBaoGia,
            GhiChu = GhiChu,
            MaPhieu = MaPhieu,
            CreatedBy = CreatedBy,
            UpdatedBy = UpdatedBy,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        return vatTuBaoGia;
    }
}

