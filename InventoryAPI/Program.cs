using InventoryAPI.ApiEndPoints;
using InventoryAPI.AppServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAuthenticationJwt();

var app = builder.Build();

app.MapAuthenticationEndPoint();
app.MapCategoriesEndPoints();
app.MapProductsEndPoints();

var environment = app.Environment;
app.UseExceptionHandling(environment)
   .UseSwaggerMiddleware()
   .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();
app.Run();