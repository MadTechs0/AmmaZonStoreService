using StoreService.EntityModels;
using StoreService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreService.Interface
{
    public interface IProductService
    {
        Task<List<Product>> GetProductDetails(List<Guid> productIds);
        Task<IEnumerable<Product>> GetProducts();
        Task<Guid> PostProduct(NewProduct newProduct);
        Task<List<Product>> SearchProducts(string productName);
        Task<bool> MarkOutOfStock(Guid productId);
        Task<bool> UpdateQuantity(ProductRequest product);
        Task<bool> UpdatePrice(ProductRequest product);
    }
}
