using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using Microsoft.EntityFrameworkCore;
using app.DTOs;
using System.Data.SqlClient;
using Microsoft.Extensions.ObjectPool;
using System.Runtime.CompilerServices;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using DocumentFormat.OpenXml.Office.CustomUI;

namespace app.Services;
public class PhieuNVTService : IPhieuNVTService
{
    private readonly ApplicationDbContextPhieuNVT dbContext;
    private readonly ApplicationDbContext dbContext1;

    public PhieuNVTService(ApplicationDbContextPhieuNVT dbContext, ApplicationDbContext dbContext1){
        this.dbContext = dbContext;
        this.dbContext1 = dbContext1;
    }

    public async Task<PhieuNVTGetAllDTO?> GetAll(bool isDeleted, int page)
    {
        var listPhieuDaDuyet = new List<PhieuDeNghiNhanVatTuDaDuyet>();
        PhieuNVTGetAllDTO phieuNVT = new();
        if(isDeleted==false){
            listPhieuDaDuyet = await dbContext1.PhieuDeNghiNhanVatTuDaDuyet
                                       .Where(x=>x.IsDeleted==false)
                                       .OrderByDescending(x=>x.CreatedTime)
                                       .Skip(0).Take(200).ToListAsync();
            phieuNVT.listPhieuNVTDaDuyet = listPhieuDaDuyet;
        }else{
            listPhieuDaDuyet = await dbContext1.PhieuDeNghiNhanVatTuDaDuyet
                                       .Where(x=>x.IsDeleted==false)
                                       .OrderByDescending(x=>x.CreatedTime)
                                       .Skip(0).Take(200).ToListAsync();
            var listPhieu = await dbContext.PhieuNhanVatTuFasts.FromSqlRaw("EXEC FastBusiness$App$Voucher$Loading {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
            "PX0", 
            "c87$000000", 
            "m87$", 
            "ngay_ct", 
            "convert(char(6), {0}, 112)", 
            "000000", 
            200, 
            "stt_rec", 
            "rtrim(stt_rec) as stt_rec,rtrim(ma_dvcs) as ma_dvcs,ngay_ct,rtrim(so_ct) as so_ct,rtrim(dept_id) as dept_id,t_so_luong,rtrim(dien_giai) as dien_giai,rtrim(ma_ct) as ma_ct,rtrim(status) as status,datetime0,datetime2,user_id0,user_id2", 
            "rtrim(stt_rec) as stt_rec,rtrim(ma_dvcs) as ma_dvcs,ngay_ct,rtrim(so_ct) as so_ct,rtrim(a.dept_id) as dept_id,b.ten_bp,t_so_luong,rtrim(dien_giai) as dien_giai,rtrim(a.ma_ct) as ma_ct,rtrim(a.status) as status,rtrim(u0.statusname) as u0,a.datetime0,a.datetime2,a.user_id0,rtrim(u1.u_name) as u1,a.user_id2,rtrim(u2.u_name) as u2,'''' as Hash", 
            "a left join dmbp b on a.dept_id = b.ma_bp left join dmttct u0 on a.ma_ct = u0.ma_ct and a.status = u0.status left join vsysuser u1 on a.user_id0 = u1.u_id left join vsysuser u2 on a.user_id2 = u2.u_id", 
            "ngay_ct, so_ct", 
            1, 
            1, 
            1, 
            0).ToListAsync();

            phieuNVT.listPhieuNVTFast = listPhieu;
            phieuNVT.listPhieuNVTDaDuyet = listPhieuDaDuyet;
        }
        return phieuNVT;
        
    }

