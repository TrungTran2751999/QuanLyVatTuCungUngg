using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using app.Utils;

namespace app.Models;
[Table("DM_VatTu")]
public class VatTu{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("Guid")]
    public Guid? Guid{get;set;}
    [Column("MaVatTu")]
    public string? MaVatTu{get;set;}
    [Column("MaVatTuTuongDuong")]
    public string? MaVatTuTuongDuong{get;set;}
    [Column("TenVatTu")]
    public string? TenVatTu{get;set;}
    [Column("DonViTinh")]
    public string? DonViTinh{get;set;}
    [Column("MaNhomVatTu1")]
    public string? MaNhomVatTu1{get;set;}
    [Column("MaNhomVatTu2")]
    public string? MaNhomVatTu2{get;set;}
    [Column("MaNhomVatTu3")]
    public string? MaNhomVatTu3{get;set;}
    [Column("NhomVatTu1Id")]
    public long? NhomVatTu1Id{get;set;}
    [Column("NhomVatTu2Id")]
    public long? NhomVatTu2Id{get;set;}
    [Column("NhomVatTu3Id")]
    public long? NhomVatTu3Id{get;set;}
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

    public string RemoveDauTiengViet(IUtil util, string hint){
        return util.RemoveDauTiengViet(hint);
    }
}
