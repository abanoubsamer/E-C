using Core.Basic;
using Core.Meditor.Card.Commend.Models;
using Domain.Models;
using MediatR;
using Services.CardServices;
using Services.ProductServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Card.Commend.Handler
{
    public class CardHnalderCommend : ResponseHandler,
        IRequestHandler<AddCardItemModelCommend, Response<string>>,
        IRequestHandler<DeleteCardItemModelCommend, Response<string>>,
        IRequestHandler<UpdateUserCardItemsModelCommend, Response<string>>
    {
        #region Fildes
        private readonly ICardServices _cardServices;
        private readonly IProductServices _productServices;
        #endregion

        #region Constractor
        public CardHnalderCommend(ICardServices cardServices, IProductServices productServices)
        {
            _productServices = productServices;
            _cardServices = cardServices;
        }
        #endregion

        #region Hnalder
        public async Task<Response<string>> Handle(AddCardItemModelCommend request, CancellationToken cancellationToken)
        {
            var productPrice = await _productServices.GetProductPriceByID(request.ProductID);
           
            if (productPrice == null)
            {
                return BadRequest<string>("Product not found or price unavailable.");
            
            }
            
            var HasCard = await _cardServices.HasCard(request.UserId);
            if (HasCard == null) return BadRequest<string>("Invalid UserId");
     
            var cardItems = new CardItem
            {
                CardId = HasCard.Id,
                ProductID = request.ProductID,
                Quantity = request.Quantity,
                Price = productPrice
            };
            var resutAdd = await _cardServices.AddCardItems(cardItems);
            if(!resutAdd.Succesd) return BadRequest<string>(resutAdd.Msg);

            return Success<string>("Succed Add Items");

        }

        public async Task<Response<string>> Handle(UpdateUserCardItemsModelCommend request, CancellationToken cancellationToken)
        {
            var carditems = await _cardServices.FindCardItemsById(request.CardItemsId);
            
            if (carditems == null) return BadRequest<string>("Not Found Card");
            
            carditems.Quantity = request.quantity;

            var result = await _cardServices.UpdateCardItemsUser(carditems);
            if(!result.Succesd) return BadRequest<string>(result.Msg);

            return Updated<string>("Succed Update Card Items");

        }

        public async Task<Response<string>> Handle(DeleteCardItemModelCommend request, CancellationToken cancellationToken)
        {
            var carditems = await _cardServices.FindCardItemsById(request.Id);
            if (carditems == null) return NotFound<string>("Not Found Cart Items");
            var result = await _cardServices.DeleteCardItemsUser(carditems);    
            if(!result.Succesd) return BadRequest<string>(result.Msg);

            return Deleted<string>("Succesd Delete Cart Items");

        }
        #endregion


    }
}
