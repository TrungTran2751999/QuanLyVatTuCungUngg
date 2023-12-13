using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace app.Models;
[Table("VatTu_BaoGia_ChiTiet")]
public class VatTuBaoGiaChiTiet{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("BaoGiaId")]
    public Guid BaoGiaId{get;set;}
    [Column("TenVatTu")]
    public string? TenVatTu{get;set;}
    [Column("VatTuId")]
    public long VatTuId{get;set;}
    [Column("Stt")]
    public int Stt{get;set;}
    [Column("YeuCauKiThuat")]
    public string? YeuCauKiThuat{get;set;}
    [Column("MaVatTu")]
    public string? MaVatTu{get;set;}
    [Column("SoLuongBaogia")]
    public decimal SoLuongBaogia{get;set;}
    [Column("CodeYear")]
    public string? CodeYear{get;set;}
    [Column("GhiChu")]
    public string? GhiChu{get;set;}
    [Column("MaPhieu")]
    public string? MaPhieu{get;set;}
    [Column("DonViTinh")]
    public string? DonViTinh{get;set;}
    [Column("CreatedAt")]
    public DateTime CreatedAt{get;set;}
    [Column("UpdatedAt")]
    public DateTime UpdatedAt{get;set;}
    [Column("CreatedBy")]
    public long CreatedBy{get;set;}
    [Column("UpdatedBy")]
    public long UpdatedBy{get;set;}
}
