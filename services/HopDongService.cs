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
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

// using System.Data.Entity;
namespace app.Services;
public class HopDongService : IHopdongSerVice
{
    private readonly ApplicationDbContext dbContext;
    private IUtil util;

    public HopDongService(ApplicationDbContext dbContext, IUtil util){
        this.dbContext = dbContext;
        this.util = util;
    }
    public byte[] XuatHopDong(CreateHopDongDTO xuatHopDongDTO)
    {
        string hopDongPath = "./wwwroot/document/HopDongMuaBan.docx";
        string tenFileCopy = xuatHopDongDTO.TenFile;
        string hopDongXuat = "./wwwroot/document/"+tenFileCopy;

        File.Copy(hopDongPath,hopDongXuat);

        using(MemoryStream stream = new MemoryStream()){
            using (WordprocessingDocument document = WordprocessingDocument.Open(hopDongXuat, true))
            {
                // Truy cập phần tài liệu chính
                MainDocumentPart mainPart = document.MainDocumentPart;
                Body doc = mainPart.Document.Body;
                string tableHopDong = "bangmuahang";
                // Tạo hàng mới
                TableRow tableRow = new TableRow();

                // var infoNhaCungUng = dbContext.NhaCungUng.
                
                foreach (Text textElement in document.MainDocumentPart.Document.Descendants<Text>())
                {
                    // Console.WriteLine(textElement.Text);
                    textElement.Text = textElement.Text.Replace("{NgayHopDong}", util.DoiNgayThangHienTai("KiHieuHopDong"));
                    textElement.Text = textElement.Text.Replace("{NgayThang}", util.DoiNgayThangHienTai("ChinhXac"));
                    textElement.Text = textElement.Text.Replace("{TongGiamDoc}", xuatHopDongDTO.DaiDienBenA);
                    textElement.Text = textElement.Text.Replace("{TaiKhoanHueWaco}", xuatHopDongDTO.TaiKhoanBenA);
                    textElement.Text = textElement.Text.Replace("{DaiDienNhaCungUng}", xuatHopDongDTO.DaiDienNhaCungUng);
                    textElement.Text = textElement.Text.Replace("{ChucVuNhaCungUng}", xuatHopDongDTO.ChucVuNhaCungUng);
                    textElement.Text = textElement.Text.Replace("{DiaChiNhaCungUng}", xuatHopDongDTO.DiaChiNhaCungUng);
                    textElement.Text = textElement.Text.Replace("{DienThoaiNhaCungUng}", xuatHopDongDTO.DienThoaiNhaCungUng);
                    textElement.Text = textElement.Text.Replace("{TaiKhoanNhaCungUng}", xuatHopDongDTO.TaiKhoanNhaCungUng);
                    textElement.Text = textElement.Text.Replace("{MaSoThueNhaCungUng}", xuatHopDongDTO.MaSoThueNhaCungUng);
                }
                //them bang mua hang vao table
                BookmarkStart bookmarkStart = doc.Descendants<BookmarkStart>().FirstOrDefault(b => b.Name == tableHopDong);
                if (bookmarkStart != null){
                    Table targetTable = bookmarkStart.Ancestors<Table>().FirstOrDefault();
                    if (targetTable != null){
                        List<TableRow> listRow = util.TaoBangHopDong(xuatHopDongDTO.ListHang);
                        foreach(TableRow row in listRow){
                            targetTable.Append(row);
                        }
                    }
                }
                document.MainDocumentPart.Document.Save(stream);
            }
            byte[] result = File.ReadAllBytes(hopDongXuat);
            File.Delete(hopDongXuat);
            return result;
        }
    }
    public string LuuHopDong(CreateHopDongDTO createHopDongDTO)
    {
        using(var transaction = dbContext.Database.BeginTransaction()){
            try{
                var hopDong = createHopDongDTO.ToModel();
                
                transaction.Commit();
            }catch(Exception e){
                Console.WriteLine(e);
                transaction.Rollback();
            }
        }
    }
}

