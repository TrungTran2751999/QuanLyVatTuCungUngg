using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class ParamTongHopPhieu{
    public List<Guid>? listId{get;set;}
}
