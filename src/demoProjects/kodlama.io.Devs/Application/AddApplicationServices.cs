using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using Core.Application.Pipelines.Validation;
using Application.Features.ProgrammingLanguages.Rules;
using Application.Features.Technologies.Rules;
using Application.Features.Users.Rules;
using Core.Security.JWT;
using Application.Features.SocialMedias.Rules;
using Application.Services.AuthService;
using Core.Application.Pipelines.Authorization;
using Microsoft.IdentityModel.Tokens;
using Core.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Application.Features.OperationClaims.Rules;
using Application.Features.UserOperationClaims.Rules;
using Core.ElasticSearch;
using Core.Application.Pipelines.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Logger;
using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.Application.Pipelines.Caching;
using Core.Mailing.MailKitImplementations;
using Core.Mailing;
using Application.Services.UserService;

namespace Application
{
     public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<ProgrammingLanguageBusinessRules>();
            services.AddScoped<TechnologyBusinessRules>();
            services.AddScoped<AuthBusinessRules>();
            services.AddScoped<SocialMediaBusinessRules>();
            services.AddScoped<OperationClaimBusinessRules>();
            services.AddScoped<UserOperationClaimBusinessRules>();
            
            
            

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheRemovingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<IMailService, MailKitMailService>();
            services.AddScoped<IUserService, UserManager>();

            services.AddSingleton<LoggerServiceBase, FileLogger>();
            services.AddSingleton<IElasticSearch, ElasticSearchManager>();

            return services;

        }
       
    }

   
}
