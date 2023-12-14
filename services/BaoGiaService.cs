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

// using System.Data.Entity;
namespace app.Services;
public class BaoGiaService : IBaogiaService
{
    private readonly ApplicationDbContext dbContext;

    public BaoGiaService(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }

    public async Task<List<BaoGiaResponse>?> GetAll(bool isDeleted)
    {
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
                               .Skip(0).Take(10)
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
        var checkListVatTu = await dbContext.VatTu
                                            .Where(x=>listVatTuId.Contains(x.Id))
                                            .ToListAsync();
        if(listNhaCungUngId.Count != checkListNhaCungUng.Count) return "NhaCungUng:Tồn tại nhà cung ứng không thuộc danh sách";
        
        var listBaoGiaId = baoGiaCreate.ListPhieuPheDuyet;

        using(var transaction = dbContext.Database.BeginTransaction()){
            try{
                //check xem phieu de nghi da duoc lap bao gia chua
                // await XoaBaoGia(listBaoGiaId);
                //add du lieu vao bang bao gia
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
                        var phieuNhanVatTus = await dbContext.PhieuDeNghiNhanVatTuDaDuyet.Where(x=>x.Id==ids).FirstOrDefaultAsync();
                        phieuNhanVatTus.BaoGiaId = null;
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
            MemoryStream stream = await ExportBaoGiaToExcel(baoGia.ListVatTu);
            listMemoryStream.Add(stream);
        }
        exportFile.files = listMemoryStream;
        List<string> names = new List<string>();
        foreach(var nhaCungUng in listVatTu){
            names.Add(nhaCungUng.TenNhaCungUng);
        }
        exportFile.names = names;
        return ZipFile(exportFile);
    }
    private byte[] ZipFile(ExportFile exportFile){
        using(MemoryStream memoryStream = new MemoryStream()){
            using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                for(int i=0; i<exportFile.names.Count; i++){
                    // Tạo một đối tượng Entry trong tệp ZIP
                    ZipArchiveEntry entry = zipArchive.CreateEntry(exportFile.names[i]+".xlsx");

                    // Ghi dữ liệu tài liệu vào Entry
                    using (Stream entryStream = entry.Open())
                    {
                        entryStream.Write(exportFile.files[i].ToArray(), 0, exportFile.files[i].ToArray().Length);
                    }
                }
            }
            return memoryStream.ToArray();
        }
    }
    //export ra file excel
    private async Task<MemoryStream> ExportBaoGiaToExcel(List<VatTuInBaoGia> listVatTu){
         using(var excelPackage = new EXCEL.ExcelPackage()){
            EXCEL.ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
            worksheet.Cells[1, 1].Value = "Stt";
            worksheet.Cells[1, 2].Value = "Tên vật tư, hàng hóa";
            worksheet.Cells[1, 3].Value = "Đơn vị tính";
            worksheet.Cells[1, 4].Value = "Yêu cầu kĩ thuật, quy cách, chất lượng";
            worksheet.Cells[1, 5].Value = "Số lượng";
            worksheet.Cells[1, 6].Value = "Đơn giá";
            worksheet.Cells[1, 7].Value = "Thành tiền";
            worksheet.Cells[1, 8].Value = "Ghi chú";

            EXCEL.ExcelRange title = worksheet.Cells["A1:Z1"];
            title.Style.Font.Bold = true;

            for(int j=1; j<=8; j++){
                worksheet.Cells[1, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            for(int i=0; i<listVatTu.Count; i++){
                worksheet.Cells[i + 2, 1].Value = i+1;
                worksheet.Cells[i + 2, 2].Value = listVatTu[i].TenVatTu;
                worksheet.Cells[i + 2, 3].Value = listVatTu[i].DonViTinh;
                worksheet.Cells[i + 2, 4].Value = listVatTu[i].YeuCauKiThuat;
                worksheet.Cells[i + 2, 5].Value = listVatTu[i].SoLuong;
                worksheet.Cells[i + 2, 6].Value = "";
                worksheet.Cells[i + 2, 7].Value = "";
                worksheet.Cells[i + 2, 8].Value = listVatTu[i].GhiChu;


                for(int j=1; j<=8; j++){
                    worksheet.Cells[i + 2, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[i + 2, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[i + 2, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[i + 2, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
            };
            worksheet.Cells.AutoFitColumns();
            MemoryStream stream = new MemoryStream(excelPackage.GetAsByteArray());
            return stream;
         }
    }
    //export ra file word
    private async Task<MemoryStream> ExportBaoGiaToWord(List<VatTuInBaoGia> listVatTu){
        MemoryStream stream = new MemoryStream();
        using (WordprocessingDocument document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
        {
            MainDocumentPart mainPart = document.AddMainDocumentPart();
            mainPart.Document = new Document();
            Body body = mainPart.Document.AppendChild(new Body());
            //in tile cong hoa xa hoi chu nghia viet nam
            body.Append(CreateTitle());
            
            //in hue ngay thang nam
            string hueNgayThangNam = "Huế, ngày "+DateTime.Now.ToString("dd")+" tháng "+DateTime.Now.ToString("MM")+" năm "+DateTime.Now.ToString("yyyy");
            Paragraph hueNgayThangNamePara = InDoanVan(hueNgayThangNam, "italic", "right" ,null, 14);
            body.Append(hueNgayThangNamePara);

            //in THU MOI CHAO GIA
            Paragraph thuMoichaoGia = InDoanVan("THƯ MỜI CHÀO GIÁ", "bold", "center" ,null, 17);
            body.Append(thuMoichaoGia);

            //in kinh gui cong ty hop tac
            Paragraph kinhGui = InDoanVan("Kính gửi:","bold","center",null,14);
            Paragraph congTyHopTac = InDoanVan("Quý đối tác",null,"center",null,14);
            Paragraph kinhGuiCongTyDoiTac = KetHopParagraph(new List<Paragraph>(){kinhGui, congTyHopTac}, "center","n");
            body.Append(kinhGuiCongTyDoiTac);

            //in Công ty Cổ phần Cấp nước Thừa Thiên Huế tổ chức mua sắm hàng hóa
            Paragraph congTyTochucMuaSam = InDoanVan("Công ty Cổ phần Cấp nước Thừa Thiên Huế tổ chức mua sắm hàng hóa", null, "center", null, 14);
            body.Append(congTyTochucMuaSam);

            //tao khoang trang de nguoi dung dien vao
            body.Append(TaoTabDot(168, "dots"));
            body.Append(TaoTabDot(168, "dots"));

            //in Nội dung chính gói mua sắm
            Paragraph tabs1 = TaoTabDot(12, "none");
            Paragraph noiDungChinhGoiMuaSam = InDoanVan("Nội dung chính gói mua sắm:", null, null, null, 14);
            Paragraph tabs1CombinenoiDungChinhGoiMuaSam = KetHopParagraph(new List<Paragraph>(){tabs1, noiDungChinhGoiMuaSam}, "left","");
            body.Append(tabs1CombinenoiDungChinhGoiMuaSam);

            //tao khoang trang de nguoi dung dien vao
            body.Append(TaoTabDot(168, "dots"));
            body.Append(TaoTabDot(168, "dots"));
            
            //in Nội dung chính gói mua sắm
            Paragraph tabs2 = TaoTabDot(12, "none");
            Paragraph noiDungYeuCau = InDoanVan("Nội dung yêu cầu:", null, null, null, 14);
            Paragraph tabs2CombineNoiDungYeuCau = KetHopParagraph(new List<Paragraph>(){tabs2, noiDungYeuCau}, "left", "");
            body.Append(tabs2CombineNoiDungYeuCau);

            //tao khoang trang de nguoi dung dien vao
            body.Append(TaoTabDot(168, "dots"));
            body.Append(TaoTabDot(168, "dots"));

            //yeu cau hang hoa
            Paragraph tabs3 = TaoTabDot(12, "none");
            Paragraph yeuCauHangHoa = InDoanVan("1.Yêu cầu hàng hóa:", null, null, null, 14);
            Paragraph tabs3CombineNoiDungYeuCau = KetHopParagraph(new List<Paragraph>(){tabs3, yeuCauHangHoa}, "left", "");
            body.Append(tabs3CombineNoiDungYeuCau);
            
            //bang mua hang
            List<string> headers = new List<string>(){"STT","Tên vật tư, hàng hóa", "Đơn vị tính", "Yêu cầu kĩ thuật, quy cách, chất lượng","Xuất xứ","Số lượng","Ghi chú"};
            Table table = TaoBang(headers, listVatTu);
            body.Append(table);

            //in "du kien thoi gian thuc hien"
            Paragraph tabs4 = TaoTabDot(12, "none");
            Paragraph duKienThoiGianThucHien = InDoanVan("Dự kiến thời gian thực hiện:", null, null, null, 14);
            Paragraph tab4CombineduKienThoiGian = KetHopParagraph(new List<Paragraph>(){tabs4, duKienThoiGianThucHien}, "left", "");
            body.Append(tab4CombineduKienThoiGian);

            //in "2.Yeu cau ve gia"
            Paragraph tabs5 = TaoTabDot(12, "none");
            Paragraph yeuCauVeGia = InDoanVan("2. Yêu cầu về giá: Giá nhà cung cấp chào bao gồm đầy đủ các chi phí: Thuế VAT, chi phí thiết bị, nhân công, vận chuyển, lắp đặt, các chi phí khác (nếu có)...", null, null, null, 14);
            Paragraph tab5CombineYeuCauVeGia = KetHopParagraph(new List<Paragraph>(){tabs5, yeuCauVeGia}, "left", "");
            body.Append(tab5CombineYeuCauVeGia);

            //in "3. Các yêu cầu khác"
            Paragraph tabs6 = TaoTabDot(12, "none");
            Paragraph yeuCauKhac = InDoanVan("3. Các yêu cầu khác: Hàng mới 100% chưa qua sử dụng (hay cũ), địa điểm giao hàng, tiến độ giao hàng, yêu cầu bảo hành, thời hạn thanh toán, hình thức thanh toán hồ sơ chứng từ, các yêu cầu khác.", null, null, null, 14);
            Paragraph tab6CombineyeuCauKhac = KetHopParagraph(new List<Paragraph>(){tabs6, yeuCauKhac}, "left", "");
            body.Append(tab6CombineyeuCauKhac);

            //in "4. Địa điểm thời gian nhận hồ sơ báo giá"
            Paragraph tabs7 = TaoTabDot(12, "none");
            Paragraph diaDiemThoiGian = InDoanVan("4. Địa điểm, thời gian nhận hồ sơ báo giá:", null, null, null, 14);
            Paragraph tabs8 = TaoTabDot(12, "dots");
            Paragraph tab7CombineDiaDiemThoiGian = KetHopParagraph(new List<Paragraph>(){tabs7, diaDiemThoiGian, tabs8}, "left", "");
            body.Append(tab7CombineDiaDiemThoiGian);

            //in ĐƠN VỊ MỜI CHÀO GIÁ
            Paragraph chuKiDonViMoiChaoGia = InDoanVan("ĐƠN VỊ MỜI CHÀO GIÁ", "bold", "right", null, 14);
            body.Append(chuKiDonViMoiChaoGia);
            
            mainPart.Document.Save();
        }
        stream.Position = 0;
        return stream;
    }
    private Table TaoBang(List<string> headers, List<VatTuInBaoGia> listVatTu){
        Table table = new Table();
        TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
        table.AppendChild(tableWidth);
        TableRow headerRow = new TableRow();
        //in header cua bang
        foreach(string header in headers){
            Paragraph paragraph = InDoanVan(header, "bold", "center", null, 14);
            TableCell headerCell = CreateTableCellWithBorders(paragraph);
            headerRow.Append(headerCell);
        }
        table.Append(headerRow);
        //in body cua bang
        for(int i=0; i<listVatTu.Count; i++){
            TableRow bodyRow = new TableRow();

            Paragraph stt = InDoanVan((i+1).ToString(), "null", "center", null, 14);
            TableCell cellStt = CreateTableCellWithBorders(stt);

            Paragraph tenVatTu = InDoanVan(listVatTu[i].TenVatTu, null, "center", null, 14);
            TableCell cellTenVatTu = CreateTableCellWithBorders(tenVatTu);

            Paragraph donViTinh = InDoanVan(listVatTu[i].DonViTinh, null, "center", null, 14);
            TableCell cellDonViTinh = CreateTableCellWithBorders(donViTinh);

            Paragraph yeucauKiThuat = InDoanVan(listVatTu[i].YeuCauKiThuat, null, "center", null, 14);
            TableCell cellYeucauKiThuat = CreateTableCellWithBorders(yeucauKiThuat);
            
            Paragraph xuatXu = InDoanVan(listVatTu[i].XuatXu, null, "center", null, 14);
            TableCell cellXuatXu = CreateTableCellWithBorders(xuatXu);

            Paragraph soLuong = InDoanVan(listVatTu[i].SoLuong.ToString(), null, "center", null, 14);
            TableCell cellSoLuong = CreateTableCellWithBorders(soLuong);

            Paragraph ghiChu = InDoanVan(listVatTu[i].GhiChu.ToString(), null, "center", null, 14);
            TableCell cellGhiChu = CreateTableCellWithBorders(ghiChu);

            bodyRow.Append(cellStt, cellTenVatTu, cellDonViTinh, cellYeucauKiThuat, cellXuatXu, cellSoLuong, cellGhiChu);
            table.Append(bodyRow);
        }
        return table;
    }
    private Paragraph TaoTabDot(int count, string type){
        string dots = "";
        for(int i=1; i<=count; i++){
            dots+=".";
        }
        Paragraph paragraph = new Paragraph();
        switch (type){
            case "none":
                Run run = new Run();
                RunProperties runProperties = new RunProperties();
                run.AppendChild(new Text(dots));
                runProperties.AppendChild(new Color(){Val="#FFFFFF"});
                run.RunProperties = runProperties;

                paragraph.AppendChild(run);
                break;
            case "dots":
                paragraph.Append(new Run(new Text(dots)));
                break;
            default:
                break;
        }
        
        return paragraph;
    }
    private Paragraph KetHopParagraph(List<Paragraph> listParagraph, string canLe, string space){
        Paragraph result = new Paragraph();
        ParagraphProperties paragraphProperties = new ParagraphProperties();
        int i=0; 
        foreach(Paragraph paragraph in listParagraph){
            foreach(Run run in paragraph.Elements<Run>()){
                Run newrun = (Run)run.CloneNode(true);
                result.AppendChild(newrun);
            }
            result.AppendChild(paragraph);
            //set properties space

            Run runSpace = new Run(new Text(space));
            RunProperties runSpaceProperties = new RunProperties();
            runSpaceProperties.AppendChild(new Color(){Val="#FFFFFF"});
            runSpace.RunProperties = runSpaceProperties;
            if(i!=listParagraph.Count-1){
                result.AppendChild(runSpace);
            };
            i++;
        }
        Justification justification = new Justification();
        switch(canLe){
            case "left":
                justification.Val = JustificationValues.Left;
                paragraphProperties.Append(justification);
                break;
            case "center":
                justification.Val = JustificationValues.Center;
                paragraphProperties.Append(justification);
                break;
            case "right":
                justification.Val = JustificationValues.Right;
                paragraphProperties.Append(justification);
                break;
            default:
                break;
        }
         foreach(Paragraph paragraph in listParagraph){
            paragraph.Remove();
         }
        result.ParagraphProperties = paragraphProperties;
        return result;

    }
    private Run TaoKhoangTrang(string space){
        Run run = new Run();
        Text text = new Text(space);
        RunProperties runProperties = new RunProperties(new Color(){Val="#FFFFFF"});
        run.RunProperties = runProperties;
        run.Append(text);
        return run;
    }
    private Paragraph InDoanVan(string str, string fontStyle, string canLe, string fontFamily, int fontSize){
        Paragraph paragraph = new Paragraph();
        Run run = new Run();
        Text text = new Text(str);
        RunProperties runProperties = new RunProperties();
        ParagraphProperties paragraphProperties = new ParagraphProperties();
        
        //set font chu 
        if(fontFamily==null){
            RunFonts runFonts = new RunFonts(){Ascii= "Times New Roman", HighAnsi="Times New Roman"};
            runProperties.Append(runFonts);
        }else{
            RunFonts runFonts = new RunFonts(){Ascii = fontFamily, HighAnsi = fontFamily};
            runProperties.Append(runFonts);
        }

        //set font size
        FontSize size = new FontSize();
        size.Val = (fontSize*2).ToString();
        runProperties.Append(size);

        //set font style
        switch (fontStyle){
            case "bold":
                Bold bold = new Bold();
                runProperties.Append(bold);
                break;
            case "italic":
                Italic italic = new Italic();
                runProperties.Append(italic);
                break;
            default:
                break;
        }

        //set can le
        Justification justification = new Justification();
        switch(canLe){
            case "left":
                justification.Val = JustificationValues.Left;
                paragraphProperties.Append(justification);
                break;
            case "center":
                justification.Val = JustificationValues.Center;
                paragraphProperties.Append(justification);
                break;
            case "right":
                justification.Val = JustificationValues.Right;
                paragraphProperties.Append(justification);
                break;
            default:
                break;
        }
        run.RunProperties = runProperties;
        paragraph.ParagraphProperties = paragraphProperties;
        run.Append(text);
        paragraph.Append(run);
        return paragraph;

    }
    private Table CreateTitle(){
         Table table = new Table();
         TableWidth tableWidth = new TableWidth() { Width = "5500", Type = TableWidthUnitValues.Pct };
         table.AppendChild(tableWidth);
         
         TableRow tableRow = new TableRow();
         //cell 1 chua CONG TY CO PHAN CAP NUOC - THUA THIEN HUE
         Paragraph tenCongTy = InDamVaCanGiua("CÔNG TY CỔ PHẦN CẤP NƯỚC", 13);
         Paragraph thuaThienHue = InDamVaCanGiua("THỪA THIÊN HUẾ", 13);
         Paragraph cachDeu1 = InDamVaCanGiua("_______________", 13);
         Paragraph cachDeu2 = InDamVaCanGiua("_______________",13);
         Paragraph quocNgu1 = InDamVaCanGiua("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM", 13);
         Paragraph quocNgu2 = InDamVaCanGiua("ĐỘC LẬP - TỰ DO – HẠNH PHÚC", 13);
        
         TableCell cellTenCongTy = new TableCell(tenCongTy, thuaThienHue, cachDeu1);
         TableCell cellTinh = new TableCell(quocNgu1, quocNgu2, cachDeu2);
         tableRow.Append(cellTenCongTy, cellTinh);
         table.Append(tableRow);
         return table;

    }
    private Paragraph InDamVaCanGiua(string text, int size){
         Paragraph paragraph = new Paragraph();
         ParagraphProperties paragraphProperties = new ParagraphProperties();
         Run run = new Run();
         RunProperties runProperties = new RunProperties();
         Text textTenCongTy = new Text(text);
         
         run.Append(textTenCongTy);
         //in dam
         Bold bold = new Bold();
         //dieu chinh font
         RunFonts fonts = new RunFonts(){HighAnsi = "Times New Roman", Ascii="Times New Roman"};
         //dieu chinh size
         FontSize fontSize = new FontSize();
         fontSize.Val = (size*2).ToString();
         runProperties.Append(bold, fonts, fontSize);
         run.RunProperties = runProperties;

         //can giua
         Justification justification = new Justification();
         justification.Val = JustificationValues.Center; // Căn giữa
         paragraphProperties.AppendChild(justification);

         paragraph.ParagraphProperties = paragraphProperties;
         paragraph.AppendChild(run);
         return paragraph;
    }
    private void Template(Body body){
         Paragraph paragraph = new Paragraph();
            Run run = new Run();

            // Tạo đối tượng Text và đặt nội dung
            Text text = new Text("Inserted text");

            // Chèn đối tượng Text vào trong Run
            run.AppendChild(text);

            // Chèn đối tượng Run vào trong Paragraph
            paragraph.AppendChild(run);
            
            RunProperties runProperties = new RunProperties();

            // Đặt thuộc tính FontSize
            FontSize fontSize = new FontSize();
            fontSize.Val = "28"; // Kích cỡ font 24pt
            runProperties.AppendChild(fontSize);

            // Đặt thuộc tính Bold
            Bold bold = new Bold();
            runProperties.AppendChild(bold);

            // Đặt thuộc tính Color
            Color color = new Color();
            color.Val = "FF0000"; // Màu đỏ
            runProperties.AppendChild(color);

            RunFonts runFonts = new RunFonts() { Ascii = "Times New Roman" };

            // Áp dụng đối tượng RunProperties vào trong Run
            run.RunProperties = runProperties;

            ParagraphProperties paragraphProperties = new ParagraphProperties();

            // Tạo đối tượng Justification để căn lề đoạn văn bản
            Justification justification = new Justification();
            justification.Val = JustificationValues.Center; // Căn giữa
            paragraphProperties.AppendChild(justification);

            // Áp dụng đối tượng ParagraphProperties vào trong Paragraph
            paragraph.ParagraphProperties = paragraphProperties;


            // Chèn đối tượng Paragraph vào trong Body
            body.AppendChild(paragraph);

            Table table = new Table();
            TableWidth tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
            table.AppendChild(tableWidth);

            // Tạo các đối tượng TableRow và TableCell để chèn vào bảng
            // Tạo các đối tượng TableRow và TableCell để chèn vào bảng
            // TableRow headerRow = new TableRow();
            // TableCell headerCell1 = CreateTableCellWithBorders("Header 1");
            // TableCell headerCell2 = CreateTableCellWithBorders("Header 2");
            // headerRow.Append(headerCell1, headerCell2);

            // TableRow dataRow1 = new TableRow();
            // TableCell dataCell1 = CreateTableCellWithBorders("Data 1");
            // TableCell dataCell2 = CreateTableCellWithBorders("Data 2");
            // dataRow1.Append(dataCell1, dataCell2);

            // TableRow dataRow2 = new TableRow();
            // TableCell dataCell3 = CreateTableCellWithBorders("Data 3");
            // TableCell dataCell4 = CreateTableCellWithBorders("Data 4");
            // dataRow2.Append(dataCell3, dataCell4);

            // // Chèn các đối tượng TableRow vào trong Table
            // table.Append(headerRow, dataRow1, dataRow2);
            // body.AppendChild(table);
    }
    private TableCell CreateTableCellWithBorders(Paragraph paragraph)
    {
        // Paragraph paragraph = new Paragraph();
        // Run run = new Run();
        // RunProperties runProperties = new RunProperties();

        // Bold bold = new Bold();

        // RunFonts runFonts = new RunFonts() { Ascii = "Times New Roman" };
        // runProperties.Append(runFonts, bold);

        // run.RunProperties = runProperties;
        // run.Append(new Text(text));

        // ParagraphProperties paragraphProperties = new ParagraphProperties();
        // Justification justification = new Justification();
        // justification.Val = JustificationValues.Center; // Căn giữa
        // paragraphProperties.AppendChild(justification);

        // paragraph.Append(run);

        // // Áp dụng đối tượng ParagraphProperties vào trong Paragraph
        // paragraph.ParagraphProperties = paragraphProperties;

        TableCell cell = new TableCell(paragraph);

        // Tạo đối tượng TableCellProperties để định dạng kiểu dáng ô
        TableCellProperties cellProperties = new TableCellProperties();

        // Tạo đối tượng TableCellBorders để định dạng biên của ô
        TableCellBorders cellBorders = new TableCellBorders();

        // Đặt thuộc tính BorderValues cho các biên
        cellBorders.TopBorder = new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 };
        cellBorders.BottomBorder = new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 };
        cellBorders.LeftBorder = new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 };
        cellBorders.RightBorder = new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 4 };

        // Áp dụng đối tượng TableCellBorders vào trong TableCellProperties
        cellProperties.AppendChild(cellBorders);

        // Áp dụng đối tượng TableCellProperties vào trong TableCell
        cell.AppendChild(cellProperties);

        return cell;
    }

}
class ExportFile{
    public List<MemoryStream>? files;
    public List<string>? names;
}
