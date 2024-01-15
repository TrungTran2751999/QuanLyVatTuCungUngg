using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text.Json;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.DTOs;

public class HopDongResponse{
    public Guid Id{get;set;}
    public string? SoHopDong{get;set;}
    public DateTime NgayKiKet{get;set;}
    public string? NhaCungUng{get;set;}
    public bool GioiTinhNhaCungUng{get;set;}
    public string? DaiDienNhaCungUng{get;set;}
    public string? ChucVuNhaCungUng{get;set;}
    public string? DiaChiNhaCungUng{get;set;}
    public string? DienThoaiNhaCungUng{get;set;}
    public string? TaiKhoanNhaCungUng{get;set;}
    public string? MaSoThueNhaCungUng{get;set;}
    public List<Hang>? ListHang{get;set;}
    public List<DieuKhoan>? ListDieuKhoan{get;set;}
    public string? DiaChiNhanHang{get;set;}
}

