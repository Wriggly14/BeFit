using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BeFit.Models.BeFit
{
    public class TrainingSession : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Data i czas rozpoczęcia")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Data i czas zakończenia")]
        public DateTime End { get; set; }

        // Powiązanie z użytkownikiem (Identity)
        [Required]
        [ValidateNever]      // Nie pokazujemy w formularzu, ustawiamy w kontrolerze
        public string UserId { get; set; } = default!;

        [Display(Name = "Opis sesji")]
        [StringLength(200)]
        public string? Notes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (End <= Start)
            {
                yield return new ValidationResult(
                    "Czas zakończenia musi być późniejszy niż rozpoczęcia.",
                    new[] { nameof(End) });
            }
        }
    }
}
