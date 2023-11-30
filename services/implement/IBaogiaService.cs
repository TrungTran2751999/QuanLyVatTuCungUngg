using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Services;

public interface IBaogiaService{
    Task<List<BaoGiaResponse>?> GetAll(bool isDeleted);
    Task<BaoGia?> GetById(Guid id, bool isDeleted);
    Task<byte[]> LapBaoGia(string data);
    Task<string> SaveBaoGia(BaoGiaCreateDTO baoGiaCreate);
}