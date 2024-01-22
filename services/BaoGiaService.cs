using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using System.Transactions;
using System.Net;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using DocumentFormat.OpenXml.Office.CustomUI;
using System.Text.Json;
using System.IO.Compression;
using EXCEL = OfficeOpenXml;
using OfficeOpenXml.Style;
using DW = System.Drawing;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using app.Utils;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using ZstdSharp.Unsafe;
using Org.BouncyCastle.Crypto.Parameters;
using DocumentFormat.OpenXml.Office2010.Excel;

// using System.Data.Entity;
namespace app.Services;
public class BaoGiaService : IBaogiaService
{
    private readonly ApplicationDbContext dbContext;
    private readonly ApplicationDbContextQLVT dbContextQLVT;
    private IUtil util;

    public BaoGiaService(ApplicationDbContext dbContext, ApplicationDbContextQLVT dbContextQLVT, IUtil util){
        this.dbContext = dbContext;
        this.dbContextQLVT = dbContextQLVT;
        this.util = util;
    }

    public async Task<List<BaoGiaResponse>?> GetAll(bool isDeleted, int page)
    {
        int limit = 10;
        int start = limit*(page-1);
        var listResult = await (from baoGia in dbContext.BaoGia 
                               join userVT in dbContext.UserVatTus on baoGia.CreatedAt equals userVT.Id
                               select new BaoGiaResponse{
                                  Id = baoGia.Id,
                                  MaBaoGia = baoGia.MaBaoGia,
                                  CreatedTime = baoGia.CreatedTime,
                                  UpdatedTime = baoGia.UpdatedTime,
                                  UserBaoGia = userVT.UserName,
                                  UserId = userVT.Id
                               }).OrderByDescending(x=>x.UpdatedTime)
                               .Skip(start).Take(limit)
                               .ToListAsync();
        return listResult;
    }
    public async Task<List<NhaCungUngListVatTu>> GetById(Guid BaoGiaId)
    {
        var result = await dbContext.BaoGiaChiTiet
                                    .Select(x=>new NhaCungUngListVatTu{
                                        BaoGiaChiTietId = x.Id,
                                        TenNhaCungUng = x.TenNhaCungUng,
                                        IdNhaCungUng = x.NhaCungUngId,
                                        BaoGiaId = x.BaoGiaId,
                                        ListVatTu = dbContext.BaoGiaChitietVatTu.Select(y=>new VatTuInBaoGia{
                                            IdNhaCungUng = x.NhaCungUngId,
                                            VatTuId = y.VatTuId,
                                            TenVatTu = y.TenVatTu,
                                            MaPhieu = y.MaPhieu,
                                            DonViTinh = y.DonViTinh,
                                            YeuCauKiThuat = y.YeucauKiThuat,
                                            CodeYear = y.CodeYear,
                                            SoLuong = y.SoLuongBaoGia,
                                            GhiChu = y.GhiChu,
                                            BaoGiaChiTietId = y.BaoGiaChiTietId
                                        })
                                        .Where(z=>x.Id==z.BaoGiaChiTietId)
                                        .ToList()
                                    })
                                    .Where(x=>x.BaoGiaId == BaoGiaId)
                                    .ToListAsync();
    
        return result;
    }
    public async Task<List<NhaCungUngListVatTu>> GetListNhaCungUng(Guid baoGiaId){
        var listResult = await dbContext.BaoGiaChiTiet
                                        .Where(x=>x.BaoGiaId==baoGiaId)
                                        .Select(x=>new NhaCungUngListVatTu{
                                            BaoGiaChiTietId = x.Id,
                                            IdNhaCungUng = x.NhaCungUngId,
                                            TenNhaCungUng = x.TenNhaCungUng 
                                        }).ToListAsync();
        return listResult;
    }
    public async Task<HopDongResponse> GetListVatTuAndNhaCungUng(BaoGiaLapHopDongParam lapHopDongParam){
        var nhaCungUng = await dbContext.NhaCungUng.Where(x=>x.Id==lapHopDongParam.NhaCungUngId).FirstOrDefaultAsync();

        Guid baoGiaChiTietId = await dbContext.BaoGiaChiTiet
                                        .Where(x=>x.BaoGiaId==lapHopDongParam.BaoGiaId && x.NhaCungUngId==lapHopDongParam.NhaCungUngId)
                                        .Select(x=>x.Id)
                                        .FirstOrDefaultAsync();
        long countHopDongPerMonth = dbContext.HopDongMuaHang
                                             .Count(x=>
                                                x.NgayKiKet.Month == lapHopDongParam.NgayKiKet.Month &&
                                                x.NgayKiKet.Year == lapHopDongParam.NgayKiKet.Year) + 1;
        string sttHopDong = "Số "+ countHopDongPerMonth +"-"+util.DoiNgayThangHienTai(lapHopDongParam.NgayKiKet, "KiHieuHopDong");
        string dienThoai = "";
        if(nhaCungUng.DienThoai!=null && nhaCungUng?.DienThoaiDiDong!=null){
            dienThoai = nhaCungUng.DienThoai +" - "+nhaCungUng.DienThoaiDiDong;
        }else if(nhaCungUng?.DienThoai!=null){
            dienThoai = nhaCungUng.DienThoai;
        }else if(nhaCungUng?.DienThoaiDiDong!=null){
            dienThoai = nhaCungUng.DienThoaiDiDong;
        }
        var result = new HopDongResponse{
            SoHopDong = sttHopDong + "/HĐMB/HueWACO-"+nhaCungUng?.TenVietTat,
            NgayKiKet = lapHopDongParam.NgayKiKet,
            NhaCungUng = nhaCungUng?.TenNhaCungUng,
            GioiTinhNhaCungUng = (bool)nhaCungUng?.GioiTinhNguoiDaiDien,
            DaiDienNhaCungUng = nhaCungUng.NguoiDaiDien,
            ChucVuNhaCungUng = nhaCungUng.ChucVu,
            DiaChiNhaCungUng = nhaCungUng.DiaChi,
            DienThoaiNhaCungUng = dienThoai,
            TaiKhoanNhaCungUng = nhaCungUng.SoTaiKhoan,
            MaSoThueNhaCungUng = nhaCungUng.MaSoThue,
            ListHang = await dbContext.BaoGiaChitietVatTu 
                                .Where(x=>x.BaoGiaChiTietId==baoGiaChiTietId)
                                .Select(x=>new Hang{
                                    TenHang = x.TenVatTu,
                                    DonVi = x.DonViTinh,
                                    SoLuong = (decimal)x.SoLuongBaoGia
                                }).ToListAsync(),
            DiaChiNhanHang = lapHopDongParam.DiaChiNhanHang
        };
        return result;
    }
    public async Task<string> SaveBaoGia(BaoGiaCreateDTO baoGiaCreate)
    {
        List<long> listVatTuId = new();
        List<long> listNhaCungUng = new();
        List<long> listNhaCungUngId = new();

        List<Guid> listPhieuDaDuyetId = baoGiaCreate.ListPhieuPheDuyet;
        long createdBy = baoGiaCreate.CreatedBy;
        long updatedBy = baoGiaCreate.UpdateBy;
        
        List<NhaCungUngListVatTu> listNhaCungUngListVatTu = baoGiaCreate.ListNhaCungUngListVatTu;
        
        List<VatTuInBaoGia> listVatTuAndIdNhaCungUng = new();
        for(int i=0; i<listNhaCungUngListVatTu.Count; i++){
            listVatTuAndIdNhaCungUng.AddRange(listNhaCungUngListVatTu[i].ListVatTu);
        }

        for(int i=0; i<listNhaCungUngListVatTu.Count; i++){
            listNhaCungUng.Add(listNhaCungUngListVatTu[i].IdNhaCungUng);
            // listNhaCungUngId.AddRange(listNhaCungUng.Select(x=>x.Id).ToList());
        }
        listVatTuId = listVatTuId.Distinct().ToList();
        listNhaCungUngId = listNhaCungUngId.Distinct().ToList();

        var checkListNhaCungUng = await dbContext.NhaCungUng
                                                 .Where(x=>listNhaCungUngId.Contains(x.Id))
                                                 .ToListAsync();
        var checkListVatTu = await dbContextQLVT.VatTu
                                            .Where(x=>listVatTuId.Contains(x.Id))
                                            .ToListAsync();
        if(listNhaCungUngId.Count != checkListNhaCungUng.Count) return "NhaCungUng:Tồn tại nhà cung ứng không thuộc danh sách";
        
        var listBaoGiaId = baoGiaCreate.ListPhieuPheDuyet;

        using(var transaction = dbContext.Database.BeginTransaction()){
            try{
                // check xem phieu de nghi da duoc lap bao gia chua
                // await XoaBaoGiaByPhieuDeNghi(listBaoGiaId, transaction);
                
                // add du lieu vao bang bao gia
                await dbContext.BaoGia.AddAsync(baoGiaCreate.ToModel());
                dbContext.SaveChanges();

                //record moi tao
                var lastRecordBaoGia = await dbContext.BaoGia
                                                 .OrderByDescending(x=>x.CreatedTime)
                                                 .Select(x=>new{
                                                    x.Id
                                                 }).FirstOrDefaultAsync(); 
            
                for(int i=0; i<listBaoGiaId.Count; i++){
                    var phieuPheDuyet = await dbContext.PhieuDeNghiNhanVatTuDaDuyet
                                                       .Where(x=>x.Id==listBaoGiaId[i])
                                                       .FirstOrDefaultAsync();
                    phieuPheDuyet.BaoGiaId = lastRecordBaoGia.Id;
                    dbContext.SaveChanges();
                }
                
                for(int i=0; i<listNhaCungUngListVatTu.Count; i++){
                    //add du lieu vao bang BaoGia_ChiTiet
                    var vatTuNhaCungUngParam = listNhaCungUngListVatTu[i].ToBaoGiaChiTiet(lastRecordBaoGia.Id, createdBy, updatedBy);
                    await dbContext.BaoGiaChiTiet.AddAsync(vatTuNhaCungUngParam);
                    dbContext.SaveChanges();


                    var listBaoGiaChiTietVatTu = listNhaCungUngListVatTu[i].ToListBaoGiaChiTietVatTu(vatTuNhaCungUngParam.Id, createdBy, updatedBy);

                    //add du lieu vao BaoGia_ChiTiet_NhacungUng
                    await dbContext.BaoGiaChitietVatTu.AddRangeAsync(listBaoGiaChiTietVatTu);
                    
                    dbContext.SaveChanges();
                }
                
                CreateVatTuBaoGiaChiTiet(lastRecordBaoGia.Id, baoGiaCreate.ListVatTuBaoGiaChiTietDTO, createdBy, updatedBy, transaction);
                // foreach(var vatTuBaoGia in baoGiaCreate.ListVatTuBaoGiaChiTietDTO){
                //     var vatTuBaoGiaParam = vatTuBaoGia.ToModel(lastRecordBaoGia.Id, createdBy, updatedBy);
                //     await dbContext.VatTuBaoGiaChiTiets.AddAsync(vatTuBaoGiaParam);
                //     dbContext.SaveChanges();
                    
                //     var listVatTuBaogia = vatTuBaoGia.ToListVatTuBaoGiaChiTietNhaCungUng(vatTuBaoGiaParam.Id, createdBy, updatedBy);
                //     await dbContext.VatTuBaoGiaChiTietNhaCungUngs.AddRangeAsync(listVatTuBaogia);
                //     dbContext.SaveChanges();
                // }

                transaction.Commit();
                return "OK";
            }catch(Exception e){
                Console.WriteLine(e);
                transaction.Rollback();
                return "NOT OK";
            }
        }
    }
    private void CreateVatTuBaoGiaChiTiet(Guid baoGiaId, List<VatTuBaoGiaChiTietDTO> listVatTuBaoGias, long CreatedBy, long UpdatedBy, IDbContextTransaction transaction){
        try{
            foreach(var vatTuBaoGia in listVatTuBaoGias){
                var vatTuBaoGiaParam = vatTuBaoGia.ToModel(baoGiaId, CreatedBy, UpdatedBy);
                dbContext.VatTuBaoGiaChiTiets.Add(vatTuBaoGiaParam);
                dbContext.SaveChanges();
                
                var listVatTuBaogia = vatTuBaoGia.ToListVatTuBaoGiaChiTietNhaCungUng(baoGiaId, vatTuBaoGiaParam.Id, CreatedBy, UpdatedBy);
                dbContext.VatTuBaoGiaChiTietNhaCungUngs.AddRangeAsync(listVatTuBaogia);
                dbContext.SaveChanges();
            }
        }catch(Exception e){
            Console.WriteLine(e);
            transaction.Rollback();
            return;
        }
    }
    public async Task<string> XoaBaoGiaByPhieuDeNghi(List<Guid> listId, IDbContextTransaction transaction){
        try{
            foreach(var id in listId){
                var phieuNhanVatTu = await dbContext.PhieuDeNghiNhanVatTuDaDuyet.Where(x=>x.Id==id).FirstOrDefaultAsync();
                var baoGia = await dbContext.BaoGia.Where(x=>x.Id==phieuNhanVatTu.BaoGiaId).FirstOrDefaultAsync();
            
                if(baoGia!=null){
                    foreach(var ids in listId){
                        var phieuNhanVatTus = await dbContext.PhieuDeNghiNhanVatTuDaDuyet.Where(x=>x.BaoGiaId==baoGia.Id).ToListAsync();
                        phieuNhanVatTus.ForEach(x => x.BaoGiaId = null);
                        dbContext.SaveChanges();
                    }
                    var listBaoGiaChiTiet = await dbContext.BaoGiaChiTiet.Where(x=>x.BaoGiaId==baoGia.Id).ToListAsync();
                    foreach(var baoGiachiTiet in listBaoGiaChiTiet){
                        var baoGiaChiTietVatTu = await dbContext.BaoGiaChitietVatTu.Where(x=>x.BaoGiaChiTietId==baoGiachiTiet.Id).ToListAsync();
                        dbContext.BaoGiaChitietVatTu.RemoveRange(baoGiaChiTietVatTu);
                        dbContext.SaveChanges();
                    }
                    dbContext.BaoGiaChiTiet.RemoveRange(listBaoGiaChiTiet);
                    dbContext.SaveChanges();

                    var vatTuBaoGiaChiTiet = await dbContext.VatTuBaoGiaChiTiets.Where(x=>x.BaoGiaId==baoGia.Id).ToListAsync();
                    var vatTuBaoGiaChiTietNhaCungUng = await dbContext.VatTuBaoGiaChiTietNhaCungUngs.Where(x=>x.BaoGiaId==baoGia.Id).ToListAsync();

                    dbContext.VatTuBaoGiaChiTietNhaCungUngs.RemoveRange(vatTuBaoGiaChiTietNhaCungUng);
                    dbContext.VatTuBaoGiaChiTiets.RemoveRange(vatTuBaoGiaChiTiet);
                    dbContext.SaveChanges();

                    dbContext.BaoGia.Remove(baoGia);
                    dbContext.SaveChanges();

                }
            }
            return "OK";
        }catch(Exception e){
            Console.WriteLine(e);
            transaction.Rollback();
            return "NOT OK";
        }
    }
    public async Task<string> XoaBaoGiaByBaoGiaId(Guid baoGiaId, IDbContextTransaction transaction)
    {
        try{
            var baoGia = await dbContext.BaoGia.Where(x=>x.Id==baoGiaId).FirstOrDefaultAsync();
            if(baoGia == null)  return "NOT OK";

            var listBaoGiaChiTiet = await dbContext.BaoGiaChiTiet.Where(x=>x.BaoGiaId==baoGia.Id).ToListAsync();
            foreach(var baoGiachiTiet in listBaoGiaChiTiet){
                var baoGiaChiTietVatTu = await dbContext.BaoGiaChitietVatTu.Where(x=>x.BaoGiaChiTietId==baoGiachiTiet.Id).ToListAsync();
                dbContext.BaoGiaChitietVatTu.RemoveRange(baoGiaChiTietVatTu);
                dbContext.SaveChanges();
            }
            dbContext.BaoGiaChiTiet.RemoveRange(listBaoGiaChiTiet);
            dbContext.SaveChanges();

            var vatTuBaoGiaChiTiet = await dbContext.VatTuBaoGiaChiTiets.Where(x=>x.BaoGiaId==baoGia.Id).ToListAsync();
            var vatTuBaoGiaChiTietNhaCungUng = await dbContext.VatTuBaoGiaChiTietNhaCungUngs.Where(x=>x.BaoGiaId==baoGia.Id).ToListAsync();

            dbContext.VatTuBaoGiaChiTietNhaCungUngs.RemoveRange(vatTuBaoGiaChiTietNhaCungUng);
            dbContext.VatTuBaoGiaChiTiets.RemoveRange(vatTuBaoGiaChiTiet);
            dbContext.SaveChanges();

            return "OK";

        }catch(Exception e){
            Console.WriteLine(e);
            transaction.Rollback();
            return "NOT OK";
        }
    }
    public async Task<string> CapNhatBaoGia(BaoGiaUpdateDTO baoGiaUpdateDTO)
    {
        var baoGiaUpdate = await dbContext.BaoGia.Where(x=>x.Id == baoGiaUpdateDTO.MaBaoGia).FirstOrDefaultAsync();
        if(baoGiaUpdate==null) return "BaoGia:Không tồn tại báo giá";
        
        using(var transaction = dbContext.Database.BeginTransaction()){
            try{
                var maBaoGia = new List<Guid>(){baoGiaUpdate.Id};
                var xoa = await XoaBaoGiaByBaoGiaId(baoGiaUpdate.Id, transaction);
                if(xoa=="OK"){
                    baoGiaUpdate.UpdatedTime = DateTime.Now;
                    baoGiaUpdate.UpdatedAt = baoGiaUpdateDTO.UpdateBy;
                    dbContext.SaveChanges();

                    foreach(var vatTuNhaCungUng in baoGiaUpdateDTO.ListNhaCungUngListVatTu){
                        var baoGiaChiTiet = vatTuNhaCungUng.ToBaoGiaChiTiet(baoGiaUpdate.Id, baoGiaUpdateDTO.CreatedBy, baoGiaUpdateDTO.UpdateBy);
                        dbContext.BaoGiaChiTiet.Add(baoGiaChiTiet);

                        var listVatTuBaoGiaChitiet = vatTuNhaCungUng.ToListBaoGiaChiTietVatTu(baoGiaChiTiet.Id, baoGiaUpdateDTO.CreatedBy, baoGiaUpdateDTO.UpdateBy);
                        dbContext.BaoGiaChitietVatTu.AddRange(listVatTuBaoGiaChitiet);
                    }

                    CreateVatTuBaoGiaChiTiet(baoGiaUpdate.Id, baoGiaUpdateDTO.ListVatTuBaoGiaChiTietDTO, baoGiaUpdateDTO.CreatedBy, baoGiaUpdateDTO.UpdateBy, transaction);
                    transaction.Commit();
                    return "OK";
                }else{
                    return ":Lỗi hệ thống";
                }
            }catch(Exception e){
                Console.WriteLine(e);
                transaction.Rollback();
                return "NOT OK";
            }
        }
    }
    public async Task<byte[]> LapBaoGia(string data){
        BaoGiaCreateDTO baoGias = JsonSerializer.Deserialize<BaoGiaCreateDTO>(data);
        var listVatTu = baoGias.ListNhaCungUngListVatTu;
        ExportFile exportFile = new ExportFile();
        List<MemoryStream> listMemoryStream = new List<MemoryStream>();
        foreach(var baoGia in listVatTu){
            MemoryStream stream = await util.ExportBaoGiaToExcel(baoGia.ListVatTu);
            listMemoryStream.Add(stream);
        }
        exportFile.files = listMemoryStream;
        List<string> names = new List<string>();
        foreach(var nhaCungUng in listVatTu){
            names.Add(nhaCungUng.TenNhaCungUng);
        }
        exportFile.names = names;
        return util.ZipFile(exportFile);
    }
    public async Task<List<BaoGiaResponse>> Filter(BaoGiaFilter baoGiaFilter){
        List<BaoGiaResponse> listResult = new();
        bool condition(BaoGiaResponse x){
            if(baoGiaFilter.Search!=null){
                if(util.RemoveDauTiengViet(x.MaBaoGia.ToLower()).Contains(util.RemoveDauTiengViet(baoGiaFilter.Search.ToLower())) ||
                   util.RemoveDauTiengViet(x.BaoGiaChiTietRes.TenNhaCungUng.ToLower()).Contains(util.RemoveDauTiengViet(baoGiaFilter.Search).ToLower())
                ){
                    if(baoGiaFilter.NgayTaoBaoGiaStart!=null && baoGiaFilter.NgayTaoBaoGiaKetThuc!=null){
                        if(baoGiaFilter.NgayTaoBaoGiaStart?.Date <= x.CreatedTime?.Date && baoGiaFilter.NgayTaoBaoGiaKetThuc?.Date >= x.CreatedTime?.Date){
                            return true;
                        }else{
                            return false;
                        }
                    }else if(baoGiaFilter.NgayTaoBaoGiaStart?.Date!=null){
                        if(baoGiaFilter.NgayTaoBaoGiaStart?.Date <= x.CreatedTime?.Date){
                            return true;
                        }else{
                            return false;
                        }
                    }else if(baoGiaFilter.NgayTaoBaoGiaKetThuc?.Date!=null){
                        if(baoGiaFilter.NgayTaoBaoGiaKetThuc?.Date >= x.CreatedTime?.Date){
                            return true;
                        }else{
                            return false;
                        }
                    }
                    return true;
                }
            }else{
                if(baoGiaFilter.NgayTaoBaoGiaStart?.Date!=null && baoGiaFilter.NgayTaoBaoGiaKetThuc?.Date!=null){
                    if(baoGiaFilter.NgayTaoBaoGiaStart?.Date <= x.CreatedTime?.Date && baoGiaFilter.NgayTaoBaoGiaKetThuc?.Date >= x.CreatedTime?.Date){
                        return true;
                    }else{
                        return false;
                    }
                }else if(baoGiaFilter.NgayTaoBaoGiaStart?.Date!=null){
                    if(baoGiaFilter.NgayTaoBaoGiaStart?.Date <= x.CreatedTime?.Date){
                        return true;
                    }else{
                        return false;
                    }
                }else if(baoGiaFilter.NgayTaoBaoGiaKetThuc?.Date!=null){
                    if(baoGiaFilter.NgayTaoBaoGiaKetThuc?.Date >= x.CreatedTime?.Date){
                        return true;
                    }else{
                        return false;
                    }
                }
                return false;
            }
            return false;
        }
        listResult = dbContext.BaoGia.Join(dbContext.BaoGiaChiTiet,
                                           baoGia=>baoGia.Id,
                                           baoGiachitiet=>baoGiachitiet.BaoGiaId,
                                           (baoGia, baoGiachitiet)=>new BaoGiaResponse{
                                            Id = baoGia.Id,
                                            MaBaoGia = baoGia.MaBaoGia,
                                            CreatedTime = baoGia.CreatedTime,
                                            UpdatedTime = baoGia.UpdatedTime,
                                            BaoGiaChiTietRes = new BaoGiaChiTietResponse{
                                                Id = baoGiachitiet.Id,
                                                BaoGiaId = baoGia.Id,
                                                NhaCungUngId = baoGiachitiet.NhaCungUngId,
                                                TenNhaCungUng = baoGiachitiet.TenNhaCungUng
                                            },
                                            UserBaoGia = dbContext.UserVatTus.Where(x=>x.Id==baoGia.CreatedAt).Select(x=>x.UserName).FirstOrDefault()
                                           })
                                           .Where(condition)
                                           .ToList();
        
        List<BaoGiaResponse> listResultFinal = new();
        listResultFinal = listResult.Select(x=>new BaoGiaResponse{
                                Id = x.Id,
                                MaBaoGia = x.MaBaoGia,
                                CreatedTime = x.CreatedTime,
                                UpdatedTime = x.UpdatedTime,
                                UserBaoGia  = x.UserBaoGia,
                                ListBaoGiaChiTietRes = dbContext.BaoGiaChiTiet.Where(z=>z.BaoGiaId==x.Id)
                                                                 .Select(x=>new BaoGiaChiTietResponse{
                                                                    Id = x.Id,
                                                                    NhaCungUngId = x.NhaCungUngId,
                                                                    TenNhaCungUng = x.TenNhaCungUng
                                                                 })
                                                                 .ToList()
                          })
                          .DistinctBy(x=>x.Id)
                          .OrderByDescending(x=>x.CreatedTime)
                          .ToList();
        return listResultFinal;
    }
}

