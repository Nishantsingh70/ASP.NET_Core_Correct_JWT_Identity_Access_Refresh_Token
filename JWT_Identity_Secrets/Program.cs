using Amazon;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using JWT_Identity_Secrets.Configuration;
using JWT_Identity_Secrets.GlobalExceptionMiddleware;
using JWT_Identity_Secrets.Models;
using JWT_Identity_Secrets.Repositories;
using JWT_Identity_Secrets.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Azure.Core.HttpHeader;

namespace JWT_Identity_Secrets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var env = builder.Environment.EnvironmentName;
            var appName = builder.Environment.ApplicationName;

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            /*
        
        Method 1:
             Suppose you have two AWS Secerts Manager and you want to configure the JWT Secret in one Secret Manager and
             Connection String in another one, then you need to inject both the configuration like below for getting correct data -
             
            Secret should be stored like this for this configuration
             Secret Names are -
             
            Secret1: "Development_JWT_Identity_Secrets_ConnectionStrings__DefaultConnection" secret manager
             Server = DESKTOP - 64SOF1U; Database = DevDB; User Id = admin_Dev; Password = MyDevStrongPassword; Trusted_Connection = True; TrustServerCertificate = True;

            Secret2: "JwtSettings" secret manager
             
            {
            "ValidateIssuer": "MyAuthWindowsServer",
            "ValidateAudience": "MyApiWindowsUsers",
            "SecretKey": "THIS_IS_MY_SuperSecure_JwtKey_123456"
            }
            */

            // Adding AWS Secrets Manager - 
            // This below configuration handle Development_JWT_Identity_Secrets_ConnectionStrings__DefaultConnection secret manager

            /*
             
            builder.Configuration.AddSecretsManager(region: RegionEndpoint.APSouth1,
                configurator: options =>
                {
                    options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}_");
                    options.KeyGenerator = (_, s) => s.Replace($"{env}_{appName}_", string.Empty)
                                                        .Replace("__", ":");

                    // if your key is rorating then this will help to fetch correct inform.
                    options.PollingInterval = TimeSpan.FromMinutes(5);
                }
                );

            // This below configuration handle JwtSettings secret manager
            builder.Configuration.AddSecretsManager(region: RegionEndpoint.APSouth1,
                configurator: options =>
                {
                    options.SecretFilter = entry => entry.Name == "JwtSettings";

                    //options.KeyGenerator = (_, s) =>
                    //s.Replace("JWT_Secrets", String.Empty);
                    options.KeyGenerator = (_, key) => key;


                    // if your key is rorating then this will help to fetch correct inform.
                    options.PollingInterval = TimeSpan.FromMinutes(5);
                });

            // These are for debugging point of view no business use case of it.
            builder.Services.Configure<DatabaseSetting>(builder.Configuration.GetSection(DatabaseSetting.SectionName));
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection(JWTSettings.SectionName));

            var Issuer = builder.Configuration["JwtSettings:ValidateIssuer"];
            var Audience = builder.Configuration["JwtSettings:ValidateAudience"];
            var SecretKey = builder.Configuration["JwtSettings:SecretKey"];
            var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

            */

            //================================================================================================


        //Method2:   

            /*
            Suppose you have 1 AWS Secerts Manager and you want to configure the JWT Secret & connection string both in
             one Secret Manager, then you need to inject only one AWS Secret manager configuration like
             below for getting correct data -


            Secret should be stored like this for this configuration

            Secret Names are -

            Jwt_ConnectionString_Settings secret manager
            {
               "ValidateIssuer": "MyAuthWindowsServer",
               "ValidateAudience": "MyApiWindowsUsers",
               "SecretKey": "THIS_IS_MY_SuperSecure_JwtKey_123456",
               "DefaultConnection": "Server=DESKTOP-64SOF1U;Database=DevDB;User Id=admin_Dev;Password=MyDevStrongPassword;Trusted_Connection=True;TrustServerCertificate=True;"
            }

            */

            // Add Secrets Manager
            builder.Configuration.AddSecretsManager(region: RegionEndpoint.APSouth1,
                configurator: options =>
                {
                    options.SecretFilter = entry => entry.Name == "Jwt_ConnectionString_Settings";
                    options.KeyGenerator = (_, key) => key;

                    // If your key is rotating then this will help to fetch correct inform.
                    options.PollingInterval = TimeSpan.FromMinutes(5);
                }

            );

            // These are for debugging point of view no business use case of it.
            builder.Services.Configure<Jwt_Connection_Settings>(builder.Configuration.GetSection(Jwt_Connection_Settings.SectionName));

            var Issuer = builder.Configuration["Jwt_ConnectionString_Settings:ValidateIssuer"];
            var Audience = builder.Configuration["Jwt_ConnectionString_Settings:ValidateAudience"];
            var SecretKey = builder.Configuration["Jwt_ConnectionString_Settings:SecretKey"];
            var connectionString = builder.Configuration["Jwt_ConnectionString_Settings:DefaultConnection"];


            builder.Services.AddDbContext<DevDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Adding JWT Bearer Token
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = Audience,
                    ValidIssuer = Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
                };

            });

            // Register all the services and repositories.
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // For handling global exception
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
