using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using FishermanApp.Objects;
using FishermanApp.Resources.Localization;
using FishermanApp.Services.AppUpdateService;
using FishermanApp.Services.LocationService;
using FishermanApp.Views.Pages;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IUpdateService _updateService;

        private string _versionString;
        private string _username;
        private bool _rememberMe;
        private bool _isRemembered;
        private bool IsFlashLightOn = false;

        public string VersionString { get { return _versionString; } set { SetProperty(ref _versionString, value); } }
        public string Username { get { return _username; } set { SetProperty(ref _username, value); } }
        public bool RememberMe { get { return _rememberMe; } set { SetProperty(ref _rememberMe, value); } }
        public bool IsRemembered { get { return _isRemembered; } set { SetProperty(ref _isRemembered, value); } }

        public ICommand LoginCommand { private set; get; }
        public ICommand RegisterCommand { private set; get; }
        public LoginPageViewModel()
        {
            VersionString = $"{AppResources.Version} {AppInfo.VersionString}";
            LoginCommand = new Command(DoLoginAsync);
            RegisterCommand = new Command(DoRegisterAsync);

            Username = string.Empty;
#if DEBUG
            //Username = $"J3-1656-PM";
#else
            //Username = $"J3-1656-PM";
#endif

            try
            {
                IsRemembered = Preferences.Get(Pref.REMEMBER_ME, false);
                if (Preferences.Get(Pref.REMEMBER_ME, false))
                {
                    Username = Preferences.Get(Pref.USERNAME, string.Empty);
                }

            }
            catch { }
          
            //_updateService.DownloadPercentageChanged += _updateService_DownloadPercentageChanged;
        }

        public async Task UpdatePercent(string percent)
        {
            try
            {
                Percentage = percent;
            }
            catch { }
        }

        private async void DoRegisterAsync(object obj)
        {
            //await Shell.Current.Navigation.PushModalAsync(new RegisterAccountPage());
            //Application.Current.MainPage = new RegisterAccountPage();          
        }     

        private async void DoLoginAsync(object obj)
        {
            if(!IsNotBusy) return;

            if (Username.Length == 0) 
            {
                await Toast.Make(AppResources.LinkVesselTextHeader,ToastDuration.Long).Show();
                return;
            }
            SetBusyStatusAsync(false);
            try
            {
                string json = "";
                //var client = new System.Net.Http.HttpClient();
                //client.Timeout = TimeSpan.FromMilliseconds(10000);

                //string request = "http://174.47.67.167:8076/nextt.asmx/CatchAppLogin"
                //        + "?vesselId=" + EntryUsername.Text.Trim()
                //        + "&idpassword=" + EntryPassword.Text.Trim();

                string hostname = "http://" + AppResources.Hostname;
                var client2 = new RestClient(hostname);

                var username = "Samuel";
                var password = "Baday1234";
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                               .GetBytes(username + ":" + password));


                client2.AddDefaultHeader("Authorization", "Basic " + encoded);

                var request2 = new RestRequest("nextt.asmx/{function}", Method.GET);
                request2.AddParameter("vesselId", Username.Trim());
                request2.AddParameter("idpassword", "");


                request2.AddUrlSegment("function", "CatchAppLogin");

                IRestResponse response2 = client2.Execute(request2);

                string resposeString = response2.Content;
                json = response2.Content;

                //var response = await client.GetAsync(request);
                //json = response.Content.ReadAsStringAsync().Result;

                var returnValue = JsonConvert.DeserializeObject<List<RegistrationObject>>(json);

                if (returnValue.Count == 0) {
                    await Toast.Make(AppResources.NoVesselRecord, ToastDuration.Long).Show();
                    return;
                }

                UserDataObject.UserObject = returnValue[0];
                List<CrewObject> returnValue2 = JsonConvert.DeserializeObject<List<CrewObject>>(json);

                try
                {
                    RegistrationObject loginJsonObject = returnValue[0] as RegistrationObject;

                    if (loginJsonObject.VesId.Length > 0)
                    {
                        //       App.Current.Properties["RegistrationObject"] = json;

                        // Preferences.Set(PREF.PASSWORD, EntryPassword.Text.Trim());

                        //App.Current.Properties["Username"] = EntryUsername.Text.Trim();
                        //App.Current.Properties["Password"] = EntryPassword.Text.Trim();

                        //App.Current.SavePropertiesAsync();
                         
                        Preferences.Set(Pref.LOGGED_USER, resposeString);

                        Preferences.Set(Pref.REMEMBER_ME, IsRemembered);
                        Preferences.Set(Pref.USERNAME, Username);

                        hostname = "http://" + AppResources.RemoteAppHostName;
                        client2 = new RestClient(hostname);

                        username = "Samuel";
                        password = "Baday1234";
                        encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                                       .GetBytes(username + ":" + password));

                        client2.AddDefaultHeader("Authorization", "Basic " + encoded);

                        request2 = new RestRequest("Transaction.asmx/{function}", Method.GET);

                        request2.AddUrlSegment("function", "GetGear");

                        response2 = client2.Execute(request2);
                        json = response2.Content;

                        Preferences.Set(Pref.GEAR_LIST, json);


                        if (Preferences.Get(Pref.LOGGED_USER, string.Empty) != string.Empty)
                        {
                            string jsonLogged = Preferences.Get(Pref.LOGGED_USER, string.Empty);
                            var returnValueLogged = JsonConvert.DeserializeObject<List<RegistrationObject>>(jsonLogged);

                            UserDataObject.UserObject = returnValueLogged[0];
                        }

                        Application.Current.MainPage = new AppShell(this, _updateService);
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //var toastConfig = new ToastConfig("An Error Occurred");
                    //toastConfig.SetDuration(3000);

                    //UserDialogs.Instance.Toast(toastConfig);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                SetBusyStatusAsync(true);
            }
           
        }
        public async Task LoginWithoutId() 
        { 
            
        }
    }
}
