namespace BeFit.Models.BeFit
{
    public class ExerciseStatsViewModel
    {
        public string ExerciseName { get; set; } = string.Empty;
        public int TimesPerformed { get; set; }
        public int TotalReps { get; set; }
        public decimal AverageLoad { get; set; }
        public decimal MaxLoad { get; set; }
    }
}
