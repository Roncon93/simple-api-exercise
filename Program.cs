using Microsoft.EntityFrameworkCore;
using SimpleApiProject.Configuration;
using SimpleApiProject.Data.Sqlite.Contexts;
using SimpleApiProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ApplicationConfiguration>(builder.Configuration);

builder.Services.AddDbContextFactory<CompanySqliteDbContext>(
    options => options.UseSqlite(builder.Configuration.GetSection("Sqlite").GetValue<string>("ConnectionString")));

builder.Services.AddTransient<IDataImportService, DataImportService>();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
