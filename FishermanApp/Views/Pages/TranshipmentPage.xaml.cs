using FishermanApp.Views.Transhipment;
using ZXing.Net.Maui;

namespace FishermanApp.Views.Pages;

public partial class TranshipmentPage : ContentPage
{
	public TranshipmentPage()
	{
		InitializeComponent();
	}
    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        foreach (var barcode in e.Results)
            Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.Navigation.PushAsync(new TransferPage());
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {

    }
}