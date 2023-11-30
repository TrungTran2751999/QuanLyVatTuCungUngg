using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class NhaCungUngUpdateDTO{
    [Required]
    public long? Id{get;set;}
    [Required]
    public string? Name {get;set;}
    [Required]
    public long? UpdateBy{get;set;}
    public NhaCungUngVatTu ToModel(){
        NhaCungUngVatTu nhaCungUng = new NhaCungUngVatTu();
        nhaCungUng.UpdateAt = DateTime.Now;
        nhaCungUng.UpdateBy = UpdateBy;
        return nhaCungUng;
    }
    
}
