using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("VatTu_BaoGiaChiTiet_NhaCungUng")]
public class VatTuBaoGiaChiTietNhaCungUng{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("VatTuBaoGiaChiTietId")]
    public Guid? VatTuBaoGiaChiTietId{get;set;}
    [Column("NhaCungUngId")]
    public long NhaCungUngId{get;set;}
    [Column("TenNhaCungUng")]
    public string? TenNhaCungUng{get;set;}
    [Column("CreatedAt")]
    public DateTime CreatedAt{get;set;}
    [Column("UpdatedAt")]
    public DateTime UpdatedAt{get;set;}
    [Column("UpdatedBy")]
    public long UpdatedBy{get;set;}
    [Column("CreatedBy")]
    public long CreatedBy{get;set;}

}
