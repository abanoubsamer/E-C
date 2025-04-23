using Core.Basic;
using Core.Meditor.Seller.Commend.Models;
using MediatR;
using Services.SellerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Seller.Commend.Handler
{
    public class SellerHnadlerCommend : ResponseHandler,
        IRequestHandler<SellerEmialIsExistModel, Response<bool>>
    {
        private readonly ISellerServices sellerServices;

        #region Fialds

        #endregion

        #region Constractor
        public SellerHnadlerCommend(ISellerServices sellerServices)
        {
            this.sellerServices = sellerServices;
        }
        #endregion

        #region Handler

        #endregion

        public async Task<Response<bool>> Handle(SellerEmialIsExistModel request, CancellationToken cancellationToken)
        {
            return Success(await this.sellerServices.EmailIsExist(request.Email));
        }
    }
}
