using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("NhaCungUngVatTu")]
public class NhaCungUngVatTu{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("Guid")]
    public Guid? Guid{set;get;}
    [Column("TenNhaCungUng")]
    public string? TenNhaCungUng{set;get;}
    [Column("CreatedAt")]
    public DateTime? CreatedAt{set;get;}
    [Column("CreatedBy")]
    public long? CreatedBy{set;get;}
    [Column("UpdateAt")]
    public DateTime? UpdateAt{set;get;}
    [Column("UpdateBy")]
    public long? UpdateBy{set;get;}
    [Column("IsDeleted")]
    public bool? IsDeleted{set;get;}
    
}
