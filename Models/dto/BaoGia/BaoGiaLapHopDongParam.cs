using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text.Json;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.DTOs;

public class BaoGiaLapHopDongParam{
    public Guid BaoGiaId{get;set;}
    public long NhaCungUngId{get;set;}
    public DateTime NgayKiKet{get;set;}
    public string DiaChiNhanHang{get;set;}
}

