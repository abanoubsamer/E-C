using Core.Basic;
using Core.Meditor.Mail.Commend.Models;
using Core.Meditor.Order.Commend.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Cms;
using Services.MailServices;
using Services.SellerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Mail.Commend.Handler
{
    public class MialHandlerCommend : ResponseHandler,
        IRequestHandler<SendEmailOtp, Response<string>>,
        IRequestHandler<VerifyOtpModel, Response<string>>
    {
        private readonly IMailServices mailServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MialHandlerCommend(IMailServices mailServices, IHttpContextAccessor httpContextAccessor)
        {
         _httpContextAccessor = httpContextAccessor;
            this.mailServices = mailServices;
        }


        public async Task<Response<string>> Handle(SendEmailOtp request, CancellationToken cancellationToken)
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            DateTime expiryTime = DateTime.UtcNow.AddMinutes(5);
            string htmlTemplate = File.ReadAllText("wwwroot/Templete/OtpTemplet.html");
            _httpContextAccessor.HttpContext?.Session.SetString($"OTP_{request.Email}", otp.ToString());
            _httpContextAccessor.HttpContext?.Session.SetString($"OTP_Expiry_{request.Email}", expiryTime.ToString());
            string emailBody = htmlTemplate
                     .Replace("{{OTP_CODE}}", otp.ToString())
                     .Replace("{{VERIFY_LINK}}", "https://yourwebsite.com/verify");

             await mailServices.SendMail(request.Email, "VERIFY EMAIL", emailBody);

            return Success("Succed Send Email");

        }

        public async Task<Response<string>> Handle(VerifyOtpModel request, CancellationToken cancellationToken)
        {

            string? storedOtp = _httpContextAccessor.HttpContext.Session.GetString($"OTP_{request.Email}");
            string? Expiration = _httpContextAccessor.HttpContext.Session.GetString($"OTP_Expiry_{request.Email}");

            if (storedOtp == null || Expiration == null)
            {
                return BadRequest<string>("OTP expired. Please request a new one.");
            }

            if (DateTime.UtcNow > DateTime.Parse(Expiration))
            {
                _httpContextAccessor.HttpContext.Session.Remove($"OTP_{request.Email}");
                _httpContextAccessor.HttpContext.Session.Remove($"OTP_Expiry_{request.Email}");

                return BadRequest<string>("OTP expired. Please request a new one.");
            }

            if (request.otp.ToString() != storedOtp)
            {
                return BadRequest<string>("Invalid OTP.");
            }

            // ✅ إذا كان OTP صحيح، نحذفه من الجلسة
            _httpContextAccessor.HttpContext.Session.Remove($"OTP_{request.Email}");
            _httpContextAccessor.HttpContext.Session.Remove($"OTP_Expiry_{request.Email}");

            return Success("OTP verified successfully.");
        }

    }
}
