using app.DTOs;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace app.Controllers;

[ApiController]
[Authorize]
[Route("api/nha-cung-ung")]
public class ApiNhaCungUngController:Controller{
    private INhaCungUngService NhaCungUngService;
    public ApiNhaCungUngController(INhaCungUngService NhaCungUngService){
        this.NhaCungUngService = NhaCungUngService;
    }
    public async Task<IActionResult> GetAll(bool isDeleted, int index){
        var nhaCungUng = await NhaCungUngService.GetAll(isDeleted, index);
        return Ok(nhaCungUng);
    }
    [Route("{id}")]
    public async Task<IActionResult> GetById(long id, bool isDeleted){
        var nhaCungUng = await NhaCungUngService.GetById(id, isDeleted);
        return Ok(nhaCungUng);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NhaCungUngCreateDTO nhaCungUng){
        var result = await NhaCungUngService.Save(nhaCungUng);
        if(result!="OK"){
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] NhaCungUngUpdateDTO nhaCungUng){
        var result = await NhaCungUngService.Update(nhaCungUng);
        if(result!="OK"){
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long id, long userId){
        var result = await NhaCungUngService.SoftDelete(id, userId);
        if(result!="OK"){
            return BadRequest(result);
        }
        return Ok(result);
    }
    [Route("restore")]
    [HttpPut]
    public async Task<IActionResult> Restore(long id, long userId){
        var result = await NhaCungUngService.Restore(id, userId);
        if(result!="OK"){
            return BadRequest(result);
        }
        return Ok(result);
    }

}