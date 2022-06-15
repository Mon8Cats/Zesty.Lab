using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Dtos
{
    public record ProductDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Description { get; set; }
        public decimal Price { get; init; }

        public DateTimeOffset CreatedDate { get; init;}
    }
}