using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class BaoGiaDetail{
    public string? TenVatTu{get;set;}
    public string? GhiChu{get;set;}
    public DateTime? UpdatedTime{get;set;}
    public string? UserBaoGia{get;set;}
    public long? UserId{get;set;}
}
