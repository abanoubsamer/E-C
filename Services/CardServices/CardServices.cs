using Domain.Models;
using Infrastructure.UnitOfWork;
using Services.ExtinsionServies;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CardServices
{
    public class CardServices : ICardServices
    {

        #region Failds
        private readonly IUnitOfWork _unitOfWork;
        #endregion


        #region Constractor
        public CardServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }

      
        #endregion


        #region Implemntation
        public async Task<ResultServices> AddCardItems(CardItem entity)
        {
            if(entity == null) return new ResultServices { Msg = "Invalid Card"};
            try
            {
                var exsit = await _unitOfWork.Repository<CardItem>().IsExistAsync(x => x.ProductID == entity.ProductID && x.CardId == entity.CardId);
                if (exsit) return new ResultServices { Msg = "Prodcut Aread Exist In Card" };
                await _unitOfWork.Repository<CardItem>().AddAsync(entity);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex) {
                return new ResultServices { Msg = ex.Message };
            }
          
        }

        public async Task<ResultServices> DeleteCardItemsUser(CardItem entity)
        {
            if(entity == null) return new ResultServices { Msg = " Invalid Card Item "};
            try
            {
            
                await _unitOfWork.Repository<CardItem>().DeleteAsync(entity);
                return new ResultServices { Succesd = true};
            }
            catch (Exception ex) {
                return new ResultServices { Msg = ex.Message};

            }


        }

        public async Task<CardItem> FindCardItemsById(string CardItmesId)
        {
            return await _unitOfWork.Repository<CardItem>().FindOneAsync(x => x.Id == CardItmesId);
        }

        public async Task<Card> GetCardItemsUser(string UserId)
        {
            return await _unitOfWork.Repository<Card>().FindOneWithNoTrackingAsync(x=>x.UserID == UserId);
        }

        public async Task<Card> HasCard(string UserId)
        {
            return await _unitOfWork.Repository<Card>().FindOneWithNoTrackingAsync(x => x.UserID == UserId);
        }

        public async Task<ResultServices> UpdateCardItemsUser(CardItem entity)
        {
            if (entity == null) return new ResultServices { Msg = "Invalid Card Items" };

            try
            {
                await _unitOfWork.Repository<CardItem>().UpdateAsync(entity);
                return new ResultServices { Succesd = true};
            }catch(Exception ex)
            {
                return new ResultServices { Msg = ex.Message };
            }
        }
        #endregion


    }
}
