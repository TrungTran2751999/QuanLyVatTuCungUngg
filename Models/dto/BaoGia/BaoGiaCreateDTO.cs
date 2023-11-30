using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class BaoGiaCreateDTO{
    public List<Guid>? ListPhieuPheDuyet{get;set;}
    public long CreatedBy{get;set;}
    public long UpdateBy{get;set;}
    public List<VatTuListNhaCungUng>? ListVatTuNhaCungUng{get;set;}
    public List<NhaCungUngListVatTu>? ListNhaCungUngListVatTu{get;set;}
    public BaoGia ToModel(){
        BaoGia baoGia = new()
        {
            Guid = Guid.NewGuid(),
            IsDeleted = false,
            CreatedTime = DateTime.Now,
            UpdatedTime = DateTime.Now,
            MaBaoGia = CreateMaBaoGia(DateTime.Now),
            CreatedAt = CreatedBy,
            UpdatedAt = UpdateBy
        };
        return baoGia;
    } 
    private string CreateMaBaoGia(DateTime createdTime){
        var maBaoGia = "BG"+Guid.NewGuid().ToString().Split("-")[0]+createdTime.ToString("ddMMyyyyHHmmss");
        return maBaoGia;
    }
    
}
