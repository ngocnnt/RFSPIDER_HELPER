using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APP_KTRA_ROUTER.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<bool> TruncateTableAsync();
        Task<T> GetItemAsync(string id);
        Task<List<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
