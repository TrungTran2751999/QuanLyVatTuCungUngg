using app.Models;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public class ApplicationDbContextUsers : DbContext
{
     public ApplicationDbContextUsers(DbContextOptions<ApplicationDbContextUsers> options): base(options)
    {
    }
    public virtual DbSet<Users> Users{get;set;}
    public virtual DbSet<Departments> Departments{get;set;}
    public virtual DbSet<UsersDTO> UsersDTO{get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured){
            
        }
    }
}