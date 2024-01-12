using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using app.Utils;
using DocumentFormat.OpenXml.Wordprocessing;
namespace app.Utils;

public interface IUtil{
    public string RemoveDauTiengViet(string input);
    public Task<MemoryStream> ExportBaoGiaToExcel(List<VatTuInBaoGia> listVatTu);
    public Task<MemoryStream> ExportBaoGiaToWord(List<VatTuInBaoGia> listVatTu);
    public byte[] ZipFile(ExportFile exportFile);
    public string DoiNgayThangHienTai(DateTime ngayKiKet, string type);
    public List<TableRow> TaoBangHopDong(List<Hang> tableHopDongs, decimal total);
    public Paragraph InDoanVan(string str, string fontStyle, string canLe, string fontFamily, int fontSize);
    public List<Paragraph> InDieuKhoan(DieuKhoan dieuKhoanObj, string noiGiaoHang);
}