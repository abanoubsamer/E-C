using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Behavior
{
    public class ValidationBehiveor<TRequest, TResponse> : 
        IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {

        #region Filads
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        #endregion

        public ValidationBehiveor(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResult = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var Errors = validationResult.SelectMany(v => v.Errors).Where(e => e != null).ToList();
                if (Errors.Count != 0)
                {
                    var MSG = "Validation Errors";
                    throw new ValidationException(MSG,Errors); 
                }

            }
            return await next();
        }
    }
}
