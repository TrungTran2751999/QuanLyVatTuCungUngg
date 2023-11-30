using app.DTOs;
using app.Models;
using Microsoft.AspNetCore.Mvc;

namespace app.Services;

public interface IUserService{
    Task<string> ChangePass(UsersParamDTO usersParamDTO);
}