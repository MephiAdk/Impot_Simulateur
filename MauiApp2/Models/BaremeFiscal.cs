namespace MauiApp2.Models
{
    public class BaremeFiscal
    {
        public int Annee { get; set; }
        public TranchesImposition? Tranches { get; set; }
        public ParametresDecote? Decote { get; set; }
        public ParametresAbattement? Abattement { get; set; }
        public ParametresPlafonnement? Plafonnement { get; set; }
    }

    public class TranchesImposition
    {
        public decimal Seuil1 { get; set; }
        public decimal Seuil2 { get; set; }
        public decimal Seuil3 { get; set; }
        public decimal Seuil4 { get; set; }
        public decimal Taux0 { get; set; }
        public decimal Taux1 { get; set; }
        public decimal Taux2 { get; set; }
        public decimal Taux3 { get; set; }
        public decimal Taux4 { get; set; }
    }

    public class ParametresDecote
    {
        public decimal PlafondCelibataire { get; set; }
        public decimal PlafondCouple { get; set; }
        public decimal MontantBaseCelibataire { get; set; }
        public decimal MontantBaseCouple { get; set; }
        public decimal Coefficient { get; set; }
    }

    public class ParametresAbattement
    {
        public decimal TauxAbattement { get; set; }
        public decimal PlafondAbattement { get; set; }
    }

    public class ParametresPlafonnement
    {
        public decimal PlafondAvantageDemiPart { get; set; }
    }
}

