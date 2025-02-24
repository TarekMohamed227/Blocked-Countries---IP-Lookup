using Application_Layer;
using Infrastructure_Layer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();   //  HttpClient Service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Register Services


builder.Services.AddSingleton<IBlockedCountryService, BlockedCountryService>();
builder.Services.AddSingleton<IGeolocationService, GeolocationService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
