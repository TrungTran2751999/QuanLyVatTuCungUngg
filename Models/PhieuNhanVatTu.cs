using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("PhieuNhanVatTu")]
public class PhieuNhanVatTu{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("Guid")]
    public Guid? Guid{set;get;}
    [Column("MaKeHoach")]
    public long? MaKeHoach{set;get;}
    [Column("MaHangMuc")]
    public long? MaHangMuc{set;get;}
    [Column("MaDonVi")]
    public long? MaDonVi{set;get;}
    [Column("Nam")]
    public int? Nam{set;get;}
    [Column("Quy")]
    public int? Quy{set;get;}
    [Column("Thang")]
    public int? Thang{set;get;}
    [Column("LyDoNhan")]
    public string? LyDoNhan{set;get;}
    [Column("MaNguoiNhan")]
    public long? MaNguoiNhan{set;get;}
    [Column("TenNguoiNhan")]
    public string? TenNguoiNhan{set;get;}
    [Column("MaTruongBoPhan")]
    public long? MaTruongBoPhan{set;get;}
    [Column("TenTruongBoPhan")]
    public string? TenTruongBoPhan{set;get;}
    [Column("MaKeToanVanTu")]
    public long? MaKeToanVanTu{set;get;}
    [Column("TenKeToanVatTu")]
    public string? TenKeToanVatTu{set;get;}
    [Column("MaKeHoachKyThuat")]
    public long? MaKeHoachKyThuat{set;get;}
    [Column("TenKeHoachKyThuat")]
    public string? TenKeHoachKyThuat{set;get;}
    [Column("MaKeToanTruong")]
    public long? MaKeToanTruong{set;get;}
    [Column("TenKeToanTruong")]
    public string? TenKeToanTruong{set;get;}
    [Column("MaLanhDaoPheDuyet")]
    public long? MaLanhDaoPheDuyet{set;get;}
    [Column("TenLanhDaoPheDuyet")]
    public string? TenLanhDaoPheDuyet{set;get;}
    [Column("TrangThai")]
    public int? TrangThai{set;get;}
    [Column("IsDeleted")]
    public bool? IsDeleted{set;get;}
    [Column("CreatedBy")]
    public long? CreatedBy{set;get;}
    [Column("CreatedTime")]
    public DateTime? CreatedTime{set;get;}
    [Column("UpdatedBy")]
    public long? UpdatedBy{set;get;}
    [Column("UpdatedTime")]
    public DateTime? UpdatedTime{set;get;}

}
