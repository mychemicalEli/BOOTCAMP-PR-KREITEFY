namespace api.Application.Services.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool CheckPassword(string storedPassword, string inputPassword);
}