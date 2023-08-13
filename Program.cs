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
            var CorsAll = "AllowAll";

            var builder = WebApplication.CreateBuilder(args);
            SqlConnectionStringBuilder conStrBuilder = null;
            // if development, read from app settinfs
            if (builder.Environment.IsDevelopment())
            {
                Passcode = builder.Configuration["Passcode"];
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy.WithOrigins(builder.Configuration["localClient"], builder.Configuration["firebaseClient"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });

                // Build a connection string using secrets
                conStrBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = builder.Configuration["DataSource"],
                    UserID = builder.Configuration["DbUser"],
                    Password = builder.Configuration["DbPassword"],
                    InitialCatalog = builder.Configuration["InitCatalog"],
                    PersistSecurityInfo = false,
                    Encrypt = true
                };

            } 
            else // read from environment variables
            {
                Passcode = Environment.GetEnvironmentVariable("Passcode");
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy.WithOrigins(Environment.GetEnvironmentVariable("localClient"), Environment.GetEnvironmentVariable("firebaseClient"))
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });

                // Build a connection string using secrets
                conStrBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = Environment.GetEnvironmentVariable("DataSource"),
                    UserID = Environment.GetEnvironmentVariable("DbUser"),
                    Password = Environment.GetEnvironmentVariable("DbPassword"),
                    InitialCatalog = Environment.GetEnvironmentVariable("InitCatalog"),
                    PersistSecurityInfo = false,
                    Encrypt = true
                };
            }


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

            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}