using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using app.Models;
namespace app.DTOs;

public class PhieuNVTGetAllDTO{
    public List<PhieuNhanVatTuFast> listPhieuNVTFast{get;set;}
    public List<PhieuDeNghiNhanVatTuDaDuyet> listPhieuNVTDaDuyet{get;set;}
}
