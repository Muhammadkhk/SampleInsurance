
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Sample.ApplicationService.PersonDetail.Command;
using Sample.Core.Infrastructure;
using Sample.Domain.PersonDetail;
using Sample.Framework.Infrastructure;
using Sample.Infrastructure;
using Sample.Infrastructure.Repositories;
using System.Data.Common;
using System.Text;

namespace CoreInspect.Core.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString =
                configuration["Database:ConnectionString"];

            builder.Services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<SampleDbContext>(
                    options => options.UseLazyLoadingProxies().UseNpgsql(connectionString));

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "your-issuer",
                    ValidAudience = "your-audience",
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                };
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddHangfire(config => config
                 .UseRecommendedSerializerSettings()
                 .UsePostgreSqlStorage(c =>
                 c.UseNpgsqlConnection(configuration["Database:HangfireConnection"])));

            builder.Services.AddHangfireServer();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.CustomSchemaIds(type => type.FullName.Replace("+", "_"));
                options.AddSecurityDefinition("customAuth", new OpenApiSecurityScheme
                {
                    Description = "Custom authentication using JWT tokens.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "customAuth"
                            }
                        },
                        new string[] { }
                    }
                });
            });




            builder.Services.AddScoped<DbConnection>(c => new NpgsqlConnection(connectionString));
            builder.Services.AddScoped<IPersonDetailRepository, PersonDetailRepository>();
            builder.Services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            builder.Services.AddScoped<PersonDetailCommandApplicationService>();

            builder.Services.AddSingleton<IConfiguration>(configuration);
            

            var app = builder.Build();


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelsExpandDepth(-1);
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
            });

            //  }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .SetIsOriginAllowed((host) => true)
               .AllowCredentials());

            app.MapControllers();

            app.Run();
        }
    }
}