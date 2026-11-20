
using CommanLib.DependencyInjection;
using Hangfire;
using Hangfire.Dashboard;
using Localization.Persistence.DependencyInjection;
using Localization.Service.DependencyInjection;
using Localization.ServiceAbstraction.Azur;
using Localization.ServiceAbstraction.Jobs;
using Microsoft.OpenApi.Models;
namespace LocalizationService
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
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddLocalizationService(builder.Configuration);
            builder.Services.AddLocalizationService(builder.Configuration);
            if (builder.Environment.IsDevelopment())
            builder.Services.AddSingleton<Localization.ServiceAbstraction.Azur.ITranslationProvider, FakeProvider>();

        
            builder.Services.AddTokenService(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Issue Service API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token.\n\nExample:\nBearer eyJhbGciOiJIUzI1NiIs..."
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
                        Array.Empty<string>()
                    }
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();

            app.UseAuthentication();    
            app.UseAuthorization();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                // Dev: localhost only. Elsewhere: authenticated Admin role only.
                Authorization = app.Environment.IsDevelopment()
                    ? [new LocalRequestsOnlyAuthorizationFilter()]
                    : [new HangfireDashboardAuthorizationFilter("Admin")]
            });
            app.MapControllers();

            app.Run();
        }
    }
}
