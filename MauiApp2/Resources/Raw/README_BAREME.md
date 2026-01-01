# Configuration du Barème Fiscal

## 📋 Description

Le fichier `bareme_fiscal.json` contient toutes les valeurs fiscales utilisées par le simulateur d'impôts. 

**Important :** Ce fichier peut être modifié **sans recompiler l'application** ! Il suffit de modifier les valeurs dans le fichier JSON, et l'application les utilisera au prochain démarrage.

## 📝 Structure du fichier JSON

```json
{
  "annee": 2024,                    // Année fiscale du barème
  
  "tranches": {
    "seuil1": 11497,                // Fin de la tranche à 0% (€)
    "seuil2": 29315,                // Fin de la tranche à 11% (€)
    "seuil3": 83823,                // Fin de la tranche à 30% (€)
    "seuil4": 180294,               // Fin de la tranche à 41% (€)
    "taux0": 0.00,                  // Taux de la 1ère tranche (0%)
    "taux1": 0.11,                  // Taux de la 2ème tranche (11%)
    "taux2": 0.30,                  // Taux de la 3ème tranche (30%)
    "taux3": 0.41,                  // Taux de la 4ème tranche (41%)
    "taux4": 0.45                   // Taux de la 5ème tranche (45%)
  },
  
  "decote": {
    "plafondCelibataire": 1964,     // Plafond d'impôt pour bénéficier de la décote (célibataire) (€)
    "plafondCouple": 3248,          // Plafond d'impôt pour bénéficier de la décote (couple) (€)
    "montantBaseCelibataire": 889,  // Montant de base de la décote (célibataire) (€)
    "montantBaseCouple": 1470,      // Montant de base de la décote (couple) (€)
    "coefficient": 0.4525           // Coefficient de la décote (45,25%)
  },
  
  "abattement": {
    "tauxAbattement": 0.10,         // Taux d'abattement forfaitaire (10%)
    "plafondAbattement": 13522      // Plafond de l'abattement forfaitaire (€)
  },
  
  "plafonnement": {
    "plafondAvantageDemiPart": 1791 // Plafond de l'avantage par demi-part fiscale (€)
  }
}
```

## 🔄 Comment mettre à jour pour une nouvelle année ?

### Méthode 1 : Avant déploiement (recommandé)
1. Ouvrez le fichier `bareme_fiscal.json` dans `MauiApp2/Resources/Raw/`
2. Modifiez les valeurs selon le nouveau barème fiscal officiel
3. Changez `"annee": 2024` pour la nouvelle année (ex: `"annee": 2025`)
4. Sauvegardez le fichier
5. Redéployez l'application

**⚠️ Aucune recompilation nécessaire !** Le nom du fichier est générique, vous modifiez simplement les valeurs à l'intérieur.

### Méthode 2 : Après déploiement (avancé)
**Windows :**
Le fichier se trouve dans : `[AppFolder]/Resources/Raw/bareme_fiscal.json`

Vous pouvez le modifier directement avec un éditeur de texte. L'application utilisera les nouvelles valeurs au prochain démarrage.

## 📚 Sources officielles

Pour obtenir les valeurs officielles chaque année, consultez :
- **Site des impôts** : https://www.impots.gouv.fr
- **Barème progressif** : Section "Barème de l'impôt sur le revenu"
- **Décote** : Recherchez "décote impôt sur le revenu"
- **Plafonnement du quotient familial** : Section "Quotient familial"

## ⚠️ Attention

- **Les montants sont en euros**
- **Les taux sont en décimal** (0.11 = 11%, 0.30 = 30%, etc.)
- **Ne supprimez aucun champ**, même si vous ne modifiez pas sa valeur
- **Respectez la syntaxe JSON** (virgules, accolades, guillemets)
- **Testez toujours** après modification pour vous assurer que le fichier est valide

## ✅ Validation

Pour vérifier que votre fichier JSON est valide :
1. Copiez le contenu
2. Allez sur https://jsonlint.com/
3. Collez et validez

## 🎯 Exemple de mise à jour pour 2025

Quand le barème 2025 sera publié :

1. Ouvrez `bareme_fiscal.json`
2. Remplacez les valeurs :

```json
{
  "annee": 2025,  // ← Changez juste l'année
  "tranches": {
    "seuil1": 11700,  // ← Mettez les nouvelles valeurs officielles
    "seuil2": 29800,
    // ... etc
  }
}
```

3. Sauvegardez et redéployez - **C'est tout !**

## 📅 Historique des barèmes

### 2024
- Tranches : 0% jusqu'à 11 497 €, puis 11%, 30%, 41%, 45%
- Décote : 889 € (célibataire), 1 470 € (couple)
- Plafond décote : 1 964 € (célibataire), 3 248 € (couple)
- Plafonnement QF : 1 791 € par demi-part

