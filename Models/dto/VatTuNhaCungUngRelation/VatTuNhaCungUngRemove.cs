using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class VatTuNhaCungUngRemove{
    [Required]
    public long? NhaCungUngId{get;set;}
    [Required]
    public long? MaVatTu {get;set;}
    
}
