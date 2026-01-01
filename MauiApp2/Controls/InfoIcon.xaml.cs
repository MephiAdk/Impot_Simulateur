using MauiApp2.Services;

namespace MauiApp2.Controls
{
    public partial class InfoIcon : ContentView
    {
        public static readonly BindableProperty TooltipKeyProperty =
            BindableProperty.Create(nameof(TooltipKey), typeof(string), typeof(InfoIcon), string.Empty);

        private TooltipService _tooltipService;

        public InfoIcon()
        {
            InitializeComponent();
            _tooltipService = new TooltipService();
        }

        public string TooltipKey
        {
            get => (string)GetValue(TooltipKeyProperty);
            set => SetValue(TooltipKeyProperty, value);
        }

        private async void OnInfoTapped(object? sender, EventArgs e)
        {
            var tooltipInfo = _tooltipService.GetTooltip(TooltipKey);
            if (tooltipInfo != null)
            {
                string message = tooltipInfo.Explication;
                
                if (!string.IsNullOrEmpty(tooltipInfo.Exemple))
                {
                    message += "\n\n" + tooltipInfo.Exemple;
                }

                var mainPage = Application.Current?.MainPage;
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert(
                        tooltipInfo.Titre,
                        message,
                        "OK");
                }
            }
        }
    }
}

