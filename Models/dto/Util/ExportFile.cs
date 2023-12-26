using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
namespace app.DTOs;

public class ExportFile{
    public List<MemoryStream>? files;
    public List<string>? names;
    
}
