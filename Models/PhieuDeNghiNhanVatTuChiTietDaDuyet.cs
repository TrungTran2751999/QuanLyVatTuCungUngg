using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("PhieuDeNghiNhanVatTu_ChiTiet_DaDuyet")]
public class PhieuDeNghiNhanVatTuChiTietDaDuyet{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("PhieuDeNghiNhanVatTuId")]
    public Guid PhieuId{get;set;}
    [ForeignKey("PhieuId")]
    public PhieuDeNghiNhanVatTuDaDuyet? PhieuDeNghi{get;set;}
    [Column("MaVatTu")]
    public string?  MaVatTu{get;set;}
    [Column("SoLuong")]
    public Decimal? SoLuong{get;set;}
    [Column("GhiChu")]
    public string? GhiChu{get;set;}
    [Column("CreatedTime")]
    public DateTime CreatedTime{get;set;}
    [Column("CreatedAt")]
    public long CreatedAt{get;set;}
    [Column("UpdateTime")]
    public DateTime UpdateTime{get;set;}
    [Column("UpdateAt")]
    public long UpdateAt{get;set;}
}
