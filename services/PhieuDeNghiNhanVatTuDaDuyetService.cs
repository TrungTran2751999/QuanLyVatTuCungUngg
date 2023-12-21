using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;
using app.DTOs;
using Microsoft.EntityFrameworkCore;
// using System.Data.Entity;
namespace app.Services;
public class PhieuDeNghiNhanVatTuDaDuyetService : IPhieuDeNghiNhanVatTuDaPheDuyet
{
    private readonly ApplicationDbContext dbContext;
    public PhieuDeNghiNhanVatTuDaDuyetService(ApplicationDbContext dbContext){
        this.dbContext = dbContext;
    }

    public async Task<List<PhieuDeNghiNhanVatTuDaDuyet>?> GetAll(bool isDeleted, int page)
    {
        var listPhieu = await dbContext.PhieuDeNghiNhanVatTuDaDuyet
                                       .Where(x=>x.IsDeleted==isDeleted)
                                       .OrderByDescending(x=>x.CreatedTime)
                                       .Skip(page).Take(10).ToListAsync();
        
        return listPhieu;
    }

    public async Task<PhieuDeNghiNhanVatTuDaDuyet?> GetById(Guid id)
    {
        var listPhieu = await dbContext.PhieuDeNghiNhanVatTuDaDuyet
                                       .Where(x=>x.Id==id)
                                       .FirstOrDefaultAsync();
        return listPhieu;
    }

    public async Task<string> PheDuyet(List<PhieuDeNghiPheDuyetCreateDTO> phieuDeNghiPheDuyet)
    {
         using (var transaction = dbContext.Database.BeginTransaction()){
            try{
                for(int i=0; i<phieuDeNghiPheDuyet.Count; i++){
                    //check ma phieu da ton tai trong table
                    var checkExistMaPhieu = await dbContext.PhieuDeNghiNhanVatTuDaDuyet.Where(x=>x.MaPhieu==phieuDeNghiPheDuyet[i].MaPhieu).FirstOrDefaultAsync();
                    if(checkExistMaPhieu!=null) return "Phiếu này đã được phê duyệt";

                    var listVatTu = phieuDeNghiPheDuyet[i].ListVatTuChangeSoLuong;
                    List<PhieuDeNghiNhanVatTuChiTietDaDuyet> listPhieuChiTiet = new();


                    // add phieu de nghi phe duyet vao table
                    await dbContext.PhieuDeNghiNhanVatTuDaDuyet.AddAsync(phieuDeNghiPheDuyet[i].ToModel());
                    dbContext.SaveChanges();

                    var lastRecordPhieuDeNghi = await dbContext.PhieuDeNghiNhanVatTuDaDuyet
                                                    .OrderByDescending(x=>x.CreatedTime)
                                                    .Select(x=>new{
                                                        x.Id
                                                    }).FirstOrDefaultAsync();

                    // //check xem co thieu thong tin danh sch vat tu khong
                    listPhieuChiTiet.AddRange(phieuDeNghiPheDuyet[i].ToListPhieuChiTiet(lastRecordPhieuDeNghi.Id)); 
                    
                    
                    // if(listPhieuChiTiet.Count==0) return "Thiếu thông tin danh sách vật tư";

                    
                    // var listMaVatTu = new List<string>();
                    // for(int i=0; i<listVatTu.Count; i++){
                    //     listMaVatTu.Add(listVatTu[i].MaVatTu);
                    // }

                    // var countListMaVatTu = await dbContext.VatTu.CountAsync(x=>listMaVatTu.Contains(x.MaVatTu));

                    // if(phieuDeNghiPheDuyet.ListVatTu.Count!=countListMaVatTu) return "Tồn tại mã vật tư ngoài danh sách";

                    // //add phieu de nghi nhan vat tu chi tiet da duyet tuong ung voi id phieu nhan vat tu
                    if(listPhieuChiTiet.Count > 0){
                        await dbContext.PhieuDeNghiNhanVatTuChiTietDaDuyet.AddRangeAsync(listPhieuChiTiet);
                        dbContext.SaveChanges();
                    }
                }

                transaction.Commit();
                return "OK";

            }catch(Exception e){
                Console.WriteLine(e);
                transaction.Rollback();
                throw;
            }
        }
    }
    public async Task<string> HuyPheDuyet(List<Guid> listIdPhieuPheDuyet)
    {
        using(var transaction = dbContext.Database.BeginTransaction()){
            try{
                foreach(var id in listIdPhieuPheDuyet){
                    var phieu = await dbContext.PhieuDeNghiNhanVatTuDaDuyet.FirstOrDefaultAsync(x=>x.Id==id);
                    if(phieu==null || phieu.BaoGiaId!=null) throw new Exception();

                    dbContext.Remove(phieu);
                    dbContext.SaveChanges();
                }
                transaction.Commit();
                return "OK";
            }catch(Exception e){
                Console.WriteLine(e);
                transaction.Rollback();
                return "NOT OK";
            }
        }
    }

}