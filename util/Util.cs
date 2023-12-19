using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Globalization;
// using System.Data.Entity;
namespace app.Utils;
public class Util:IUtil{
    public string RemoveDauTiengViet(string input){
        string decomposed = input.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();

        foreach (char c in decomposed)
        {
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}