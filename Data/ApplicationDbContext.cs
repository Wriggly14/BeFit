using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BeFit.Models.BeFit;

namespace BeFit.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<PerformedExercise> PerformedExercises { get; set; }
    }
}
