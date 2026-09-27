using Microsoft.EntityFrameworkCore;
using QuizForge.Desktop.Data;
using QuizForge.Desktop.Models;

namespace QuizForge.Desktop.Services
{
    public class CategoryService
    {
        public async Task<List<Category>> GetCategoriesAsync()
        {
            using var db = new AppDbContext();

            return await db.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }
    }
}