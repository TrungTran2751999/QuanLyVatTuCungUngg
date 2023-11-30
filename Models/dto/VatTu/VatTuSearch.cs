using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class VatTuSearch{
    public List<VatTu>? ListVatTu{get;set;}
    public long? Count{get;set;}
}
