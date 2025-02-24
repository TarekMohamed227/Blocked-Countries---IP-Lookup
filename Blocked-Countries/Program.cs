using Application_Layer;
using Infrastructure_Layer;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);



// ✅ Get Configuration from builder
var configuration = builder.Configuration;


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();

// ✅ Correct service registration
builder.Services.AddSingleton<IBlockedCountryService, BlockedCountryService>();
builder.Services.AddSingleton<IGeolocationService, GeolocationService>(); 
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IGeolocationService>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    return new GeolocationService(httpClient, configuration);
});
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
