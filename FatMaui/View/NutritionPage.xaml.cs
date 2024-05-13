using FatMaui.ViewModel;

namespace FatMaui.View;

public partial class NutritionPage : ContentPage
{
	public NutritionPage()
	{
		InitializeComponent();
        // ������������� �� ������� Appearing
        this.Appearing += OnPageAppearing;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

    }

    private void OnPageAppearing(object sender, EventArgs e)
    {
        // �������� ViewModel
        var viewModel = BindingContext as NutritionViewModel;

        // ��������� ������
        if (viewModel != null)
        {
            Task.Run(() => viewModel.LoadData());
        }
    }
}