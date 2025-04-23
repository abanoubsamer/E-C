using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Dtos.Product.Queries;

namespace Domain.Dtos
{
    public class CardItemsDto
    {
        public string Id { get; set; }

        public GetProductDto Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }
}
