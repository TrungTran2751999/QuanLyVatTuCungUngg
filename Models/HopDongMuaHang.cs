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
    [Column("GioiTinhNhaCungUng")]
    public bool GioiTinhNhaCungUng{get;set;}
    [Column("DiaChiNhanHang")]
    public string? DiaChiNhanHang{get;set;}
    [Column("DieuKhoan")]
    public byte[]? DieuKhoan{get;set;}
    [Column("DaiDienNhaCungUng")]
    public string? DaiDienNhaCungUng{get;set;}
    [Column("ChucVuNhaCungUng")]
    public string? ChucVuNhaCungUng{get;set;}
    [Column("DiaChiNhaCungUng")]
    public string? DiaChiNhaCungUng{get;set;}
    [Column("DienThoaiNhaCungUng")]
    public string? DienThoaiNhaCungUng{get;set;}
    [Column("TaiKhoanNhaCungUng")]
    public string? TaiKhoanNhaCungUng{get;set;}
    [Column("MaSoThue")]
    public string? MaSoThue{get;set;}
    [Column("NgayKiKet")]
    public DateTime NgayKiKet{get;set;}
    [Column("CreatedBy")]
    public long? CreatedBy {get;set;}
    [Column("CreatedAt")]
    public DateTime? CreatedAt {get;set;}
    [Column("UpdatedBy")]
    public long? UpdatedBy {get;set;}
    [Column("UpdatedAt")]
    public DateTime? UpdatedAt {get;set;}
}
