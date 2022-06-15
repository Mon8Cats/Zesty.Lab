using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Dtos
{
    public record UpdateProductDto
    {
        [Required]
        public string Name { get; init; }
        public string Description { get; set; }
        
        [Required]
        [Range(1, 1000)]
        public decimal Price { get; init; }
    }
}