using Domain.Dtos;
using Domain.Enums.Oredring;
using Domain.Enums.Status;
using Domain.Models;
using Domain.OptionsConfiguration;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolWep.Data.Enums.Oredring;
using Services.ExtinsionServies;
using Services.PaymentServices;
using Services.Result;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace Services.OderServices
{
    public class OrderServices : IOrderServices
    {

        #region Fialds
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentServices _paymentService;
        private readonly IOptions<JwtOptions> _jwtOptions;
        #endregion

        #region Constractor
        public OrderServices(IUnitOfWork unitOfWork, IOptions<JwtOptions> jwtOptions, IPaymentServices paymentService)
        {
            _jwtOptions = jwtOptions;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Implemntation

        public async Task<ResultServices> AddOrder(Order entity)
        {
            if (entity == null)
                return new ResultServices { Msg = "invalid Order" };
            try
            {
                await _unitOfWork.Repository<Order>().AddAsync(entity);
                return new ResultServices { Succesd= true };
            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.Message };

            }
        }

        public Expression<Func<T, TResponse>> expression<T,TResponse>(Func<T, TResponse> func)
        {
           return x => func(x);
        }

        public async Task<Order> GetOrderById(string id)
        {
            return await _unitOfWork.Repository<Order>().FindOneAsync(x => x.OrderID == id);
        }

        public async Task<ResultServices> UpdateStatusOrder(string OrderId,string productId, OrderItemStatus Status)
        {
            if (OrderId == null) return new ResultServices { Msg = "Invalid OrderId" };

            try
            {
                var order = await _unitOfWork.Repository<OrderItem>().FindOneAsync(x => x.OrderID == OrderId && x.ProductID == productId); ;
                
                if (order == null) return new ResultServices { Msg = $"Not Found Order With Id {OrderId}" };
                
                  order.Status = Status ;
                
                await _unitOfWork.Repository<OrderItem>().UpdateAsync(order);
                return new ResultServices { Succesd = true };
            }
            catch (Exception ex) {
                return new ResultServices { Msg = ex.Message };
            }
        }

        #region User
        public async Task<ResultServices> CancelOrder(string Id)
        {
            if (Id.IsNullOrEmpty()) return new ResultServices { Msg = "Invalid Id " };

            try
            {
                var order = await _unitOfWork.Repository<Order>().FindOneAsync(x => x.OrderID == Id);
                if (order == null)
                    return new ResultServices { Msg = "Not Found Product" };
                //order.Status = OrderStatus.Cancelled;
                await _unitOfWork.Repository<Order>().UpdateAsync(order);
                return new ResultServices { Succesd = true };

            }
            catch (Exception ex)
            {
                return new ResultServices { Msg = ex.InnerException.Message };
            }
        }


        public async Task<OrderWithTokenResult> GetOrderWithJWT(Order entity)
        {
            var JwtOrderResult = new OrderWithTokenResult();
            //1
            if (entity == null) {
                JwtOrderResult.IsAuthenticated = false;
                JwtOrderResult.Messgage = "invalid Order";
                return JwtOrderResult;
            }
            //2 
            // Create Claims
            var Claim = await GetClaimOrder(entity);
            var NewClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, entity.UserID),
                new Claim("OrderData",JsonSerializer.Serialize(new Order
                {
                    OrderDate =entity.OrderDate,
                    OrderID = entity.OrderID,
                    OrderItems = entity.OrderItems.Select(x=>new OrderItem { 
                    Price = x.Price,
                    ProductID = x.ProductID,
                    SellerID = x.Product.SellerID,
                    Status = OrderItemStatus.Pending,
                    Quantity = x.Quantity,
                    }).ToList(),
                    UserID = entity.UserID,
                    AddressId = entity.AddressId,
                    PhoneId = entity.PhoneId,
                  
                   
                    TotalAmount = entity.TotalAmount,
                })),
            };
            //3 Gnrate token
            var Securtytoken = GenrateToken(NewClaims);
            JwtOrderResult.Token = new JwtSecurityTokenHandler().WriteToken(Securtytoken);
            JwtOrderResult.IsAuthenticated = true;
            JwtOrderResult.Order = Claim.Order;
            return JwtOrderResult;
        }

        private SecurityToken GenrateToken(List<Claim> claims)
        {
            // Gnrate key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
            //gnrate Descrptor
            var TokenDecrpotr = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Value.Audience,
                Issuer = _jwtOptions.Value.Issuer,
                Expires = DateTime.UtcNow.AddHours(_jwtOptions.Value.LiveTimeHouer),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var tokenhandler = new JwtSecurityTokenHandler();

            return tokenhandler.CreateToken(TokenDecrpotr);


        }

        private async Task<GetOrderClaimsResult> GetClaimOrder(Order entity)
        {
            if (entity == null || !entity.OrderItems.Any())

            return new GetOrderClaimsResult { Msg = "Invalid Order" };
           
            try
            {
                var productIds = entity.OrderItems.Select(x => x.ProductID).ToList();
                var products = await _unitOfWork.Repository<Domain.Models.Product>().FindMoreAsync(x => productIds.Contains(x.ProductID));
                if (!products.Any())
                    return new GetOrderClaimsResult { Msg = "Invalid Order" };


                foreach (var orderItem in entity.OrderItems)
                {
                    var matchingProduct = products.FirstOrDefault(p => p.ProductID == orderItem.ProductID);
                    if (matchingProduct != null)
                    {
                        orderItem.Product = matchingProduct;
                        orderItem.Price = matchingProduct.Price; // تحديث السعر
                    }
                }

                entity.TotalAmount = entity.OrderItems.Sum(x => x.Price * x.Quantity);
               

                return new GetOrderClaimsResult
                { Succesd = true, Order = entity };
            }
            catch (Exception ex) {
                return new GetOrderClaimsResult { Msg = ex.Message };
            }
   

        }


        public async Task<List<Order>> GetUserOrders(string Userid)
        {
            return await _unitOfWork.Repository<Order>().FindMoreAsync(x => x.UserID == Userid );
        }

        #endregion



        #region Admin
        public IQueryable<Order> FilterOrder(
         string? OrderId,
         string? ProductID,
         string? UserID,
         DateTime? OrderDate,
         OrderStatus? Status,
         string? TransactionID,
         string? PaymentMethod,
         OrederBy? orederBy,
         OrderOredringEnum? orderOredringEnum)
        {
            // Get Query
            var Query = GetOrderQuerybleAdmin();
            //Filter
            if (!OrderId.IsNullOrEmpty()) Query = Query.Where(x => x.OrderID.Contains(OrderId));
            if (!ProductID.IsNullOrEmpty()) Query = Query.Where(x => x.OrderItems.Any(x => x.ProductID.Contains(ProductID)));
            if (!UserID.IsNullOrEmpty()) Query = Query.Where(x => x.UserID.Contains(UserID));
            if (!OrderDate.ToString().IsNullOrEmpty()) Query = Query.Where(x => x.OrderDate.Equals(OrderDate));
            if (Status != null) Query = Query.Where(x => x.Status == Status);
            if (!TransactionID.IsNullOrEmpty()) Query = Query.Where(x => x.Payment.TransactionID.Contains(TransactionID));
            if (!PaymentMethod.IsNullOrEmpty()) Query = Query.Where(x => x.Payment.PaymentMethod.Contains(PaymentMethod));

            // order 

            return orderByAdmin(Query, orederBy, orderOredringEnum);
        }
        private IQueryable<Order> GetOrderQuerybleAdmin()
        {
            return _unitOfWork.Repository<Order>()
                   .GetQueryable()
                   .Include(x => x.User)
                   .Include(x => x.OrderItems)
                   .Include(x => x.Payment);

        }

        private IQueryable<Order> orderByAdmin(IQueryable<Order> orders, OrederBy? orederBy,
         OrderOredringEnum? orderOredringEnum)
        {
            // init 
            bool Asinding = orederBy == 0 || orederBy == null;
            orderOredringEnum ??= 0;


            switch (orderOredringEnum)
            {
                case OrderOredringEnum.Time:
                    orders = Asinding ? orders.OrderBy(x => x.OrderDate) : orders.OrderByDescending(x => x.OrderDate);
                    break;
                case OrderOredringEnum.TotalAmount:
                    orders = Asinding ? orders.OrderBy(x => x.TotalAmount) : orders.OrderByDescending(x => x.TotalAmount);
                    break;
                case OrderOredringEnum.Status:
                    orders = Asinding ? orders.OrderBy(x => x.Status) : orders.OrderByDescending(x => x.Status);
                    break;
            }

            return orders;

        }

        public async Task<ResultServices> DeleteOrderFormDb(string Id)
        {
            if (Id.IsNullOrEmpty()) return new ResultServices { Msg = "Invalid Id " };
            var Transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var Order = await _unitOfWork.Repository<Order>().FindOneAsync(x => x.OrderID == Id);
                if (Order == null)
                    return new ResultServices { Msg = "Not Found Product" };

                await _unitOfWork.Repository<OrderItem>().DeleteRangeAsync(Order.OrderItems);
                await _unitOfWork.Repository<Payment>().DeleteAsync(Order.Payment);
                await _unitOfWork.Repository<Order>().DeleteAsync(Order);
                await _unitOfWork.CommentAsync();
                return new ResultServices { Succesd = true };

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return new ResultServices { Msg = ex.InnerException.Message };
            }

        }

        #endregion






        #region Seller
        public IQueryable<GetSellerOrderDto> GetSellerOrders(
          string Sellerid,
          string? Searchtearm,
          DateTime? fromDate,
          DateTime? toDate,
          OrderItemStatus? Status,
          OrederBy? orederBy,
          OrderOredringEnum? orderOredringEnum)
        {
            // Get Query
            var Query = GetOrderQuerybleSeller(Sellerid);
            //Filter
            if (!string.IsNullOrEmpty(Searchtearm))
            {
                Query = Query.Where(x =>
                   x.OrderID.Contains(Searchtearm) ||
                   x.Product.Name.Contains(Searchtearm) ||
                   x.User.Email.Contains(Searchtearm) 
                
               );
            }

            if (fromDate.HasValue) Query = Query.Where(x => x.OrderDate >= fromDate.Value);
            if (toDate.HasValue) Query = Query.Where(x => x.OrderDate <= toDate.Value);

            if (Status.HasValue) Query = Query.Where(x => x.Status == Status);

            // order
            return orderBySeller(Query, orederBy, orderOredringEnum);
        }
        private IQueryable<GetSellerOrderDto> GetOrderQuerybleSeller(string Sellerid)
        {
            return _unitOfWork.Repository<OrderItem>()
                 .GetQueryable()
                  .AsNoTracking()
                 .Where(x => x.SellerID.Contains(Sellerid))
                 .Select(order => new GetSellerOrderDto
                 {
                     OrderID = order.OrderID,
                     User = new UserDto
                     {
                         Email = order.Order.User.Email,
                         Id = order.Order.User.Id,
                         Name = order.Order.User.Name
                     },
                     
                     OrderDate = order.Order.OrderDate,
                     TotalAmount = order.Quantity * order.Price,
                     Status = order.Status,
                     Product = new ProductDto
                     {
                         Id = order.Product.ProductID,
                         Name = order.Product.Name,
                     },
                     Quantity = order.Quantity,
                     Price = order.Price,
                     PhoneNumber = order.Order.UserPhoneNumber.PhoneNumber,
                     ShippingAddresses = new ShippingAddressesDto
                     {
                         AddressID = order.Order.ShippingAddress.AddressID,
                         City = order.Order.ShippingAddress.City,
                         Country = order.Order.ShippingAddress.Country,
                         HouseNumber = order.Order.ShippingAddress.HouseNumber,
                         PostalCode = order.Order.ShippingAddress.PostalCode,
                         Suburb = order.Order.ShippingAddress.Suburb,
                         State = order.Order.ShippingAddress.State,
                         Street = order.Order.ShippingAddress.Street
                     }
                 });
        }

       
       private IQueryable<GetSellerOrderDto> orderBySeller(IQueryable<GetSellerOrderDto> orders, OrederBy? orederBy,
     OrderOredringEnum? orderOredringEnum)
        {
            // init 
            bool Asinding = orederBy == 0 || orederBy == null;
            orderOredringEnum ??= 0;


            switch (orderOredringEnum)
            {
                case OrderOredringEnum.Time:
                    orders = Asinding ? orders.OrderBy(x => x.OrderDate) : orders.OrderByDescending(x => x.OrderDate);
                    break;
                case OrderOredringEnum.TotalAmount:
                    orders = Asinding ? orders.OrderBy(x => x.Price) : orders.OrderByDescending(x => x.Price);
                    break;
                case OrderOredringEnum.Status:
                    orders = Asinding ? orders.OrderBy(x => x.Status) : orders.OrderByDescending(x => x.Status);
                    break;
            }

            return orders;

        }

      
        #endregion



        #endregion


    }
}
