using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace app.Models;
[Table("BaoGia_ChiTiet")]
public class BaoGiaChiTiet{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("VatTuId")]
    public long VatTuId{get;set;}
    [ForeignKey("VatTuId")]
    public VatTu? VatTu{get;set;}
    [Column("SoLuongBaoGia")]
    public Decimal? SoLuongBaoGia{get;set;}
    [Column("MaPhieu")]
    public string? MaPhieu{get;set;}
    [Column("YeuCauKiThuat")]
    public string? YeuCauKiThuat{get;set;}
    [Column("codeYear")]
    public string? CodeYear{get;set;}
    [Column("BaoGiaId")]
    public Guid BaoGiaId{get;set;}
    [ForeignKey("BaoGiaId")]
    public BaoGia? BaoGia{get;set;}
    [Column("CreatedBy")]
    public long CreatedAt{get;set;}
    [Column("UpdatedBy")]
    public long UpdatedAt{get;set;}
    [Column("CreatedTime")]
    public DateTime CreatedTime{get;set;}
    [Column("UpdatedTime")]
    public DateTime UpdatedTime{get;set;}
    [Column("GhiChu")]
    public string? GhiChu{get;set;}
}
