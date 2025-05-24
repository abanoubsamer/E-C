using Core.Basic;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Meditor.Product.Commend.Models
{
    public class UpdateProductModelCommend : IRequest<Response<string>>
    {
        public string Id { get; set; }

        [MinLength(3)]
        public string? Name { get; set; }

        [MinLength(3)]
        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public List<IFormFile>? FormImages { get; set; }
        public IFormFile? MainImages { get; set; }

        public List<string>? IdIamgesDelteted { get; set; }

        public int? StockQuantity { get; set; }

        public string? SellerID { get; set; }

        public string? CategoryID { get; set; }
    }
}