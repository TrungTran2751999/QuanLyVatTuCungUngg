using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class VatTuInBaoGia{
    public long VatTuId{get;set;}
    public string? TenVatTu{set;get;}
    public string? DonViTinh{get;set;}
    public string? YeuCauKiThuat{get;set;}
    public string? XuatXu{get;set;}
    public Decimal? SoLuong{get;set;}
    public string? GhiChu{get;set;}

}
