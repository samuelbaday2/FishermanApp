using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.AndroidEnvironmentService
{
    public class AndroidEnvironmentService : IAndroidEnvironmentService
    {
        public async Task RunAndroidCodeAsync(Action action)
        {
#if ANDROID
            action();
#endif
        }
    }
}
