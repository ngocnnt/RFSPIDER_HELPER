using APP_KTRA_ROUTER.Database;
using APP_KTRA_ROUTER.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP_KTRA_ROUTER.Services
{
    public class TRAMReposity : IDataStore<db_TRAM>
    {
        public EMEC_DB_Context _Dbcontext;
        public TRAMReposity(EMEC_DB_Context dbcontext)
        {
            _Dbcontext = dbcontext;
        }
        public async Task<bool> AddItemAsync(db_TRAM item)
        {
            var tracking = await _Dbcontext.Db_TRAMs.AddAsync(item);
            await _Dbcontext.SaveChangesAsync();

            var isAdded = tracking.State == EntityState.Added;

            return isAdded;
        }

        public async Task<bool> DeleteItemAsync(string ma_tram)
        {
            try
            {
                var product = await _Dbcontext.Db_TRAMs.FindAsync(ma_tram);

                var tracking = _Dbcontext.Db_TRAMs.Remove(product);

                await _Dbcontext.SaveChangesAsync();

                var isDeleted = tracking.State == EntityState.Deleted;

                return isDeleted;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<bool> DeleteTram_DonVi(string donvi)
        {
            List<db_TRAM> trams = await _Dbcontext.Db_TRAMs.Where(p => p.MA_DVIQLY == donvi).ToListAsync().ConfigureAwait(false);
            foreach (db_TRAM _tram in trams)
            {
                _Dbcontext.Db_TRAMs.Remove(_tram);
            }
            await _Dbcontext.SaveChangesAsync();
            return true;
        }
        public async Task<db_TRAM> GetItemAsync(string id)
        {
            try
            {
                var product = _Dbcontext.Db_TRAMs.Where(p => p.MA_TRAM == id).FirstOrDefault();

                return product;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<db_TRAM> GetTramTheoDVQLAsync(string ma_donvi)
        {
            try
            {
                var product = _Dbcontext.Db_TRAMs.Where(p => p.MA_DVIQLY == ma_donvi).ToList();
                return product;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<db_TRAM>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                var products = await _Dbcontext.Db_TRAMs.ToListAsync().ConfigureAwait(false);

                return products;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateItemAsync(TRAM item)
        {
            try
            {
                var tracking = _Dbcontext.Update(item);

                await _Dbcontext.SaveChangesAsync();

                var isModified = tracking.State == EntityState.Modified;

                return isModified;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        public async Task<bool> TruncateTableAsync()
        {
            _Dbcontext.Db_TRAMs.RemoveRange(_Dbcontext.Db_TRAMs);
            await _Dbcontext.SaveChangesAsync();
            return true;
        }

        public Task<bool> UpdateItemAsync(db_TRAM item)
        {
            throw new NotImplementedException();
        }
    }
}
