using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Text.Json;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
namespace app.DTOs;

public class XuatHopDongRes{
    public byte[] file{get;set;}
    public string tenFile{get;set;}
}

