using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class BaoGiaFilter{
    public DateTime? NgayTaoBaoGiaStart{get;set;}
    public DateTime? NgayTaoBaoGiaKetThuc{get;set;}
    public string? Search{get;set;}
}
