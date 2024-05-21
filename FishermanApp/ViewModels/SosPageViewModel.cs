using FishermanApp.Services.SosFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FishermanApp.ViewModels
{
    public class SosPageViewModel : BaseViewModel
    {
        private readonly ISosService _sosService;
        private bool _isSosOn;
        private Color _sosBackGround;

        public bool IsSosOn { get { return _isSosOn; } set { SetProperty(ref _isSosOn, value); } }
        public Color SosBackGround { get { return _sosBackGround; } set { SetProperty(ref _sosBackGround, value); } }
        public ICommand SosSignalCommand { private set; get; }

        public SosPageViewModel(ISosService sosService)
        {
            SosSignalCommand = new Command(DoSosSignal);
            _sosService = sosService;
            SosBackGround = Color.Parse("#ffffff");
        }

        private async void DoSosSignal(object obj)
        {
            if (!IsSosOn)
            {
                IsSosOn = true;
                _sosService.TurnSosSignalAsync();

                List<Color> background = new List<Color>();
                background.Add(Color.Parse("#99323e"));
                background.Add(Color.Parse("#ffffff"));
                background.Add(Color.Parse("#163975"));

                int counter = 0;

                while (IsSosOn) 
                {
                    SosBackGround = background[counter];
                    await _sosService.InitiateSosSignalAsync(true);
                    await Task.Delay(1000);
                    counter++;
                    if (counter >= 3) {
                        counter = 0;
                    }
                }              
            }
            else 
            {
                TurnOffSignal();
                SosBackGround = Color.Parse("#ffffff");
            }
        }

        public async Task TurnOffSignal()
        {
            IsSosOn = false;
            _sosService.TurnOffSosSignalAsync();
            _sosService.InitiateSosSignalAsync(false);
            SosBackGround = Color.Parse("#ffffff");
        }
    }
}
