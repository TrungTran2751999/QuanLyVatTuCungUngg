using app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public class ApplicationDbContextPhieuNVT : DbContext
{
     public ApplicationDbContextPhieuNVT(DbContextOptions<ApplicationDbContextPhieuNVT> options): base(options)
    {
    }
    public virtual DbSet<PhieuNhanVatTuFast> PhieuNhanVatTuFasts{get;set;}
    public virtual DbSet<PhieuNhanVatTuChiTietFast> PhieuNhanVatTuChiTietFasts{get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured){
            
        }
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Bỏ qua (ignore) khóa chính trong một bảng
        modelBuilder.Entity<PhieuNhanVatTuFast>().HasNoKey();
        modelBuilder.Entity<PhieuNhanVatTuChiTietFast>().HasNoKey();
    }
}