using Coursna.Infrastrcuter.DataContext;
using Coursna.Core.Domain.Entities;

public class AppDataSeeder
{
    private readonly AppDbContext _context;

    public AppDataSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        // Subjects
        if (!_context.Subjects.Any())
        {
            _context.Subjects.AddRange(
                new Subject { Name = "Math" },
                new Subject { Name = "Physics" },
                new Subject { Name = "Programming" },
                new Subject { Name = "Bio" }
               
            );
        }

        // Grades
        if (!_context.Grades.Any())
        {
            _context.Grades.AddRange(
                    new Grade { Id = 1, Name = "First Secondary" },
                     new Grade { Id = 2, Name = "Second Secondary" },
                    new Grade { Id = 3, Name = "Third Secondary" },
                    new Grade { Id=4,Name= "First Preparatory" },
                     new Grade { Id = 5, Name = "Second Preparatory" },
                      new Grade { Id = 6, Name = "Third Preparatory" }
            );
        }

        await _context.SaveChangesAsync();
    }
}