using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using FishermanApp.Objects;
using FishermanApp.Resources.Localization;
using FishermanApp.Services.AppUpdateService;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels.Modals
{
    public class RegisterVesselModalViewModel : BaseViewModel
    {
        private readonly IUpdateService _updateService;
        private readonly LoginPageViewModel _loginPageViewModel;

        private string _vesselName;
        private string _captainName;
        public string VesselName { get { return _vesselName; } set { SetProperty(ref _vesselName, value); } }
        public string CaptainName { get { return _captainName; } set { SetProperty(ref _captainName, value); } }
        public ICommand RegisterCommand { private set; get; }

        public RegisterVesselModalViewModel(IUpdateService updateService, LoginPageViewModel loginPageViewModel) 
        {
            RegisterCommand = new Command(DoRegister);
            _updateService = updateService;
            _loginPageViewModel = loginPageViewModel;
        }

        private async void DoRegister(object obj)
        {
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
                request2.AddParameter("VesselName", VesselName.Trim());
                request2.AddParameter("Captain", CaptainName.Trim());


                request2.AddUrlSegment("function", "InsertVesselRecord");

                IRestResponse response2 = client2.Execute(request2);

                string resposeString = response2.Content;
                json = response2.Content;

                //var response = await client.GetAsync(request);
                //json = response.Content.ReadAsStringAsync().Result;

                var returnValue = JsonConvert.DeserializeObject<List<RegistrationObject>>(json);

                if (returnValue.Count == 0)
                {
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

                        Application.Current.MainPage = new AppShell(_loginPageViewModel, _updateService);
                    }
                    else
                    {

                    }


                }
                catch
                {

                }
            }
            catch
            {

            }
            finally
            {
                SetBusyStatusAsync(true);
            }
        }
    }
}
