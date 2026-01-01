# 💰 Simulateur d'Impôts Français

<div align="center">

![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-9.0-purple?style=for-the-badge)
![C#](https://img.shields.io/badge/C%23-12.0-blue?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)
![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS%20%7C%20Windows%20%7C%20macOS-orange?style=for-the-badge)

Une application multiplateforme moderne et intuitive pour simuler votre impôt sur le revenu français 🇫🇷

[Fonctionnalités](#-fonctionnalités) • [Installation](#-installation) • [Configuration](#-configuration-du-barème-fiscal) • [Architecture](#-architecture) • [Contribuer](#-contribuer)

</div>

---

## 📋 Description

**Simulateur d'Impôts Français** est une application .NET MAUI qui permet de calculer précisément votre impôt sur le revenu français selon le barème officiel. L'application offre une interface moderne, pédagogique et entièrement configurable sans recompilation.

### 🎯 Objectifs du projet

- ✅ **Précision** : Calculs conformes au barème fiscal français 2024
- ✅ **Pédagogie** : Explications détaillées de chaque étape du calcul
- ✅ **Flexibilité** : Mise à jour du barème sans recompilation
- ✅ **Accessibilité** : Interface intuitive avec aide contextuelle
- ✅ **Multiplateforme** : Fonctionne sur Android, iOS, Windows et macOS

---

## ✨ Fonctionnalités

### 🧮 Calcul d'impôt complet

- **Abattement forfaitaire automatique** (10% plafonné)
- **Quotient familial** avec calcul automatique des parts fiscales
- **Plafonnement du quotient familial** (1 791 € par demi-part)
- **Décote** pour les foyers modestes
- **Taux marginal d'imposition (TMI)** et taux effectif

### 📊 Interface intuitive

- **Sélecteur de situation** : Célibataire/Couple avec stepper pour les enfants
- **Calcul automatique des parts** : Jusqu'à 15+ enfants supportés
- **Prélèvement mensuel** : Estimation du prélèvement à la source
- **Visualisations graphiques** : Barre de répartition revenu/impôt
- **Mode sombre/clair** : Support du thème système

### 💡 Section pédagogique

- **Calcul pas à pas** détaillé et expandable
- **Tableau des tranches** avec surlignage dynamique
- **Tooltips explicatifs** sur tous les termes fiscaux
- **Affichage du quotient familial** et de son application

### 🔄 Comparaison de scénarios

- **Génération automatique** de scénarios pertinents :
  - Impact d'une augmentation de salaire
  - Effet du mariage/pacs
  - Ajout d'un enfant supplémentaire
  - Changement de temps de travail
- **Affichage côte à côte** des résultats

### 📚 Glossaire fiscal intégré

- **Lexique complet** de tous les termes fiscaux
- **Explications détaillées** avec exemples concrets
- **Accès rapide** depuis le menu principal

### ⚙️ Configuration externe

- **Barème fiscal modifiable** sans recompilation
- **Fichier JSON** pour toutes les valeurs fiscales
- **Mise à jour annuelle** simplifiée
- **Documentation complète** des paramètres

---

## 🚀 Installation

### Prérequis

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) ou supérieur
- Visual Studio 2022 17.8+ avec la charge de travail .NET MAUI
- Pour Android : SDK Android 21.0+
- Pour iOS/macOS : Xcode 15+

### Cloner le repository

```bash
git clone https://github.com/votre-username/simulateur-impots-francais.git
cd simulateur-impots-francais
```

### Compiler et lancer

#### Sous Windows (Visual Studio)
1. Ouvrir `Impot_Simulateur.sln`
2. Sélectionner la plateforme cible (Windows, Android, etc.)
3. Appuyer sur F5 pour compiler et lancer

#### En ligne de commande

```bash
# Windows
dotnet build -f net9.0-windows10.0.19041.0

# Android
dotnet build -f net9.0-android

# iOS
dotnet build -f net9.0-ios

# macOS
dotnet build -f net9.0-maccatalyst
```

---

## 📁 Structure du projet

```
MauiApp2/
├── Contract/              # Interfaces
│   └── IImpotCalculator.cs
├── Converters/            # Convertisseurs XAML
│   ├── BoolToExpandIconConverter.cs
│   ├── PercentageToStarConverter.cs
│   └── ...
├── Controls/              # Contrôles personnalisés
│   └── InfoIcon.xaml      # Icône d'aide contextuelle
├── Models/                # Modèles de données
│   ├── BaremeFiscal.cs
│   ├── PartOption.cs
│   └── Scenario.cs
├── Pages/                 # Pages de l'application
│   ├── ComparaisonPage.xaml
│   └── GlossairePage.xaml
├── Resources/             # Ressources de l'app
│   └── Raw/
│       ├── bareme_fiscal.json    # ⭐ Configuration fiscale
│       └── README_BAREME.md      # Documentation du barème
├── Service/               # Services métier
│   └── ImpotCalculator.cs
├── Services/              # Services utilitaires
│   ├── BaremeFiscalService.cs
│   └── TooltipService.cs
├── ViewModels/            # ViewModels MVVM
│   ├── Mainviewmodel.cs
│   └── ComparaisonViewModel.cs
└── MainPage.xaml          # Page principale
```

---

## ⚙️ Configuration du barème fiscal

### 📝 Fichier `bareme_fiscal.json`

Le fichier `MauiApp2/Resources/Raw/bareme_fiscal.json` contient **toutes les valeurs fiscales** utilisées par l'application.

**✨ Avantage majeur** : Vous pouvez modifier ce fichier **sans recompiler l'application** !

### Structure du fichier

```json
{
  "annee": 2024,
  "tranches": {
    "seuil1": 11497,
    "seuil2": 29315,
    "seuil3": 83823,
    "seuil4": 180294,
    "taux0": 0.00,
    "taux1": 0.11,
    "taux2": 0.30,
    "taux3": 0.41,
    "taux4": 0.45
  },
  "decote": {
    "plafondCelibataire": 1964,
    "plafondCouple": 3248,
    "montantBaseCelibataire": 889,
    "montantBaseCouple": 1470,
    "coefficient": 0.4525
  },
  "abattement": {
    "tauxAbattement": 0.10,
    "plafondAbattement": 13522
  },
  "plafonnement": {
    "plafondAvantageDemiPart": 1791
  }
}
```

### Mise à jour pour 2025

1. Ouvrez `bareme_fiscal.json`
2. Modifiez les valeurs selon le nouveau barème officiel
3. Changez `"annee": 2025`
4. Sauvegardez et redéployez

🔗 [Documentation complète du barème](MauiApp2/Resources/Raw/README_BAREME.md)

---

## 🏗️ Architecture

### Technologies utilisées

- **Framework** : .NET MAUI 9.0
- **Langage** : C# 12.0
- **Pattern** : MVVM avec CommunityToolkit.Mvvm
- **UI** : XAML avec Compiled Bindings
- **Configuration** : JSON avec System.Text.Json
- **Injection de dépendances** : Microsoft.Extensions.DependencyInjection

### Composants principaux

#### `ImpotCalculator`
Calcule l'impôt selon le barème fiscal français :
- Abattement forfaitaire
- Quotient familial
- Application des tranches
- Plafonnement du QF
- Décote

#### `BaremeFiscalService`
Charge le barème fiscal depuis le fichier JSON au démarrage de l'application.

#### `TooltipService`
Fournit les explications contextuelles pour chaque terme fiscal.

### Principes de conception

- ✅ **Separation of Concerns** : Logique métier séparée de l'UI
- ✅ **Dependency Injection** : Couplage faible entre composants
- ✅ **Configuration externe** : Paramètres modifiables sans recompilation
- ✅ **Testabilité** : Interfaces pour faciliter les tests unitaires
- ✅ **Compiled Bindings** : Performance optimisée

---

## 🧪 Tests

### Cas de test principaux

L'application a été testée avec différents scénarios :

- ✅ Célibataire sans enfant
- ✅ Couple sans enfant
- ✅ Famille avec 1, 2, 3+ enfants
- ✅ Parent isolé avec enfants
- ✅ Revenus faibles (avec décote)
- ✅ Revenus élevés (avec plafonnement QF)
- ✅ Revenus très élevés (TMI 45%)


<div align="center">

**⭐ Si ce projet vous est utile, n'hésitez pas à lui donner une étoile ! ⭐**

Made with ❤️ in France 🇫🇷

</div>
