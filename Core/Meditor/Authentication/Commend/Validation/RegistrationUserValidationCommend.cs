using Core.Meditor.Authentication.Commend.Model;
using Core.Meditor.User.Commend.Model;
using FluentValidation;
using Services.SellerServices;
using Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Authentication.Commend.Validation
{
    public class RegistrationUserValidationCommend:AbstractValidator<RegistrationUserModelCommend>
    {


        #region Fialds
        private readonly IUserServices _UserServices;
        #endregion

        #region Constractor
        public RegistrationUserValidationCommend(IUserServices UserServices)
        {
            _UserServices = UserServices;
            ApplyValiadtion();
            ApplyCustomValiadtion();
        }

        #endregion


        #region Validation
        public void ApplyValiadtion()
        {
            RuleFor(auth => auth.Email)
                .NotNull().WithMessage("Is Not Null")
                .NotEmpty().WithMessage("Is Required")
                .MaximumLength(30).WithMessage("Is Max Lenght is 30 Char")
                .MinimumLength(12).WithMessage("Is Min Lenght is 12 Char")
                .Matches(@"^[\w\.-]+@[a-zA-Z0-9\.-]+\.[cC][oO][mM]$").WithMessage("Invalid");

            RuleFor(auth => auth.UserName)
               .NotNull().WithMessage("Is Not Null")
               .NotEmpty().WithMessage("Is Required")
               .MaximumLength(20).WithMessage("Is Max Lenght is 20 Char")
               .MinimumLength(3).WithMessage("Is Min Lenght is 3 Char")
               .Matches(@"^[A-Z][a-z]+$").WithMessage("Must Start With Uppercase Letter");


            RuleFor(auth => auth.Password)
              .NotNull().WithMessage("Is Not Null")
              .NotEmpty().WithMessage("Is Required")
              .MinimumLength(6).WithMessage("Is Min Lenght is 6 Char");

            RuleFor(auth => auth.ComperPassword)
                .NotNull().WithMessage("Is Not Null")
                .NotEmpty().WithMessage("Is Required")
                .MinimumLength(6).WithMessage("Is Min Lenght is 6 Char")
                .Equal(x => x.Password).WithMessage("Not Match");


        }
        public void ApplyCustomValiadtion()
        {
            RuleFor(auth => auth.Email).MustAsync(async (key, CancellationToken) => !await _UserServices.EmailIsExist(key))
                .WithMessage("Aready Exist");
        }
        #endregion

       
       





    }
}
