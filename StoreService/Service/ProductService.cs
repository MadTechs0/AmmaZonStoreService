using AutoMapper;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StoreService.Service
{
    public class ProductService:IProductService
    {
        private readonly StoreContext _dbContext;
        private readonly IMapper _mapper;
        public ProductService(StoreContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                return await _dbContext.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Guid> PostProduct(NewProduct newProduct)
        {
            try
            {
                Product product = _mapper.Map<Product>(newProduct);
                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();
                return product.Id;
            }
            catch(Exception ex)
            {
                return Guid.Empty;
            }
        }
        public async Task<List<Product>> GetProductDetails(List<Guid> productIds)
        {
            try
            {
                if (productIds == null || !productIds.Any())
                {
                    return new List<Product>();
                }

                List<Product> products = await _dbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                return new List<Product>();
            }
        }
        public async Task<List<Product>> SearchProducts(string productName)
        {
            try
            {
                List<Product> products = await _dbContext.Products
                .Where(p => p.Name.ToLower().Contains(productName.ToLower()))
                .ToListAsync();

                return products;
            }
            catch(Exception ex)
            {
                return new List<Product>();
            }
        }
        public async Task<bool> MarkOutOfStock(Guid productId)
        {
            try
            {
                Product? product = await _dbContext.Products.Where(p => p.Id == productId).FirstOrDefaultAsync();
                if(product == null)
                {
                    return false;
                }
                product.Quantity = 0;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateQuantity(ProductRequest product)
        {
            try
            {
                Product? productDb = await _dbContext.Products.Where(p => p.Id == product.id).FirstOrDefaultAsync();
                if( productDb == null)
                {
                    return false;
                }
                productDb.Quantity = product.Quantity;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public async Task<bool> UpdatePrice(ProductRequest product)
        {
            try
            {
                Product? productDb = await _dbContext.Products.Where(p => p.Id == product.id).FirstOrDefaultAsync();
                if (productDb == null)
                {
                    return false;
                }
                productDb.Price = (decimal)product.price;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
