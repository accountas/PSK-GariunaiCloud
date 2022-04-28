using GariunaiCloud_ToolSharing.IServices;

namespace GariunaiCloud_ToolSharing.Services;

public class Hmacsha512PasswordHashingStrategy : IPasswordHashingStrategy
{
    public void HashPassword(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        
        salt = hmac.Key;
        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPassword(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(salt);
        
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return !computedHash.Where((t, i) => t != hash[i]).Any();
    }
}