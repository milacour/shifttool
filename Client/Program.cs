
using Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using ShiftTool.Client.Services;
using ShiftTool.Shared.Interfaces;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7217")
});
builder.Services.AddBlazoredLocalStorage();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<UserState>();
builder.Services.AddSingleton<MessageService>();




// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7217")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();

await app.RunAsync();
