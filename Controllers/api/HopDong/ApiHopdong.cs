using System.Runtime.CompilerServices;
using System.Text.Json;
using app.DTOs;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.IO;
namespace app.Controllers;

[ApiController]
[Route("api/hop-dong")]
public class ApiHopdong:Controller{
    private IHopdongSerVice hopDongSerVice;
    private IVatTuBaoGiaChiTietService vatTuBaoGiaChiTietService;
    public ApiHopdong(IHopdongSerVice baogiaService, IVatTuBaoGiaChiTietService vatTuBaoGiaChiTietService){
        this.hopDongSerVice = baogiaService;
        this.vatTuBaoGiaChiTietService = vatTuBaoGiaChiTietService;
    }
    [HttpPost]
    public ActionResult XuatHopDong([FromForm] string data){
        CreateHopDongDTO createHopDongDTO = JsonSerializer.Deserialize<CreateHopDongDTO>(data);
        var result = hopDongSerVice.XuatHopDong(createHopDongDTO);
        
        return File(result, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", createHopDongDTO?.TenFile+".docx"); 
    }
}