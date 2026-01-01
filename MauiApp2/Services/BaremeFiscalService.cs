using MauiApp2.Models;
using System.Text.Json;

namespace MauiApp2.Services
{
    public class BaremeFiscalService
    {
        private BaremeFiscal? _bareme;
        private readonly string _fileName = "bareme_fiscal.json";

        public async Task<BaremeFiscal> ChargerBaremeAsync()
        {
            if (_bareme != null)
                return _bareme;

            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(_fileName);
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                _bareme = JsonSerializer.Deserialize<BaremeFiscal>(json, options);
                
                if (_bareme == null)
                    throw new Exception("Impossible de charger le barème fiscal");
                    
                return _bareme;
            }
            catch (Exception ex)
            {
                // En cas d'erreur, retourner un barème par défaut
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement du barème: {ex.Message}");
                return CreerBaremeParDefaut();
            }
        }

        public BaremeFiscal? GetBareme()
        {
            return _bareme;
        }

        private BaremeFiscal CreerBaremeParDefaut()
        {
            return new BaremeFiscal
            {
                Annee = 2024,
                Tranches = new TranchesImposition
                {
                    Seuil1 = 11497m,
                    Seuil2 = 29315m,
                    Seuil3 = 83823m,
                    Seuil4 = 180294m,
                    Taux0 = 0m,
                    Taux1 = 0.11m,
                    Taux2 = 0.30m,
                    Taux3 = 0.41m,
                    Taux4 = 0.45m
                },
                Decote = new ParametresDecote
                {
                    PlafondCelibataire = 1964m,
                    PlafondCouple = 3248m,
                    MontantBaseCelibataire = 889m,
                    MontantBaseCouple = 1470m,
                    Coefficient = 0.4525m
                },
                Abattement = new ParametresAbattement
                {
                    TauxAbattement = 0.10m,
                    PlafondAbattement = 13522m
                },
                Plafonnement = new ParametresPlafonnement
                {
                    PlafondAvantageDemiPart = 1791m
                }
            };
        }
    }
}

