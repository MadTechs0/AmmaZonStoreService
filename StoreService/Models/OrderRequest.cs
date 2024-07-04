using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreService.Models
{
    public class OrderRequest
    {
        public Guid Id { get; set; }
        public int OrderValue { get; set; }
        public bool IsConfirmed { get; set; }
        public List<ProductRequest> Products { get; set; }
    }
}
