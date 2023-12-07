using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class VatTuListNhaCungUng{
    public string? TenVatTu{get;set;}
    public long VatTuId{get;set;}
    public string? MaPhieu{get;set;}
    public string? CodeYear{get;set;}
    public List<long>? ListNhaCungUng{get;set;}
    public string? YeuCauKiThuat{get;set;}
    public Decimal? SoLuongBaoGia{get;set;}
    public string? GhiChu{get;set;}
    public BaoGiaChiTiet ToModelBaoGiaChiTiet(long createdAt, long updatedAt, Guid baoGiaId){
        BaoGiaChiTiet baoGiaChiTiet = new()
        {
            TenVatTu = TenVatTu,
            BaoGiaId = baoGiaId,
            VatTuId = VatTuId,
            SoLuongBaoGia = SoLuongBaoGia,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            MaPhieu = MaPhieu,
            CodeYear = CodeYear,
            CreatedTime = DateTime.Now,
            UpdatedTime = DateTime.Now,
            YeuCauKiThuat = YeuCauKiThuat,
            GhiChu = GhiChu
        };
        return baoGiaChiTiet;
    }
    public List<BaoGiaChitietNhaCungUng> ToModelBaoGiaChiTietNhaCungUng(long createdAt, long updatedAt, Guid baoGiaChiTietId){
        List<BaoGiaChitietNhaCungUng> listBaoGiaChiTietNhaCungUng = new();
        for(int i=0; i<ListNhaCungUng.Count; i++){
            BaoGiaChitietNhaCungUng baoGiaChitietNhaCungUng = new()
            {
                VatTuId = VatTuId,
                BaoGiaChiTietId = baoGiaChiTietId,
                NhaCungUngId = ListNhaCungUng[i],
                CreatedTime = DateTime.Now,
                UpdatedTime = DateTime.Now,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                // GhiChu = ListNhaCungUng[i].GhiChu
            };
            listBaoGiaChiTietNhaCungUng.Add(baoGiaChitietNhaCungUng);
        }
        return listBaoGiaChiTietNhaCungUng;
    }
}
public class NhaCungUngVatTuParam{
    public long Id{get;set;}
    public string? GhiChu{get;set;}
}

