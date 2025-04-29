using Core.Meditor.Authentication.Commend.Model;
using FluentValidation;
using Services.SellerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Meditor.Authentication.Commend.Validation
{
    public class RegistrationSellerValidationCommend : AbstractValidator<RegistrationSellerModelCommend>
    {
        #region Fialds

        private readonly ISellerServices _sellerServices;

        #endregion Fialds

        #region Constractor

        public RegistrationSellerValidationCommend(ISellerServices sellerServices)
        {
            _sellerServices = sellerServices;
            ApplyValiadtion();
            ApplyCustomValiadtion();
        }

        #endregion Constractor

        #region Validation

        public void ApplyValiadtion()
        {
            RuleFor(auth => auth.Email)
                .NotNull().WithMessage("Is Not Null")
                .NotEmpty().WithMessage("Is Required")
                .MaximumLength(60).WithMessage("Is Max Lenght is 60 Char")
                .MinimumLength(12).WithMessage("Is Min Lenght is 12 Char")
                .Matches(@"^[\w\.-]+@[a-zA-Z0-9\.-]+\.[cC][oO][mM]$").WithMessage("Invalid");

            RuleFor(auth => auth.ShopName)
               .NotNull().WithMessage("Is Not Null")
               .NotEmpty().WithMessage("Is Required")
               .MaximumLength(40).WithMessage("Is Max Lenght is 40 Char")
               .MinimumLength(5).WithMessage("Is Min Lenght is 5 Char");

            RuleFor(auth => auth.ContactInfo)
               .NotNull().WithMessage("Is Not Null")
               .NotEmpty().WithMessage("Is Required")
               .MaximumLength(40).WithMessage("Is Max Lenght is 40 Char")
               .MinimumLength(5).WithMessage("Is Min Lenght is 5 Char");

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
            RuleFor(auth => auth.Email).MustAsync(async (key, CancellationToken) => !await _sellerServices.EmailIsExist(key))
                .WithMessage("Aready Exist");
        }

        #endregion Validation
    }
}