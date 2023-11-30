using app.DTOs;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app.Controllers;

[ApiController]
[Authorize]
[Route("api/vat-tu-nha-cung-ung")]
public class ApiVatTuNhaCungUngRelation:Controller{
    private IVatTuNhaCungUngRelationService vatTuNhaCungUngRelationService;
    public ApiVatTuNhaCungUngRelation(IVatTuNhaCungUngRelationService vatTuNhaCungUngRelationService){
        this.vatTuNhaCungUngRelationService = vatTuNhaCungUngRelationService;
    }
    // [Authorize(Roles = "ADMIN,USER")]
    public async Task<IActionResult> GetAll(){
        var students = await vatTuNhaCungUngRelationService.GetAll();
        return Ok(students);
    }
    [Route("detail")]
    public async Task<IActionResult> GetById(long idNhaCung, bool isDeleted){
        var vatTu = await vatTuNhaCungUngRelationService.GetByNhaCungUng(idNhaCung,isDeleted);
        return Ok(vatTu);
    }
    [Route("max-id")]
    public async Task<IActionResult> GetMaxId(){
        var idMax = await vatTuNhaCungUngRelationService.GetMaxId();
        return Ok(idMax);
    }
    [HttpPost]
    public async Task<IActionResult> AddVatTuToNhaCungUng([FromBody] VatTuNhaCungUngAddingDTO vatTuAdding){
        var vatTu = await vatTuNhaCungUngRelationService.AddVatTuToNhaCungUng(vatTuAdding);
        if(vatTu.result != "OK"){
            return BadRequest(vatTu);
        }else{
           return Ok(vatTu);
        }
    }
    [HttpDelete]
    public async Task<IActionResult> RemoveVatTuFromNhaCungUng(long id){
        var result = await vatTuNhaCungUngRelationService.RemoveVatTuFromNhaCungUng(id);
        if(result != "OK"){
            return BadRequest(result);
        }else{
            return Ok(result);
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateVatTuNhaCungUng([FromBody] VatTuNhaCungUngAddingDTO vatTuNhaCungUngRemove){
        var result = await vatTuNhaCungUngRelationService.UpdateVatTuNhaCungUng(vatTuNhaCungUngRemove);
        if(result.result != "OK"){
            return BadRequest(result);
        }else{
            return Ok(result);
        }
    }

}