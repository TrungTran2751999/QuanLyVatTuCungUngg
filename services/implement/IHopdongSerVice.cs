using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace app.Services;

public interface IHopdongSerVice{
    public byte[] XuatHopDong(string data);
}