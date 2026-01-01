using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp2.Contract;
using MauiApp2.Models;
using System.Collections.ObjectModel;

namespace MauiApp2.ViewModels
{
    public partial class ComparaisonViewModel : ObservableObject
    {
        private IImpotCalculator _impotCalculator;

        [ObservableProperty]
        private Scenario? _scenarioActuel;

        public ObservableCollection<Scenario> ScenariosComparaison { get; }

        public ComparaisonViewModel(IImpotCalculator impotCalculator)
        {
            _impotCalculator = impotCalculator;
            ScenariosComparaison = new ObservableCollection<Scenario>();
        }

        public void InitialiserComparaison(decimal salaireNet, decimal nombreDeParts, bool isCouple, string descriptionSituation)
        {
            // Créer le scénario actuel
            _impotCalculator.CalculImpot(salaireNet, nombreDeParts, isCouple);
            ScenarioActuel = new Scenario
            {
                Nom = "Situation actuelle",
                SalaireNet = salaireNet,
                NombreDeParts = nombreDeParts,
                IsCouple = isCouple,
                DescriptionSituation = descriptionSituation,
                ImpotAPayer = _impotCalculator.ImpotAPayer,
                TauxEffectif = _impotCalculator.PourcentageImpot,
                PrelevementMensuel = _impotCalculator.ImpotAPayer / 12,
                SalaireApresImpot = salaireNet - _impotCalculator.ImpotAPayer
            };

            // Générer des scénarios de comparaison
            GenererScenarios(salaireNet, nombreDeParts, isCouple);
        }

        private void GenererScenarios(decimal salaireActuel, decimal partsActuelles, bool isCoupleActuel)
        {
            ScenariosComparaison.Clear();

            // Scénario 1 : Augmentation de 5000€
            AjouterScenario(
                "Augmentation de 5 000 €",
                salaireActuel + 5000,
                partsActuelles,
                isCoupleActuel,
                $"Même situation avec un salaire de {salaireActuel + 5000:N0} €"
            );

            // Scénario 2 : Si célibataire, simuler en couple
            if (!isCoupleActuel && partsActuelles == 1)
            {
                AjouterScenario(
                    "Si vous vous mariez/pacsez",
                    salaireActuel,
                    2m,
                    true,
                    $"Couple sans enfant avec {salaireActuel:N0} € de revenus communs (conjoint sans revenus propres)"
                );
            }

            // Scénario 3 : Avec un enfant supplémentaire
            decimal nouvellesParts = partsActuelles + 0.5m;
            string descEnfant = isCoupleActuel ? "couple" : "parent isolé";
            AjouterScenario(
                "Avec un enfant supplémentaire",
                salaireActuel,
                nouvellesParts,
                isCoupleActuel,
                $"Même revenu en {descEnfant} avec +0.5 part"
            );

            // Scénario 4 : Diminution de 5000€ (changement de carrière)
            if (salaireActuel > 10000)
            {
                AjouterScenario(
                    "Réduction du temps de travail (-5 000 €)",
                    salaireActuel - 5000,
                    partsActuelles,
                    isCoupleActuel,
                    $"Même situation avec un salaire de {salaireActuel - 5000:N0} €"
                );
            }

            // Scénario 5 : Si couple, simuler célibataire
            if (isCoupleActuel && partsActuelles >= 2)
            {
                decimal salaireParPersonne = salaireActuel / 2;
                AjouterScenario(
                    "En cas de célibat (revenu divisé par 2)",
                    salaireParPersonne,
                    1m,
                    false,
                    $"Célibataire avec {salaireParPersonne:N0} € de revenu"
                );
            }
        }

        private void AjouterScenario(string nom, decimal salaire, decimal parts, bool isCouple, string description)
        {
            _impotCalculator.CalculImpot(salaire, parts, isCouple);
            
            var scenario = new Scenario
            {
                Nom = nom,
                SalaireNet = salaire,
                NombreDeParts = parts,
                IsCouple = isCouple,
                DescriptionSituation = description,
                ImpotAPayer = _impotCalculator.ImpotAPayer,
                TauxEffectif = _impotCalculator.PourcentageImpot,
                PrelevementMensuel = _impotCalculator.ImpotAPayer / 12,
                SalaireApresImpot = salaire - _impotCalculator.ImpotAPayer
            };

            ScenariosComparaison.Add(scenario);
        }
    }
}

