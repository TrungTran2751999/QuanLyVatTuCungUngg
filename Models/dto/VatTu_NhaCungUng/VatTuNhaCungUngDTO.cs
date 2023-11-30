using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
namespace app.DTOs;

public class VatTuNhaCungUngDTO{
    public long? Id{get;set;}
    public string? TenNhaCungUng{get;set;}
    public long? NhaCungUngId{get;set;}
}
