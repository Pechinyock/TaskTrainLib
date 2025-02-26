using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TT.Core;

public static class CommonSerivces
{
    public static IServiceCollection AddSwaggerGenAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    }
                    , new List<string>()
                }
            });
        });
        return services;
    }

    public static IServiceCollection AddJwtAuth(this IServiceCollection services)
    {
        var auth = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        auth.AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            var authOptions = AuthenticationDefaults.GetDefaultOptions();
            options.TokenValidationParameters = AuthenticationDefaults.GetValidationParameters(authOptions);
        });

        return services;
    }

    public static IServiceCollection AddRabbitMQPublisher(this IServiceCollection services, string host) 
    {
        if(String.IsNullOrEmpty(host))
            throw new ArgumentNullException(nameof(host));

        services.AddSingleton<IMessagePublisher, RabbitMQPublisher>();
        services.Configure<RabbitMQPublisherOptions>(options =>
        {
            options.Host = host;
        });
        return services;
    }

    public static IServiceCollection AddRabbitMQSubscriber(this IServiceCollection services
        , string host
        , ExchangeInfo exchangeInfo
        , Action<byte[]> onMessageReceived) 
    {
        services.AddHostedService<RabbitMQSubscriber>();
        services.Configure<RabbitMQSubscriberOptions>(options =>
        {
            options.Host = host;
            options.ExchangeName = exchangeInfo.Name;
            options.ExchangeType = exchangeInfo.ExchangeType;
            options.OnMessageRecived = onMessageReceived;
        });

        return services;
    }
}
