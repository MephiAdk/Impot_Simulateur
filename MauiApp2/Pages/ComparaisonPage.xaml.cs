using MauiApp2.ViewModels;

namespace MauiApp2.Pages
{
    public partial class ComparaisonPage : ContentPage
    {
        public ComparaisonPage(ComparaisonViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnFermerClicked(object? sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}

