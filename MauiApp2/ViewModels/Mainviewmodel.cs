using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp2.Contract;
using MauiApp2.Models;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MauiApp2.ViewModels
{
    public partial class Mainviewmodel : ObservableObject
    {
        [ObservableProperty]
        private decimal _abattementForfaitaire;

        [ObservableProperty]
        private decimal _coutPlafonnement;

        [ObservableProperty]
        private decimal _decote;

        [ObservableProperty]
        private decimal _impotAPayer;

        [ObservableProperty]
        private decimal _impotBrut;

        private IImpotCalculator _impotCalculator;

        [ObservableProperty]
        private decimal _impotTheorique;

        [ObservableProperty]
        private bool _isPlafonne;

        [ObservableProperty]
        private decimal _pourcentageImpot;

        [ObservableProperty]
        private decimal _prelevementMensuel;

        [ObservableProperty]
        private decimal _salaireNetApresAbattement;

        [ObservableProperty]
        private string _salaireNetInput;

        [ObservableProperty]
        private decimal _tauxMarginal;

        [ObservableProperty]
        private PartOption selectedPartOption;

        [ObservableProperty]
        private bool _estEnCouple;

        [ObservableProperty]
        private int _nombreEnfants;

        [ObservableProperty]
        private decimal _nombreDePartsCalcule;

        [ObservableProperty]
        private string _descriptionParts = string.Empty;

        [ObservableProperty]
        private decimal _quotientFamilial;

        [ObservableProperty]
        private decimal _montantTranche0;

        [ObservableProperty]
        private decimal _montantTranche1;

        [ObservableProperty]
        private decimal _montantTranche2;

        [ObservableProperty]
        private decimal _montantTranche3;

        [ObservableProperty]
        private decimal _montantTranche4;

        [ObservableProperty]
        private bool _isDetailCalculExpanded;

        [ObservableProperty]
        private decimal _salaireMensuelNet;

        [ObservableProperty]
        private decimal _salaireMensuelApresImpot;

        [ObservableProperty]
        private decimal _pourcentageResteApresImpot;

        [ObservableProperty]
        private decimal _salaireApresImpot;

        public Mainviewmodel(IImpotCalculator impotCalculator)
        {
            _impotCalculator = impotCalculator;
            
            // Initialiser le calculateur avec le barème fiscal
            _ = InitialiserAsync();

            // Valeurs par défaut
            EstEnCouple = false;
            NombreEnfants = 0;
            SalaireNetInput = "30000";
            
            // Calculer les parts initiales
            CalculerNombreDeParts();

            // On initialise la liste des options (conservée pour compatibilité)
            PartOptions = new ObservableCollection<PartOption>
            {
                new PartOption { Description = "Célibataire (1 part)", Value = 1m, IsCouple = false }
            };
            SelectedPartOption = PartOptions[0];
        }

        public bool CanCalculerImpot => decimal.TryParse(SalaireNetInput, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal salaire) && salaire >= 0;
        public ObservableCollection<PartOption> PartOptions { get; }

        private void LancerCalcul()
        {
            // On essaie de convertir la valeur saisie
            if (decimal.TryParse(SalaireNetInput, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal salaireAnnuel) && salaireAnnuel >= 0)
            {
                // Si c'est un nombre valide, on lance le calcul
                _impotCalculator.CalculImpot(salaireAnnuel, NombreDePartsCalcule, EstEnCouple);

                // Mettre à jour TOUTES les propriétés
                AbattementForfaitaire     = _impotCalculator.SalaireNet - _impotCalculator.SalaireNetApresAbattement;
                SalaireNetApresAbattement = _impotCalculator.SalaireNetApresAbattement;
                ImpotTheorique            = _impotCalculator.ImpotTheorique;
                IsPlafonne                = _impotCalculator.IsPlafonne;
                CoutPlafonnement          = _impotCalculator.CoutPlafonnement;
                ImpotBrut                 = _impotCalculator.ImpotBrut;
                Decote                    = _impotCalculator.Decote;
                ImpotAPayer               = _impotCalculator.ImpotAPayer;
                PourcentageImpot          = _impotCalculator.PourcentageImpot;
                TauxMarginal              = _impotCalculator.TauxMarginal;
                PrelevementMensuel        = _impotCalculator.ImpotAPayer / 12;
                
                // Calcul du quotient familial et des tranches
                QuotientFamilial = NombreDePartsCalcule > 0 ? SalaireNetApresAbattement / NombreDePartsCalcule : 0;
                CalculerMontantsParTranche();
                
                // Calcul des montants mensuels
                SalaireMensuelNet = salaireAnnuel / 12;
                SalaireMensuelApresImpot = (salaireAnnuel - _impotCalculator.ImpotAPayer) / 12;
                
                // Calcul du pourcentage restant après impôt
                PourcentageResteApresImpot = 1 - PourcentageImpot;
                SalaireApresImpot = salaireAnnuel - _impotCalculator.ImpotAPayer;
            }
            else
            {
                SalaireNetApresAbattement = 0;
                ImpotBrut                 = 0;
                Decote                    = 0;
                ImpotAPayer               = 0;
                PourcentageImpot          = 0;
                TauxMarginal              = 0;
            }
        }

        // Le nom doit correspondre : "On" + NomDeLaPropriété + "Changed"
        partial void OnSalaireNetInputChanged(string value) => LancerCalcul();

        partial void OnSelectedPartOptionChanged(PartOption value) => LancerCalcul();

        partial void OnEstEnCoupleChanged(bool value)
        {
            CalculerNombreDeParts();
            LancerCalcul();
        }

        partial void OnNombreEnfantsChanged(int value)
        {
            CalculerNombreDeParts();
            LancerCalcul();
        }

        private void CalculerNombreDeParts()
        {
            decimal parts;
            
            if (EstEnCouple)
            {
                // Couple : 2 parts de base
                parts = 2m;
                
                if (NombreEnfants >= 1)
                    parts += 0.5m; // 1er enfant : 0.5 part
                if (NombreEnfants >= 2)
                    parts += 0.5m; // 2ème enfant : 0.5 part
                if (NombreEnfants >= 3)
                    parts += (NombreEnfants - 2) * 1m; // À partir du 3ème : 1 part entière par enfant
            }
            else
            {
                // Célibataire : 1 part de base
                parts = 1m;
                
                if (NombreEnfants >= 1)
                    parts += 1m; // 1er enfant pour parent isolé : 1 part entière (majoration)
                if (NombreEnfants >= 2)
                    parts += 0.5m; // 2ème enfant : 0.5 part
                if (NombreEnfants >= 3)
                    parts += (NombreEnfants - 2) * 1m; // À partir du 3ème : 1 part entière par enfant
            }
            
            NombreDePartsCalcule = parts;
            
            // Description lisible
            string situationBase = EstEnCouple ? "Couple" : "Célibataire";
            string enfantsText = NombreEnfants switch
            {
                0 => "sans enfant",
                1 => "avec 1 enfant",
                _ => $"avec {NombreEnfants} enfants"
            };
            
            DescriptionParts = $"{situationBase} {enfantsText} = {parts:0.0} part(s)";
            
            // Mettre à jour l'option sélectionnée pour la compatibilité
            SelectedPartOption = new PartOption 
            { 
                Description = DescriptionParts, 
                Value = parts, 
                IsCouple = EstEnCouple 
            };
        }

        private async Task InitialiserAsync()
        {
            if (_impotCalculator is Service.ImpotCalculator calculator)
            {
                await calculator.InitialiserAsync();
                LancerCalcul(); // Relancer le calcul après l'initialisation
            }
        }

        private void CalculerMontantsParTranche()
        {
            // Récupérer les seuils depuis le service (avec valeurs par défaut si non disponibles)
            decimal SEUIL_TRANCHE_1 = 11497m;
            decimal SEUIL_TRANCHE_2 = 29315m;
            decimal SEUIL_TRANCHE_3 = 83823m;
            decimal SEUIL_TRANCHE_4 = 180294m;
            decimal TAUX_TRANCHE_1 = 0.11m;
            decimal TAUX_TRANCHE_2 = 0.30m;
            decimal TAUX_TRANCHE_3 = 0.41m;
            decimal TAUX_TRANCHE_4 = 0.45m;

            // Si le calculateur est disponible, utiliser ses valeurs
            if (_impotCalculator is Service.ImpotCalculator calculator)
            {
                var bareme = calculator._baremeFiscalService?.GetBareme();
                if (bareme?.Tranches != null)
                {
                    SEUIL_TRANCHE_1 = bareme.Tranches.Seuil1;
                    SEUIL_TRANCHE_2 = bareme.Tranches.Seuil2;
                    SEUIL_TRANCHE_3 = bareme.Tranches.Seuil3;
                    SEUIL_TRANCHE_4 = bareme.Tranches.Seuil4;
                    TAUX_TRANCHE_1 = bareme.Tranches.Taux1;
                    TAUX_TRANCHE_2 = bareme.Tranches.Taux2;
                    TAUX_TRANCHE_3 = bareme.Tranches.Taux3;
                    TAUX_TRANCHE_4 = bareme.Tranches.Taux4;
                }
            }

            decimal qf = QuotientFamilial;
            
            // Tranche 0 (0%)
            MontantTranche0 = Math.Min(qf, SEUIL_TRANCHE_1);
            
            // Tranche 1 (11%)
            if (qf > SEUIL_TRANCHE_1)
            {
                decimal baseTrache1 = Math.Min(qf, SEUIL_TRANCHE_2) - SEUIL_TRANCHE_1;
                MontantTranche1 = baseTrache1 * TAUX_TRANCHE_1 * NombreDePartsCalcule;
            }
            else
            {
                MontantTranche1 = 0;
            }
            
            // Tranche 2 (30%)
            if (qf > SEUIL_TRANCHE_2)
            {
                decimal baseTrache2 = Math.Min(qf, SEUIL_TRANCHE_3) - SEUIL_TRANCHE_2;
                MontantTranche2 = baseTrache2 * TAUX_TRANCHE_2 * NombreDePartsCalcule;
            }
            else
            {
                MontantTranche2 = 0;
            }
            
            // Tranche 3 (41%)
            if (qf > SEUIL_TRANCHE_3)
            {
                decimal baseTrache3 = Math.Min(qf, SEUIL_TRANCHE_4) - SEUIL_TRANCHE_3;
                MontantTranche3 = baseTrache3 * TAUX_TRANCHE_3 * NombreDePartsCalcule;
            }
            else
            {
                MontantTranche3 = 0;
            }
            
            // Tranche 4 (45%)
            if (qf > SEUIL_TRANCHE_4)
            {
                decimal baseTrache4 = qf - SEUIL_TRANCHE_4;
                MontantTranche4 = baseTrache4 * TAUX_TRANCHE_4 * NombreDePartsCalcule;
            }
            else
            {
                MontantTranche4 = 0;
            }
        }
    }
}