using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app.Models;
[Table("Departments")]
public class Departments{
    [Key]
    [Column("Id")]
    public long Id{get;set;}
    [Column("Guid")]
    public Guid? Guid{get;set;}
    [Column("HrReferCode")]
    public string? HrReferCode{get;set;}
    [Column("ParentId")]
    public long? ParentId{get;set;}
    [Column("DepartmentName")]
    public string? DepartmentName{get;set;}
    [Column("DepartmentShortName")]
    public string? DepartmentShortName{get;set;}
    [Column("ShortestName")]
    public string? ShortestName{get;set;}
    [Column("Mission")]
    public string? Mission{get;set;}
    [Column("DepartmentType")]
    public int? DepartmentType {get;set;}
    [Column("DepartmentLevel")]
    public int? DepartmentLevel {get;set;}
    [Column("DisplayOrder")]
    public int? DisplayOrder {get;set;}
    [Column("MeetingOrder")]
    public int? MeetingOrder {get;set;}
    [Column("IsShowPublic")]
    public bool? IsShowPublic {get;set;}
    [Column("IsOffice")]
    public bool? IsOffice {get;set;}
    [Column("IsXNCN")]
    public bool? IsXNCN {get;set;}
    [Column("TenHieuEbilling")]
    public string? TenHieuEbilling {get;set;}
    [Column("IsDeleted")]
    public bool? IsDeleted {get;set;}
    [Column("CreatedBy")]
    public long? CreatedBy {get;set;}
    [Column("CreatedTime")]
    public DateTime? CreatedTime {get;set;}
    [Column("UpdatedBy")]
    public long? UpdatedBy {get;set;}
    [Column("UpdatedTime")]
    public DateTime? UpdatedTime {get;set;}
}
