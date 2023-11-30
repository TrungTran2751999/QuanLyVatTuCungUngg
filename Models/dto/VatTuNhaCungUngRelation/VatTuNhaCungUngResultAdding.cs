using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class VatTuNhaCungUngResultAdding{
    public string? result{get;set;}
    public long? idMax{get;set;}
}
