using AutoMapper.Configuration.Annotations;
using Core.Basic;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Core.Meditor.Product.Commend.Models
{
    public class AddProductModelCommend : IRequest<Response<string>>
    {
        [MinLength(3)]
        public string Name { get; set; }

        [MinLength(3)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public List<IFormFile> FormImages { get; set; } = new List<IFormFile>();

        public IFormFile MainImage { get; set; }

        public int StockQuantity { get; set; }

        public string SellerID { get; set; }

        public string CategoryID { get; set; }

        public string SKU { get; set; }
    }
}