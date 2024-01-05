using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("HopDongChiTietMuaHang_ChiTiet")]
public class HopDongMuaHangChiTiet{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("TenHang")]
    public string? TenHang{get;set;}
    [Column("DonViTinh")]
    public string? DonViTinh{get;set;}
    [Column("SoLuong")]
    public decimal? SoLuong{get;set;}
    [Column("DonGia")]
    public decimal? DonGia{get;set;}
    [Column("HopDongId")]
    public Guid HopDongId{get;set;}
    [ForeignKey("HopDongId")]
    public HopDongMuaHangChiTiet HopDong{get;set;}
    [Column("CreatedBy")]
    public long? CreatedBy {get;set;}
    [Column("CreatedAt")]
    public DateTime? CreatedAt {get;set;}
    [Column("UpdatedBy")]
    public long? UpdatedBy {get;set;}
    [Column("UpdatedAt")]
    public DateTime? UpdatedAt {get;set;}
}
