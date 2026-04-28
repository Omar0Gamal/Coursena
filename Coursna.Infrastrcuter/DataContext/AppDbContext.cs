using Coursna.Core.Domain.Entities;
using Coursna.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Infrastrcuter.DataContext
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseContent> CourseContents { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<CourseCode> courseCodes { get; set; }
        public DbSet<CourseReview> courseReviews { get; set; }
        public DbSet<Quiz> quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<QuizAttempt> QuizAttempts{ get; set; }
        public DbSet<StudentResponse> StudentResponses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // el student leh teacher wa7d bs w el teacher leh kza student
            builder.Entity<ApplicationUser>()
                .HasOne(u => u.Teacher)
                .WithMany(t => t.Students)
                .HasForeignKey(u => u.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            // el message leha sender wa7d bs w el user leh kza message

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            // el message leha receiver wa7d bs w el user can receive many messages
            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // coures leh teacher wa7d w el teacher leh kza course
            builder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique bmna3 el repeat
            builder.Entity<Enrollment>()
                .HasIndex(e => new { e.StudentId, e.CourseId, e.StartDate })
                .IsUnique();
            // el code lazem yb2a unique
            builder.Entity<CourseCode>()
              .HasIndex(c => c.Code)
              .IsUnique();
            //one to many rel between course and quiz
            builder.Entity<Quiz>()
                .HasOne(q => q.course)
                .WithMany(u => u.quizzes)
                .HasForeignKey(q => q.CourseId);
            //one to many rel between quiz and questions
            builder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.QuizId);
            //one to mane rel between question and options 
            builder.Entity<Option>()
               .HasOne(o => o.Question)
               .WithMany(q => q.Options)
               .HasForeignKey(o => o.QuestionId);




        }
    }
}
