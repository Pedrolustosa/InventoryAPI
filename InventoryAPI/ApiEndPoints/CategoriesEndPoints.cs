using InventoryAPI.Models;
using InventoryAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.ApiEndPoints
{
    public static class CategoriesEndPoints
    {
        public static void MapCategoriesEndPoints(this WebApplication app)
        {
            app.MapGet("/categories", async (AppDbContext db) => await db.Categories.ToListAsync()).RequireAuthorization().WithTags("Category");

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

                if (category is null)
                    return Results.NotFound();

                db.Categories.Remove(category);
                await db.SaveChangesAsync();

                return Results.NoContent();
            }).WithTags("Category");

        }
    }
}
