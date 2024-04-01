using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.AppUpdateService
{
    public interface IUpdateService
    {
        string BuildNumber { get; set; }
        event EventHandler<int> DownloadPercentageChanged;
        Task<bool> CheckIfFileExistsOnServerAsync();
        Task CallFileOpener(string path);
        Task DownloadApk(CancellationToken cancellationToken);
    }
}
