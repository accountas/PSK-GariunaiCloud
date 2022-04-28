namespace GariunaiCloud_ToolSharing.IServices;

public interface IPasswordHashingStrategy
{
    public void HashPassword(string password, out byte[] hash, out byte[] salt);
    public bool VerifyPassword(string password, byte[] hash, byte[] salt);
}