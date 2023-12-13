using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
// using System.Data.Entity;
namespace app.Services;
public class VatTuBaoGiaChiTietService : IVatTuBaoGiaChiTietService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IPhieuNVTService phieuNVTService;

    public VatTuBaoGiaChiTietService(ApplicationDbContext dbContext, IPhieuNVTService phieuNVTService){
        this.dbContext = dbContext;
        this.phieuNVTService = phieuNVTService;
    }

    public async Task<List<VatTuBaoGiaChiTietReponse>> GetByBaoGiaId(Guid BaoGiaId)
    {
        var listVatTuHasNhaCungUng = await dbContext.VatTuBaoGiaChiTiets
                                .Where(x=>x.BaoGiaId==BaoGiaId)
                                .Select(x=>new VatTuBaoGiaChiTietReponse{
                                    Id = x.Id,
                                    TenVatTu = x.TenVatTu,
                                    VatTuId = x.VatTuId,
                                    MaVatTu = x.MaVatTu,
                                    MaPhieu = x.MaPhieu,
                                    YeuCauKiThuat = x.YeuCauKiThuat,
                                    SoLuongBaoGia = x.SoLuongBaogia,
                                    DonViTinh = x.DonViTinh,
                                    GhiChu = x.GhiChu,
                                    CodeYear = x.CodeYear,
                                    ListNhaCungUng = dbContext.VatTuBaoGiaChiTietNhaCungUngs
                                                    .Where(y=>y.BaoGiaId==BaoGiaId && y.VatTuBaoGiaChiTietId==x.Id)
                                                    .Select(x=>x.NhaCungUngId).ToList()
                                }).ToListAsync();
        var listPhieuDeNghiNhanVatTu = await dbContext.PhieuDeNghiNhanVatTuDaDuyet.Where(x=>x.BaoGiaId==BaoGiaId).ToListAsync();
        List<VatTuBaoGiaChiTietReponse> listPhieuDeNghiConvert = new();
        foreach(var phieuDeNghiNhanVatTu in listPhieuDeNghiNhanVatTu){
            var listPhieuFast = await phieuNVTService.GetByMaPhieu(phieuDeNghiNhanVatTu.MaPhieu, phieuDeNghiNhanVatTu.CodeYear);
            var listPhieuDeNghi = listPhieuFast.Select(x=>new VatTuBaoGiaChiTietReponse{
                                        Id = null,
                                        TenVatTu = x.ten_vt,
                                        VatTuId = null,
                                        MaVatTu = x.ma_vt,
                                        MaPhieu = x.stt_rec,
                                        DonViTinh = x.dvt,
                                        CodeYear = phieuDeNghiNhanVatTu.CodeYear,
                                        SoLuongBaoGia = x.so_luong,
                                        GhiChu = x.gc_td1,
                                        ListNhaCungUng = new List<long>()
                                  }).ToList();
            listPhieuDeNghiConvert.AddRange(listPhieuDeNghi);
        }
        
        //xu ly tiep cai phan lay vat tu id
        var listPhieuDeNghiConverted = (from phieuDeNghiConvert in listPhieuDeNghiConvert
                                       join vatTu in dbContext.VatTu
                                       on phieuDeNghiConvert.MaVatTu equals vatTu.MaVatTu into joinDept
                                       from result in joinDept.DefaultIfEmpty()
                                       select new VatTuBaoGiaChiTietReponse{
                                            Id = null,
                                            TenVatTu = phieuDeNghiConvert.TenVatTu,
                                            VatTuId = result!=null ? result.Id : null,
                                            MaVatTu = phieuDeNghiConvert.MaVatTu,
                                            MaPhieu = phieuDeNghiConvert.MaPhieu,
                                            DonViTinh = phieuDeNghiConvert.DonViTinh,
                                            CodeYear = phieuDeNghiConvert.CodeYear,
                                            GhiChu = phieuDeNghiConvert.GhiChu,
                                            SoLuongBaoGia = phieuDeNghiConvert.SoLuongBaoGia,
                                            ListNhaCungUng = phieuDeNghiConvert.ListNhaCungUng
                                       }).ToList();

        var listResult = new List<VatTuBaoGiaChiTietReponse>();
        foreach(var phieuFast in listPhieuDeNghiConverted){
            var result = new VatTuBaoGiaChiTietReponse();
            result = phieuFast;
            foreach(var vatTu in listVatTuHasNhaCungUng){
                if(vatTu.MaVatTu == phieuFast.MaVatTu && vatTu.MaPhieu==phieuFast.MaPhieu){
                    result = vatTu;
                    listVatTuHasNhaCungUng.Remove(vatTu);
                    break;
                }
            }
            listResult.Add(result);
        }
        return listResult;
    }
    
}