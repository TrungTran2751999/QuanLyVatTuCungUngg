using System.Drawing;
using System.Net;
using System.Text.Json.Serialization;
using app.DTOs;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace app.Controllers;

[ApiController]
[Authorize]
[Route("api/phieu-nhan-vat-tu")]
public class ApiPhieuNhanVatTuController:Controller{
    private IPhieuNVTService phieuNVTService;
    public ApiPhieuNhanVatTuController(IPhieuNVTService phieuNVTService){
        this.phieuNVTService = phieuNVTService;
    }
    public async Task<IActionResult> GetAll(bool isDeleted, int page = 0){
        var listPhieu = await phieuNVTService.GetAll(isDeleted, page);
        return Ok(listPhieu);
    }
    [Route("chi-tiet")]
    public async Task<IActionResult> GetByMaPhieu(string code, string codeYear){
        var listPhieu = await phieuNVTService.GetByMaPhieu(code, codeYear);
        return Ok(listPhieu);
    }
    [HttpPost]
    [Route("tong-hop")]
    public async Task<IActionResult> TongHopPhieu([FromBody] ParamTongHopPhieu paramTongHop){
        var listPhieuTongHop = await phieuNVTService.TongHopPhieu(paramTongHop.listId);
        return Ok(listPhieuTongHop);
    }
   
    [HttpPost]
    [Route("excel")]
    public ActionResult ExportFileExcel([FromForm] string data){
        List<PhieuDeNghiExcelParam> listParam = JsonConvert.DeserializeObject<List<PhieuDeNghiExcelParam>>(data);
        using(var excelPackage = new ExcelPackage()){
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
            worksheet.Cells[1, 1].Value = "Stt";
            worksheet.Cells[1, 2].Value = "Tên, nhãn hiệu, quy cách, phẩm chất vật tư, dụng cụ sản phẩm, hàng hóa";
            worksheet.Cells[1, 3].Value = "Mã VT, HH";
            worksheet.Cells[1, 4].Value = "Đvt";
            worksheet.Cells[1, 5].Value = "Số lượng";
            worksheet.Cells[1, 6].Value = "Đơn giá";
            worksheet.Cells[1, 7].Value = "Thành tiền";
            worksheet.Cells[1, 8].Value = "Ghi chú";

            ExcelRange title = worksheet.Cells["A1:Z1"];
            title.Style.Font.Bold = true;

            for(int j=1; j<=8; j++){
                worksheet.Cells[1, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[1, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                worksheet.Cells[1, j].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, j].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            }

            worksheet.Cells[2, 1].Value = "(A)";
            worksheet.Cells[2, 2].Value = "(B)";
            worksheet.Cells[2, 3].Value = "(C)";
            worksheet.Cells[2, 3].Value = "(D)";
            worksheet.Cells[2, 4].Value = "(1)";
            worksheet.Cells[2, 5].Value = "(2)";
            worksheet.Cells[2, 6].Value = "(3)";
            worksheet.Cells[2, 7].Value = "";

            for(int j=1; j<=8; j++){
                worksheet.Cells[2, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[2, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[2, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells[2, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                worksheet.Cells[2, j].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[2, j].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            }
            
            for(int i=0; i<listParam.Count; i++){
                worksheet.Cells[i + 3, 1].Value = i+1;
                worksheet.Cells[i + 3, 2].Value = listParam[i].TenVatTu;
                worksheet.Cells[i + 3, 3].Value = listParam[i].MaVatTu;
                worksheet.Cells[i + 3, 4].Value = listParam[i].DonViTinh;
                worksheet.Cells[i + 3, 5].Value = listParam[i].SoLuongPheDuyet;
                worksheet.Cells[i + 3, 6].Value = listParam[i].DonGia;
                worksheet.Cells[i + 3, 7].Value = listParam[i].ThanhTien;
                worksheet.Cells[i + 3, 8].Value = listParam[i].GhiChu;

                for(int j=1; j<=8; j++){
                    worksheet.Cells[i + 3, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[i + 3, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[i + 3, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    worksheet.Cells[i + 3, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
            };
            worksheet.Cells.AutoFitColumns();
            byte[] fileContenttt = excelPackage.GetAsByteArray();

            return File(fileContenttt, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "excel_file.xlsx");

        }
    }
    
    [HttpPost]
    [Route("filter")]
    public IActionResult Filter([FromBody] FilterParam filter){
        var result = phieuNVTService.Filter(filter);
        return Ok(result);
    }

    [HttpPost]
    [Route("filter-tong-hop")]
    public IActionResult FilterTongHop([FromBody] FilterParamTongHop filter){
        var result = phieuNVTService.FilterTongHop(filter);
        return Ok(result);
    }
}