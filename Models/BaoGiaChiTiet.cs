using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace app.Models;
[Table("BaoGia_ChiTiet")]
public class BaoGiaChiTiet{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("NhaCungUngId")]
    public long NhaCungUngId{get;set;}
    [Column("TenNhaCungUng")]
    public string? TenNhaCungUng{get;set;}
    [ForeignKey("NhaCungUngId")]
    public NhaCungUngVatTu? NhaCungUng{get;set;}
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
