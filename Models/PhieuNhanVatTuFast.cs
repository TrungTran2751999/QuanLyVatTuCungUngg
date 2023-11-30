using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using app.DTOs;

namespace app.Models;

public class PhieuNhanVatTuFast{
    public string? stt_rec{get;set;}
    public string? ma_dvcs{get;set;}
    public DateTime? ngay_ct{get;set;}
    public string? so_ct{get;set;}
    public string? dept_id{get;set;}
    public string? ten_bp{get;set;}
    public Decimal? t_so_luong{get;set;}
    public string? dien_giai{get;set;}
    public string? ma_ct{get;set;}
    public string? status{get;set;}
    public string? u0{get;set;}
    public DateTime? datetime0{get;set;}
    public DateTime datetime2{get;set;}
    public int user_id0{get;set;}
    public string? u1{get;set;}
    public int user_id2{get;set;}
    public string? u2{get;set;}

    public PhieuNVTDTO ToPhieuNVTDTO(){
        PhieuNVTDTO phieuNVTDTO = new()
        {
            MaPhieu = stt_rec,
            TenBoPhan = ten_bp,
            DienGiai = dien_giai,
            NguoiYeuCau = u1
        };
        return phieuNVTDTO;
    }
}
