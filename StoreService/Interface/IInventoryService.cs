using StoreService.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreService.Interface
{
    public interface IInventoryService
    {
        Task<Guid> AddCategory(string categoryName);
        Task<List<Category>> GetCategories();
    }
}
