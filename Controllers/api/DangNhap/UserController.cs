using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using app.Services;
using app.DTOs;

namespace app.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private ApplicationDbContext dbContext;
    private ApplicationDbContextUsers dbContextUsers;
    private IHttpContextAccessor contextAccessor;
    private IUserService userService;
    private readonly JwtSettings jwtSetting;
    public UserController(ApplicationDbContext dbContext, IOptions<JwtSettings> options, IHttpContextAccessor contextAccessor, ApplicationDbContextUsers dbContextUsers, IUserService userService){
        this.dbContext = dbContext;
        this.jwtSetting = options.Value;
        this.contextAccessor = contextAccessor;
        this.dbContextUsers = dbContextUsers;
        this.userService = userService;
    }
    [Route("api/login")]
    [HttpPost]
    public async Task<IActionResult> Authenticate([FromForm] UserCred user){
        var checkUserQLVT = await dbContext.UserVatTus
                              .FirstOrDefaultAsync(x=>x.UserName==user.UserName);
        var userLogin = new UserVatTu();

        if(checkUserQLVT==null){
            var checkUser = await dbContextUsers.Users
                                .Select(x=>new{
                                    x.Id,
                                    x.Name,
                                    x.DepartmentId,
                                    x.MainDepartmentId,
                                    x.Role
                                })
                                .FirstOrDefaultAsync(x=>x.Name==user.UserName && (x.DepartmentId==7 || x.DepartmentId==126 || x.DepartmentId==125));
            if(checkUser==null){
                return Redirect("/login");
            }
            if(user.Password=="123456789"){
                var newUser = new UserVatTu{
                    UserName = user.UserName,
                    Password = user.Password,
                    DepartmentId = checkUser.DepartmentId,
                    MainDepartmentId = checkUser.MainDepartmentId,
                    Role = checkUser.Role <= 5 ? "ADMIN":"EMPLOYEE",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = 0,
                    UpdatedBy = 0
                };
                dbContext.Add(newUser);
                dbContext.SaveChanges();
                newUser.CreatedBy = newUser.Id;
                newUser.UpdatedBy = newUser.Id;
                dbContext.SaveChanges();

                userLogin = newUser;
            }else{
                return Redirect("/login");
            }          
        }else{
            if(user.Password==checkUserQLVT.Password){
                userLogin = checkUserQLVT;
            }else{
                return Redirect("/login");
            }
        }
    
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(this.jwtSetting.securitykey);
        var tokendesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]{new Claim(ClaimTypes.Role, userLogin.UserName), new Claim(ClaimTypes.Role, userLogin.Role)}
            ),
            Expires = DateTime.Now.AddDays(365*10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenHandler.CreateToken(tokendesc);
        string finalToken = tokenHandler.WriteToken(token);
        
        contextAccessor.HttpContext.Response.Cookies.Append("token",finalToken, new CookieOptions{HttpOnly=true, Expires=DateTime.Now.AddDays(365*10)});
        var jwtResponse = new JwtResponse{
            UserName = userLogin.UserName,
            Role = userLogin.Role
        };
        contextAccessor.HttpContext.Response.Cookies.Append("role",userLogin.Role, new CookieOptions{HttpOnly=false, Expires=DateTime.Now.AddDays(365*10)});
        contextAccessor.HttpContext.Response.Cookies.Append("name",userLogin.UserName, new CookieOptions{HttpOnly=false, Expires=DateTime.Now.AddDays(365*10)});
        contextAccessor.HttpContext.Response.Cookies.Append("id",userLogin.Id.ToString(), new CookieOptions{HttpOnly=false, Expires=DateTime.Now.AddDays(365*10)});
        return Redirect("/");
    }
    [Route("api/logout")]
    [HttpGet]
    public void LogOut(){
        Response.Cookies.Delete("token");
        Response.Cookies.Delete("role");
        Response.Cookies.Delete("name");
        Response.Cookies.Delete("id");
        Response.Redirect("/login");
    }
    [Route("api/change-pass")]
    [HttpPost]
   public async Task<IActionResult> ChangePass([FromBody] UsersParamDTO usersParamDTO){
     var result = await userService.ChangePass(usersParamDTO);
     if(result=="OK"){
        return Ok(result);
     }else{
        return BadRequest(result);
     }
   }
}