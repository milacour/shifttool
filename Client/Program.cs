using Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using ShiftTool.Client.Services;
using ShiftTool.Shared.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Bestem baseAddress baseret på miljøet
var baseAddress = builder.HostEnvironment.IsDevelopment()
    ? "https://localhost:7217"
    : "https://shifttoolserver.azurewebsites.net";

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<UserState>();
builder.Services.AddSingleton<MessageService>();

var app = builder.Build();

await app.RunAsync();
