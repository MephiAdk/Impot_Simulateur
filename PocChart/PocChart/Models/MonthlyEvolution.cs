using Microsoft.Maui.Graphics; // Nécessaire pour la classe Color

namespace PocChart.Models
{
    

    public class MonthlyEvolution
    {
        public string MonthName { get; set; }
        public string EvolutionValue { get; set; }
        public string EvolutionPercentage { get; set; }
        public Color TextColor { get; set; } // Pour afficher en vert ou en rouge
    }
}
