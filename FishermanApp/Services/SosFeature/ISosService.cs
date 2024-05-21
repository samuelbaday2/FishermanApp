using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.SosFeature
{
    public interface ISosService
    {
        bool IsSosSignalOn { get; set; } 
        Task TurnSosSignalAsync();
        Task InitiateSosSignalAsync(bool status);
        Task TurnOffSosSignalAsync();
    }
}
