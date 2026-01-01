namespace MauiApp2.Models
{
    public class Scenario
    {
        public string? Nom { get; set; }
        public decimal SalaireNet { get; set; }
        public decimal NombreDeParts { get; set; }
        public bool IsCouple { get; set; }
        public string? DescriptionSituation { get; set; }
        
        // Résultats calculés
        public decimal ImpotAPayer { get; set; }
        public decimal TauxEffectif { get; set; }
        public decimal PrelevementMensuel { get; set; }
        public decimal SalaireApresImpot { get; set; }
    }
}

