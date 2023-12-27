using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.DTOs;

public class XuatHopDongDTO{
    public List<ListHang> ListHang{get;set;}
    public string? TenFile{get;set;}
}
public class ListHang{
    public string? TenHang{get;set;}
    public string? DVT{get;set;}
    public decimal? SoLuong{get;set;}
    public decimal? DonGia{get;set;}
    public decimal? ThanhTien{get;set;}
    public string? TenNhaCungUng{get;set;}
}

