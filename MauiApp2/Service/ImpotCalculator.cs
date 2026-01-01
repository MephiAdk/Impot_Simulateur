using MauiApp2.Contract;
using MauiApp2.Models;
using MauiApp2.Services;

namespace MauiApp2.Service
{
    public class ImpotCalculator : IImpotCalculator
    {
        public readonly BaremeFiscalService _baremeFiscalService;
        private BaremeFiscal? _bareme;

        public ImpotCalculator(BaremeFiscalService baremeFiscalService)
        {
            _baremeFiscalService = baremeFiscalService;
        }

        public async Task InitialiserAsync()
        {
            _bareme = await _baremeFiscalService.ChargerBaremeAsync();
        }

        // Le surcoût dû au plafonnement.
        public decimal CoutPlafonnement => Math.Max(0, ImpotBrut - ImpotTheorique);

        public decimal Decote
        {
            get
            {
                if (_bareme?.Decote == null) return 0;

                decimal impotAvantDecote = ImpotBrut;
                decimal plafondImpot = IsCouple ? _bareme.Decote.PlafondCouple : _bareme.Decote.PlafondCelibataire;
                decimal montantBase = IsCouple ? _bareme.Decote.MontantBaseCouple : _bareme.Decote.MontantBaseCelibataire;

                if (impotAvantDecote < plafondImpot)
                {
                    decimal decoteCalculee = montantBase - (_bareme.Decote.Coefficient * impotAvantDecote);
                    return Math.Max(0, Math.Round(decoteCalculee, 2));
                }
                return 0;
            }
        }

        public decimal ImpotAPayer => Math.Max(0, ImpotBrut - Decote);

        // L'impôt final avant la décote. C'est ici que le plafonnement est appliqué.
        public decimal ImpotBrut
        {
            get
            {
                if (_bareme?.Plafonnement == null) return ImpotTheorique;

                decimal partsDeBase = IsCouple ? 2m : 1m;
                if (NombreDeParts > partsDeBase)
                {
                    decimal impotSansAvantage = CalculerImpotDeBasePourParts(partsDeBase);
                    decimal avantageReel = impotSansAvantage - ImpotTheorique;
                    decimal demiPartsSupplementaires = (NombreDeParts - partsDeBase) * 2;
                    decimal avantageMaxAutorise = demiPartsSupplementaires * _bareme.Plafonnement.PlafondAvantageDemiPart;

                    if (avantageReel > avantageMaxAutorise)
                    {
                        return Math.Max(0, impotSansAvantage - avantageMaxAutorise);
                    }
                }
                return ImpotTheorique;
            }
        }

        // L'impôt "idéal", sans aucune correction.
        public decimal ImpotTheorique => CalculerImpotDeBasePourParts(NombreDeParts);

        public bool IsCouple { get; private set; }

        // Le booléen qui contrôle l'affichage.
        public bool IsPlafonne => CoutPlafonnement > 0;

        public decimal NombreDeParts { get; private set; }
        public decimal PourcentageImpot => (SalaireNet > 0) ? ImpotAPayer / SalaireNet : 0;
        public decimal SalaireNet { get; private set; }
        
        public decimal SalaireNetApresAbattement
        {
            get
            {
                if (_bareme?.Abattement == null) return SalaireNet * 0.90m;

                decimal abattement = SalaireNet * _bareme.Abattement.TauxAbattement;
                abattement = Math.Min(abattement, _bareme.Abattement.PlafondAbattement);
                return SalaireNet - abattement;
            }
        }

        public decimal TauxMarginal
        {
            get
            {
                if (_bareme?.Tranches == null || NombreDeParts == 0) return 0;

                // Le TMI se calcule sur le QUOTIENT FAMILIAL, pas sur le revenu total
                decimal quotientFamilial = SalaireNetApresAbattement / NombreDeParts;
                
                if (quotientFamilial > _bareme.Tranches.Seuil4) return _bareme.Tranches.Taux4;
                if (quotientFamilial > _bareme.Tranches.Seuil3) return _bareme.Tranches.Taux3;
                if (quotientFamilial > _bareme.Tranches.Seuil2) return _bareme.Tranches.Taux2;
                if (quotientFamilial > _bareme.Tranches.Seuil1) return _bareme.Tranches.Taux1;
                return _bareme.Tranches.Taux0;
            }
        }

        public void CalculImpot(decimal salaireNet, decimal nombreDeParts, bool isCouple)
        {
            SalaireNet = salaireNet;
            NombreDeParts = nombreDeParts;
            IsCouple = isCouple;
        }

        private decimal CalculerImpotDeBasePourParts(decimal parts)
        {
            if (_bareme?.Tranches == null || parts <= 0) return 0;

            decimal quotientFamilial = SalaireNetApresAbattement / parts;
            decimal impotPourUnePart = 0;

            if (quotientFamilial > _bareme.Tranches.Seuil1)
                impotPourUnePart += (Math.Min(quotientFamilial, _bareme.Tranches.Seuil2) - _bareme.Tranches.Seuil1) * _bareme.Tranches.Taux1;

            if (quotientFamilial > _bareme.Tranches.Seuil2)
                impotPourUnePart += (Math.Min(quotientFamilial, _bareme.Tranches.Seuil3) - _bareme.Tranches.Seuil2) * _bareme.Tranches.Taux2;

            if (quotientFamilial > _bareme.Tranches.Seuil3)
                impotPourUnePart += (Math.Min(quotientFamilial, _bareme.Tranches.Seuil4) - _bareme.Tranches.Seuil3) * _bareme.Tranches.Taux3;

            if (quotientFamilial > _bareme.Tranches.Seuil4)
                impotPourUnePart += (quotientFamilial - _bareme.Tranches.Seuil4) * _bareme.Tranches.Taux4;

            return Math.Round(impotPourUnePart * parts, 2);
        }
    }
}
