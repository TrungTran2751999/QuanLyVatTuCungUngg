using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.DTOs;

public class NhaCungUngListVatTu{
    public Guid BaoGiaChiTietId{get;set;}
    public Guid BaoGiaId{get;set;}
    public long IdNhaCungUng{get;set;}
    public string? TenNhaCungUng{set;get;}
    public string? GhiChu{get;set;}
    public List<VatTuInBaoGia> ListVatTu{get;set;}

    public BaoGiaChiTiet ToBaoGiaChiTiet(Guid BaoGiaId, long CreatedBy, long UpdateBy){
        var result = new BaoGiaChiTiet{
            NhaCungUngId = IdNhaCungUng,
            TenNhaCungUng = TenNhaCungUng,
            BaoGiaId = BaoGiaId,
            CreatedAt = CreatedBy,
            UpdatedAt = UpdateBy,
            CreatedTime = DateTime.Now,
            UpdatedTime = DateTime.Now,
            GhiChu = GhiChu
        };
        return result;
    }
    public List<BaoGiaChitietVatTu> ToListBaoGiaChiTietVatTu(Guid BaogiaChiTietId, long CreatedBy, long UpdatedBy){
        var listResult = ListVatTu.Select(x=>new BaoGiaChitietVatTu{
                            BaoGiaChiTietId = BaogiaChiTietId,
                            NhaCungUngId = IdNhaCungUng,
                            VatTuId = x.VatTuId,
                            TenVatTu = x.TenVatTu,
                            SoLuongBaoGia = x.SoLuong,
                            MaPhieu = x.MaPhieu,
                            DonViTinh = x.DonViTinh,
                            YeucauKiThuat = x.YeuCauKiThuat,
                            CodeYear = x.CodeYear,
                            GhiChu = x.GhiChu,
                            CreatedAt = CreatedBy,
                            UpdatedAt = UpdatedBy,
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now
                         }).ToList();
        return listResult;
    }
}
