using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class PhieuDeNghiPheDuyetCreateDTO{
    [Required]
    public string? MaPhieu{get;set;}
    [Required]
    public string? TenNguoiDeNghi{get;set;}
    public string? DienGiai{get;set;}
    [Required]
    public string? TenBoPhan{get;set;}
    [Required]
    public DateTime DateDeNghi{get;set;}
    [Required]
    public string? CodeYear{get;set;}
    [Required]
    public List<VatTuDTO>? ListVatTu{get;set;}
    [Required]
    public long CreatedAt{get;set;}
    [Required]
    public long UpdateAt{get;set;}
    public PhieuDeNghiNhanVatTuDaDuyet ToModel(){
        PhieuDeNghiNhanVatTuDaDuyet phieu = new()
        {
            MaPhieu = MaPhieu,
            TenNguoiDeNghi = TenNguoiDeNghi,
            DienGiai = DienGiai,
            TenBoPhan = TenBoPhan,
            DateDeNghi = DateDeNghi,
            CodeYear = CodeYear,
            CreatedAt = CreatedAt,
            UpdateAt = UpdateAt,
            CreatedTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            IsDeleted = false,
            IsTongHop = false,
        };
        return phieu;
    }
    public List<PhieuDeNghiNhanVatTuChiTietDaDuyet> ToListPhieuChiTiet(Guid idPhieuDeNghiMax){
        List<PhieuDeNghiNhanVatTuChiTietDaDuyet> listPhieuDeNghiNhanVatTu = new();
        for(int i=0; i<ListVatTu.Count; i++){
            listPhieuDeNghiNhanVatTu.Add(ListVatTu[i].ToModel(CreatedAt, UpdateAt, idPhieuDeNghiMax));
        }
        return listPhieuDeNghiNhanVatTu;
    }
}