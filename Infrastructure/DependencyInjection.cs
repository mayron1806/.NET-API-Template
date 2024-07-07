using Infrastructure.Services.Email;
using Infrastructure.Services.JWT;
using Infrastructure.Settings;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        // db
        services.AddDbContext<DatabaseContext>(options => options.UseSqlite("Data Source=../database.db"));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // settings
        var smtp = configuration.GetSection("SMTP").Get<SMTPSettings>() ?? throw new Exception("SMTP settings not found");
        var jwt = configuration.GetSection("JWT").Get<JWTSettings>() ?? throw new Exception("JWT settings not found");
        services.AddSingleton(smtp);
        services.AddSingleton(jwt);

        // services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<JWTService>();
        
        return services;
    }
}
