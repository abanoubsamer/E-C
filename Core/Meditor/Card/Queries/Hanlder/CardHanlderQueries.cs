using Core.Basic;
using Core.Dtos;
using Core.Dtos.Product.Queries;
using Core.Meditor.Card.Queries.Models;
using Core.Meditor.Card.Queries.Response;
using Domain.Dtos;
using MediatR;
using Services.CardServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Card.Queries.Hanlder
{
    public class CardHanlderQueries : ResponseHandler,
        IRequestHandler<GetCardUserModelQueries, Response<GetCardUserRespones>>
    {
        private readonly ICardServices cardServices;



        #region Constracotr

        public CardHanlderQueries(ICardServices cardServices)
        {
            this.cardServices = cardServices;
        }

        #endregion Constracotr

        #region Hnalder

        public async Task<Response<GetCardUserRespones>> Handle(GetCardUserModelQueries request, CancellationToken cancellationToken)
        {
            var card = await cardServices.GetCardItemsUser(request._id);

            if (card == null) return NotFound<GetCardUserRespones>("Not Found Card");

            var cardMapping = new GetCardUserRespones
            {
                CardID = card.Id,
                cardItemsDtos = card.Items.Select(x => new CardItemsDto
                {
                    Id = x.Id,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Product = new GetProductDto
                    {
                        ProductID = x.Product.ProductID
                    ,
                        AverageRating = x.Product.AverageRating,
                        Description = x.Product.Description,
                        Name = x.Product.Name,
                        Price = x.Price,
                        Category = new CategoryDto { Id = x.Product.Product.Category.CategoryID, Name = x.Product.Product.Category.Name },
                        ImagesDto = x.Product.Images.Select(x =>
                        new ProductImagesDto
                        {
                            id = x.ImageID,
                            Image = x.ImageUrl,
                        }).ToList(),
                    }
                }).ToList()
            };

            return Success(cardMapping);
        }

        #endregion Hnalder
    }
}