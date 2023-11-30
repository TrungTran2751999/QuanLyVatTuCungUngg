using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class NhaCungUngCreateDTO{
    [Required]
    public string? Name {get;set;}
    [Required]
    public long? CreatedBy{get;set;}
    [Required]
    public long? UpdateBy{get;set;}

    public NhaCungUngVatTu ToModel(){
        NhaCungUngVatTu nhaCungUngVatTu = new NhaCungUngVatTu();
        nhaCungUngVatTu.TenNhaCungUng = Name;
        nhaCungUngVatTu.CreatedBy = CreatedBy;
        nhaCungUngVatTu.UpdateBy = UpdateBy;
        nhaCungUngVatTu.CreatedAt = DateTime.Now;
        nhaCungUngVatTu.UpdateAt = DateTime.Now;
        nhaCungUngVatTu.Guid = Guid.NewGuid();
        nhaCungUngVatTu.IsDeleted = false;
        return nhaCungUngVatTu;
    } 
    
}
