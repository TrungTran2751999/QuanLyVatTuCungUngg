using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace app.Utils;

public interface IUtil{
    string RemoveDauTiengViet(string input);
}