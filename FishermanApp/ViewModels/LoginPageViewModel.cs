using FishermanApp.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private string _versionString;

        public string VersionString { get { return _versionString; } set { SetProperty(ref _versionString, value); } }
        public LoginPageViewModel()
        {
            VersionString = $"{AppResources.Version} {AppInfo.VersionString}";
        }
    }
}
