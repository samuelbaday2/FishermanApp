using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.AndroidEnvironmentService
{
    public interface IAndroidEnvironmentService
    {
        Task RunAndroidCodeAsync(Action action);
    }
}
