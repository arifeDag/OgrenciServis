
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OgrenciServis.DataAccess;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Logic.Services;
using System.Text;

namespace OgrenciServis.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "OgrenciServis API",
                    Version = "v1",
                });
                // JWT Authhentication içibn swager yapýlandýrmasý

                c.AddSecurityDefinition("Bearer ", new OpenApiSecurityScheme

                {
                    Description = "JWT Header",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"


                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference =new OpenApiReference
                            {
                                Type =ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            builder.Services.AddDbContext<OkulContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            //1. Çözüm
            builder.Services.AddScoped<IOgrenci, OgrenciServis.Logic.Services.OgrenciServisImpl>();
            builder.Services.AddScoped<IOgretmen, OgretmenServis>();
            builder.Services.AddScoped<ISinav, SinavServis>();
            builder.Services.AddScoped<ISinif, SinifServis>();
            builder.Services.AddScoped<IDers, DersServis>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            });

            var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
            var key = Encoding.UTF8.GetBytes(jwtSecretKey);

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(option =>
              {
                  option.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(key),
                      ValidateIssuer = true,
                      ValidIssuer = builder.Configuration["jwt:Issuer"],
                      ValidateAudience = true,
                      ValidAudience = builder.Configuration["Jwt:Audience"],
                      ValidateLifetime = true,
                      ClockSkew = TimeSpan.Zero,

                  };
              });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
