using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
namespace app.DTOs;

public class UsersDTO{
    public long Id {get;set;}
    public Guid? Guid{get;set;}
    public string? UserName{get;set;}
    public long DepartmentId{get;set;}
    [ForeignKey("DepartmentId")]
    public DepartmentsDTO? Department{get;set;}
}
