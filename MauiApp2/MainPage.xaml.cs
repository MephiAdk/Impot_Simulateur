using MauiApp2.ViewModels;

namespace MauiApp2
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainMV mainViewModel)
        {
            InitializeComponent();

            BindingContext = mainViewModel;
        }
    }
}
