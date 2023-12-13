using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.DTOs;

public class BaoGiaUpdateDTO{
    public Guid? MaBaoGia{get;set;}
    public long CreatedBy{get;set;}
    public long UpdateBy{get;set;}
    public List<NhaCungUngListVatTu>? ListNhaCungUngListVatTu{get;set;}
    public List<VatTuBaoGiaChiTietDTO>? ListVatTuBaoGiaChiTietDTO{get;set;}
    public List<BaoGiaChiTiet> ToListBaoGiaChiTiet(Guid BaoGiaId){
        var listResult = ListNhaCungUngListVatTu.Select(x=>new BaoGiaChiTiet{
                            NhaCungUngId = x.IdNhaCungUng,
                            TenNhaCungUng = x.TenNhaCungUng,
                            BaoGiaId = BaoGiaId,
                            CreatedAt = CreatedBy,
                            UpdatedAt = UpdateBy,
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now
                         }).ToList();
        return listResult;
    }
}