    public async Task<List<PhieuNhanVatTuChiTietFast>?> GetByMaPhieu(string code, string codeyear)
    {
        
        var listVatTu = await dbContext.PhieuNhanVatTuChiTietFasts.FromSqlRaw(
        "declare @stt_rec varchar(14)"+" "+
        "select @stt_rec= {0}"+" "+

        "declare @$stock$View$Type char(1)"+" "+
        "if '' = '2' and exists(select 1 from options where name = 'm_instock_split' and val = '1')"+" "+
        "select @$stock$View$Type = rtrim(val) from options where name = 'm_instock_view2'"+" "+
        "else"+" "+
        "select @$stock$View$Type = rtrim(val) from options where name = 'm_instock_view'"+" "+

        "select @$stock$View$Type = isnull(@$stock$View$Type, '1')"+" "+

        "select top 0 ma_vt, ma_kho, ma_vi_tri, ma_lo, sl_nhap as so_luong0 into #d from r70"+"$"+codeyear+" "+""+" "+
        "if 0 <> 1 begin"+" "+
        "if @$stock$View$Type = '1' insert into #d select ma_vt, ma_kho, '', '', sum(sl_nhap - sl_xuat) from r70"+"$"+codeyear+" "+" where ((((((stt_rec =  {0})))))) group by ma_vt, ma_kho"+" "+
        "if @$stock$View$Type = '2' insert into #d select ma_vt, ma_kho, ma_vi_tri, '', sum(sl_nhap - sl_xuat) from r70"+"$"+codeyear+" "+" where ((((((stt_rec =  {0})))))) group by ma_vt, ma_kho, ma_vi_tri"+" "+
        "if @$stock$View$Type = '3' insert into #d select ma_vt, ma_kho, '', ma_lo, sum(sl_nhap - sl_xuat) from r70"+"$"+codeyear+" "+" where ((((((stt_rec =  {0})))))) group by ma_vt, ma_kho, ma_lo"+" "+
        "if @$stock$View$Type = '4' insert into #d select ma_vt, ma_kho, ma_vi_tri, ma_lo, sum(sl_nhap - sl_xuat) from r70"+"$"+codeyear+" "+" where ((((((stt_rec =  {0})))))) group by ma_vt, ma_kho, ma_vi_tri, ma_lo"+" "+
        "end"+" "+

        "if not exists(select 1 from options where name = 'm_instock_onhand' and val = '1') begin"+" "+
        "if @$stock$View$Type = '1' select rtrim(a.ma_vt) as ma_vt,b.ten_vt,rtrim(a.dvt) as dvt,'' as ten_dvt,he_so,rtrim(a.ma_kho) as ma_kho,'' as ten_kho,rtrim(a.ma_vi_tri) as ma_vi_tri,'' as ten_vi_tri,rtrim(a.ma_lo) as ma_lo,'' as ten_lo,(e.ton13 - isnull(d.so_luong0, 0)) / case when a.he_so <> 0 then a.he_so else 1 end as ton13,so_luong,ngay_yc,sl_xuat,rtrim(ma_vv) as ma_vv,'' as ten_vv,rtrim(ma_bp) as ma_bp,'' as ten_bp,rtrim(so_lsx) as so_lsx,'' as ten_lsx,rtrim(ma_sp) as ma_sp,'' as ten_sp,rtrim(ma_hd) as ma_hd,'' as ten_hd,rtrim(ma_phi) as ma_phi,'' as ten_phi,rtrim(ma_ku) as ma_ku,'' as ten_ku,b.nhieu_dvt,b.lo_yn,case when exists(select top 1 ma_kho from dmvitri where dmvitri.ma_kho = a.ma_kho) then 1 else 0 end as vi_tri_yn,rtrim(a.tk_du) as tk_du,'' as ten_tk_du,rtrim(a.tk_vt) as tk_vt,'' as ten_tk_vt,gia,a.tien,gia_nt,a.tien_nt,b.sua_tk_vt,rtrim(a.gc_td1) as gc_td1,rtrim(stt_rec) as stt_rec,rtrim(stt_rec0) as stt_rec0,line_nbr,rtrim(stt_rec_px) as stt_rec_px,rtrim(stt_rec0px) as stt_rec0px from d87"+"$"+codeyear+" "+" a left join dmvt b on a.ma_vt = b.ma_vt left join #d d on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho left join cdvt13 e on a.ma_vt = e.ma_vt and a.ma_kho = e.ma_kho where ((((((stt_rec =  {0})))))) order by stt_rec, line_nbr"+" "+
        "if @$stock$View$Type = '2' select rtrim(a.ma_vt) as ma_vt,b.ten_vt,rtrim(a.dvt) as dvt,'' as ten_dvt,he_so,rtrim(a.ma_kho) as ma_kho,'' as ten_kho,rtrim(a.ma_vi_tri) as ma_vi_tri,'' as ten_vi_tri,rtrim(a.ma_lo) as ma_lo,'' as ten_lo,(e.ton13 - isnull(d.so_luong0, 0)) / case when a.he_so <> 0 then a.he_so else 1 end as ton13,so_luong,ngay_yc,sl_xuat,rtrim(ma_vv) as ma_vv,'' as ten_vv,rtrim(ma_bp) as ma_bp,'' as ten_bp,rtrim(so_lsx) as so_lsx,'' as ten_lsx,rtrim(ma_sp) as ma_sp,'' as ten_sp,rtrim(ma_hd) as ma_hd,'' as ten_hd,rtrim(ma_phi) as ma_phi,'' as ten_phi,rtrim(ma_ku) as ma_ku,'' as ten_ku,b.nhieu_dvt,b.lo_yn,case when exists(select top 1 ma_kho from dmvitri where dmvitri.ma_kho = a.ma_kho) then 1 else 0 end as vi_tri_yn,rtrim(a.tk_du) as tk_du,'' as ten_tk_du,rtrim(a.tk_vt) as tk_vt,'' as ten_tk_vt,gia,a.tien,gia_nt,a.tien_nt,b.sua_tk_vt,rtrim(a.gc_td1) as gc_td1,rtrim(stt_rec) as stt_rec,rtrim(stt_rec0) as stt_rec0,line_nbr,rtrim(stt_rec_px) as stt_rec_px,rtrim(stt_rec0px) as stt_rec0px from d87"+"$"+codeyear+" "+" a left join dmvt b on a.ma_vt = b.ma_vt left join #d d on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri left join cdvitri13 e on a.ma_vt = e.ma_vt and a.ma_kho = e.ma_kho and a.ma_vi_tri = e.ma_vi_tri where ((((((stt_rec =  {0})))))) order by stt_rec, line_nbr"+" "+
        "if @$stock$View$Type = '3' select rtrim(a.ma_vt) as ma_vt,b.ten_vt,rtrim(a.dvt) as dvt,'' as ten_dvt,he_so,rtrim(a.ma_kho) as ma_kho,'' as ten_kho,rtrim(a.ma_vi_tri) as ma_vi_tri,'' as ten_vi_tri,rtrim(a.ma_lo) as ma_lo,'' as ten_lo,(e.ton13 - isnull(d.so_luong0, 0)) / case when a.he_so <> 0 then a.he_so else 1 end as ton13,so_luong,ngay_yc,sl_xuat,rtrim(ma_vv) as ma_vv,'' as ten_vv,rtrim(ma_bp) as ma_bp,'' as ten_bp,rtrim(so_lsx) as so_lsx,'' as ten_lsx,rtrim(ma_sp) as ma_sp,'' as ten_sp,rtrim(ma_hd) as ma_hd,'' as ten_hd,rtrim(ma_phi) as ma_phi,'' as ten_phi,rtrim(ma_ku) as ma_ku,'' as ten_ku,b.nhieu_dvt,b.lo_yn,case when exists(select top 1 ma_kho from dmvitri where dmvitri.ma_kho = a.ma_kho) then 1 else 0 end as vi_tri_yn,rtrim(a.tk_du) as tk_du,'' as ten_tk_du,rtrim(a.tk_vt) as tk_vt,'' as ten_tk_vt,gia,a.tien,gia_nt,a.tien_nt,b.sua_tk_vt,rtrim(a.gc_td1) as gc_td1,rtrim(stt_rec) as stt_rec,rtrim(stt_rec0) as stt_rec0,line_nbr,rtrim(stt_rec_px) as stt_rec_px,rtrim(stt_rec0px) as stt_rec0px from d87"+"$"+codeyear+" "+" a left join dmvt b on a.ma_vt = b.ma_vt left join #d d on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_lo = d.ma_lo left join cdklo13 e on a.ma_vt = e.ma_vt and a.ma_kho = e.ma_kho and a.ma_lo = e.ma_lo where ((((((stt_rec =  {0})))))) order by stt_rec, line_nbr"+" "+
        "if @$stock$View$Type = '4' select rtrim(a.ma_vt) as ma_vt,b.ten_vt,rtrim(a.dvt) as dvt,'' as ten_dvt,he_so,rtrim(a.ma_kho) as ma_kho,'' as ten_kho,rtrim(a.ma_vi_tri) as ma_vi_tri,'' as ten_vi_tri,rtrim(a.ma_lo) as ma_lo,'' as ten_lo,(e.ton13 - isnull(d.so_luong0, 0)) / case when a.he_so <> 0 then a.he_so else 1 end as ton13,so_luong,ngay_yc,sl_xuat,rtrim(ma_vv) as ma_vv,'' as ten_vv,rtrim(ma_bp) as ma_bp,'' as ten_bp,rtrim(so_lsx) as so_lsx,'' as ten_lsx,rtrim(ma_sp) as ma_sp,'' as ten_sp,rtrim(ma_hd) as ma_hd,'' as ten_hd,rtrim(ma_phi) as ma_phi,'' as ten_phi,rtrim(ma_ku) as ma_ku,'' as ten_ku,b.nhieu_dvt,b.lo_yn,case when exists(select top 1 ma_kho from dmvitri where dmvitri.ma_kho = a.ma_kho) then 1 else 0 end as vi_tri_yn,rtrim(a.tk_du) as tk_du,'' as ten_tk_du,rtrim(a.tk_vt) as tk_vt,'' as ten_tk_vt,gia,a.tien,gia_nt,a.tien_nt,b.sua_tk_vt,rtrim(a.gc_td1) as gc_td1,rtrim(stt_rec) as stt_rec,rtrim(stt_rec0) as stt_rec0,line_nbr,rtrim(stt_rec_px) as stt_rec_px,rtrim(stt_rec0px) as stt_rec0px from d87"+"$"+codeyear+" "+" a left join dmvt b on a.ma_vt = b.ma_vt left join #d d on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri and a.ma_lo = d.ma_lo left join cdlo13 e on a.ma_vt = e.ma_vt and a.ma_kho = e.ma_kho and a.ma_vi_tri = e.ma_vi_tri and a.ma_lo = e.ma_lo where ((((((stt_rec =  {0})))))) order by stt_rec, line_nbr"+" "+
        "end else begin"+" "+
        "select top 0 ma_kho, ma_vi_tri, ma_vt, ma_lo, so_luong as ton13 into #xcd from cdvt73"+" "+

        "if @$stock$View$Type = '1' insert into #d select distinct ma_vt, ma_kho, '', '', 0 from d87"+"$"+codeyear+" "+" a where ((((((stt_rec =  {0})))))) and not exists(select 1 from #d b where a.ma_vt = b.ma_vt and a.ma_kho = b.ma_kho)"+" "+
        "if @$stock$View$Type = '2' insert into #d select distinct ma_vt, ma_kho, ma_vi_tri, '', 0 from d87"+"$"+codeyear+" "+" a where ((((((stt_rec =  {0})))))) and not exists(select 1 from #d b where a.ma_vt = b.ma_vt and a.ma_kho = b.ma_kho and a.ma_vi_tri = b.ma_vi_tri)"+" "+
        "if @$stock$View$Type = '3' insert into #d select distinct ma_vt, ma_kho, '', ma_lo, 0 from d87"+"$"+codeyear+" "+" a where ((((((stt_rec =  {0})))))) and not exists(select 1 from #d b where a.ma_vt = b.ma_vt and a.ma_kho = b.ma_kho and a.ma_lo = b.ma_lo)"+" "+
        "if @$stock$View$Type = '4' insert into #d select distinct ma_vt, ma_kho, ma_vi_tri, ma_lo, 0 from d87"+"$"+codeyear+" "+" a where ((((((stt_rec =  {0})))))) and not exists(select 1 from #d b where a.ma_vt = b.ma_vt and a.ma_kho = b.ma_kho and a.ma_vi_tri = b.ma_vi_tri and a.ma_lo = b.ma_lo)"+" "+

        "if @$stock$View$Type = '1' begin"+" "+
            "insert into #xcd select a.ma_kho, '', a.ma_vt, '', sum(ton13) from #d d join cdvt13 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho group by a.ma_kho, a.ma_vt"+" "+
            "if '' = ''"+" "+
            "insert into #xcd select a.ma_kho, '', a.ma_vt, '', sum(a.so_luong) from #d d join cdvt73 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho group by a.ma_kho, a.ma_vt"+" "+
            "else"+" "+
            "insert into #xcd select a.ma_kho, '', a.ma_vt, '', sum(a.so_luong) from #d d join cdvt93 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho group by a.ma_kho, a.ma_vt"+" "+
            "select rtrim(a.ma_vt) as ma_vt,b.ten_vt,rtrim(a.dvt) as dvt,'' as ten_dvt,he_so,rtrim(a.ma_kho) as ma_kho,'' as ten_kho,rtrim(a.ma_vi_tri) as ma_vi_tri,'' as ten_vi_tri,rtrim(a.ma_lo) as ma_lo,'' as ten_lo,(e.ton13 - isnull(d.so_luong0, 0)) / case when a.he_so <> 0 then a.he_so else 1 end as ton13,so_luong,ngay_yc,sl_xuat,rtrim(ma_vv) as ma_vv,'' as ten_vv,rtrim(ma_bp) as ma_bp,'' as ten_bp,rtrim(so_lsx) as so_lsx,'' as ten_lsx,rtrim(ma_sp) as ma_sp,'' as ten_sp,rtrim(ma_hd) as ma_hd,'' as ten_hd,rtrim(ma_phi) as ma_phi,'' as ten_phi,rtrim(ma_ku) as ma_ku,'' as ten_ku,b.nhieu_dvt,b.lo_yn,case when exists(select top 1 ma_kho from dmvitri where dmvitri.ma_kho = a.ma_kho) then 1 else 0 end as vi_tri_yn,rtrim(a.tk_du) as tk_du,'' as ten_tk_du,rtrim(a.tk_vt) as tk_vt,'' as ten_tk_vt,gia,a.tien,gia_nt,a.tien_nt,b.sua_tk_vt,rtrim(a.gc_td1) as gc_td1,rtrim(stt_rec) as stt_rec,rtrim(stt_rec0) as stt_rec0,line_nbr,rtrim(stt_rec_px) as stt_rec_px,rtrim(stt_rec0px) as stt_rec0px from d87"+"$"+codeyear+" "+" a left join dmvt b on a.ma_vt = b.ma_vt left join #d d on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho left join (select ma_kho, ma_vt, sum(ton13) as ton13 from #xcd group by ma_kho, ma_vt) e on a.ma_vt = e.ma_vt and a.ma_kho = e.ma_kho where ((((((stt_rec =  {0})))))) order by stt_rec, line_nbr"+" "+
        "end"+" "+
        
        "if @$stock$View$Type = '2' begin"+" "+
            "insert into #xcd select a.ma_kho, a.ma_vi_tri, a.ma_vt, '', sum(ton13) from #d d join cdvitri13 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri group by a.ma_kho, a.ma_vi_tri, a.ma_vt"+" "+
            "if '' = ''"+" "+
            "insert into #xcd select a.ma_kho, a.ma_vi_tri, a.ma_vt, '', sum(a.so_luong) from #d d join cdvt73 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri group by a.ma_kho, a.ma_vi_tri, a.ma_vt"+" "+
            "else"+" "+
            "insert into #xcd select a.ma_kho, a.ma_vi_tri, a.ma_vt, '', sum(a.so_luong) from #d d join cdvt93 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri group by a.ma_kho, a.ma_vi_tri, a.ma_vt"+" "+
            "select rtrim(a.ma_vt) as ma_vt,b.ten_vt,rtrim(a.dvt) as dvt,'' as ten_dvt,he_so,rtrim(a.ma_kho) as ma_kho,'' as ten_kho,rtrim(a.ma_vi_tri) as ma_vi_tri,'' as ten_vi_tri,rtrim(a.ma_lo) as ma_lo,'' as ten_lo,(e.ton13 - isnull(d.so_luong0, 0)) / case when a.he_so <> 0 then a.he_so else 1 end as ton13,so_luong,ngay_yc,sl_xuat,rtrim(ma_vv) as ma_vv,'' as ten_vv,rtrim(ma_bp) as ma_bp,'' as ten_bp,rtrim(so_lsx) as so_lsx,'' as ten_lsx,rtrim(ma_sp) as ma_sp,'' as ten_sp,rtrim(ma_hd) as ma_hd,'' as ten_hd,rtrim(ma_phi) as ma_phi,'' as ten_phi,rtrim(ma_ku) as ma_ku,'' as ten_ku,b.nhieu_dvt,b.lo_yn,case when exists(select top 1 ma_kho from dmvitri where dmvitri.ma_kho = a.ma_kho) then 1 else 0 end as vi_tri_yn,rtrim(a.tk_du) as tk_du,'' as ten_tk_du,rtrim(a.tk_vt) as tk_vt,'' as ten_tk_vt,gia,a.tien,gia_nt,a.tien_nt,b.sua_tk_vt,rtrim(a.gc_td1) as gc_td1,rtrim(stt_rec) as stt_rec,rtrim(stt_rec0) as stt_rec0,line_nbr,rtrim(stt_rec_px) as stt_rec_px,rtrim(stt_rec0px) as stt_rec0px from d87"+"$"+codeyear+" "+" a left join dmvt b on a.ma_vt = b.ma_vt left join #d d on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri left join (select ma_kho, ma_vt, ma_vi_tri, sum(ton13) as ton13 from #xcd group by ma_kho, ma_vt, ma_vi_tri) e on a.ma_vt = e.ma_vt and a.ma_kho = e.ma_kho and a.ma_vi_tri = e.ma_vi_tri where ((((((stt_rec =  {0})))))) order by stt_rec, line_nbr"+" "+
        "end"+" "+

        "if @$stock$View$Type = '3' begin"+" "+
            "insert into #xcd select a.ma_kho, '', a.ma_vt, a.ma_lo, sum(ton13) from #d d join cdklo13 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_lo = d.ma_lo group by a.ma_kho, a.ma_vt, a.ma_lo"+" "+
            "if '' = ''"+" "+
            "insert into #xcd select a.ma_kho, '', a.ma_vt, a.ma_lo, sum(a.so_luong) from #d d join cdvt73 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_lo = d.ma_lo group by a.ma_kho, a.ma_vt, a.ma_lo"+" "+
            "else"+" "+
            "insert into #xcd select a.ma_kho, '', a.ma_vt, a.ma_lo, sum(a.so_luong) from #d d join cdvt93 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_lo = d.ma_lo group by a.ma_kho, a.ma_vt, a.ma_lo"+" "+
            "select rtrim(a.ma_vt) as ma_vt,b.ten_vt,rtrim(a.dvt) as dvt,'' as ten_dvt,he_so,rtrim(a.ma_kho) as ma_kho,'' as ten_kho,rtrim(a.ma_vi_tri) as ma_vi_tri,'' as ten_vi_tri,rtrim(a.ma_lo) as ma_lo,'' as ten_lo,(e.ton13 - isnull(d.so_luong0, 0)) / case when a.he_so <> 0 then a.he_so else 1 end as ton13,so_luong,ngay_yc,sl_xuat,rtrim(ma_vv) as ma_vv,'' as ten_vv,rtrim(ma_bp) as ma_bp,'' as ten_bp,rtrim(so_lsx) as so_lsx,'' as ten_lsx,rtrim(ma_sp) as ma_sp,'' as ten_sp,rtrim(ma_hd) as ma_hd,'' as ten_hd,rtrim(ma_phi) as ma_phi,'' as ten_phi,rtrim(ma_ku) as ma_ku,'' as ten_ku,b.nhieu_dvt,b.lo_yn,case when exists(select top 1 ma_kho from dmvitri where dmvitri.ma_kho = a.ma_kho) then 1 else 0 end as vi_tri_yn,rtrim(a.tk_du) as tk_du,'' as ten_tk_du,rtrim(a.tk_vt) as tk_vt,'' as ten_tk_vt,gia,a.tien,gia_nt,a.tien_nt,b.sua_tk_vt,rtrim(a.gc_td1) as gc_td1,rtrim(stt_rec) as stt_rec,rtrim(stt_rec0) as stt_rec0,line_nbr,rtrim(stt_rec_px) as stt_rec_px,rtrim(stt_rec0px) as stt_rec0px from d87"+"$"+codeyear+" "+" a left join dmvt b on a.ma_vt = b.ma_vt left join #d d on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_lo = d.ma_lo left join (select ma_kho, ma_vt, ma_lo, sum(ton13) as ton13 from #xcd group by ma_kho, ma_vt, ma_lo) e on a.ma_vt = e.ma_vt and a.ma_kho = e.ma_kho and a.ma_lo = e.ma_lo where ((((((stt_rec =  {0})))))) order by stt_rec, line_nbr"+" "+
        "end"+" "+

        "if @$stock$View$Type = '4' begin"+" "+
            "insert into #xcd select a.ma_kho, a.ma_vi_tri, a.ma_vt, a.ma_lo, sum(ton13) from #d d join cdlo13 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri and a.ma_lo = d.ma_lo group by a.ma_kho, a.ma_vt, a.ma_vi_tri, a.ma_lo"+" "+
            "if '' = ''"+" "+
            "insert into #xcd select a.ma_kho, a.ma_vi_tri, a.ma_vt, a.ma_lo, sum(a.so_luong) from #d d join cdvt73 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri and a.ma_lo = d.ma_lo group by a.ma_kho, a.ma_vt, a.ma_vi_tri, a.ma_lo"+" "+
            "else"+" "+
            "insert into #xcd select a.ma_kho, a.ma_vi_tri, a.ma_vt, a.ma_lo, sum(a.so_luong) from #d d join cdvt93 a on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri and a.ma_lo = d.ma_lo group by a.ma_kho, a.ma_vt, a.ma_vi_tri, a.ma_lo"+" "+
            "select rtrim(a.ma_vt) as ma_vt,b.ten_vt,rtrim(a.dvt) as dvt,'' as ten_dvt,he_so,rtrim(a.ma_kho) as ma_kho,'' as ten_kho,rtrim(a.ma_vi_tri) as ma_vi_tri,'' as ten_vi_tri,rtrim(a.ma_lo) as ma_lo,'' as ten_lo,(e.ton13 - isnull(d.so_luong0, 0)) / case when a.he_so <> 0 then a.he_so else 1 end as ton13,so_luong,ngay_yc,sl_xuat,rtrim(ma_vv) as ma_vv,'' as ten_vv,rtrim(ma_bp) as ma_bp,'' as ten_bp,rtrim(so_lsx) as so_lsx,'' as ten_lsx,rtrim(ma_sp) as ma_sp,'' as ten_sp,rtrim(ma_hd) as ma_hd,'' as ten_hd,rtrim(ma_phi) as ma_phi,'' as ten_phi,rtrim(ma_ku) as ma_ku,'' as ten_ku,b.nhieu_dvt,b.lo_yn,case when exists(select top 1 ma_kho from dmvitri where dmvitri.ma_kho = a.ma_kho) then 1 else 0 end as vi_tri_yn,rtrim(a.tk_du) as tk_du,'' as ten_tk_du,rtrim(a.tk_vt) as tk_vt,'' as ten_tk_vt,gia,a.tien,gia_nt,a.tien_nt,b.sua_tk_vt,rtrim(a.gc_td1) as gc_td1,rtrim(stt_rec) as stt_rec,rtrim(stt_rec0) as stt_rec0,line_nbr,rtrim(stt_rec_px) as stt_rec_px,rtrim(stt_rec0px) as stt_rec0px from d87"+"$"+codeyear+" "+" a left join dmvt b on a.ma_vt = b.ma_vt left join #d d on a.ma_vt = d.ma_vt and a.ma_kho = d.ma_kho and a.ma_vi_tri = d.ma_vi_tri and a.ma_lo = d.ma_lo left join (select ma_kho, ma_vt, ma_vi_tri, ma_lo, sum(ton13) as ton13 from #xcd group by ma_kho, ma_vt, ma_vi_tri, ma_lo) e on a.ma_vt = e.ma_vt and a.ma_kho = e.ma_kho and a.ma_vi_tri = e.ma_vi_tri and a.ma_lo = e.ma_lo where ((((((stt_rec =  {0})))))) order by stt_rec, line_nbr"+" "+
        "end"+" "+
        "end"+" "+
        "DROP TABLE #d"

        ,code).ToListAsync();
        return listVatTu;
    }
    
