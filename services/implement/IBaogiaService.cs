using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace app.Services;

public interface IBaogiaService{
    Task<List<BaoGiaResponse>?> GetAll(bool isDeleted);
    Task<List<NhaCungUngListVatTu>> GetById(Guid id);
    Task<byte[]> LapBaoGia(string data);
    Task<string> SaveBaoGia(BaoGiaCreateDTO baoGiaCreate);
    Task<string> XoaBaoGiaByPhieuDeNghi(List<Guid> listId, IDbContextTransaction transaction);
    Task<string> XoaBaoGiaByBaoGiaId(Guid baoGiaId, IDbContextTransaction transaction);
    Task<string> CapNhatBaoGia(BaoGiaUpdateDTO baoGiaUpdate); 
}