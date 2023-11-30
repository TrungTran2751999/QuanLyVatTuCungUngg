using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class NhaCungUngListVatTu{
    public long IdNhaCungUng{get;set;}
    public string? TenNhaCungUng{set;get;}
    public List<VatTuInBaoGia> ListVatTu{get;set;}
}
