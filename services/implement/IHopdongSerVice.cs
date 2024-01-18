using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace app.Services;

public interface IHopdongSerVice{
    public List<HopDongMuaHang> GetAll(int pageNumber);
    public HopDongResponse GetById(Guid id);
    public byte[] XuatHopDong(CreateHopDongDTO data);
    public Task<string> LuuHopDong(CreateHopDongDTO data);
    public Task<string> CapNhatHopDong(CreateHopDongDTO data);
    public Task<List<HopDongMuaHang>> FilterHopDong(HopDongFilter hopDongFilter);
}