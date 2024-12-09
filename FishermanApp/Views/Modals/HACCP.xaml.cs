using FishermanApp.Constants.LocalDatabase.Tables;

namespace FishermanApp.Views.Modals;

public partial class HACCP : ContentPage
{
	public HACCP()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await new HACCPTable().SaveItemAsync(new Objects.DbObjects.DBHACCPObject { 
            Ice = CheckIce.IsChecked,
            Water = CheckWater.IsChecked,
            Container = CheckContainer.IsChecked,
            Cleaning = CheckClean.IsChecked,
        });
        Shell.Current.Navigation.PopModalAsync();
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        CheckClean.IsChecked = true;
    }

    private void Button_Clicked_2(object sender, EventArgs e)
    {
        CheckClean.IsChecked = false;
    }

    private void Button_Clicked_3(object sender, EventArgs e)
    {
        CheckIce.IsChecked = true;
    }

    private void Button_Clicked_4(object sender, EventArgs e)
    {
        CheckIce.IsChecked = false;
    }

    private void Button_Clicked_5(object sender, EventArgs e)
    {
        CheckWater.IsChecked = true;
    }

    private void Button_Clicked_6(object sender, EventArgs e)
    {
        CheckWater.IsChecked = false;
    }

    private void Button_Clicked_7(object sender, EventArgs e)
    {
        CheckContainer.IsChecked = true;

    }

    private void Button_Clicked_8(object sender, EventArgs e)
    {
        CheckContainer.IsChecked = false;
    }
}