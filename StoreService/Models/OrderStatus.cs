using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreService.Models
{
    public class OrderStatus
    {
        public Guid OrderId { get; set; }
        public bool Status { get; set; }
    }
}