    // public async Task<List<VatTuNhaCungUngResultTongHop>> TongHopPhieu(List<Guid> listId)
    // {
    //         List<VatTuNhaCungUngResultTongHop> listPhieuNhanVatTu = new();
    //         var listResult = await 
    //                         (from PhieuDeNghiNhanVatTuChiTietDaDuyet in dbContext1.PhieuDeNghiNhanVatTuChiTietDaDuyet
    //                            join PhieuDeNghi in dbContext1.PhieuDeNghiNhanVatTuDaDuyet 
    //                            on PhieuDeNghiNhanVatTuChiTietDaDuyet.PhieuId equals PhieuDeNghi.Id 

    //                            join VatTu in dbContext1.VatTu 
    //                            on PhieuDeNghiNhanVatTuChiTietDaDuyet.MaVatTu equals VatTu.MaVatTu 

    //                            where listId.Contains(PhieuDeNghi.Id) orderby PhieuDeNghiNhanVatTuChiTietDaDuyet.Id, PhieuDeNghi.Id
    //                            select new VatTuNhaCungUngResultTongHop{
    //                              Id = PhieuDeNghiNhanVatTuChiTietDaDuyet.Id,
    //                              IdVatTu = VatTu.Id,
    //                              IdPhieu = PhieuDeNghiNhanVatTuChiTietDaDuyet.PhieuId,
    //                              MaPhieu = PhieuDeNghi.MaPhieu,
    //                              CodeYear = PhieuDeNghi.CodeYear,
    //                              TenNguoiDeNghi = PhieuDeNghi.TenNguoiDeNghi,
    //                              TenBoPhan = PhieuDeNghi.TenBoPhan,
    //                              TenVatTu = VatTu.TenVatTu,
    //                              MaVatTu = PhieuDeNghiNhanVatTuChiTietDaDuyet.MaVatTu,
    //                              Soluong = PhieuDeNghiNhanVatTuChiTietDaDuyet.SoLuong,
    //                              DonViTinh = VatTu.DonViTinh,
    //                              GhiChu = PhieuDeNghiNhanVatTuChiTietDaDuyet.GhiChu,
    //                              ListNhaCungUngId = dbContext1.VatTuNhaCungUngRelation
    //                                                           .Where(x => x.MaVatTu == VatTu.Id && x.IsDeleted==false)
    //                                                           .Select(x=>x.NhaCungUngId)
    //                                                           .ToList()
    //                            }).ToListAsync();

