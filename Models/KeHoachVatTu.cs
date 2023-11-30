using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("KeHoach_VatTu")]
public class KeHoachVatTu{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("Guid")]
    public Guid? Guid{get;set;}
    [Column("MaKeHoach")]
    public long? MaKeHoach{get;set;}
    [Column("MaKeHoachChiTietNam")]
    public long? MaKeHoachChiTietNam{get;set;}
    [Column("GuidKeHoach")]
    public Guid? GuidKeHoach{get;set;}
    [Column("MaKeHoachCongViec")]
    public long? MaKeHoachCongViec{get;set;}
    [Column("GuildKeHoachCongViec")]
    public Guid? GuildKeHoachCongViec{get;set;}
    [Column("MaHangMuc")]
    public long? MaHangMuc{get;set;}
    [Column("MaDonVi")]
    public long? MaDonVi{get;set;}
    [Column("MaVatTu")]
    public long? MaVatTu{get;set;}
    [Column("GuidVatTu")]
    public Guid? GuidVatTu{get;set;}
    [Column("TenVatTu")]
    public string? TenVatTu{get;set;}
    [Column("MaVatTuThamChieu")]
    public string? MaVatTuThamChieu{get;set;}
    [Column("DonViTinh")]
    public string? DonViTinh{get;set;}
    [Column("NhomVatTu1Id")]
    public long? NhomVatTu1Id{get;set;}
    [Column("TenNhomVatTu1")]
    public string? TenNhomVatTu1{get;set;}
    [Column("NhomVatTu2Id")]
    public long? NhomVatTu2Id{get;set;}
    [Column("TenNhomVatTu2")]
    public string? TenNhomVatTu2{get;set;}
    [Column("NhomVatTu3Id")]
    public long? NhomVatTu3Id{get;set;}
    [Column("TenNhomVatTu3")]
    public string? TenNhomVatTu3{get;set;}
    [Column("Quy1_PheDuyet")]
    public Decimal? Quy1PheDuyet{get;set;}
    [Column("Quy2_PheDuyet")]
    public Decimal? Quy2PheDuyet{get;set;}
    [Column("Quy3_PheDuyet")]
    public Decimal? Quy3PheDuyet{get;set;}
    [Column("Quy4_PheDuyet")]
    public Decimal? Quy4PheDuyet{get;set;}
    [Column("DonGia_PheDuyet")]
    public Decimal? TongCongPheDuyet{get;set;}
    [Column("DonGia_PheDuyet")]
    public Decimal? DonGiaPheDuyet{get;set;}
    [Column("Quy1_DaNhan")]
    public Decimal? Quy1DaNhan{get;set;}
    [Column("Quy2_DaNhan")]
    public Decimal? Quy2DaNhan{get;set;}
    [Column("Quy3_DaNhan")]
    public Decimal? Quy3DaNhan{get;set;}
    [Column("Quy4_DaNhan")]
    public Decimal? Quy4DaNhan{get;set;}
    [Column("TongCong_DaNhan")]
    public Decimal? TongCongDaNhan{get;set;}
    [Column("ThanhTien_PheDuyet")]
    public Decimal? ThanhTienPheDuyet{get;set;}
    [Column("ThanhTien_DaNhan")]
    public Decimal? ThanhTienDaNhan{get;set;}
    [Column("DienGiai")]
    public string? DienGiai{get;set;}
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
    [Column("MaVatTuTuongDuong")]
    public string? MaVatTuTuongDuong{get;set;}
    [Column("NgoaiKeHoach")]
    public bool? NgoaiKeHoach{get;set;}
}
