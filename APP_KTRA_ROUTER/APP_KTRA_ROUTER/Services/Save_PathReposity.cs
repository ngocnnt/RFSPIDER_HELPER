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
    public class Save_PathReposity : IDataStore<Save_Path> 
    {
        public EMEC_DB_Context _Dbcontext;
        public Save_PathReposity(EMEC_DB_Context dbcontext)
        {
            _Dbcontext = dbcontext;
        }
        

        public async Task<bool> DeleteItemAsync(string path)
        {
            try
            {
                var product = await _Dbcontext.Save_Paths.FindAsync(path);

                var tracking = _Dbcontext.Remove(product);

                await _Dbcontext.SaveChangesAsync();

                var isDeleted = tracking.State == EntityState.Deleted;

                return isDeleted;
            }
            catch (Exception e)
            {
                return false;
            }
        }   

        
        public async Task<List<Save_Path>> GetAllPath_TramAsync(string matram)
        {
            try
            {
                var listObjects = await _Dbcontext.Save_Paths.Where(p => p.MA_TRAM == matram).ToListAsync();

                return listObjects;
            }
            catch (Exception e)
            {
                return null;
            }
        } 
               

       

     
    

        public async Task<bool> AddItemAsync(Save_Path item)        {
            var tracking = await _Dbcontext.Save_Paths.AddAsync(item);
            await _Dbcontext.SaveChangesAsync();
            var isAdded = tracking.State == EntityState.Added;
            return isAdded;
        }

        public Task<bool> UpdateItemAsync(Save_Path item)
        {
            throw new NotImplementedException();
        }

        Task<Save_Path> IDataStore<Save_Path>.GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task<List<Save_Path>> IDataStore<Save_Path>.GetItemsAsync(bool forceRefresh)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TruncateTableAsync()
        {
            throw new NotImplementedException();
        }
    }
}
