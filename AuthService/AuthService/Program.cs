using Auth.Domain.Contractts;
using Auth.Persistence.DependencyInjection;
using Auth.Service;
using Auth.Service.DependanceInjection;
using CommanLib.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace Auth_Services
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddPersistenceServices(builder.Configuration);

            builder.Services.AddTokenService(builder.Configuration);
            builder.Services.AddServices();
            //builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSetting"));

            //builder.Services.AddTokenService(builder.Configuration);

            //builder.Services.Configure<JwtOption>(builder.Configuration.GetSection(JwtOption.SectionName));

            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    var jwt = builder.Configuration.GetSection(JwtOption.SectionName).Get<JwtOption>();
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidateLifetime = true,
            //        ValidIssuer = jwt.Issuer,
            //        ValidAudience = jwt.Audience,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey))
            //    };
            //});

            var app = builder.Build();


            //dataseed

            var scape = app.Services.CreateScope();
            var dbInitializer = scape.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // serves wwwroot/uploads/* (profile pictures) as static content
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
