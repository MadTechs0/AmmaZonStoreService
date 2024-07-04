using Microsoft.EntityFrameworkCore;
using StoreService.Context;
using StoreService.EntityModels;
using StoreService.Interface;
using StoreService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreService.Service
{
    public class OrderService: IOrderService
    {
        private readonly StoreContext _dbContext;   

        public OrderService(StoreContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<bool> ProcessOrder(OrderRequest newOrder)
        {
            try
            {
                List<Guid> productIds = new List<Guid>();
                foreach(var product in newOrder.Products)
                {
                    productIds.Add(product.id);
                }
                List<Product> productsInDb = await _dbContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                Guid? orderId = null;
                if(productsInDb.Count != newOrder.Products.Count)
                {
                    return false;
                }

                int index = 0;
                foreach(var product in newOrder.Products)
                {
                    var dbProduct = productsInDb.FirstOrDefault(p => p.Id == product.id);
                    if(dbProduct == null || dbProduct.Quantity < product.Quantity)
                    {
                        return false;
                    }
                    index++;
                }

                index = 0;
                foreach (var product in newOrder.Products)
                {
                    var dbProduct = productsInDb.First(p => p.Id == product.id);
                    dbProduct.Quantity -= product.Quantity;
                    index++;
                    await _dbContext.SaveChangesAsync();
                }

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
