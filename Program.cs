using FullstackQnA_API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FullstackQnA_API
{
    public class Program
    {
        public static string Passcode = "";
        public static void Main(string[] args)
        {
            //var myAllowAllOrigins = "_AllowAllOrigins";

            var builder = WebApplication.CreateBuilder(args);
            Passcode = builder.Configuration["Passcode"];

            builder.Services.AddCors();

            // Build a connection string using secrets
            var conStrBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = builder.Configuration["DataSource"],
                UserID = builder.Configuration["DbUser"],
                Password = builder.Configuration["DbPassword"],
                InitialCatalog = builder.Configuration["InitCatalog"],
                PersistSecurityInfo = false,
                Encrypt = true
            };

            // Get the new connection string
            var connectionString = conStrBuilder.ConnectionString;

            // Add services to the container.
            builder.Services.AddControllers();
        
            builder.Services.AddDbContext<QuestionsContext>(
                // Use the connection string from appsettings.json
                opt => opt.UseSqlServer(connectionString)
            );
            builder.Services.AddDbContext<AnswersContext>(
                // Use the connection string from appsettings.json
                opt => opt.UseSqlServer(connectionString)
            );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Enable wwwroot statics :)
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors(p =>
            {
                p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}