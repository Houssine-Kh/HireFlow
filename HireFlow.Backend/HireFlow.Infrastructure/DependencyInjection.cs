using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HireFlow.Application.Common.Interfaces.Auth;
using HireFlow.Application.Common.Interfaces.Persistence;
using HireFlow.Application.Common.Interfaces.Services;
using HireFlow.Domain.Candidates.Repositories;
using HireFlow.Infrastructure.Identity;
using HireFlow.Infrastructure.Persistence;
using HireFlow.Infrastructure.Persistence.Repositories;
using HireFlow.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HireFlow.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HireFlow.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<PublishDomainEventsInterceptor>();

            // DbContext with the Interceptor
            services.AddDbContext<HireFlowDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<PublishDomainEventsInterceptor>();

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                       .AddInterceptors(interceptor); 
            });

            services.AddIdentity<AppUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<HireFlowDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                // 1. Override default Cookie schemes to use JWT
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // 2. Configure how to validate the token
                options.SaveToken = true;
                options.RequireHttpsMetadata = false; // Set to true in Production
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    
                    // These must match your appsettings.json "JwtSettings"
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                    )
                };
            });

            services.AddScoped<ITokenService,TokenService>();
            services.AddScoped<IIdentityService,IdentityService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IEmailService, FileEmailService>();

            return services;
        }
    }
}