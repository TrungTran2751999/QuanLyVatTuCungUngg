using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;
namespace app.Services;
public class NhomVatTuService : INhomVatTuService
{
    private readonly ApplicationDbContext dbContext;

    public NhomVatTuService(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }

    public async Task<List<NhomVatTu>?> GetAll()
    {
        var nhomVatTu = dbContext.NhomVatTu.ToListAsync();
        return await nhomVatTu;
    }

    public async Task<NhomVatTu?> GetById(long id)
    {
        var students = dbContext.NhomVatTu.FirstOrDefaultAsync(param=>param.Id==id);
        return await students;
    }
    // [HttpPost]
    // public async Task<bool?> Save(Student student)
    // {
    //     var maxInt = dbContext.Students.Max(x=>x.IdSystem);
    //     student.IdSystem = maxInt+1;
    //     await dbContext.Students.AddAsync(student);
    //     dbContext.SaveChanges();
    //     return true;
    // }
    // [HttpPut]

    // public async Task<bool?> Update(Student student)
    // {
    //     Console.WriteLine(student.IdSystem);
    //     var result = await dbContext.Students.FirstOrDefaultAsync(param=>param.IdSystem==student.IdSystem);
    //     if(result==null){
    //         return false;
    //     }
    //     Console.WriteLine(result);
    //     result.Name = student.Name;
    //     result.Email = student.Email;
    //     result.PhoneNumber = student.PhoneNumber;
    //     result.StudentCode = student.StudentCode;
    //     result.StudentGrade = student.StudentGrade;
    //     dbContext.SaveChanges();
    //     return true;
    // }
}