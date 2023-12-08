using app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using app.DTOs;

public class ApplicationDbContext : DbContext
{
     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    public virtual DbSet<NhomVatTu> NhomVatTu{get;set;}
    public virtual DbSet<VatTu> VatTu{get;set;}
    public virtual DbSet<NhaCungUngVatTu> NhaCungUng{get;set;}
    public virtual DbSet<PhieuNhanVatTuFast> PhieuNhanVatTuFasts{get;set;}

    public virtual DbSet<PhieuNhanVatTu> PhieuNhanVatTu{get;set;}
    public virtual DbSet<PhieuNhanVatTuChiTiet> PhieuNhanVatTuChiTiet{get;set;}
    public virtual DbSet<VatTuNhaCungUngRelation> VatTuNhaCungUngRelation{get;set;}
    public virtual DbSet<PhieuDeNghiNhanVatTuDaDuyet> PhieuDeNghiNhanVatTuDaDuyet{get;set;}
    public virtual DbSet<PhieuDeNghiNhanVatTuChiTietDaDuyet> PhieuDeNghiNhanVatTuChiTietDaDuyet{get;set;}
    public virtual DbSet<BaoGia> BaoGia{get;set;}
    public virtual DbSet<BaoGiaChiTiet> BaoGiaChiTiet{get;set;}
    public virtual DbSet<BaoGiaChitietVatTu> BaoGiaChitietVatTu{get;set;}

    public virtual DbSet<UserVatTu> UserVatTus{get;set;}


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
    }
}