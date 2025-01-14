using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Order : BaseEntity
    {
        [Required(ErrorMessage ="Customer Name Is Requiered")]
        public string CustomerName { get; set; }

        public double Total { get; set; }


        public DateTime OrderTime { get; set; }

        [MinLength(1, ErrorMessage = "Please add at least one product.")]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
