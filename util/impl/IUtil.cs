using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using app.Utils;
namespace app.Utils;

public interface IUtil{
    public string RemoveDauTiengViet(string input);
    public Task<MemoryStream> ExportBaoGiaToExcel(List<VatTuInBaoGia> listVatTu);
    public Task<MemoryStream> ExportBaoGiaToWord(List<VatTuInBaoGia> listVatTu);
    public byte[] ZipFile(ExportFile exportFile);
    public string DoiNgayThangHienTai(string type);
}