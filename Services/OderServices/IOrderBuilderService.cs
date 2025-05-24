using Azure;
using Domain.Dtos.Product.Commend;
using Domain.Models;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OderServices
{
    public interface IOrderBuilderService
    {
        Task<(Order order, ResultServices result)> BuildOrderAsync(Order request);
    }
}