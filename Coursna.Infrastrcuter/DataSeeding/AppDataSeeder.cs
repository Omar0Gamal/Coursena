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
                    new Grade { Name = "First Secondary" },
                     new Grade {Name = "Second Secondary" },
                    new Grade { Name = "Third Secondary" },
                    new Grade {Name= "First Preparatory" },
                     new Grade { Name = "Second Preparatory" },
                      new Grade { Name = "Third Preparatory" }
            );
        }

        await _context.SaveChangesAsync();
    }
}