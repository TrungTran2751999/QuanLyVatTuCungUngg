using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;

public class PhieuNhanVatTuChiTietFast{
    [NotMapped]
    public int stt{get;set;}
    public string? ma_vt{get;set;}
    public string? ten_vt{get;set;}
    public string? dvt{get;set;}

    public string? ten_dvt{get;set;}
    // public decimal? he_so{get;set;}
    public string? ma_kho{get;set;}

    // public string? ten_kho{get;set;}
    // public string? ma_vi_tri{get;set;}
    // public string? ten_vi_tri{get;set;}
    // public string? ma_lo{get;set;}
    // public string? ten_lo{get;set;}

        // public Decimal? ton13{get;set;}
    public Decimal? so_luong{get;set;}
        // public DateTime? ngay_yc{get;set;}
        // public Decimal? sl_xuat{get;set;}

    // public string? ma_vv{get;set;}
    // public string? ten_vv{get;set;}
    // public string? ma_bp{get;set;}
    // public string? ten_bp{get;set;}
    // public string? so_lsx{get;set;}
    // public string? ten_lsx{get;set;}
    // public string? ma_sp{get;set;}
    // public string? ten_sp{get;set;}
    // public string? ma_hd{get;set;}
    // public string? ten_hd{get;set;}
    // public string? ma_phi{get;set;}
    // public string? ten_phi{get;set;}
    // public string? ma_ku{get;set;}
    // public string? ten_ku{get;set;}

    // public string? nhieu_dvt{get;set;}
    // public string? lo_yn{get;set;}
    // public string? vi_tri_yn{get;set;}
    // public string? tk_du{get;set;}

    // public string? ten_tk_du{get;set;}

    // public string? tk_vt{get;set;}

    // public string? ten_tk_vt{get;set;}

        // public Decimal? gia{get;set;}
        // public Decimal? tien{get;set;}
        // public Decimal? gia_nt{get;set;}
        // public Decimal? tien_nt{get;set;}
    // public string? sua_tk_vt{get;set;}
    public string? stt_rec{get;set;}
    public string? stt_rec0{get;set;}
    // public string? line_nbr{get;set;}
    // public string? stt_rec_px{get;set;}
    // public string? stt_rec0px{get;set;}
    public string? gc_td1{get;set;}
    public Decimal? gia{get;set;}
    public PhieuDeNghiNhanVatTuChiTietDaDuyet toPhieuDeNghiChiTiet(){
        return new PhieuDeNghiNhanVatTuChiTietDaDuyet(){
            MaVatTu = ma_vt,
            SoLuong = so_luong,
            GhiChu = gc_td1,
            MaPhieu = stt_rec
        };
    }
    
}
