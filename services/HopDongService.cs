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

    public byte[] XuatHopDong(string data)
    {
        XuatHopDongDTO xuatHopDongDTO = JsonSerializer.Deserialize<XuatHopDongDTO>(data);
        string hopDongPath = "./wwwroot/document/HopDongMuaBan.docx";
        string tenFileCopy = xuatHopDongDTO.TenFile;
        string hopDongXuat = "./wwwroot/document/"+tenFileCopy;

        File.Copy(hopDongPath,hopDongXuat);

        using(MemoryStream stream = new MemoryStream()){
            using (WordprocessingDocument document = WordprocessingDocument.Open(hopDongXuat, true))
            {
                // Truy cập phần tài liệu chính
                MainDocumentPart mainPart = document.MainDocumentPart;
                Document doc = mainPart.Document;
                foreach (Text textElement in document.MainDocumentPart.Document.Descendants<Text>())
                {
                    // Console.WriteLine(textElement.Text);
                    textElement.Text = textElement.Text.Replace("{NgayHopDong}", util.DoiNgayThangHienTai("KiHieuHopDong"));
                    textElement.Text = textElement.Text.Replace("{NgayThang}", util.DoiNgayThangHienTai("ChinhXac"));
                    textElement.Text = textElement.Text.Replace("{TongGiamDoc}", "Ông Dương Quý Dương");
                    textElement.Text = textElement.Text.Replace("{TaiKhoanHueWaco}", "5511 0000 000 370 tại NH Đầu tư và Phát triển T.T Huế");
                    textElement.Text = textElement.Text.Replace("{DaiDienNhaCungUng}", "Bà Nguyễn Thị Hường");
                    textElement.Text = textElement.Text.Replace("{ChucVuNhaCungUng}", "Giám đốc");
                    textElement.Text = textElement.Text.Replace("{DiaChiNhaCungUng}", "Số 4 Kiệt 272 Điện Biên Phủ, P.Trường An, TP Huế, Tỉnh Thừa Thiên Huế");
                    textElement.Text = textElement.Text.Replace("{DienThoaiNhaCungUng}", "0234.3839099-0905009055");
                    textElement.Text = textElement.Text.Replace("{TaiKhoanNhaCungUng}", "1019700320 tại ngân hàng Ngoại thương Việt Nam - CN Thừa Thiên Huế");
                    textElement.Text = textElement.Text.Replace("{MaSoThueNhaCungUng}", "3301629767");
                    
                }
                document.MainDocumentPart.Document.Save(stream);
            }
            byte[] result = File.ReadAllBytes(hopDongXuat);
            File.Delete(hopDongXuat);
            return result;
        }
    }
}

