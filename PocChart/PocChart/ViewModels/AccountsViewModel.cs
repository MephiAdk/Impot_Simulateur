using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PocChart.Models;
using PocChart.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PocChart.ViewModels
{
    public partial class AccountsViewModel : ObservableObject
    {
        // Le service de BDD est injecté via le constructeur
        private readonly DatabaseService _databaseService;

        // --- PROPRIÉTÉS LIÉES À LA VUE (BINDING) ---

        // La liste des comptes pour le Picker. ObservableCollection notifie l'UI
        // automatiquement quand on ajoute/supprime un élément.
        [ObservableProperty]
        private ObservableCollection<Account> _accounts = new();

        // Le compte actuellement sélectionné dans le Picker
        [ObservableProperty]
        private Account _selectedAccount;

        // Propriétés pour les champs de saisie (Entry, DatePicker)
        [ObservableProperty]
        private string _newAccountName;

        [ObservableProperty]
        private decimal _newBalanceValue;

        [ObservableProperty]
        private DateTime _newBalanceDate = DateTime.Today; // Valeur par défaut

        private bool _isViewModelInitialized = false;

        [ObservableProperty]
        private string _resultatEvolution;

        [ObservableProperty]
        private bool _resultatVisible;

        [ObservableProperty]
        private ObservableCollection<MonthlyEvolution> _evolutionMensuelle = new();

        // --- CONSTRUCTEUR ---

        public AccountsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

        }

        // --- LOGIQUE MÉTIER ---


        private async Task LoadAccountsAsync()
        {
            var accountsFromDb = await _databaseService.GetAccountsAsync();
            Accounts = new ObservableCollection<Account>(accountsFromDb);
        }
        // --- COMMANDES (Actions appelées par les boutons) ---

        async partial void OnSelectedAccountChanged(Account value) => EvolutionMensuelle.Clear();

        [RelayCommand]
        private async Task InitializeAsync()
        {
            if (_isViewModelInitialized) return;
            await LoadAccountsAsync();
            _isViewModelInitialized = true;
        }

        [RelayCommand]
        private async Task AddAccountAsync()
        {
            if (string.IsNullOrWhiteSpace(NewAccountName)) return;
            var newAccount = new Account { Name = NewAccountName };
            await _databaseService.AddAccountAsync(newAccount);
            Accounts.Add(newAccount);
            SelectedAccount = newAccount;
            NewAccountName = string.Empty;
        }

        [RelayCommand]
        private async Task AddBalanceEntryAsync()
        {
            if (SelectedAccount == null || NewBalanceValue <= 0) return;
            var newEntry = new BalanceEntry { AccountId = SelectedAccount.Id, Value = NewBalanceValue, Date = NewBalanceDate };
            await _databaseService.AddBalanceEntryAsync(newEntry);
            NewBalanceValue = 0;
            NewBalanceDate = DateTime.Today;
            EvolutionMensuelle.Clear(); // On vide les résultats car ils sont obsolètes
        }

        [RelayCommand]
        private async Task CalculerEvolutionAsync(string periodeEnMoisStr)
        {
            if (SelectedAccount == null || !int.TryParse(periodeEnMoisStr, out int periodeEnMois)) return;

            var historiqueComplet = await _databaseService.GetBalanceHistoryForAccountAsync(SelectedAccount.Id);
            EvolutionMensuelle.Clear();

            if (!historiqueComplet.Any()) return; // S'il n'y a aucune donnée, on s'arrête.

            var culture = new CultureInfo("fr-FR");

            for (int i = 0; i < periodeEnMois; i++)
            {
                var dateFinMoisCourant = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-i).AddMonths(1).AddDays(-1);
                var dateFinMoisPrecedent = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-i - 1).AddMonths(1).AddDays(-1);

                // NOUVELLE LOGIQUE : Trouver le dernier solde enregistré AVANT ou PENDANT la fin du mois courant
                var soldeFinMois = historiqueComplet
                    .Where(e => e.Date <= dateFinMoisCourant)
                    .OrderByDescending(e => e.Date)
                    .FirstOrDefault();

                // NOUVELLE LOGIQUE : Trouver le dernier solde enregistré AVANT ou PENDANT la fin du mois précédent
                var soldeDebutMois = historiqueComplet
                    .Where(e => e.Date <= dateFinMoisPrecedent)
                    .OrderByDescending(e => e.Date)
                    .FirstOrDefault();

                // Si on n'a pas de solde pour le début de la période, on ne peut pas calculer
                if (soldeDebutMois == null) continue;

                // Si le dernier solde de fin de mois est le même que celui de début, cela signifie qu'il n'y a pas eu de nouvelle entrée
                // On considère donc le solde de fin comme étant égal à celui de début.
                var soldeFin = soldeFinMois ?? soldeDebutMois;

                // Si après tout ça, les deux soldes sont identiques, l'évolution est nulle.
                // On l'affiche quand même pour montrer que le mois a été analysé.
                if (soldeDebutMois.Id == soldeFin.Id && soldeDebutMois.Date > dateFinMoisPrecedent)
                {
                    // Cas où la seule transaction disponible est trop récente pour calculer une évolution
                    continue;
                }

                var evolutionAbsolue = soldeFin.Value - soldeDebutMois.Value;
                var evolutionPourcentage = (soldeDebutMois.Value == 0) ? 0 : (evolutionAbsolue / soldeDebutMois.Value);

                string signe = evolutionAbsolue >= 0 ? "+" : "";
                var textColor = Colors.Gray; // Couleur par défaut pour une évolution nulle
                if (evolutionAbsolue > 0) textColor = Colors.Green;
                if (evolutionAbsolue < 0) textColor = Colors.Red;

                EvolutionMensuelle.Add(new MonthlyEvolution
                {
                    MonthName = dateFinMoisCourant.ToString("MMMM yyyy", culture).ToUpper(),
                    EvolutionValue = $"{signe}{evolutionAbsolue.ToString("C", culture)}",
                    EvolutionPercentage = $"{signe}{evolutionPourcentage.ToString("P2", culture)}",
                    TextColor = textColor
                });
            }
        }
    }
}
