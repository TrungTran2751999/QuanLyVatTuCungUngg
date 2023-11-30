using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("UserVatTu")]
public class UserVatTu{
    [Key]
    [Column("Id")]
    public long? Id{get;set;}
    [Column("UserName")]
    public string? UserName{get;set;}
    [Column("Password")]
    public string? Password{get;set;}
    [Column("Role")]
    public string? Role{get;set;}
    [Column("DepartmentId")]
    public long DepartmentId{get;set;}
    [Column("MainDepartmentId")]
    public long MainDepartmentId{get;set;}
    [Column("CreatedAt")]
    public DateTime? CreatedAt{get;set;}
    [Column("UpdatedAt")]
    public DateTime? UpdatedAt{get;set;}
    [Column("CreatedBy")]
    public long? CreatedBy{get;set;}
    [Column("UpdatedBy")]
    public long? UpdatedBy{get;set;}
}
