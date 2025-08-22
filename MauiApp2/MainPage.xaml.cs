using MauiApp2.ViewModels;

namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        public MainPage(Mainviewmodel mainViewModel)
        {
            InitializeComponent();

            BindingContext = mainViewModel;
        }
    }
}