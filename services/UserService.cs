using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
// using System.Data.Entity;
namespace app.Services;
public class UserService : IUserService
{
    private readonly ApplicationDbContext dbContext;

    public UserService(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }

    public async Task<string> ChangePass(UsersParamDTO usersParamDTO)
    {
        var checkUser = await dbContext.UserVatTus.FirstOrDefaultAsync(x=>x.UserName==usersParamDTO.UserName && x.Password==usersParamDTO.OldPassword);
        if(checkUser==null){
            return "User:Mật khẩu không đúng hoặc người dùng không tồn tại";
        }
        checkUser.Password = usersParamDTO.NewPassword;
        checkUser.UpdatedBy = usersParamDTO.Id==0 ? checkUser.Id : usersParamDTO.Id;
        checkUser.UpdatedAt = DateTime.Now;
        dbContext.SaveChanges();

        return "OK";
    }
}