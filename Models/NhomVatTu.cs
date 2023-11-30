using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("DM_NhomVatTu")]
public class NhomVatTu{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("Guid")]
    public Guid? Guid{get;set;}
    [Column("LoaiNhom")]
    public int? LoaiNhom{get;set;}
    [Column("TenNhomVatTu")]
    public string? TenNhomVatTu{get;set;}
    [Column("TrangThai")]
    public int? TrangThai{get;set;}
    [Column("IsDeleted")]
    public bool? IsDeleted{get;set;}
    [Column("CreatedBy")]
    public long? CreatedBy{get;set;}
    [Column("CreatedTime")]
    public DateTime? CreatedTime{get;set;}
    [Column("UpdatedBy")]
    public long? UpdatedBy{get;set;}
    [Column("UpdatedTime")]
    public DateTime? UpdatedTime{get;set;}
}
