using app.DTOs;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace app.Controllers;

[ApiController]
[Authorize]
[Route("api/phieu-nhan-vat-tu-da-duyet")]
public class ApiPhieuDeNghiNhanVatTuDaDuyet:Controller{
    private IPhieuDeNghiNhanVatTuDaPheDuyet phieuDaDuyet;
    private IPhieuNVTService phieuNVTService;
    public ApiPhieuDeNghiNhanVatTuDaDuyet(IPhieuDeNghiNhanVatTuDaPheDuyet phieuDaDuyet, IPhieuNVTService phieuNVTService){
        this.phieuDaDuyet = phieuDaDuyet;
        this.phieuNVTService = phieuNVTService;
    }
    public async Task<IActionResult> GetAll(bool isDeleted, int page){
        var listPhieu = await phieuNVTService.GetAll(isDeleted, page);
        return Ok(listPhieu);
    }
    [Route("chi-tiet")]
    public async Task<IActionResult> GetByMaPhieu(Guid id){
        var listPhieu = await phieuDaDuyet.GetById(id);
        return Ok(listPhieu);
    }
    [HttpPost]
    public async Task<IActionResult>PheDuyet([FromBody] List<PhieuDeNghiPheDuyetCreateDTO> phieuDeNghiPheDuyet){
        var result = await phieuDaDuyet.PheDuyet(phieuDeNghiPheDuyet);

        if(result!="OK") return BadRequest(result);

        return Ok(result);
    }
    [HttpDelete]
    public async Task<IActionResult>HuyPheDuyet(List<Guid> listIdPhieuPheDuyet){
        var result = await phieuDaDuyet.HuyPheDuyet(listIdPhieuPheDuyet);
        if(result!="OK") return BadRequest(result);
        return Ok(result);
    }
}