using Core.Dtos;
using Core.Dtos.Product.Queries;
using Core.Meditor.Order.Queries.Response;
using Domain.Dtos;
using Domain.Enums.Status;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping.Order
{
    public partial class OrderProfile
    {
        //public string OrderID { get; set; }

        //public string OrderDate { get; set; }

        //public decimal TotalAmount { get; set; }

        //public OrderStatus Status { get; set; }

        //public virtual PaymentDto Payment { get; set; }

        //public virtual List<OrderItemsDto> OrderItems { get; set; }

        public void GetUserOrders()
        {

            CreateMap<Domain.Models.Order, GetUserOrdersResponse>()
                .ForMember(des => des.OrderID, opt => opt.MapFrom(src => src.OrderID))
                .ForMember(des => des.OrderDate, opt => opt.MapFrom(src => src.OrderDate.ToString()))
                .ForMember(des => des.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(des => des.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(des => des.Phone, opt => opt.MapFrom(src => new PhoneNumberDto { 
                    id = src.UserPhoneNumber.Id,
                    phoneNumber = src.UserPhoneNumber.PhoneNumber
                } ))

                .ForMember(des => des.shippingAddresses, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(des => des.Payment, opt => opt.MapFrom(
                    src =>
                    new PaymentDto
                    {
                        Amount = src.Payment.Amount,
                        PaymentMethod = src.Payment.PaymentMethod,
                        TransactionID = src.Payment.TransactionID
                    }))
                .ForMember(des => des.OrderItems, opt => opt.MapFrom(src => src.OrderItems.
                Select(x => new GetOrderItemsWithOrderDto
                {
                    Price = x.Price,
                    ProductID = x.ProductID,
                    Quantity = x.Quantity,
                    ImagesDto = x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault(),
                    Name = x.Product.Name,
                   
                }).ToList()
                ));
        }

    }
}
