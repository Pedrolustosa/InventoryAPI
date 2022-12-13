using InventoryAPI.Models;
using InventoryAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace InventoryAPI.ApiEndPoints
{
    public static class AuthenticationEndPoints
    {
        public static void MapAuthenticationEndPoint(this WebApplication app)
        {
            app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
            {
                if (userModel == null)
                {
                    return Results.BadRequest("Login Inválido");
                }
                if (userModel.UserName == "pedrolustosa" && userModel.Password == "pedro@123")
                {
                    var tokenString = tokenService.GenerateToken(app.Configuration["Jwt:Key"],
                        app.Configuration["Jwt:Issuer"],
                        app.Configuration["Jwt:Audience"],
                        userModel);
                    return Results.Ok(new { token = tokenString });
                }
                else
                {
                    return Results.BadRequest("Login Inválido");
                }
            }).Produces(StatusCodes.Status400BadRequest)
              .Produces(StatusCodes.Status200OK)
              .WithName("Login")
              .WithTags("Authentication");

        }
    }
}
