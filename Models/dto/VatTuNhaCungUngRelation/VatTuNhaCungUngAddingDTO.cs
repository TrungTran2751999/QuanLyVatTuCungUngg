using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class VatTuNhaCungUngAddingDTO{
    public long? Id{get;set;}
    [Required]
    public string? NhaCungUng{get;set;}
    [Required]
    public long? NhaCungUngId{get;set;}
    [Required]
    public string? TenVatTu {get;set;}
    [Required]
    public long? MaVatTu {get;set;}
    [Required]
    public long? CreatedBy{get;set;}
    [Required]
    public long? UpdateBy{get;set;}
    public DateTime? CreatedTime{get;set;}
    public DateTime? UpdatedTime{get;set;}
    public VatTuNhaCungUngRelation ToModel(long id){
        VatTuNhaCungUngRelation vatTuModel = new()
        {
            Id = id+1,
            NhaCungUngId = NhaCungUngId,
            TenNhaCungUng = NhaCungUng,
            TenVatTu = TenVatTu,
            MaVatTu = MaVatTu,
            CreatedBy = CreatedBy,
            UpdatedBy = UpdateBy,
            CreatedTime = DateTime.Now,
            UpdatedTime = DateTime.Now,
            IsDeleted = false
        };
        return vatTuModel;
    }
    
}
