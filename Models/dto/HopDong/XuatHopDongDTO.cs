using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text.Json;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.DTOs;

public class CreateHopDongDTO{
    [Required]
    public string? TenFile{get;set;}
    [Required]
    public long NhaCungUngId{get;set;}
    [Required]
    public string? DaiDienBenA{get;set;}
    [Required]
    public string? DiaChiBenA{get;set;}
    [Required]
    public string? DienThoaiBenA{get;set;}
    [Required]
    public string? TaiKhoanBenA{get;set;}
    [Required]
    public string? MaSoThueBenA{get;set;}
    [Required]
    public string? SoHopDong{get;set;}
    [Required]
    public DateTime NgayKiKet{get;set;}
    [Required]
    public string? NhaCungUng{get;set;}
    [Required]
    public string? DaiDienNhaCungUng{get;set;}
    [Required]
    public string? ChucVuNhaCungUng{get;set;}
    [Required]
    public string? DiaChiNhaCungUng{get;set;}
    [Required]
    public string? DienThoaiNhaCungUng{get;set;}
    [Required]
    public string? TaiKhoanNhaCungUng{get;set;}
    [Required]
    public string? MaSoThueNhaCungUng{get;set;}
    [Required]
    public List<Hang>? ListHang{get;set;}
    [Required]
    public List<DieuKhoan>? ListDieuKhoan{get;set;}
    [Required]
    public string? DiaChiNhanHang{get;set;}
    [Required]
    public long CreatedBy{get;set;}
    [Required]
    public long UpdatedBy{get;set;}
    public HopDongMuaHang ToModel(){
        HopDongMuaHang hopDongMuaHang = new(){
            SoHopDong = SoHopDong,
            NhaCungUngId = NhaCungUngId,
            TenNhaCungUng = NhaCungUng,
            DiaChiNhanHang = DiaChiNhanHang,
            DieuKhoan = JsonSerializer.Serialize(ListDieuKhoan),
            CreatedBy = CreatedBy,
            UpdatedBy = UpdatedBy,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        return hopDongMuaHang;
    }
    public List<HopDongMuaHangChiTiet> ToListModelHang(Guid hopDongId){
        var result = ListHang.Select(
                        x=>new HopDongMuaHangChiTiet{
                            TenHang = x.TenHang,
                            DonViTinh = x.DonVi,
                            SoLuong = x.SoLuong,
                            DonGia = x.DonGia,
                            CreatedBy = CreatedBy,
                            UpdatedBy = UpdatedBy,
                            HopDongId = hopDongId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        }
                     ).ToList();
        return result;
    }
}
public class Hang{
    public string? TenHang{get;set;}
    public string? DonVi{get;set;}
    public decimal? SoLuong{get;set;}
    public decimal? DonGia{get;set;}
}
public class DieuKhoan{
    public string? TenDieuKhoan{get;set;}
    public string? NoiDungDieuKhoan{get;set;}
}

