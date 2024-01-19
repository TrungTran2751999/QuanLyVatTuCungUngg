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
using app.Utils;
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
    [HttpGet]
    public ActionResult GetAll(int pageNumber=1){
        var result = hopDongSerVice.GetAll(pageNumber);
        return Ok(result);
    }
    [HttpGet]
    [Route("detail")]
    public ActionResult GetById(Guid id){
        var result = hopDongSerVice.GetById(id);
        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult> LapHopDong([FromBody] CreateHopDongDTO createHopDongDTO){
        var result = await hopDongSerVice.LuuHopDong(createHopDongDTO);
        if(result==null) return BadRequest(result);
        return Ok(result);
    }
    [HttpPost]
    [Route("export")]
    public ActionResult XuatHopDong([FromForm] string data){
        CreateHopDongDTO createHopDongDTO = JsonSerializer.Deserialize<CreateHopDongDTO>(data);
        var result = hopDongSerVice.XuatHopDong(createHopDongDTO);
        return File(result.file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", result.tenFile+".docx"); 
    }
    [HttpPost]
    [Route("update")]
    public async Task<ActionResult> CapNhatHopDong([FromBody] CreateHopDongDTO createHopDongDTO){
        var result = await hopDongSerVice.CapNhatHopDong(createHopDongDTO);
        if(result!="OK") return BadRequest(result);
        return Ok(result);
    }
    [HttpPost]
    [Route("filter")]
    public async Task<ActionResult> Filter([FromBody] HopDongFilter hopDongFilter){
        var result = await hopDongSerVice.FilterHopDong(hopDongFilter);
        return Ok(result);
    }
}