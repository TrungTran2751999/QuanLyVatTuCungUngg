using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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
    public List<NhaCungUng> ListNhaCungUng{get;set;}
    public decimal SoLuongBaoGia{get;set;}
    public string? GhiChu{get;set;}
    public string? MaPhieu{get;set;}
    public int Stt{get;set;}
    public string? YeuCauKiThuat{get;set;}
    public string? DonViTinh{get;set;}
    public string? CodeYear{get;set;}
    public VatTuBaoGiaChiTiet ToModel(Guid BaoGiaId, long CreatedBy, long UpdatedBy){
        VatTuBaoGiaChiTiet vatTuBaoGia = new(){
            BaoGiaId = BaoGiaId,
            Stt = Stt,
            DonViTinh = DonViTinh,
            CodeYear = CodeYear,
            TenVatTu = TenVatTu,
            VatTuId = VatTuId,
            MaVatTu = MaVatTu,
            SoLuongBaogia = SoLuongBaoGia,
            YeuCauKiThuat = YeuCauKiThuat,
            GhiChu = GhiChu,
            MaPhieu = MaPhieu,
            CreatedBy = CreatedBy,
            UpdatedBy = UpdatedBy,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        return vatTuBaoGia;
    }
    public class NhaCungUng{
        public string? TenNhaCungUng{get;set;}
        public long NhaCungUngId{get;set;}
    }
    public List<VatTuBaoGiaChiTietNhaCungUng> ToListVatTuBaoGiaChiTietNhaCungUng(Guid BaoGiaId, Guid VatTuBaoGiaChiTietId, long CreatedBy, long UpdatedBy){
        List<VatTuBaoGiaChiTietNhaCungUng> listNhaCungUng = new();
        foreach(var nhaCungUngParam in ListNhaCungUng){
            VatTuBaoGiaChiTietNhaCungUng nhaCungUng = new(){
                BaoGiaId = BaoGiaId,
                VatTuBaoGiaChiTietId = VatTuBaoGiaChiTietId,
                NhaCungUngId = nhaCungUngParam.NhaCungUngId,    
                TenNhaCungUng = nhaCungUngParam.TenNhaCungUng,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy
            };
            listNhaCungUng.Add(nhaCungUng);
        }
        return listNhaCungUng;
    }
}

