using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text.Json;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.DTOs;

public class HopDongFilter{
    public DateTime? NgayKiKetBatDau{get;set;}
    public DateTime? NgayKiKetKetThuc{get;set;}
    public string? Search{get;set;}
}

