using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class PhieuNVTDTO{
    public string? MaPhieu{get;set;}
    public string? TenBoPhan{get;set;}
    public string? DienGiai{get;set;}
    public string? NguoiYeuCau{get;set;}
    public PhieuNhanVatTuFast? PhieuNhanVatTuFast;
    public PhieuDeNghiNhanVatTuDaDuyet? PhieuDeNghiNhanVatTuDaDuyet;
}
