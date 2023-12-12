using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using DocumentFormat.OpenXml.Bibliography;
namespace app.DTOs;

public class VatTuBaoGiaChiTietReponse{
    public Guid? Id{get;set;}
    public string? TenVatTu{get;set;}
    public long? VatTuId{get;set;}
    public string? MaVatTu{get;set;}
    public string? MaPhieu{get;set;}
    public List<NhaCungUng> ListNhaCungUng{get;set;}
}
public class NhaCungUng{
    public long NhaCungUngId{get;set;}
    public string? TenNhaCungUng{get;set;}
}
