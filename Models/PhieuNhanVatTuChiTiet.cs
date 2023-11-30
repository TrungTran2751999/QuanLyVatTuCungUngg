using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("PhieuNhanVatTuChiTiet")]
public class PhieuNhanVatTuChiTiet{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("Guid")]
    public Guid? Guid{set;get;}
    [Column("MaPhieuNhanVatTu")]
    public long? MaPhieuNhanVatTu{set;get;}
    [Column("MaKeHoach")]
    public long? MaKeHoach{set;get;}
    [Column("MaDonVi")]
    public long? MaDonVi{set;get;}
    [Column("Quy")]
    public int? Quy{set;get;}
    [Column("MaKeHoachChiTiet")]
    public long? MaKeHoachChiTiet{set;get;}
    [Column("MaHangMuc")]
    public long? MaHangMuc{set;get;}
    [Column("MaKeHoachCongViec")]
    public long? MaKeHoachCongViec{set;get;}
    [Column("MaVatTu")]
    public long? MaVatTu{set;get;}
    [Column("TenVatTu")]
    public string? TenVatTu{set;get;}
    [Column("MaVatTuThamChieu")]
    public string? MaVatTuThamChieu{set;get;}
    [Column("DonViTinh")]
    public string? DonViTinh{set;get;}
    [Column("DonGia")]
    public Decimal? DonGia{set;get;}
    [Column("NhomVatTu1Id")]
    public long? NhomVatTu1Id{set;get;}
    [Column("TenNhomVatTu1")]
    public string? TenNhomVatTu1{set;get;}
    [Column("NhomVatTu2Id")]
    public long? NhomVatTu2Id{set;get;}
    [Column("TenNhomVatTu2")]
    public string? TenNhomVatTu2{set;get;}
    [Column("NhomVatTu3Id")]
    public long? NhomVatTu3Id{set;get;}
    [Column("TenNhomVatTu3")]
    public string? TenNhomVatTu3{set;get;}
    [Column("SoLuong_KeHoach")]
    public Decimal? SoLuongKeHoach{set;get;}
    [Column("SoLuong_DaNhan")]
    public Decimal? SoLuongDaNhan{set;get;}
    [Column("SoLuong_ConLai")]
    public Decimal? SoLuongConLai{set;get;}
    [Column("SoLuong_XinNhan")]
    public Decimal? SoLuongXinNhan{set;get;}
    [Column("SoLuong_ThucNhan")]
    public Decimal? SoLuongThucNhan{set;get;}
    [Column("DienGiai")]
    public string? DienGiai{set;get;}
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
