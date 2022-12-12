using InventoryAPI.Models;

namespace InventoryAPI.Services;

public interface ITokenService
{
    string GenerateToken(string key, string issuer, string audience, UserModel user);
}
