using FatMaui.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Maui.Controls;

namespace FatMaui.View
{
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
            // Подписываемся на событие Appearing
            this.Appearing += OnPageAppearing;
        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //}
        private void OnPageAppearing(object sender, EventArgs e)
        {
            // Получаем ViewModel
            var viewModel = BindingContext as MainViewModel;

            // Загружаем данные
            if (viewModel != null)
            {
                Task.Run(() => viewModel.LoadData());
                Task.Run(() => viewModel.LoadMealProduct());
            }
        }

    }

}
