using BankBlazor.Client.Models;
using System.Net.Http.Json;

namespace BankBlazor.Client.Services
{
    public class HolidayService
    {
        private readonly HttpClient _http;

        public HolidayService(HttpClient http)
        {
            _http = http;
        }

        public async Task<HolidayEvent?> GetNextHolidayAsync()
        {
            // API returnează un obiect care conține "england-and-wales", "scotland", "northern-ireland"
            var data = await _http.GetFromJsonAsync<Dictionary<string, BankHolidayResponse>>("https://www.gov.uk/bank-holidays.json");

            if (data != null && data.TryGetValue("scotland", out var scotland))
            {
                var nextHoliday = scotland.Events
                    .Where(e => DateTime.Parse(e.Date) > DateTime.Now)
                    .OrderBy(e => DateTime.Parse(e.Date))
                    .FirstOrDefault();

                return nextHoliday;
            }

            return null;
        }
    }

    public class BankHolidayResponse
    {
        public string Division { get; set; } = "";
        public List<HolidayEvent> Events { get; set; } = new();
    }
}
