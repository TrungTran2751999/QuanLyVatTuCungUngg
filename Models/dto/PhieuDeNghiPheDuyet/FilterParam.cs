using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
using DocumentFormat.OpenXml.Presentation;
namespace app.DTOs;

public class FilterParam{
    public string? Search{get;set;}
    public string? Time{get;set;}
    public string? Status{get;set;}
    public List<Filter> ListFilters{get;set;}
}
public class Filter{
    public int? Id{get;set;}
    public string? Time{get;set;}
    public string? MaPhieu{get;set;}
    public string? TenBoPhan{get;set;}
    public string? DienGiai{get;set;}
    public string? NguoiYeuCau{get;set;}
    public string? Status{get;set;}
}
