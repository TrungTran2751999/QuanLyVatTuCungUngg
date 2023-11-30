using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
namespace app.DTOs;

public class TongHopDTO{
    public string? maPhieu{get;set;}
    public string? phieuDatetime{get;set;}
}
