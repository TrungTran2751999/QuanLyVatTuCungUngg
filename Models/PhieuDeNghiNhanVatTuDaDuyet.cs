using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using app.DTOs;

namespace app.Models;
[Table("PhieuDeNghiNhanVatTu_DaDuyet")]
public class PhieuDeNghiNhanVatTuDaDuyet{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("MaPhieu")]
    public string? MaPhieu{get;set;}
    [Column("TenNguoiDeNghi")]
    public string? TenNguoiDeNghi{get;set;}
    [Column("CodeYear")]
    public string? CodeYear{get;set;}
    [Column("DienGiai")]
    public string? DienGiai{get;set;}
    [Column("TenBoPhan")]
    public string? TenBoPhan{get;set;}
    [Column("CreatedTime")]
    public DateTime CreatedTime{get;set;}
    [Column("CreatedAt")]
    public long? CreatedAt{get;set;}
    [Column("UpdateTime")]
    public DateTime UpdateTime{get;set;}
    [Column("UpdateAt")]
    public long? UpdateAt{get;set;}
    [Column("IsPheDuyet")]
    public bool? IsDeleted{get;set;}
    [Column("IsTongHop")]
    public bool? IsTongHop{get;set;}
    [Column("DateDeNghi")]
    public DateTime DateDeNghi{get;set;}
    [Column("BaoGiaId")]
    public Guid? BaoGiaId{get;set;}
    [ForeignKey("BaoGiaId")]
    public BaoGia? BaoGia;
    public PhieuNVTDTO ToPhieuNVTDTO(){
        PhieuNVTDTO phieuNVTDTO = new()
        {
            MaPhieu = MaPhieu,
            TenBoPhan = TenBoPhan,
            NguoiYeuCau = TenNguoiDeNghi
        };
        return phieuNVTDTO;
    }
    
}
