using app.DTOs;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace app.Controllers;

[ApiController]
[Authorize]
[Route("api/vat-tu")]
public class ApiVatTuController:Controller{
    private IVatTuService vatTuService;
    public ApiVatTuController(IVatTuService vatTuService){
        this.vatTuService = vatTuService;
    }
    public async Task<IActionResult> GetAll(int index){
        var listPhieu = await vatTuService.GetAll(index);
        return Ok(listPhieu);
    }
    [Route("nha-cung-ung")]
    public async Task<IActionResult> GetNhaVatTuByVatTu(long idVatTu){
        var listNhaCungUng = await vatTuService.GetNhaCungUngByVatTu(idVatTu);
        return Ok(listNhaCungUng);
    }
    [Route("search")]
    public async Task<IActionResult> Search(string hint, int index){
        var listResult = await vatTuService.Search(hint,index);
        return Ok(listResult);
    }
    
}