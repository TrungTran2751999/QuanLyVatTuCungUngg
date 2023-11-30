using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class VatTuNhaCungUngResultTongHop{
    public Guid Id{get;set;}
    public long IdVatTu{get;set;}
    public Guid IdPhieu{get;set;}
    public string? MaPhieu{get;set;}
    public string? CodeYear{get;set;}
    public string? TenNguoiDeNghi{get;set;}
    public string? TenBoPhan{get;set;}
    public string? TenVatTu{get;set;}
    public string? MaVatTu{get;set;}
    public Decimal? Soluong{get;set;}
    public string? DonViTinh{get;set;}
    public string? GhiChu{get;set;}
    public List<long?> ListNhaCungUngId{get;set;}
    
}
