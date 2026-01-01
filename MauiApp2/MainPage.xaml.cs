using MauiApp2.ViewModels;
using MauiApp2.Pages;
using MauiApp2.Contract;

namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        private Mainviewmodel _viewModel;
        private IImpotCalculator _impotCalculator;

        public MainPage(Mainviewmodel mainViewModel, IImpotCalculator impotCalculator)
        {
            InitializeComponent();

            _viewModel = mainViewModel;
            _impotCalculator = impotCalculator;
            BindingContext = mainViewModel;
        }

        private void OnDetailCalculTapped(object? sender, EventArgs e)
        {
            _viewModel.IsDetailCalculExpanded = !_viewModel.IsDetailCalculExpanded;
        }

        private async void OnPrelevementInfoTapped(object? sender, EventArgs e)
        {
            await DisplayAlert(
                "Prélèvement à la source",
                "Depuis 2019, l'impôt sur le revenu est prélevé mensuellement directement sur votre salaire par votre employeur.\n\n" +
                "Le montant indiqué ici est une estimation du prélèvement mensuel moyen que vous subiriez avec cette situation fiscale.\n\n" +
                "Note : Le taux réel peut varier légèrement selon votre situation et les ajustements de l'administration fiscale.",
                "OK");
        }

        private async void OnComparerScenariosClicked(object? sender, EventArgs e)
        {
            var comparaisonViewModel = new ComparaisonViewModel(_impotCalculator);
            comparaisonViewModel.InitialiserComparaison(
                decimal.Parse(_viewModel.SalaireNetInput),
                _viewModel.NombreDePartsCalcule,
                _viewModel.EstEnCouple,
                _viewModel.DescriptionParts
            );

            var comparaisonPage = new ComparaisonPage(comparaisonViewModel);
            await Navigation.PushModalAsync(new NavigationPage(comparaisonPage));
        }

        private async void OnGlossaireClicked(object? sender, EventArgs e)
        {
            var glossairePage = new GlossairePage();
            await Navigation.PushModalAsync(new NavigationPage(glossairePage));
        }
    }
}