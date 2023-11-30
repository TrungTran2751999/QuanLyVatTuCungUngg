using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class VatTuDTO{
    public string? MaVatTu{get;set;}
    public Decimal? SoLuongPheDuyet{get;set;}
    public string? GhiChu{get;set;}
    public PhieuDeNghiNhanVatTuChiTietDaDuyet ToModel(long CreatedAt, long UpdateAt, Guid phieuId){
        PhieuDeNghiNhanVatTuChiTietDaDuyet phieuChiTiet = new()
        {
            MaVatTu = MaVatTu,
            SoLuong = SoLuongPheDuyet,
            PhieuId = phieuId,
            CreatedAt = CreatedAt,
            UpdateAt = UpdateAt,
            CreatedTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            GhiChu = GhiChu
        };
        return phieuChiTiet;
    }
}