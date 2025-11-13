using System.ComponentModel.DataAnnotations;

namespace BeFit.Models.BeFit
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa ćwiczenia")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Minimalne obciążenie [kg]")]
        [Range(0, 1000)]
        public decimal? MinLoad { get; set; }

        [Display(Name = "Maksymalne obciążenie [kg]")]
        [Range(0, 1000)]
        public decimal? MaxLoad { get; set; }

        [Display(Name = "Minimalna liczba powtórzeń")]
        [Range(1, 1000)]
        public int? MinReps { get; set; }

        [Display(Name = "Maksymalna liczba powtórzeń")]
        [Range(1, 1000)]
        public int? MaxReps { get; set; }
    }
}
