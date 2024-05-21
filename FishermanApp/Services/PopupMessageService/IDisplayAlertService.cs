using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.PopupMessageService
{
    public interface IDisplayAlertService
    {
        Task<bool> DisplayALertAsync(string title,string message);
    }
}
