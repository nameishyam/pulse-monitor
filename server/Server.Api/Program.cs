using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.Api.Extensions;
using Server.Infrastructure.Services;
using Server.Infrastructure.Workers;
using System.Text;
using Server.Domain.Interfaces.Infrastructure;
using Supabase;
using SupabaseOptions = Server.Domain.Dto.Options.SupabaseOptions;

namespace Server.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        var jwtSection = builder.Configuration.GetSection("Jwt");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSection["Key"]!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["access_token"];

                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("Frontend", policy =>
            {
                policy
                    .WithOrigins(builder.Configuration
                        .GetSection("Cors:AllowedOrigins")
                        .Get<string[]>() ?? [])
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        builder.Services.Configure<SupabaseOptions>(
            builder.Configuration.GetSection("Supabase"));

        builder.Services.AddSingleton<Client>(_ =>
        {
            var options = builder.Configuration
                .GetSection("Supabase")
                .Get<SupabaseOptions>();

            var client = new Client(
                options!.Url,
                options.Key);

            client.InitializeAsync()
                .GetAwaiter()
                .GetResult();

            return client;
        });

        builder.Services.AddRepository(builder.Configuration);
        builder.Services.AddService(builder.Configuration);

        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IMonitorChecker, MonitorChecker>();

        builder.Services.AddHostedService<MonitorWorker>();

        builder.Logging.AddFilter(
            "Microsoft.EntityFrameworkCore.Database.Command",
            LogLevel.Warning);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.Use(async (context, next) =>
        {
            if (context.Request.Path == "/")
            {
                await context.Response.WriteAsync("imdb api application");
                return;
            }
            await next();
        });

        app.UseHttpsRedirection();

        app.UseCors("Frontend");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}