    //         listPhieuNhanVatTu = listResult;
    //     return listPhieuNhanVatTu;
    // }

    public async Task<List<VatTuNhaCungUngResultTongHop>> TongHopPhieu(List<TongHopParam> listTongHop){
        var listPhieuChiTiet = new List<PhieuDeNghiNhanVatTuChiTietDaDuyet>();
        for(int i=0; i<listTongHop.Count; i++){
            var listPhieuChiTietFast = await GetByMaPhieu(listTongHop[i].maPhieu, listTongHop[i].codeYear);
            var listPhieuDeNghiNhanVatTu = listPhieuChiTietFast.Select(x=>new PhieuDeNghiNhanVatTuChiTietDaDuyet{
                                                                            MaVatTu = x.ma_vt,
                                                                            SoLuong = x.so_luong,
                                                                            GhiChu = x.gc_td1,
                                                                            MaPhieu = x.stt_rec
                                                                      });
            listPhieuChiTiet.AddRange(listPhieuDeNghiNhanVatTu);
        }

        List<VatTuNhaCungUngResultTongHop> listPhieuNhanVatTu = new();
        var listResult = (from PhieuDeNghiNhanVatTuChiTietDaDuyet in listPhieuChiTiet
        
                               join PhieuDeNghi in dbContext1.PhieuDeNghiNhanVatTuDaDuyet 
                               on PhieuDeNghiNhanVatTuChiTietDaDuyet.MaPhieu equals PhieuDeNghi.MaPhieu 

                               join VatTu in dbContext1.VatTu
                               on PhieuDeNghiNhanVatTuChiTietDaDuyet.MaVatTu equals VatTu.MaVatTu 

                               select new VatTuNhaCungUngResultTongHop{
                                 Id = PhieuDeNghiNhanVatTuChiTietDaDuyet.Id,
                                 IdVatTu = VatTu.Id,
                                 IdPhieu = PhieuDeNghiNhanVatTuChiTietDaDuyet.PhieuId,
                                 MaPhieu = PhieuDeNghi.MaPhieu,
                                 CodeYear = PhieuDeNghi.CodeYear,
                                 TenNguoiDeNghi = PhieuDeNghi.TenNguoiDeNghi,
                                 TenBoPhan = PhieuDeNghi.TenBoPhan,
                                 TenVatTu = VatTu.TenVatTu,
                                 MaVatTu = PhieuDeNghiNhanVatTuChiTietDaDuyet.MaVatTu,
                                 Soluong = PhieuDeNghiNhanVatTuChiTietDaDuyet.SoLuong,
                                 DonViTinh = VatTu.DonViTinh,
                                 GhiChu = PhieuDeNghiNhanVatTuChiTietDaDuyet.GhiChu,
                                 ListNhaCungUngId = dbContext1.VatTuNhaCungUngRelation
                                                              .Where(x => x.MaVatTu == VatTu.Id && x.IsDeleted==false)
                                                              .Select(x=>x.NhaCungUngId)
                                                              .ToList()
                               }).ToList();
        return listResult;
    }   
    public List<Filter> Filter(FilterParam filter)
    {
        List<Filter> listResult = new List<Filter>();
        List<Filter> listFilter = filter.ListFilters;
        listResult = listFilter;
        if(filter.Search != "" && filter.Search!=null){
            listResult =  listFilter.Where(x=>
                x.DienGiai.Contains(filter.Search) ||
                x.MaPhieu.Contains(filter.Search) || 
                x.TenBoPhan.Contains(filter.Search) || 
                x.NguoiYeuCau.Contains(filter.Search))
            .ToList();
            if(listResult.Count==0){
                return listResult.DistinctBy(x=>x.Id).ToList();
            }
        }
        if(filter.Time!="" && filter.Time != null){
            List<Filter> listFil = new List<Filter>();
            if(listResult.Count > 0){
                listFil = listResult;
            }else{
                listFil = listFilter;
            }
            listResult = listFil.Where(x=>x.Time==filter.Time).ToList();
            if(listResult.Count==0){
                return listResult.DistinctBy(x=>x.Id).ToList();
            }
        }
        if(filter.Status!="" && filter.Status != null){
            List<Filter> listFil = new List<Filter>();
            if(listResult.Count > 0){
                listFil = listResult;
            }else{
                listFil = listFilter;
            }
            listResult = listFil.Where(x=>x.Status==filter.Status).ToList();
            if(listResult.Count==0){
                return listResult.DistinctBy(x=>x.Id).ToList();
            }
        }
        return listResult.DistinctBy(x=>x.Id).ToList();
    }

    public List<int> FilterTongHop(FilterParamTongHop filter)
    {
        List<int> listResult = new();
        foreach(var item in filter.ListFilters){
            listResult.Add(item.Id);
        }
        if(filter.Search!=null && filter.Search!=""){
            listResult = filter.ListFilters.Where(x=>
                            x.TenVatTu.Contains(filter.Search) ||
                            x.GhiChu.Contains(filter.Search))
                            .Select(x=>x.Id)
                            .ToList();
            if(listResult.Count==0) return listResult.Distinct().ToList();
        }
        return listResult.Distinct().ToList();
    }
}