using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Services;

public interface INhomVatTuService{
    Task<List<NhomVatTu>?> GetAll();
    Task<NhomVatTu?> GetById(long id);
}