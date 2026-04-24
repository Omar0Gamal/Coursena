
using Coursna.Core.Contracts;
using Coursna.Core.Domain.IdentityEntities;
using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Core.Service;
using Coursna.Core.ServiceContracts;
using Coursna.Infrastrcuter.DataContext;
using Coursna.Infrastrcuter.Identity;
using Coursna.Infrastrcuter.Repositories;
using Coursna.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Coursna
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

         

            builder.Services.AddControllers();
           
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            }) .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/denied";
            });
            builder.Services.AddScoped<IIdentitySeeder, IdentitySeeder>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IcourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ICourseCodeRepository, CourseCodeRepository>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

            builder.Services.AddScoped<ICourseCodeService, CourseCodeService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<ICourseContentRepository, CourseContentRepository>();
            builder.Services.AddScoped<ICourseContentService, CourseContentService>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<ITeacherDashboardRepository, TeacherDashboardRepository>();
            builder.Services.AddScoped<ITeacherDashboardService, TeacherDashboardService>();
            builder.Services.AddScoped<ILookUpService, LookUpService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddSignalR();

            builder.Services.AddScoped<AppDataSeeder>();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
        

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<IIdentitySeeder>();

                await seeder.SeedRolesAsync();
                await seeder.SeedAdminAsync();
                var dataSeeder = scope.ServiceProvider.GetRequiredService<AppDataSeeder>();
                await dataSeeder.SeedAsync();
            }
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<NotFoundMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
