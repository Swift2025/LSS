// Program.cs
using LaptopSupport.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// --- Add detailed debug logging ---
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// --- Configure Kestrel to listen on ALL network interfaces ---
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5215, o => o.Protocols = HttpProtocols.Http2);
});


// --- Register Services and Interceptors ---
builder.Services.AddSingleton<ServerLoggingInterceptor>();
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ServerLoggingInterceptor>();
});
builder.Services.AddSingleton<WingetManager>();
builder.Services.AddSingleton<SystemInfoService>();
builder.Services.AddSingleton<EnvironmentManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGrpcService<SupportServiceImpl>();
app.MapGrpcService<AdminServiceImpl>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

var logger = app.Services.GetRequiredService<ILogger<Program>>();
var addresses = app.Urls;
logger.LogInformation("==================================================================");
logger.LogInformation($"Application is running. Listening on addresses: {string.Join(", ", addresses)}");
logger.LogInformation("The Python gRPC client should connect to one of these addresses.");
logger.LogInformation("==================================================================");


app.Run();
