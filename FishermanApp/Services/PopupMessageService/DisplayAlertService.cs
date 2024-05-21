using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.PopupMessageService
{
    public class DisplayAlertService : IDisplayAlertService
    {
        public async Task<bool> DisplayALertAsync(string title, string message)
        {
            var result = await App.Current.MainPage.DisplayAlert(title, message,"Confirm" ,"Cancel", FlowDirection.LeftToRight);
            return result;
        }
    }
}
