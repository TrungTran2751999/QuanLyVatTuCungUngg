using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Services;

public interface IVatTuNhaCungUngRelationService{
    Task<List<VatTuNhaCungUngRelation>?> GetAll();
    Task<List<VatTuNhaCungUngRelation>> GetByNhaCungUng(long idNhaCungUng, bool isDeleted);
    Task<VatTuNhaCungUngResultAdding> AddVatTuToNhaCungUng(VatTuNhaCungUngAddingDTO vatTuAdding);
    Task<string> RemoveVatTuFromNhaCungUng(long id);
    Task<VatTuNhaCungUngResultAdding> UpdateVatTuNhaCungUng(VatTuNhaCungUngAddingDTO vatTuUpdated);
    Task<long> GetMaxId();
}