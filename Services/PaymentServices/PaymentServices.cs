using Domain.Enums.Status;
using Domain.Models;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PaymentServices
{
    public class PaymentServices 
    {

        private readonly IUnitOfWork _unitOfWork;
        
        public PaymentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
      

        public async Task<bool> ConfirmPayment(string orderID, string transactionID)
        {
            var order = await _unitOfWork.Repository<Order>().FindOneAsync(o => o.OrderID == orderID && o.Status == OrderStatus.Pending);
            if (order == null)
                return false; // ❌ الطلب غير موجود أو حالته ليست Pending

            //order.Status = OrderStatus.Confirm; // ✅ تم تأكيد الدفع
            order.Payment.TransactionID = transactionID; 
            order.Payment.PaymentDate = DateTime.UtcNow;
      
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
      
            return true;
        }
    }
}
