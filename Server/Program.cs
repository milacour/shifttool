using Microsoft.OpenApi.Models;
using ShiftTool.Server.Data;
using ShiftTool.Server.Repositories;
using ShiftTool.Shared.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Tilføj konfiguration og services her
var configuration = builder.Configuration;

// Registrer repositories som scoped services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Konfigurer databaseforbindelsen
var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton<INpgsqlConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));

// Tilføj controllers
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Konfigurer Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShiftTool API", Version = "v1" });
});


// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("https://blue-beach-0540da41e.4.azurestaticapps.net")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


// Tilføj sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Konfigurer HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShiftTool API V1"));
}

app.UseHttpsRedirection();
app.UseCors("MyCorsPolicy");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllers();

app.Run();
