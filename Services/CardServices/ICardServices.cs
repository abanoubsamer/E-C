using Domain.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CardServices
{
    public interface ICardServices
    {
        public Task<ResultServices> AddCardItems(CardItem entity);

        public Task<Card> GetCardItemsUser(string UserId);

        public Task<ResultServices> UpdateCardItemsUser(CardItem entity);

        public Task<ResultServices> DeleteCardItemsUser(CardItem entity);

        public Task<ResultServices> DeleteCardItemsUser(string UserId);

        public Task<Card> HasCard(string UserId);

        public Task<CardItem> FindCardItemsById(string CardId);
    }
}