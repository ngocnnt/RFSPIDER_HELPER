using APP_KTRA_ROUTER.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP_KTRA_ROUTER.Services
{
    public class InfomationReposity : IDataStore<Information>
    {
        public EMEC_DB_Context _Dbcontext;
        public InfomationReposity(EMEC_DB_Context dbcontext)
        {
            _Dbcontext = dbcontext;
        }
        public async Task<bool> AddItemAsync(Information item)
        {
            var tracking = await _Dbcontext.Informations.AddAsync(item);
            await _Dbcontext.SaveChangesAsync();

            var isAdded = tracking.State == EntityState.Added;

            return isAdded;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {
                var product = await _Dbcontext.Informations.FindAsync(id);

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

        public async Task<bool> DeleteItem_TramAsync(string matram)
        {
            try
            {
                List<Information> list = _Dbcontext.Informations.Where(p => p.MA_TRAM == matram).ToList();
                foreach (Information item in list)
                {
                    _Dbcontext.Remove(item);
                }
                await _Dbcontext.SaveChangesAsync();



                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Information>> GetItemAsync(string sericto)
        {
            try
            {
                var product = await _Dbcontext.Informations.Where(p => p.SERY_CTO == sericto).ToListAsync().ConfigureAwait(false);

                return product;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Information> GetOnlyItemAsync(string sericto)
        {
            try
            {
                var product = await _Dbcontext.Informations.Where(p => p.SERY_CTO == sericto).FirstOrDefaultAsync().ConfigureAwait(false);

                return product;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Information>> GetItem_TramAsync(string matram)
        {
            try
            {
                //var product = await _Dbcontext.Informations.Where(p=> p.MA_TRAM == matram).ToListAsync().ConfigureAwait(false);
                var listObjects = (from obj in _Dbcontext.Informations
                                   where obj.MA_TRAM == matram
                                   orderby obj.MA_GC

                                   select new { obj.MA_KHANG, obj.SERY_CTO, obj.TEN_KHANG, obj.MA_DDO, obj.METER_TYPE, obj.KY, obj.THANG, obj.NAM, obj.DIA_CHI, obj.MA_GC }
                                   ).ToList().Distinct();

                List<Information> items = new List<Information>();
                foreach (var item in listObjects)
                {
                    Information i = new Information();
                    i.MA_KHANG = item.MA_KHANG;
                    i.TEN_KHANG = item.TEN_KHANG;
                    i.SERY_CTO = item.SERY_CTO;
                    i.MA_DDO = item.MA_DDO;
                    i.METER_TYPE = item.METER_TYPE;
                    i.KY = item.KY;
                    i.THANG = item.THANG;
                    i.NAM = item.NAM;
                    i.DIA_CHI = item.DIA_CHI;


                    List<Information> ifs = _Dbcontext.Informations.Where(p => p.SERY_CTO == item.SERY_CTO).ToList();
                    if (ifs.Count <= 1)
                    {
                        i.STATUS = ifs[0].STATUS;
                        i.CS_MOI = ifs[0].CS_MOI;
                        i.SL_MOI = ifs[0].SL_MOI;
                        i.MA_GC = ifs[0].MA_GC;
                        i.MA_COT = ifs[0].MA_COT;
                    }

                    else
                    {
                        int statusnull = 0;
                        int status1 = 0;
                        int status0 = 0;
                        i.CS_MOI = ifs[0].CS_MOI;
                        i.SL_MOI = ifs[0].SL_MOI;
                        i.MA_GC = ifs[0].MA_GC;
                        i.MA_COT = ifs[0].MA_COT;
                        foreach (Information ist in ifs)
                        {
                            if (ist.STATUS == 1) status1 += 1;
                            else if (ist.STATUS == null) statusnull += 1;
                            else status0 += 1;
                        }
                        if (status1 == ifs.Count)
                            i.STATUS = 1;
                        else if (statusnull == ifs.Count)
                            i.STATUS = null;
                        else
                            i.STATUS = 0;

                    }
                    items.Add(i);
                }
                return items;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Information>> GetAllItem_TramAsync(string matram)
        {
            try
            {
                var listObjects = await _Dbcontext.Informations.Where(p => p.MA_TRAM == matram).ToListAsync();

                return listObjects;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Information>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                var products = await _Dbcontext.Informations.ToListAsync().ConfigureAwait(false);

                return products;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateItemAsync(Information item)
        {
            try
            {
                var tracking = _Dbcontext.Update(item);

                await _Dbcontext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        public async Task<bool> TruncateTableAsync()
        {
            _Dbcontext.Informations.RemoveRange(_Dbcontext.Informations);
            await _Dbcontext.SaveChangesAsync();
            return true;
        }

        Task<Information> IDataStore<Information>.GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<Information> GetInformationAsync(string Seri_cto, string BCS)
        {
            var product = await _Dbcontext.Informations.Where(p => p.SERY_CTO == Seri_cto && p.LOAI_BCS == BCS).FirstOrDefaultAsync().ConfigureAwait(false);

            return product;
        }
    }
}
