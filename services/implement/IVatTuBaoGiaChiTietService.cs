using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace app.Services;

public interface IVatTuBaoGiaChiTietService{
    public Task<VatTuBaoGiaChiTiet> GetByBaoGiaChiTietId(Guid BaoGiaChiTietId);
}