using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace app.Models;
[Table("BaoGia_ChiTiet_NhaCungUng")]
public class BaoGiaChitietNhaCungUng{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("BaoGiaChiTietId")]
    public Guid BaoGiaChiTietId{get;set;}
    public long NhaCungUngId{get;set;}
    [Column("VatTuId")]
    public long VatTuId{get;set;}
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
