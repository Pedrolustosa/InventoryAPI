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

app.MapGet("/categories", async(AppDbContext db) => await db.Categories.ToListAsync()).WithTags("Category");

app.MapGet("/category/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categories.FindAsync(id) is Category category ? Results.Ok(category) : Results.NotFound();
}).WithTags("Category");

app.MapPost("/categories", async ([FromBody] Category category, [FromServices] AppDbContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();

    return Results.Created($"/categories/{category.CategoryId}", category);

}).Accepts<Category>("application/json")
  .Produces<Category>(StatusCodes.Status201Created)
  .WithName("NewCategory")
  .WithTags("Category");

app.MapPut("categories/{id:int}", async (int id, Category category, AppDbContext db) =>
{
    if (category.CategoryId != id)
        return Results.BadRequest();

    var categoryDB = await db.Categories.FindAsync(id);

    if (categoryDB is null) return Results.NotFound();

    categoryDB.Name = category.Name;
    categoryDB.Description = category.Description;

    await db.SaveChangesAsync();
    return Results.Ok(category);
}).WithTags("Category");

app.MapDelete("/categories/{id:int}", async (int id, AppDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);

    if(category is null)
        return Results.NotFound();

    db.Categories.Remove(category);
    await db.SaveChangesAsync();

    return Results.NoContent();
}).WithTags("Category"); 

#endregion

#region Product

app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync()).WithTags("Product"); ;

app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Products.FindAsync(id) is Product product ? Results.Ok(product) : Results.NotFound();
}).WithTags("Product"); ;

app.MapPost("/products", async ([FromBody] Product product, [FromServices] AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();

    return Results.Created($"/products/{product.ProductId}", product);

}).Accepts<Product>("application/json")
  .Produces<Product>(StatusCodes.Status201Created)
  .WithName("NewProduct")
  .WithTags("Product"); ;

app.MapPut("products/{id:int}", async (int id, Product product, AppDbContext db) =>
{
    if (product.ProductId != id)
        return Results.BadRequest();

    var productDB = await db.Products.FindAsync(id);

    if (productDB is null) return Results.NotFound();

    productDB.Name = product.Name;
    productDB.Description = product.Description;
    productDB.Price = product.Price;
    productDB.Image = product.Image;
    productDB.PurchaseDate = product.PurchaseDate;
    productDB.Inventory = product.Inventory;
    productDB.CategoryId = product.CategoryId;

    await db.SaveChangesAsync();
    return Results.Ok(product);
}).WithTags("Product");

app.MapDelete("/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null)
        return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    return Results.NoContent();
}).WithTags("Product"); ;

#endregion

// Configure the HTTP request pipeline. - Configure
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();