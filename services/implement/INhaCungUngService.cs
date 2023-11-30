using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Services;

public interface INhaCungUngService{
    Task<List<NhaCungUngVatTu>?> GetAll(bool isDeleted, int index);
    Task<NhaCungUngVatTu?> GetById(long id, bool isDeleted);
    Task<string?> Save(NhaCungUngCreateDTO nhaCungUngCreate);
    Task<string?> Update(NhaCungUngUpdateDTO nhaCungUngUpdate);
    Task<string?> SoftDelete(long id, long userId);
    Task<string?> Restore(long id, long userId);
}