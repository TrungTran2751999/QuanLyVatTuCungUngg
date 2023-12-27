using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class NhaCungUngUpdateDTO{
    [Required]
    public long? Id{get;set;}
    [Required]
    public string? TenVietTat{get;set;}
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
    public string? ChucVu{get;set;}
    [Required]
    public string? NguoiDaiDien{get;set;}
    [Required]
    public bool? GioiTinhNguoiDaiDien{get;set;}
    [Required]
    public long? UpdateBy{get;set;}
    public NhaCungUngVatTu ToModel(){
        NhaCungUngVatTu nhaCungUng = new()
        {
            UpdateAt = DateTime.Now,
            MaSoThue = MaSoThue,
            DiaChi = DiaChi,
            DienThoai = DienThoai,
            NganHang = NganHang,
            SoTaiKhoan = SoTaiKhoan,
            DienThoaiDiDong = DienThoaiDiDong,
            ChiNhanhNganHang = ChiNhanhNganHang,
            ChucVu = ChucVu,
            NguoiDaiDien = NguoiDaiDien,
            GioiTinhNguoiDaiDien = GioiTinhNguoiDaiDien,
            UpdateBy = UpdateBy
        };
        return nhaCungUng;
    }
    
}
