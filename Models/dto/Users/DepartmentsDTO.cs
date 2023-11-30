using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
namespace app.DTOs;

public class DepartmentsDTO{
    public long Id {get;set;}
    public Guid? Guid{get;set;}
    public string? DepartmentName {get;set;}
    
}
