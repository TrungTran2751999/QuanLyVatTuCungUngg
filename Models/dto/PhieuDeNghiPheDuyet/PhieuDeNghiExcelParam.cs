using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class PhieuDeNghiExcelParam{
    public string? TenVatTu{get;set;}
    public string? MaVatTu{get;set;}
    public string? DonViTinh{get;set;}
    public Decimal? SoLuongPheDuyet{get;set;}
    public Decimal? DonGia{get;set;}
    public Decimal? ThanhTien{get;set;}
    public string? GhiChu{get;set;}
}