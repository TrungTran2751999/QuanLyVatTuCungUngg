using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Services;

public interface IVatTuService{
    Task<List<VatTu>> GetAll(int index);
    Task<List<NhaCungUngVatTu>> GetNhaCungUngByVatTu(long idVatTu);
    Task<VatTuSearch> Search(string hint, int index);
}