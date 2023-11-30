using app.Models;
using app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app.Controllers;

[ApiController]
[Authorize]
[Route("api/nhom-vat-tu")]
public class ApiStudentController:Controller{
    private INhomVatTuService nhomVatTuService;
    public ApiStudentController(INhomVatTuService nhomVatTuService){
        this.nhomVatTuService = nhomVatTuService;
    }
    // [Authorize(Roles = "ADMIN,USER")]
    public async Task<IActionResult> GetAll(){
        var listNhomVatTu = await nhomVatTuService.GetAll();
        return Ok(listNhomVatTu);
    }
    [Route("{id}")]
    public async Task<IActionResult> GetById(long id){
        var vatTu = await nhomVatTuService.GetById(id);
        return Ok(vatTu);
    }

    // [Authorize(Roles = "ADMIN,USER")]
    // [HttpPost]
    // public async Task<IActionResult> Create([FromBody] Student student){
    //     var result = await studentService.Save(student);
    //     return Ok(result);
    // }

    // [HttpPut]
    // // [Authorize(Roles = "ADMIN")]
    // public async Task<IActionResult> Update([FromBody] Student student){
    //     var result = await studentService.Update(student);
    //     if(result==false){
    //         return BadRequest("NOT");
    //     }
    //     return Ok(result);
    // }
}