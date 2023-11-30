using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace app.Services;

public interface IPhieuNVTService{
    Task<PhieuNVTGetAllDTO?> GetAll(bool isDeleted, int page);
    Task<List<PhieuNhanVatTuChiTietFast>?> GetByMaPhieu(string maPhieu, string codeYear);
    Task<List<VatTuNhaCungUngResultTongHop>> TongHopPhieu(List<Guid> listId);
    List<Filter> Filter(FilterParam listFilter);

    List<int> FilterTongHop(FilterParamTongHop filterParamTongHop);
}