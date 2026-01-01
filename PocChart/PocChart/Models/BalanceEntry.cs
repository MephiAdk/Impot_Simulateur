using SQLite;

namespace PocChart.Models
{
    public class BalanceEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public decimal Value { get; set; } // 'decimal' est mieux pour les montants financiers

        public DateTime Date { get; set; }

        [Indexed] // Important pour rechercher rapidement les soldes d'un compte
        public int AccountId { get; set; }
    }
}
