using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BeFit.Models.BeFit
{
    public class PerformedExercise
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Sesja treningowa")]
        public int TrainingSessionId { get; set; }

        [ValidateNever]
        public TrainingSession TrainingSession { get; set; } = default!;

        [Required]
        [Display(Name = "Typ ćwiczenia")]
        public int ExerciseTypeId { get; set; }

        [ValidateNever]
        public ExerciseType ExerciseType { get; set; } = default!;

        [Required]
        [Range(1, 100)]
        [Display(Name = "Liczba serii")]
        public int Sets { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Liczba powtórzeń w serii")]
        public int RepsPerSet { get; set; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Obciążenie [kg]")]
        public decimal Load { get; set; }

        // Dodatkowo zabezpieczamy przynależność do użytkownika
        [Required]
        [ValidateNever]
        public string UserId { get; set; } = default!;

        [NotMapped]
        [Display(Name = "Łączna liczba powtórzeń")]
        public int TotalReps => Sets * RepsPerSet;
    }
}
