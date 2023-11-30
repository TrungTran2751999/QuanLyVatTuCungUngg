using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
namespace app.DTOs;

public class UsersParamDTO{
    [Required]
    public long Id {get;set;}
    [Required]
    public string? UserName{get;set;}
    [Required]
    public string? NewPassword{get;set;}
    [Required]
    public string? OldPassword{get;set;}
}
