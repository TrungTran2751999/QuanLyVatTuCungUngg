using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("NhaCungUngVatTu")]
public class NhaCungUngVatTu{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("Guid")]
    public Guid? Guid{set;get;}
    [Column("TenNhaCungUng")]
    public string? TenNhaCungUng{set;get;}
    [Column("MaSoThue")]
    public string? MaSoThue{set;get;}
    [Column("DiaChi")]
    public string? DiaChi{set;get;}
    [Column("DienThoai")]
    public string? DienThoai{set;get;}
    [Column("DienThoaiDiDong")]
    public string? DienThoaiDiDong{set;get;}
    [Column("SoTaiKhoan")]
    public string? SoTaiKhoan{set;get;}
    [Column("NganHang")]
    public string? NganHang{set;get;}
    [Column("ChiNhanhNganHang")]
    public string? ChiNhanhNganHang{set;get;}
    [Column("CreatedAt")]
    public DateTime? CreatedAt{set;get;}
    [Column("CreatedBy")]
    public long? CreatedBy{set;get;}
    [Column("UpdateAt")]
    public DateTime? UpdateAt{set;get;}
    [Column("UpdateBy")]
    public long? UpdateBy{set;get;}
    [Column("IsDeleted")]
    public bool? IsDeleted{set;get;}
    
}
