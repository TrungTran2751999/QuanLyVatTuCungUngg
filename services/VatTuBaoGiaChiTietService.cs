using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
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
                                    ListNhaCungUng = dbContext.VatTuBaoGiaChiTietNhaCungUngs
                                                    .Where(x=>x.BaoGiaId==BaoGiaId)
                                                    .Select(x=>new NhaCungUng{
                                                        NhaCungUngId = x.NhaCungUngId,
                                                        TenNhaCungUng = x.TenNhaCungUng
                                                    }).ToList()
                                }).ToListAsync();
        var listPhieuDeNghiNhanVatTu = await dbContext.PhieuDeNghiNhanVatTuDaDuyet.Where(x=>x.BaoGiaId==BaoGiaId).ToListAsync();
        List<PhieuNhanVatTuChiTietFast> listPhieuNVTFasts = new();
        foreach(var phieuDeNghiNhanVatTu in listPhieuDeNghiNhanVatTu){
            var listPhieuFast = await phieuNVTService.GetByMaPhieu(phieuDeNghiNhanVatTu.MaPhieu, phieuDeNghiNhanVatTu.CodeYear);
            listPhieuNVTFasts.AddRange(listPhieuFast);
        }
        
        var listResult = (from phieuFast in listPhieuNVTFasts
                         join vatTuBaoGia in dbContext.VatTuBaoGiaChiTiets on phieuFast.stt_rec equals vatTuBaoGia.MaPhieu into joinDept
                         from joinDeptItem in joinDept.DefaultIfEmpty()
                         select new VatTuBaoGiaChiTietReponse{
                            Id = joinDeptItem==null ? null : joinDeptItem.Id,
                            TenVatTu = phieuFast.ten_vt,
                            MaVatTu = phieuFast.ma_vt,
                            MaPhieu = phieuFast.stt_rec,
                            ListNhaCungUng = joinDeptItem!=null ? dbContext.VatTuBaoGiaChiTietNhaCungUngs
                                            .Where(x=>x.BaoGiaId==BaoGiaId && x.VatTuBaoGiaChiTietId ==joinDeptItem.Id)
                                            .Select(x=>new NhaCungUng{
                                                NhaCungUngId = x.NhaCungUngId,
                                                TenNhaCungUng = x.TenNhaCungUng
                                            }).ToList()
                                            :new List<NhaCungUng>()
                         }).ToList();

        return listResult;
    }
    
}