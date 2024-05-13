using FatMaui.ViewModel;

namespace FatMaui.View;

public partial class NutritionPage : ContentPage
{
	public NutritionPage()
	{
		InitializeComponent();
        // Подписываемся на событие Appearing
        this.Appearing += OnPageAppearing;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

    }

    private void OnPageAppearing(object sender, EventArgs e)
    {
        // Получаем ViewModel
        var viewModel = BindingContext as NutritionViewModel;

        // Загружаем данные
        if (viewModel != null)
        {
            Task.Run(() => viewModel.LoadData());
        }
    }
}