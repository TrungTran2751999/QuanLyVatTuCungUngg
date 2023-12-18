using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.DTOs;

public class VatTuInBaoGia{
    public long IdNhaCungUng{get;set;}
    public long VatTuId{get;set;}
    public string? TenVatTu{set;get;}
    public string? MaPhieu{set;get;}
    public string? DonViTinh{get;set;}
    public string? YeuCauKiThuat{get;set;}
    public string? CodeYear{get;set;}
    public string? XuatXu{get;set;}
    public Decimal? SoLuong{get;set;}
    public string? GhiChu{get;set;}
    public string? ChuThich{get;set;}
    public Guid BaoGiaChiTietId{get;set;}

    public BaoGiaChitietVatTu ToBaoGiaChitietVatTu(Guid BaoGiaChiTietId, long CreatedBy, long UpdatedBy){
        var result = new BaoGiaChitietVatTu{
            BaoGiaChiTietId = BaoGiaChiTietId,
            NhaCungUngId = IdNhaCungUng,
            VatTuId = VatTuId,
            TenVatTu = TenVatTu,
            SoLuongBaoGia = SoLuong,
            MaPhieu = MaPhieu,
            DonViTinh = DonViTinh,
            YeucauKiThuat = YeuCauKiThuat,
            CodeYear = CodeYear,
            GhiChu = GhiChu,
            CreatedAt = CreatedBy,
            UpdatedAt = UpdatedBy,
            CreatedTime = DateTime.Now,
            UpdatedTime = DateTime.Now
        };
        return result;
    }

}
