using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("Users")]
public class Users{
    [Key]
    [Column("Id")]
    public long Id{get;set;}

    [Column("UserName")]
    public string? Name{get;set;}
    [Column("Password")]
    public string? Password{get;set;}
    [Column("DepartmentId")]
    public long DepartmentId{get;set;}

    [ForeignKey("DepartmentId")]
    public Departments? Department{get;set;}
    [Column("MainDepartmentId")]
    public long MainDepartmentId{get;set;}

    [Column("Role")]
    public int? Role{get;set;}
    [Column("FullName")]
    public string? FullName{get;set;}
    [Column("FirstName")]
    public string? FirstName{get;set;}
    [Column("BirthDate")]
    public DateTime? BirthDate{get;set;}

}
