using FakeAsana.API.Database;
using FakeAsana.API.Enterprise;

namespace FakeAsana.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IDataBaseService, DatabaseService>();
            builder.Services.AddScoped<IProjectEC, ProjectEC>();
            builder.Services.AddScoped<IToDoEC, ToDoEC>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient",
                    policy =>
                    {
                        policy.WithOrigins(
                                "http://localhost:15276",
                                "https://localhost:15276",
                                "http://localhost:5200",
                                "https://localhost:7200")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
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

            app.UseCors("AllowBlazorClient");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}