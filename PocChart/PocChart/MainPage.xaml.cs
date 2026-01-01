using PocChart.ViewModels;

namespace PocChart
{
    public partial class MainPage : ContentPage
    {

        public MainPage(AccountsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;

        }
    }
}
