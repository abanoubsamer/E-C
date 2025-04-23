using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Dtos;
using Domain.Enums.Status;
using Domain.Dtos;

namespace Core.Meditor.Order.Queries.Response
{
    public class OrderPaginationResponse
    {
        public string OrderID { get; set; }

        public virtual UserDto User { get; set; }

        public string OrderDate { get; set; } 

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } // Pending, Shipped, Delivered, Cancelled

        public virtual PaymentDto Payment { get; set; }

        public virtual List<OrderItemsDto> OrderItems { get; set; } 

        public OrderPaginationResponse(Domain.Models.Order order)
        {
            OrderID = order.OrderID;  
            User = new UserDto {Email = order.User.Email, Id = order.User.Id , Name = order.User.Name} ;
            OrderDate = order.OrderDate.ToString();
            TotalAmount = order.TotalAmount;
            Status = order.Status;
            Payment = new PaymentDto {Amount = order.Payment.Amount , PaymentMethod = order.Payment.PaymentMethod , TransactionID = order.Payment.TransactionID } ;
            OrderItems =  order.OrderItems.Select(x=> new OrderItemsDto { 
                ProductID = x.ProductID,
                Quantity = x.Quantity,
                price = x.Price,
            }) .ToList();
        }

    }
}
