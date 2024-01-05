using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("HopDongMuaHang")]
public class HopDongMuaHang{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("SoHopDong")]
    public string? SoHopDong{get;set;}
    [Column("NhaCungUngId")]
    public long NhaCungUngId{get;set;}
    [ForeignKey("NhaCungUngId")]
    public NhaCungUngVatTu NhaCungUng{get;set;}
    [Column("TenNhaCungUng")]
    public string? TenNhaCungUng{get;set;}
    [Column("DiaChiNhanHang")]
    public string? DiaChiNhanHang{get;set;}
    [Column("DieuKhoan")]
    public string? DieuKhoan{get;set;}
    [Column("CreatedBy")]
    public long? CreatedBy {get;set;}
    [Column("CreatedAt")]
    public DateTime? CreatedAt {get;set;}
    [Column("UpdatedBy")]
    public long? UpdatedBy {get;set;}
    [Column("UpdatedAt")]
    public DateTime? UpdatedAt {get;set;}
}
