using PocChart.Models;
using SQLite;

namespace PocChart.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;
        private bool _isInitialized = false; // Un drapeau pour savoir si l'init est déjà faite

        private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, "accounts.db3");

        // Le constructeur est maintenant vide et rapide !
        public DatabaseService() { }

        // La méthode d'initialisation qui retourne un Task (et non void)
        private async Task InitializeAsync()
        {
            // On ne l'exécute qu'une seule fois
            if (_isInitialized)
                return;

            _database = new SQLiteAsyncConnection(DbPath);
            await _database.CreateTableAsync<Account>();
            await _database.CreateTableAsync<BalanceEntry>();

            _isInitialized = true;
        }

        // --- Opérations sur les Comptes ---
        public async Task<List<Account>> GetAccountsAsync()
        {
            await InitializeAsync(); // On s'assure que la BDD est prête
            return await _database.Table<Account>().ToListAsync();
        }

        public async Task<int> AddAccountAsync(Account account)
        {
            await InitializeAsync(); // On s'assure que la BDD est prête
            return await _database.InsertAsync(account);
        }

        // --- Opérations sur les Soldes ---
        public async Task<int> AddBalanceEntryAsync(BalanceEntry entry)
        {
            await InitializeAsync(); // On s'assure que la BDD est prête
            return await _database.InsertAsync(entry);
        }

        public async Task<List<BalanceEntry>> GetBalanceHistoryForAccountAsync(int accountId)
        {
            await InitializeAsync(); // On s'assure que la BDD est prête
            return await _database.Table<BalanceEntry>()
                            .Where(entry => entry.AccountId == accountId)
                            .OrderBy(entry => entry.Date)
                            .ToListAsync();
        }
    }
}
