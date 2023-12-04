using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class TongHopParam{
    public string? codeYear{get;set;}
    public string? maPhieu{get;set;}
}