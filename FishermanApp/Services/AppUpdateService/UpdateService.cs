using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using FishermanApp.Services.ConnectivityService;
using FishermanApp.Services.LocationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.AppUpdateService
{
    public class UpdateService : IUpdateService
    {
        public string BuildNumber { get; set; }
        public event EventHandler<int> DownloadPercentageChanged;

        ILocationFeatureService _locationFeatureService;
        IConnectionHandlerService _connectivityService;

        public UpdateService(ILocationFeatureService locationFeatureService, IConnectionHandlerService connectivityService)
        {
            _locationFeatureService = locationFeatureService;
            _connectivityService = connectivityService;
        }
        public async Task CallFileOpener(string filename)
        {
            var filepath = string.Empty;
#if ANDROID
                filepath =  global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryDownloads).Path;
#endif

            _locationFeatureService.InstallApk(System.IO.Path.Combine(filepath, $"fishermanapp_{BuildNumber}.apk"));
        }
        public async Task<bool> CheckIfFileExistsOnServerAsync()
        {
            if (_connectivityService.IsConnected) 
            {
            
            }
            // var dialog = ProgressDialog.Show(this, "Download", "Downloading file");
            var webClient = new WebClient();

            var username = "Samuel";
            var password = "Baday1234";
            string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                           .GetBytes(username + ":" + password));

            webClient.Headers.Add("Authorization", "Basic " + encoded);

            var text = await webClient.DownloadStringTaskAsync(new Uri("http://174.47.67.167:8081/Image/textfile.txt"));

            BuildNumber = text;
            var versionNum = text;

#if DEBUG
            Console.WriteLine("UPDATE_STATUS " + versionNum.ToString());
#endif

            return double.Parse(versionNum) > double.Parse(AppInfo.Current.BuildString);
        }

        public async Task DownloadApk(CancellationToken cancellationToken)
        {
            using (WebClient wc = new WebClient())
            {
                var filepath = string.Empty;
#if ANDROID
                filepath =  global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryDownloads).Path;
#endif
                wc.CancelAsync();
                wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                try
                {
                    string completePath = System.IO.Path.Combine(filepath, $"fishermanapp_{BuildNumber}.apk");
                    //var filepath = System.IO.Path.Combine(filepath, "fish_app.apk");
                    wc.DownloadFileAsync(new Uri($"http://174.47.67.167:8081/Image/fishermanapp_{BuildNumber}.apk"), completePath); 
                    Toast.Make($"download started : {completePath}", ToastDuration.Long).Show();
                }
                catch (Exception ex)
                {

                }

                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            }
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("DOWNLOAD_PROGRESS" + e.ProgressPercentage);
#endif

            DownloadPercentageChanged.Invoke(this,e.ProgressPercentage);

            if (e.ProgressPercentage == 100)
            {
                CallFileOpener(BuildNumber);
            }
        }
    }
}
