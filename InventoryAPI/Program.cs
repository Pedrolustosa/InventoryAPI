using InventoryAPI.Models;
using InventoryAPI.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. - ConfigureService
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefautConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, 
                                                 ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

//Endpoints
#region Category

app.MapPost("/categories", async ([FromBody] Category category, [FromServices] AppDbContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();

    return Results.Created($"/categories/{category.CategoryId}", category);
}).Accepts<Category>("application/json")
  .Produces<Category>(StatusCodes.Status201Created)
  .WithName("NewCategory")
  .WithTags("Setter");

#endregion


// Configure the HTTP request pipeline. - Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();