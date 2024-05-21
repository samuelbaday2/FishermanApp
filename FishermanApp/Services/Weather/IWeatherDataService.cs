using FishermanApp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishermanApp.Services.Weather
{
    public interface IWeatherDataService
    {
        Task<WeatherData> GetAllWeatherDataAsync();
        Task<List<Hourly>> GetHourliesAsync();
        Task<List<Daily>> GetDaysAsync();
        Task<Placemark> GetPlacemarkAsync();
    }
}
