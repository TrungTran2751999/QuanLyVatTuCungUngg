using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
using DocumentFormat.OpenXml.Presentation;
namespace app.DTOs;

public class FilterParam{
    public string? Search{get;set;}
    public DateTime? TimeFrom{get;set;}
    public DateTime? TimeTo{get;set;}
    public string? Status{get;set;}
    public string? Select{get;set;}
    public List<Filter>? ListFilters{get;set;}
}
public class Filter{
    public int? Id{get;set;}
    public DateTime DateDeNghi{get;set;}
    public string? MaPhieu{get;set;}
    public string? TenBoPhan{get;set;}
    public string? DienGiai{get;set;}
    public string? TenNguoiDeNghi{get;set;}
    public string? Status{get;set;}
    public bool Select{get;set;}
}
