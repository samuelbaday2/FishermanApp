using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.SosFeature
{
    public class SosService : ISosService
    {
        public bool IsSosSignalOn { get; set; }
        public int[] SosInterval = { 200, 200, 200, 1500, 1500, 1500, 200, 200, 200 };
        public async Task TurnOffSosSignalAsync()
        {
            IsSosSignalOn = false;
        }

        public async Task TurnSosSignalAsync()
        {
            IsSosSignalOn = true;
        }

        public async Task InitiateSosSignalAsync(bool status)
        {
            if (!status)
            {
                try
                {
                    await Flashlight.Default.TurnOffAsync();
                }
                catch { }
                finally {  }

                return;
            }

            foreach (int sosInterval in SosInterval)
            {
                if (IsSosSignalOn)
                {
                    try
                    {
                        await Flashlight.Default.TurnOnAsync();
                    }
                    catch (FeatureNotSupportedException ex)
                    {
                        // Handle not supported on device exception
                    }
                    catch (PermissionException ex)
                    {
                        // Handle permission exception
                    }
                    catch (Exception ex)
                    {
                        // Unable to turn on/off flashlight
                    }
                    await Task.Delay(sosInterval);
                }

                //await Task.Delay(1000);
                await Flashlight.Default.TurnOffAsync();
            }

        }
    }
}
