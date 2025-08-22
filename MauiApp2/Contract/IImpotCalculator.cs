namespace MauiApp2.Contract
{
    public interface IImpotCalculator
    {
        public decimal SalaireNet { get; }
        public decimal SalaireNetApresAbattement { get; }
        public decimal ImpotBrut { get; }
        public decimal Decote {  get; }
        public decimal ImpotAPayer { get; }
        public decimal PourcentageImpot { get; }
        public decimal TauxMarginal { get; }
        public decimal NombreDeParts { get; }
        decimal ImpotTheorique { get; }
        decimal CoutPlafonnement { get; }
        bool IsPlafonne { get; }
        public void CalculImpot(decimal salaireNet, decimal nombreDeParts, bool isCouple);
    }
}
