
using Hangfire;
using Hangfire.Dashboard;
using Localization.Persistence.DependencyInjection;
using Localization.Service.DependencyInjection;
using Localization.ServiceAbstraction.Jobs;
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

            builder.Services.AddSwaggerGen();

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
