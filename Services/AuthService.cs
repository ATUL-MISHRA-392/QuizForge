using Microsoft.EntityFrameworkCore;
using QuizForge.Desktop.Data;
using QuizForge.Desktop.Models;

namespace QuizForge.Desktop.Services
{
    public class AuthService
    {
        public async Task<User?> LoginAsync(string email, string password)
        {
            using var db = new AppDbContext();

            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            bool validPassword = BCrypt.Net.BCrypt.Verify(
                password,
                user.PasswordHash);

            return validPassword ? user : null;
        }

        public async Task<bool> RegisterAsync(
            string fullName,
            string email,
            string password)
        {
            using var db = new AppDbContext();

            bool exists = await db.Users
                .AnyAsync(u => u.Email == email);

            if (exists)
                return false;

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "Student",
                CreatedAt = DateTime.Now
            };

            db.Users.Add(user);

            await db.SaveChangesAsync();

            return true;
        }
    }
}