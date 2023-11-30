using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("BaoGia")]
public class BaoGia{
    [Key]
    [Column("Id")]
    public Guid Id{get;set;}
    [Column("Guid")]
    public Guid? Guid{get;set;}
    [Column("MaBaoGia")]
    public string? MaBaoGia{get;set;}
    [Column("CreatedBy")]
    public long CreatedAt{get;set;}
    [Column("UpdatedBy")]
    public long UpdatedAt{get;set;}
    [Column("CreatedTime")]
    public DateTime CreatedTime{get;set;}
    [Column("UpdatedTime")]
    public DateTime UpdatedTime{get;set;}
    [Column("IsDeleted")]
    public bool IsDeleted{get;set;}
}
