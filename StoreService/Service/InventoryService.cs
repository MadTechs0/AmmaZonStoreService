using Microsoft.EntityFrameworkCore;
using StoreService.Context;
using StoreService.EntityModels;
using StoreService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreService.Service
{
    public class InventoryService: IInventoryService
    {
        private readonly StoreContext _dbContext;

        public InventoryService(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddCategory(string categoryName)
        {
            try
            {
                Category newCategory = new Category();
                newCategory.CategoryName = categoryName;
                if(await _dbContext.Categories.AnyAsync(c => c.CategoryName == categoryName))
                {
                    return Guid.Empty;
                }
                else
                {
                    await _dbContext.AddAsync(newCategory);
                    await _dbContext.SaveChangesAsync();

                    return newCategory.Id;
                }
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }

        public async Task<List<Category>> GetCategories()
        {
            try
            {
                return await _dbContext.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
