using Core.Behavior;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;


namespace Core
{
    public static class ModelCoreDependencies
    {
        public static IServiceCollection AddCoredDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
           services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehiveor<,>));
            return services;
        }


    }
}
