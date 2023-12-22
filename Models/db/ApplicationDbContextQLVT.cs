using app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using app.DTOs;

public class ApplicationDbContextQLVT : DbContext
{
     public ApplicationDbContextQLVT(DbContextOptions<ApplicationDbContextQLVT> options): base(options)
    {
    }
    public virtual DbSet<VatTu> VatTu{get;set;}



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured){
            
        }
    }
}