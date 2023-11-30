using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("NhaCungUng_VatTu")]
public class VatTuNhaCungUngRelation{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("nhaCungUngId")]
    public long? NhaCungUngId{get;set;}
    [ForeignKey("NhaCungUngId")]
    public NhaCungUngVatTu? NhaCungUng{get;set;}
    [Column("nhaCungUng")]
    public string? TenNhaCungUng{get;set;}
    [Column("tenVatTu")]
    public string? TenVatTu{get;set;}
    [Column("vatTuId")]
    public long? MaVatTu{get;set;}
    [ForeignKey("MaVatTu")]
    public VatTu? VatTu{get;set;}
    [Column("CreatedBy")]
    public long? CreatedBy{get;set;}
    [Column("CreatedTime")]
    public DateTime? CreatedTime{get;set;}
    [Column("UpdatedBy")]
    public long? UpdatedBy{get;set;}
    [Column("UpdatedTime")]
    public DateTime? UpdatedTime{get;set;}
    [Column("isDeleted")]
    public bool? IsDeleted{get;set;}
}
