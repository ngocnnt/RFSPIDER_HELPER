using System;
using System.Collections.Generic;

namespace APP_KTRA_ROUTER.Models
{
    public interface IItemRepository
    {
        void Add(Item item);
        void Update(Item item);
        Item Remove(string key);
        Item Get(string id);
        IEnumerable<Item> GetAll();
    }
}
