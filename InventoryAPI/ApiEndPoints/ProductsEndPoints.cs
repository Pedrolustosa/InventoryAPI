using InventoryAPI.Models;
using InventoryAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.ApiEndPoints
{
    public static class ProductsEndPoints
    {
        public static void MapProductsEndPoints(this WebApplication app)
        {
            app.MapGet("/products", async (AppDbContext db) => await db.Products.ToListAsync()).RequireAuthorization().WithTags("Product"); ;

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
            }).WithTags("Product"); 
        }
    }
}
