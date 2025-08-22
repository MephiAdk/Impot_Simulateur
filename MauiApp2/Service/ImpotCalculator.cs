using MauiApp2.Contract;

namespace MauiApp2.Service
{
    public class ImpotCalculator : IImpotCalculator
    {
        private const decimal SEUIL_TRANCHE_1 = 11497m;
        private const decimal SEUIL_TRANCHE_2 = 29315m;
        private const decimal SEUIL_TRANCHE_3 = 83823m;
        private const decimal SEUIL_TRANCHE_4 = 180294m;

        private const decimal TAUX_TRANCHE_0 = 0m;
        private const decimal TAUX_TRANCHE_1 = 0.11m;
        private const decimal TAUX_TRANCHE_2 = 0.30m;
        private const decimal TAUX_TRANCHE_3 = 0.41m;
        private const decimal TAUX_TRANCHE_4 = 0.45m;

        private const decimal PLAFOND_AVANTAGE_DEMI_PART = 1791m;

        private const decimal PLAFOND_DECOTE_CELIBATAIRE = 1964m;
        private const decimal PLAFOND_DECOTE_COUPLE = 3248m;
        private const decimal MONTANT_BASE_DECOTE_CELIBATAIRE = 889m;
        private const decimal MONTANT_BASE_DECOTE_COUPLE = 1470m;
        private const decimal COEFF_DECOTE = 0.4525m;


        public decimal SalaireNet { get; private set; }
        public decimal NombreDeParts { get; private set; }
        public bool IsCouple { get; private set; }
        public decimal SalaireNetApresAbattement => SalaireNet * 0.90m;

        // L'impôt "idéal", sans aucune correction.
        public decimal ImpotTheorique => CalculerImpotDeBasePourParts(NombreDeParts);

        // L'impôt final avant la décote. C'est ici que le plafonnement est appliqué.
        public decimal ImpotBrut
        {
            get
            {
                decimal partsDeBase = IsCouple ? 2m : 1m;
                if (NombreDeParts > partsDeBase)
                {
                    decimal impotSansAvantage = CalculerImpotDeBasePourParts(partsDeBase);
                    decimal avantageReel = impotSansAvantage - ImpotTheorique;
                    decimal demiPartsSupplementaires = (NombreDeParts - partsDeBase) * 2;
                    decimal avantageMaxAutorise = demiPartsSupplementaires * PLAFOND_AVANTAGE_DEMI_PART;

                    if (avantageReel > avantageMaxAutorise)
                    {
                        return Math.Max(0, impotSansAvantage - avantageMaxAutorise);
                    }
                }
                return ImpotTheorique; // Si pas de plafonnement, le brut = le théorique
            }
        }

        // Le surcoût dû au plafonnement.
        public decimal CoutPlafonnement => Math.Max(0, ImpotBrut - ImpotTheorique);

        // Le booléen qui contrôle l'affichage.
        public bool IsPlafonne => CoutPlafonnement > 0;

        public decimal Decote
        {
            get
            {
                decimal impotAvantDecote = ImpotBrut;
                decimal plafondImpot = IsCouple ? PLAFOND_DECOTE_COUPLE : PLAFOND_DECOTE_CELIBATAIRE;
                decimal montantBase = IsCouple ? MONTANT_BASE_DECOTE_COUPLE : MONTANT_BASE_DECOTE_CELIBATAIRE;

                if (impotAvantDecote < plafondImpot)
                {
                    decimal decoteCalculee = montantBase - (COEFF_DECOTE * impotAvantDecote);
                    return Math.Max(0, Math.Round(decoteCalculee, 2));
                }
                return 0;
            }
        }

        public decimal ImpotAPayer => Math.Max(0, ImpotBrut - Decote);
        public decimal PourcentageImpot => (SalaireNet > 0) ? ImpotAPayer / SalaireNet : 0;

        public decimal TauxMarginal
        {
            get
            {
                decimal revenuImposable = SalaireNetApresAbattement;
                if (revenuImposable > SEUIL_TRANCHE_4) return TAUX_TRANCHE_4;
                if (revenuImposable > SEUIL_TRANCHE_3) return TAUX_TRANCHE_3;
                if (revenuImposable > SEUIL_TRANCHE_2) return TAUX_TRANCHE_2;
                if (revenuImposable > SEUIL_TRANCHE_1) return TAUX_TRANCHE_1;
                return TAUX_TRANCHE_0;
            }
        }

        public void CalculImpot(decimal salaireNet, decimal nombreDeParts, bool isCouple)
        {
            // Cette méthode ne fait que stocker les nouvelles données.
            SalaireNet = salaireNet;
            NombreDeParts = nombreDeParts;
            IsCouple = isCouple;
        }

        private decimal CalculerImpotDeBasePourParts(decimal parts)
        {
            if (parts <= 0) return 0;
            decimal quotientFamilial = SalaireNetApresAbattement / parts;
            decimal impotPourUnePart = 0;

            if (quotientFamilial > SEUIL_TRANCHE_1) impotPourUnePart += (Math.Min(quotientFamilial, SEUIL_TRANCHE_2) - SEUIL_TRANCHE_1) * TAUX_TRANCHE_1;
            if (quotientFamilial > SEUIL_TRANCHE_2) impotPourUnePart += (Math.Min(quotientFamilial, SEUIL_TRANCHE_3) - SEUIL_TRANCHE_2) * TAUX_TRANCHE_2;
            if (quotientFamilial > SEUIL_TRANCHE_3) impotPourUnePart += (Math.Min(quotientFamilial, SEUIL_TRANCHE_4) - SEUIL_TRANCHE_3) * TAUX_TRANCHE_3;
            if (quotientFamilial > SEUIL_TRANCHE_4) impotPourUnePart += (quotientFamilial - SEUIL_TRANCHE_4) * TAUX_TRANCHE_4;

            return Math.Round(impotPourUnePart * parts, 2);
        }
    }
}