using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FortressTrialTask_JanJeffersonLam.Data;

namespace FortressTrialTask_JanJeffersonLam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database configuration
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 21))));

            // Add services to the container.
            builder.Services.AddControllers();
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

            // Ensure static files and default files are used correctly
            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseRouting(); // Make sure routing is enabled

            app.UseAuthorization();

            // Map controllers
            app.MapControllers();

            // Map fallback route to index.html
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
