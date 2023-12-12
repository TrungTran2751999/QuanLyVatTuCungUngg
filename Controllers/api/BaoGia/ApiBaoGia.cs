using System.Runtime.CompilerServices;
using app.DTOs;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.Controllers;

[ApiController]
// [Authorize]
[Route("api/bao-gia")]
public class ApiBaoGia:Controller{
    private IBaogiaService baogiaService;
    private IVatTuBaoGiaChiTietService vatTuBaoGiaChiTietService;
    public ApiBaoGia(IBaogiaService baogiaService, IVatTuBaoGiaChiTietService vatTuBaoGiaChiTietService){
        this.baogiaService = baogiaService;
        this.vatTuBaoGiaChiTietService = vatTuBaoGiaChiTietService;
    }
    public async Task<IActionResult> GetAll(bool isDeleted){
        var listResult = await baogiaService.GetAll(isDeleted);
        return Ok(listResult);
    }
    [Route("detail")]
    public async Task<IActionResult> GetById(Guid id){
        var result = await baogiaService.GetById(id);
        return Ok(result); 
    }
    [HttpPost]
    [Route("save-bao-gia")]
    public async Task<ActionResult> SaveBaoGia([FromBody] BaoGiaCreateDTO baoGiaCreate){
        var result = await baogiaService.SaveBaoGia(baoGiaCreate);
        if(result=="OK"){
            return Ok(result);
        }else{
            return BadRequest(result);
        }
    }
    [HttpPost]
    [Route("lap-bao-gia")]
    public async Task<ActionResult> LapBaoGia([FromForm] string data){
        var result = await baogiaService.LapBaoGia(data);
        string name = "bao-gia"+"-"+DateTime.Now.ToString("dd")+"-"+DateTime.Now.ToString("MM")+"-"+DateTime.Now.ToString("yyyy")+"-"+DateTime.Now.ToString("hh")+"-"+DateTime.Now.ToString("mm")+"-"+DateTime.Now.ToString("ss");
        return File(result, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", name+".zip");

    }
    [HttpGet]
    [Route("chi-tiet-cap-nhat")]
    public async Task<ActionResult> ChiTietCapNhat(Guid id){
        var result = await vatTuBaoGiaChiTietService.GetByBaoGiaId(id);
        return Ok(result);
    }
}