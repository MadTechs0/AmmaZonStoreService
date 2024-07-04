using StoreService.Models;
using StoreService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreService.Interface
{
    public interface IOrderService
    {
        Task<bool> ProcessOrder(OrderRequest newOrder);
    }
}
