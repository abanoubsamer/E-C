using Core.Basic;
using Core.Meditor.Reviews.Queries.Response;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Reviews.Queries.Models
{
    public class GetRatingStatisticsModels:IRequest<Response<GetRatingStatisticsResponse>>
    {

        public string productid { get; set; }
        public GetRatingStatisticsModels(string id)
        {
            productid = id;
        }
    }
}
