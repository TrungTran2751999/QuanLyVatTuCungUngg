using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class VatTuDTO{
    public string? MaVatTu{get;set;}
    public Decimal? SoLuongPheDuyet{get;set;}
    public Decimal? SoLuongDeNghi{get;set;}
    public int Stt{get;set;}
    public string? MaPhieu{get;set;}
    public string? GhiChu{get;set;}
    public PhieuDeNghiNhanVatTuChiTietDaDuyet ToModel(long CreatedAt, long UpdateAt, Guid phieuId){
        PhieuDeNghiNhanVatTuChiTietDaDuyet phieuChiTiet = new()
        {
            MaVatTu = MaVatTu,
            SoLuong = SoLuongPheDuyet,
            SoLuongDeNghi = SoLuongDeNghi,
            PhieuId = phieuId,
            CreatedAt = CreatedAt,
            UpdateAt = UpdateAt,
            CreatedTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            GhiChu = GhiChu,
            Stt = Stt,
            MaPhieu = MaPhieu
        };
        return phieuChiTiet;
    }
}