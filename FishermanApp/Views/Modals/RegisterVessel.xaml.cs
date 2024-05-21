using FishermanApp.Services.AppUpdateService;
using FishermanApp.ViewModels;
using FishermanApp.ViewModels.Modals;
using System.Runtime.InteropServices;

namespace FishermanApp.Views.Modals;

public partial class RegisterVessel : ContentPage
{
    private RegisterVesselModalViewModel _viewModel;
    public RegisterVessel(IUpdateService updateService,LoginPageViewModel loginPageViewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = new RegisterVesselModalViewModel(updateService, loginPageViewModel);
	}
}