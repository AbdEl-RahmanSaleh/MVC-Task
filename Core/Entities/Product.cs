using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped] 
        public IFormFile? Image { get; set; }

        public double price { get; set; }
        public string? ImageName { get; set; }

        public bool IsDeleted { get; set; }

    }
}
