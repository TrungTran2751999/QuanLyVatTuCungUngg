using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Services;

public interface IPhieuDeNghiNhanVatTuDaPheDuyet{
    Task<List<PhieuDeNghiNhanVatTuDaDuyet>?> GetAll(bool isDeleted, int page);
    Task<PhieuDeNghiNhanVatTuDaDuyet?> GetById(Guid id);
    Task<string> PheDuyet(List<PhieuDeNghiPheDuyetCreateDTO> phieuDeNghiPheDuyet);
}