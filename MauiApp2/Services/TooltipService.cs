using System.Collections.Generic;

namespace MauiApp2.Services
{
    public class TooltipInfo
    {
        public string Titre { get; set; } = string.Empty;
        public string Explication { get; set; } = string.Empty;
        public string? Exemple { get; set; }
    }

    public class TooltipService
    {
        private readonly Dictionary<string, TooltipInfo> _tooltips;

        public TooltipService()
        {
            _tooltips = new Dictionary<string, TooltipInfo>
            {
                ["SalaireNetFiscal"] = new TooltipInfo
                {
                    Titre = "Salaire net fiscal annuel",
                    Explication = "Il s'agit du salaire net imposable indiqué sur votre fiche de paie, AVANT l'abattement forfaitaire de 10% appliqué par l'administration fiscale. Ce montant inclut tous vos revenus salariaux de l'année.",
                    Exemple = "Si votre fiche de paie indique 30 000 € de net fiscal annuel, c'est ce montant que vous devez saisir ici."
                },
                ["AbattementForfaitaire"] = new TooltipInfo
                {
                    Titre = "Abattement forfaitaire de 10%",
                    Explication = "L'administration fiscale applique automatiquement un abattement de 10% sur vos revenus salariaux pour tenir compte de vos frais professionnels (déplacements, repas, etc.). Cet abattement est plafonné à 13 522 € pour 2024.",
                    Exemple = "Pour un salaire de 30 000 €, l'abattement est de 3 000 €, ce qui donne une base imposable de 27 000 €."
                },
                ["QuotientFamilial"] = new TooltipInfo
                {
                    Titre = "Quotient familial",
                    Explication = "Le quotient familial est le résultat de la division de votre revenu imposable par le nombre de parts fiscales. C'est sur cette base que s'appliquent les tranches d'imposition. Plus vous avez de parts, plus ce quotient diminue, réduisant ainsi votre impôt.",
                    Exemple = "Revenu de 40 000 € ÷ 2 parts = 20 000 € de quotient familial. Les tranches s'appliquent sur ces 20 000 €, puis l'impôt est multiplié par 2."
                },
                ["Plafonnement"] = new TooltipInfo
                {
                    Titre = "Plafonnement du quotient familial",
                    Explication = "Pour limiter l'avantage fiscal lié aux demi-parts supplémentaires (enfants, etc.), chaque demi-part ne peut réduire l'impôt de plus de 1 791 € (2024). Si l'avantage dépasse ce plafond, l'excédent est réintégré dans l'impôt dû.",
                    Exemple = "Avec 2.5 parts, la demi-part supplémentaire peut réduire l'impôt de 1 791 € maximum, même si le calcul brut donnerait une réduction plus importante."
                },
                ["Decote"] = new TooltipInfo
                {
                    Titre = "Décote",
                    Explication = "La décote est un mécanisme qui réduit l'impôt des foyers modestes. Elle s'applique si l'impôt brut est inférieur à 1 964 € (célibataire) ou 3 248 € (couple). La formule est : décote = 889 € - (impôt × 45,25%) pour un célibataire.",
                    Exemple = "Avec un impôt brut de 1 500 €, la décote est de 889 € - (1 500 × 45,25%) = 210 €. L'impôt final sera donc de 1 290 €."
                },
                ["TauxMarginal"] = new TooltipInfo
                {
                    Titre = "Taux Marginal d'Imposition (TMI)",
                    Explication = "C'est le taux d'imposition de la tranche la plus haute dans laquelle se situe votre quotient familial. Attention : seule la partie du revenu dans cette tranche est imposée à ce taux, pas la totalité !",
                    Exemple = "Si votre quotient familial est de 35 000 €, votre TMI est de 30%, mais seule la partie au-dessus de 29 315 € est imposée à 30%."
                },
                ["TauxEffectif"] = new TooltipInfo
                {
                    Titre = "Taux d'imposition effectif",
                    Explication = "C'est le pourcentage réel d'impôt que vous payez par rapport à votre revenu total. Il est toujours inférieur au taux marginal car l'impôt est progressif. C'est le vrai indicateur de votre charge fiscale.",
                    Exemple = "Si vous payez 4 500 € d'impôt pour 30 000 € de revenu, votre taux effectif est de 15%."
                },
                ["PrelevementSource"] = new TooltipInfo
                {
                    Titre = "Prélèvement à la source",
                    Explication = "Depuis 2019, l'impôt sur le revenu est prélevé mensuellement directement sur votre salaire. Le montant indiqué ici est une estimation du prélèvement mensuel moyen que vous subiriez avec cette situation fiscale.",
                    Exemple = "Pour un impôt annuel de 3 600 €, le prélèvement mensuel sera d'environ 300 €."
                },
                ["Tranches"] = new TooltipInfo
                {
                    Titre = "Tranches d'imposition 2024",
                    Explication = "L'impôt français est progressif par tranches. Chaque tranche de revenu est imposée à un taux différent. Seule la partie du revenu dans chaque tranche est imposée au taux correspondant.",
                    Exemple = "Pour 35 000 € de quotient familial :\n• 0 à 11 497 € → 0%\n• 11 497 à 29 315 € → 11%\n• 29 315 à 35 000 € → 30%"
                },
                ["PartFiscale"] = new TooltipInfo
                {
                    Titre = "Part fiscale",
                    Explication = "Le nombre de parts fiscales dépend de votre situation familiale. Une personne seule = 1 part, un couple = 2 parts. Chaque demi-part supplémentaire (enfants) divise davantage le revenu imposable, réduisant ainsi l'impôt.",
                    Exemple = "Célibataire = 1 part • Couple = 2 parts • Couple + 1 enfant = 2,5 parts • Couple + 2 enfants = 3 parts"
                }
            };
        }

        public TooltipInfo? GetTooltip(string key)
        {
            return _tooltips.TryGetValue(key, out var tooltip) ? tooltip : null;
        }

        public bool HasTooltip(string key)
        {
            return _tooltips.ContainsKey(key);
        }
    }
}

