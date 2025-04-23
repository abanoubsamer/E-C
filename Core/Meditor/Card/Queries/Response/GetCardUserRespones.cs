using Core.Dtos;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Card.Queries.Response
{
    public class GetCardUserRespones
    {
        public string CardID { get; set; }
        
        public List<CardItemsDto> cardItemsDtos { get; set; }

    }
}
