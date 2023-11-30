using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
using DocumentFormat.OpenXml.Presentation;
namespace app.DTOs;

public class FilterParamTongHop{
    public string? Search{get;set;}
    public List<FilterTongHop> ListFilters{get;set;}
}
public class FilterTongHop{
    public int Id{get;set;}
    public string? TenVatTu{get;set;}
    public string? GhiChu{get;set;}
}