var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var sqliteConnectionString = builder.Configuration.GetSection("Sqlite").GetValue<string>("ConnectionString");

builder.Services
    .AddSqliteRepositories(sqliteConnectionString!)
    .AddServices();

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
