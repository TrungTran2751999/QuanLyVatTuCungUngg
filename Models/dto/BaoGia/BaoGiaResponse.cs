using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class BaoGiaResponse{
    public Guid? Id{get;set;}
    public string? MaBaoGia{get;set;}
    public DateTime? CreatedTime{get;set;}
    public DateTime? UpdatedTime{get;set;}
    public string? UserBaoGia{get;set;}
    public long? UserId{get;set;}
}
