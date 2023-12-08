using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace app.Models;
[Table("BaoGia_ChiTiet_VatTu")]
public class BaoGiaChitietVatTu{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("BaoGiaChiTietId")]
    public Guid BaoGiaChiTietId{get;set;}
    [Column("NhaCungUngId")]
    public long NhaCungUngId{get;set;}
    [Column("VatTuId")]
    public long VatTuId{get;set;}
    [Column("TenVatTu")]
    public string? TenVatTu{get;set;}
    [Column("SoLuongBaoGia")]
    public Decimal? SoLuongBaoGia{get;set;}
    [Column("MaPhieu")]
    public string? MaPhieu{get;set;}
    [Column("YeucauKiThuat")]
    public string? YeucauKiThuat{get;set;}
    [Column("DonViTinh")]
    public string? DonViTinh{get;set;}
    [Column("CodeYear")]
    public string? CodeYear{get;set;}
    [Column("GhiChu")]
    public string? GhiChu{get;set;}
    [Column("CreatedBy")]
    public long CreatedAt{get;set;}
    [Column("UpdatedBy")]
    public long UpdatedAt{get;set;}
    [Column("CreatedTime")]
    public DateTime? CreatedTime{get;set;}
    [Column("UpdatedTime")]
    public DateTime? UpdatedTime{get;set;}
}
