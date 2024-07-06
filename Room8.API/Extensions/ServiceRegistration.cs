using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Room8.Core.Abstractions;
using Room8.Core.Implementations;
using Room8.Domain.Entities;
using Room8.Infrastructure.Abstractions;
using Room8.Infrastructure.Implementations;
using Room8.API.Services;

namespace Room8.API.Extensions;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Room8 Api",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Key").Value);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = configuration.GetSection("JWT:Issuer").Value,
                ValidAudience = configuration.GetSection("JWT:Audience").Value
            };
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.WithOrigins(configuration.GetSection("FrontEndUrl").Value)
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
        });

        //add your interfaces and class injections here
        //eg
        services.AddSignalR();

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IApartmentService, ApartmentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRepository<Apartment>, Repository<Apartment>>();
        services.AddScoped<ISupportTicketService, SupportTicketService>();
        services.AddScoped<IRepository<SupportTicket>, Repository<SupportTicket>>();    
        services.AddScoped<IRepository<SavedApartment>, Repository<SavedApartment>>();
        services.AddScoped<IRepository<Message>, Repository<Message>>();
        services.AddScoped<IRepository<ChatRoom>, Repository<ChatRoom>>();
        services.AddScoped<IRepository<UserChatRoom>, Repository<UserChatRoom>>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IChatRoomService, ChatRoomService>();
    }
}
