using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class NhaCungUngCreateDTO{
    [Required]
    public string? Name {get;set;}
    public string? MaSoThue{get;set;}
    public string? DiaChi{get;set;}
    public string? DienThoai{get;set;}
    public string? SoTaiKhoan{get;set;}
    public string? NganHang{get;set;}
    public string? ChiNhanhNganHang{get;set;}
    public string? DienThoaiDiDong{get;set;}
    [Required]
    public long? CreatedBy{get;set;}
    [Required]
    public long? UpdateBy{get;set;}

    public NhaCungUngVatTu ToModel(){
        NhaCungUngVatTu nhaCungUngVatTu = new NhaCungUngVatTu
        {
            TenNhaCungUng = Name,
            MaSoThue = MaSoThue,
            DiaChi = DiaChi,
            DienThoai = DienThoai,
            SoTaiKhoan = SoTaiKhoan,
            NganHang = NganHang,
            ChiNhanhNganHang = ChiNhanhNganHang,
            DienThoaiDiDong = DienThoaiDiDong,
            CreatedBy = CreatedBy,
            UpdateBy = UpdateBy,
            CreatedAt = DateTime.Now,
            UpdateAt = DateTime.Now,
            Guid = Guid.NewGuid(),
            IsDeleted = false
        };
        return nhaCungUngVatTu;
    } 
    
}
