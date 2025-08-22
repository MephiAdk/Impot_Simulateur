using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp2.Contract;
using MauiApp2.Models;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MauiApp2.ViewModels
{
    public partial class MainMV : ObservableObject 
    {   
        private IImpotCalculator _impotCalculator;

        [ObservableProperty]
        private string _salaireNetInput;

        [ObservableProperty]
        private decimal _salaireNetApresAbattement;

        [ObservableProperty]
        private decimal _impotBrut;

        [ObservableProperty]
        private decimal _decote;

        [ObservableProperty]
        private decimal _impotAPayer;

        [ObservableProperty]
        private decimal _pourcentageImpot;

        [ObservableProperty]
        private decimal _tauxMarginal;

        [ObservableProperty]
        private decimal _abattementForfaitaire; 

        [ObservableProperty]
        private decimal _impotTheorique; 

        [ObservableProperty]
        private decimal _coutPlafonnement; 

        [ObservableProperty]
        private bool _isPlafonne; 

        [ObservableProperty]
        private decimal _prelevementMensuel;

        [ObservableProperty]
        private PartOption selectedPartOption;

        public ObservableCollection<PartOption> PartOptions { get; }


        public MainMV(IImpotCalculator impotCalculator)
        {
            _impotCalculator = impotCalculator;

            // On initialise la liste des options
            PartOptions = new ObservableCollection<PartOption>
            {
                new PartOption { Description = "Célibataire (1 part)",        Value = 1m,     IsCouple = false },
                new PartOption { Description = "Couple marié/pacsé (2 parts)",Value = 2m,     IsCouple = true  },
                new PartOption { Description = "Couple + 1 enfant (2.5 parts)", Value = 2.5m, IsCouple = true  },
                new PartOption { Description = "Couple + 2 enfants (3 parts)",  Value = 3m,     IsCouple = true  },
                new PartOption { Description = "Couple + 3 enfants (4 parts)",  Value = 4m,     IsCouple = true  },
    
                // Le 1er enfant compte double (0.5 + 0.5 part) pour un parent isolé
                new PartOption { Description = "Parent isolé + 1 enfant (2 parts)",  Value = 2m,     IsCouple = false },
                new PartOption { Description = "Parent isolé + 2 enfants (2.5 parts)", Value = 2.5m,   IsCouple = false },
                new PartOption { Description = "Parent isolé + 3 enfants (3.5 parts)", Value = 3.5m,   IsCouple = false },
            };
            SelectedPartOption = PartOptions[0];

            SalaireNetInput = "30000";
        }

        public bool CanCalculerImpot => decimal.TryParse(SalaireNetInput, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal salaire) && salaire >= 0;

        // 2. LA MAGIE EST ICI : C'est la méthode de réaction.
        // Le MVVM Toolkit génère automatiquement un appel à cette méthode
        // dans le setter de la propriété "SalaireNetInput".
        // Le nom doit correspondre : "On" + NomDeLaPropriété + "Changed"
        partial void OnSalaireNetInputChanged(string value) => LancerCalcul();
        partial void OnSelectedPartOptionChanged(PartOption value) => LancerCalcul();

        private void LancerCalcul()
        {
            // On essaie de convertir la valeur saisie
            if (decimal.TryParse(SalaireNetInput, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal salaireAnnuel) && salaireAnnuel >= 0)
            {
                // Si c'est un nombre valide, on lance le calcul
                _impotCalculator.CalculImpot(salaireAnnuel, SelectedPartOption.Value, SelectedPartOption.IsCouple);

                // Mettre à jour TOUTES les propriétés
                AbattementForfaitaire = _impotCalculator.SalaireNet - _impotCalculator.SalaireNetApresAbattement;
                SalaireNetApresAbattement = _impotCalculator.SalaireNetApresAbattement;
                ImpotTheorique = _impotCalculator.ImpotTheorique;
                IsPlafonne = _impotCalculator.IsPlafonne;
                CoutPlafonnement = _impotCalculator.CoutPlafonnement;
                ImpotBrut = _impotCalculator.ImpotBrut;
                Decote = _impotCalculator.Decote;
                ImpotAPayer = _impotCalculator.ImpotAPayer;
                PourcentageImpot = _impotCalculator.PourcentageImpot;
                TauxMarginal = _impotCalculator.TauxMarginal;
                PrelevementMensuel = _impotCalculator.ImpotAPayer / 12;
            }
            else
            {
                SalaireNetApresAbattement = 0;
                ImpotBrut = 0;
                Decote = 0;
                ImpotAPayer = 0;
                PourcentageImpot = 0;
                TauxMarginal = 0;
            }
        }
    }
}
